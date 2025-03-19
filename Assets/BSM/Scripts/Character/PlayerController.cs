using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine; 
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviourPun
{
    public float PosX;
    public float PosY;
    public float PosZ;

    public bool CanJump;
    public bool IsDeath;
    public GameObject PlayerBody;
    public Canvas PlayerCanvas;
    public Camera PlayerCam;
    public PlayerStats PlayerStats => _playerStats;
    public Rigidbody PlayerRb => _playerRb;
    public Collider OnTriggerOther;
    public Coroutine ConsumeStaminaCo;
    public Coroutine RecoverStaminaCo;
    public Transform ArmPos;
    public Transform HeadPos;
    public Item CurCarryItem;
    public Animator PlayerAnimator;
    public PState CurState => _curState;
    public Vector3 MoveDir = Vector3.zero;
 
    private PlayerState[] _playerStates = new PlayerState[(int)PState.SIZE];
    private PlayerStats _playerStats;
    private Rigidbody _playerRb; 
    private Inventory _inventory;
    private Item _item;
    private PopUp _popup; 
    private NewDoor _newDoor;
    private InDoor _inDoor;
    private GameObject _computerObject;
    
    private PState _curState = PState.IDLE;

    private int _curInventoryIndex = 0;
    private float _mouseX;
    private float _mouseY;
    private int _itemLayer => LayerMask.GetMask("Item");
    private int _computerLayer => LayerMask.NameToLayer("Computer");
    
    private int _getOneHandUseAniHash => Animator.StringToHash("GetUseItem");
    private int _dropOneHandUseAniHash => Animator.StringToHash("DropUseItem");
    private int _getTwoHandAniHash => Animator.StringToHash("GetTwoHandItem");
    private int _dropTwoHandAniHash => Animator.StringToHash("DropTwoHandItem");
    
    private float _sensitivity => DataManager.Instance.UserSettingData.Sensitivity;
    
    private void Awake() => Init();
    
    private void Init()
    {
        DontDestroyOnLoad(gameObject);
        PlayerAnimator = GetComponent<Animator>();
        
        if(!photonView.IsMine) return; 
        
        _inventory = GetComponent<Inventory>();
        _playerStats = GetComponent<PlayerStats>();
        _playerRb = GetComponent<Rigidbody>();
        _playerStates[(int)PState.IDLE] = new PlayerIdle(this);
        _playerStates[(int)PState.WALK] = new PlayerWalk(this);
        _playerStates[(int)PState.RUN] = new PlayerRun(this);
        _playerStates[(int)PState.JUMP] = new PlayerJump(this);
        _playerStates[(int)PState.ATTACK] = new PlayerAttack(this);
        _playerStates[(int)PState.HURT] = new PlayerHurt(this);
        _playerStates[(int)PState.DEATH] = new PlayerDeath(this);
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            //캐릭터가 생성됐을 때, 중복 카메라 및 UI 제거
            PlayerCam.gameObject.SetActive(false);
            PlayerCanvas.gameObject.SetActive(false);
            return;
        } 
        
        PlayerCam.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); 
        _playerStates[(int)_curState].Enter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        { 
            OnTriggerOther = other;
        }
        
        if (!photonView.IsMine) return; 
        _playerStates[(int)_curState].OnTrigger(); 
    }

    private void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine) return;
        OnTriggerOther = null;
    }
    
    
    private void Update()
    {
        if (!photonView.IsMine) return;
        if (_computerObject != null && _computerObject.activeSelf) return; 
            
        _playerStates[(int)_curState].Update();
        InputKey();
        PositionUpdate();
        InputRotate();
        CameraToItemRay();
        DropItem();
        ItemPositionToArm();
        SelectInventoryInItem();
    }

    private void LateUpdate()
    {
        if (_computerObject != null && _computerObject.activeSelf) return;
        if (IsDeath) return;
        
        PlayerCam.transform.position = Vector3.Lerp(PlayerCam.transform.position, HeadPos.transform.position, 5f * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        _playerStates[(int)_curState].FixedUpdate();
    }
 
    /// <summary>
    /// 키보드 입력
    /// </summary>
    private void InputKey()
    {
        MoveDir.x = Input.GetAxisRaw("Horizontal");
        MoveDir.z = Input.GetAxisRaw("Vertical");
    }

    private void PositionUpdate()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;
        
        photonView.RPC(nameof(SyncPositionUpdate), RpcTarget.AllViaServer, x, y, z); 
    }
     
    [PunRPC]
    private void SyncPositionUpdate(float posX, float posY, float posZ)
    {
        PosX = posX;
        PosY = posY;
        PosZ = posZ;
    }
    
    /// <summary>
    /// 인벤토리 키 입력
    /// </summary>
    private void SelectInventoryInItem()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            CarryItemChange(0);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            CarryItemChange(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            CarryItemChange(2);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            CarryItemChange(3);
        } 
    }
    
    /// <summary>
    /// 들고있는 아이템 변경
    /// </summary>
    /// <param name="index"></param>
    private void CarryItemChange(int index)
    {
        _curInventoryIndex = index;
        
        //들고 있는 아이템이 있는지 확인
        if (CurCarryItem != null)
        {
            if (CurCarryItem.GetHoldingType() == ItemHoldingType.ONEHANDED)
            { 
                if (!CurCarryItem.AttackItem())
                {
                    BehaviourAnimation(_dropOneHandUseAniHash);
                }
                
                CurCarryItem.gameObject.SetActive(false); 
                
                //변경할 슬롯의 아이템이 있는지 확인
                if (_inventory.SelectedItem(index) != null)
                {
                    CurCarryItem = _inventory.SelectedItem(index); 
                    CurCarryItem.gameObject.SetActive(true);

                    if (!CurCarryItem.AttackItem())
                    {
                        BehaviourAnimation(_getOneHandUseAniHash);
                    }  
                }
                else
                {
                    CurCarryItem = null; 
                }
            } 
        }
        else
        {
            if (_inventory.SelectedItem(index) != null)
            {
                CurCarryItem = _inventory.SelectedItem(index);
                CurCarryItem.gameObject.SetActive(true); 
                
                if (!CurCarryItem.AttackItem())
                {
                    BehaviourAnimation(_getOneHandUseAniHash);
                } 
            }
        }  
    }
    
    
    /// <summary>
    /// 현재 아이템의 소유권자를 확인 후 위치 동기화
    /// </summary>
    private void ItemPositionToArm()
    {
        if(CurCarryItem == null) return;
        if (!CurCarryItem.IsOwned) return;
        if (!CurCarryItem.photonView.Owner.Equals(photonView.Owner)) return;
        
        //TODO: 추후 팔 위치 조정 필요 OneHand냐 TwoHand냐?
        //CurCarryItem에서 위치 조정 함수 호출하면 될듯
        if (CurCarryItem.AttackItem())
        {
            //공격 아이템인지 아닌지에 따라 잡는 모션 다르게 처리하면 될듯
        }
        else
        {
            
        }
        
        CurCarryItem.transform.position = ArmPos.position;
        CurCarryItem.transform.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0);
    }
    
    /// <summary>
    /// 마우스 회전 입력
    /// </summary>
    private void InputRotate()
    {
        if (IsDeath) return;
        
        _mouseX += Input.GetAxisRaw("Mouse X") * _sensitivity * Time.deltaTime;
        _mouseY += Input.GetAxisRaw("Mouse Y");

        _mouseY = Mathf.Clamp(_mouseY, -90f, 90f);
        
        //캐릭터 몸체 회전
        transform.rotation = Quaternion.Euler(0, _mouseX, 0f);
        photonView.RPC(nameof(SyncCharacterRotate), RpcTarget.AllViaServer, _mouseX);
        //카메라 상하/좌우 회전
        PlayerCam.transform.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0f);
         
    }

    /// <summary>
    /// 캐릭터 회전 동기화
    /// </summary>
    /// <param name="x"></param>
    [PunRPC]
    private void SyncCharacterRotate(float x)
    {
        transform.rotation = Quaternion.Euler(0, x, 0);        
    }

    /// <summary>
    /// 카메라 정면 방향으로 레이쏨
    /// </summary>
    private void CameraToItemRay()
    {
        Ray ray = new Ray(PlayerCam.transform.position, PlayerCam.transform.forward);
        Ray jumpRay = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        
        Debug.DrawRay(ray.origin, ray.direction * 5, Color.red);
        Debug.DrawRay(jumpRay.origin, jumpRay.direction * 0.3f, Color.blue);
        
        ItemRaycast(ray);
        ObjectRaycast(ray, ref _popup);
        ObjectRaycast(ray, ref _newDoor);
        ObjectRaycast(ray, ref _inDoor);
        JumpGroundCheckRayCast(jumpRay);
    }
    
    /// <summary>
    /// 점프 가능 레이
    /// </summary>
    private void JumpGroundCheckRayCast(Ray jumpRay)
    {
        if (Physics.Raycast(jumpRay.origin, jumpRay.direction, out RaycastHit jumpHit, 0.3f))
        {
            if (jumpHit.collider != null)
            {
                float groundDistance = Mathf.Abs(jumpHit.point.y - transform.position.y);

                if (groundDistance < 0.1f)
                {
                    CanJump = true;
                }
                else
                {
                    CanJump = false;
                }                  
            }  
        }
        else
        {
            CanJump = false;
        }
    }
    
    /// <summary>
    /// 아이템 감지 레이
    /// </summary>
    /// <param name="ray"></param>
    private void ItemRaycast(Ray ray)
    {
        if (UIManager.Instance == null) return;
            
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 2f, _itemLayer))
        { 
            _item = hit.collider.GetComponent<Item>();
            
            if (!_item.IsOwned)
            {
                if (!_inventory.IsFull && Input.GetKeyDown(KeyCode.E) && _playerStats.CanCarry)
                {
                    if (_item.GetHoldingType() == ItemHoldingType.TWOHANDED)
                    {
                        _playerStats.CanCarry = false;
                    }
                    
                    ItemPickUp(_item);
                } 
            }
             
            UIManager.Instance.ItemPickObjActive(!_inventory.IsFull && !_item.IsOwned && _playerStats.CanCarry);
        }
        else
        {
            UIManager.Instance.ItemPickObjActive();
        }
    }
      
    
    /// <summary>
    /// 오브젝트를 제네릭으로 감지
    /// </summary>
    /// <param name="ray">캐릭터 -> 오브젝트 방향</param>
    /// <param name="target">PopUp, Indoor</param>
    /// <param name="layer">PopUp, Indoor 레이어</param>
    /// <typeparam name="T"></typeparam>
    private void ObjectRaycast<T>(Ray ray, ref T target) where T : MonoBehaviour, IHitMe
    {
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 3f))
        {
            T newTarget = hit.collider.GetComponent<T>();
 
            if (hit.collider.gameObject.layer == _computerLayer)
            {
                _computerObject = hit.collider.transform.GetChild(0).gameObject;
            }
            else
            {
                _computerObject = null;
            }
            
            if (newTarget != null)
            {
                target = newTarget;
                target.HitMe = true;
            } 
        }
        else
        {
            if (target != null)
            {
                target.HitMe = false;
                target = null;
            }
            
            _computerObject = null;
        } 
    }
      
    /// <summary>
    /// 들고있는 아이템 드랍
    /// </summary>
    private void DropItem()
    {
        if (CurCarryItem != null && Input.GetKeyDown(KeyCode.G))
        {
            if (CurCarryItem.GetHoldingType() == ItemHoldingType.TWOHANDED)
            {
                BehaviourAnimation(_dropTwoHandAniHash);
                _playerStats.CanCarry = true;
            }
            else
            {
                BehaviourAnimation(_dropOneHandUseAniHash);
                _inventory.DropItem(_curInventoryIndex);
            }
             
            CurCarryItem.Drop(); 
            _playerStats.IsNotHoldingItem(_item.GetItemWeight());
            CurCarryItem = null;
        } 
    }

    /// <summary>
    /// 아이템 소유권 및 잡을 위치 지정
    /// </summary>
    /// <param name="item"></param>
    private void ItemPickUp(Item item)
    {
        if (CurCarryItem == null)
        {
            CurCarryItem = item;
        }
        else
        {
            //현재 들고 있는 아이템과 다른 아이템을 주웠을 경우
            if (CurCarryItem != item)
            { 
                CurCarryItem.gameObject.SetActive(false); 
                CurCarryItem = item;
            }
        }

        if (CurCarryItem.GetHoldingType() == ItemHoldingType.ONEHANDED)
        {
            _inventory.GetItem(CurCarryItem);

            if (!CurCarryItem.AttackItem())
            {
                BehaviourAnimation(_getOneHandUseAniHash);
            } 
        }
        else
        { 
            BehaviourAnimation(_getTwoHandAniHash);
        }
        
        CurCarryItem.PickUp(PhotonNetwork.LocalPlayer);
        _playerStats.IsHoldingItem(_item.GetItemWeight());
    }

    /// <summary>
    /// 애니메이션 행동 동기화
    /// </summary>
    /// <param name="animHash">애니메이션 해시값</param>
    /// <param name="state">현재 상태</param>
    public void BehaviourAnimation(int animHash, bool state)
    { 
        photonView.RPC(nameof(SyncBehaviourAnimation), RpcTarget.AllViaServer, animHash, state);
    } 
    
    [PunRPC]
    private void SyncBehaviourAnimation(int animHash, bool state)
    { 
        PlayerAnimator.SetBool(animHash, state);
    } 
    
    /// <summary>
    /// 이동 방향 동기화
    /// </summary>
    /// <param name="animHash">애니메이션 해시값</param>
    /// <param name="direction">이동 방향</param>
    public void BehaviourAnimation(int animHash, float direction)
    {
        photonView.RPC(nameof(SyncBehaviourAnimation), RpcTarget.AllViaServer, animHash, direction);
    }
    
    [PunRPC]
    private void SyncBehaviourAnimation(int animHash, float direction)
    {
        PlayerAnimator.SetFloat(animHash, direction);
    }
    
    public void BehaviourAnimation(int animHash)
    {
        photonView.RPC(nameof(SyncBehaviourAnimation), RpcTarget.AllViaServer, animHash);
    }
    
    [PunRPC]
    private void SyncBehaviourAnimation(int animHash)
    {
        PlayerAnimator.SetTrigger(animHash);
    }
    
    /// <summary>
    /// 죽은 상태 동기화
    /// </summary>
    /// <param name="isDeath"></param>
    /// <param name="isEnable"></param>
    /// <param name="isKinematic"></param>
    [PunRPC]
    public void SyncDeathRPC(bool isDeath, bool isEnable, bool isKinematic)
    {
        IsDeath = isDeath;
        gameObject.GetComponent<CapsuleCollider>().enabled = isEnable;
        gameObject.GetComponent<Rigidbody>().isKinematic = isKinematic;
    }
    
    /// <summary>
    /// 현재 플레이어의 상태 전환
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(PState newState)
    {
        if (!photonView.IsMine) return;
        
        _playerStates[(int)_curState].Exit();
        _curState = newState;
        _playerStates[(int)_curState].Enter();
    } 
}
