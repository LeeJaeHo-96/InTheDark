using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviourPun
{
    public float PosX;
    public float PosY;
    public float PosZ;
    
    public bool IsDeath;
    public GameObject PlayerBody;
    public Canvas PlayerCanvas;
    public Camera PlayerCam;
    public PlayerStats PlayerStats => _playerStats;
    public Rigidbody PlayerRb => _playerRb;
    public Collider OnTriggerOther;
    public Coroutine ConsumeStaminaCo;
    public Coroutine RecoverStaminaCo;
    public PState CurState => _curState;
    public Vector3 MoveDir = Vector3.zero;
     
    private PlayerState[] _playerStates = new PlayerState[(int)PState.SIZE];
    private PlayerStats _playerStats;
    private Rigidbody _playerRb;
    private Transform _armPos;
    
    private Inventory _inventory;
    private Item _item;
    public Item CurCarryItem;
    private PopUp _popup;
    
    private PState _curState = PState.IDLE;

    private int _curInventoryIndex = 0;
    private float _mouseX;
    private float _mouseY;
    private int _itemLayer => LayerMask.GetMask("Item");

    
    
    private float _sensitivity => DataManager.Instance.UserSettingData.Sensitivity;
    
    private void Awake() => Init();
    
    private void Init()
    {
        if(!photonView.IsMine) return;
        
        _inventory = GetComponent<Inventory>();
        _playerStats = GetComponent<PlayerStats>();
        _playerRb = GetComponent<Rigidbody>();
        _armPos = transform.GetChild(0).GetChild(1).GetComponent<Transform>();
        
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

        _playerStates[(int)_curState].Update();
        InputKey();
        PositionUpdate();
        InputRotate();
        CameraToItemRay();
        DropItem();
        ItemPositionToArm();
        SelectInventoryInItem();
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
                CurCarryItem.gameObject.SetActive(false);
                
                //변경할 슬롯의 아이템이 있는지 확인
                if (_inventory.SelectedItem(index) != null)
                {
                    CurCarryItem = _inventory.SelectedItem(index);
                    CurCarryItem.gameObject.SetActive(true); 
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
        
        //TODO: 추후 팔 위치 조정 필요
        if (CurCarryItem.AttackItem())
        {
            //공격 아이템인지 아닌지에 따라 잡는 모션 다르게 처리하면 될듯
        }
        else
        {
            
        }
        
        CurCarryItem.transform.position = _armPos.position;
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
        
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 2f, _itemLayer))
        {
            if (hit.collider.TryGetComponent<Item>(out _item))
            {
                //해당 아이템을 누가 들고있는지 확인
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
            }

            if (hit.collider.TryGetComponent(out _popup))
            {
                _popup.hitMe = true;
            }
            
            UIManager.Instance.ItemPickObjActive(!_inventory.IsFull && !_item.IsOwned && _playerStats.CanCarry);
        }
        else
        {
            if (_popup != null)
            {
                _popup.hitMe = false;
                _popup = null;
            }
            
            UIManager.Instance.ItemPickObjActive();
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
                _playerStats.CanCarry = true;
            }
            else
            {
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
            if (CurCarryItem != item)
            { 
                CurCarryItem.gameObject.SetActive(false);
                CurCarryItem = item;
            }
        }

        if (CurCarryItem.GetHoldingType() == ItemHoldingType.ONEHANDED)
        {
            _inventory.GetItem(CurCarryItem);
        }
        
        CurCarryItem.PickUp(PhotonNetwork.LocalPlayer);
        _playerStats.IsHoldingItem(_item.GetItemWeight());
    }
  
    [PunRPC]
    public void SyncDeathRPC(bool isDeath, bool isActive, bool isEnable, bool isKinematic)
    {
        IsDeath = isDeath;
        PlayerBody.SetActive(isActive);
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
