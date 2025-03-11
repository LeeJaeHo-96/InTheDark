using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _inventoryPoint;
    
    public PlayerStats PlayerStats => _playerStats;
    public Rigidbody PlayerRb => _playerRb;
    public Coroutine ConsumeStaminaCo;
    public Coroutine RecoverStaminaCo;
    public PState CurState => _curState;
    
    public Vector3 MoveDir = Vector3.zero;
    
    private PlayerState[] _playerStates = new PlayerState[(int)PState.SIZE];
    private PlayerStats _playerStats;
    private Rigidbody _playerRb;
    private Transform _eyePos;
    private Transform _armPos;

    private Inventory _inventory;
    private Item _item;
    public Item _curCarryItem;
    
    
    private PState _curState = PState.IDLE;

    private int _curInventoryIndex = 0;
    private float _mouseX;
    private float _mouseY;
    private int _itemLayer => LayerMask.GetMask("Item");

    private int _tempLayerIndex;
    private int _tempPlayerIndex;
    
    private float _sensitivity => DataManager.Instance.UserSettingData.Sensitivity;
    
    private void Awake() => Init();
    
    private void Init()
    {
        if(!photonView.IsMine) return;
        
        //TODO: 임시로 여기서 잠금 추후 PunManager에서 방 입장 시 커서 모드 변경함
        Cursor.lockState = CursorLockMode.Locked;
        
        //TODO: 테스트씬용 Layer 무시 코드 -> GameManager에서 담당할 예정
        _tempLayerIndex = LayerMask.NameToLayer("Item");
        _tempPlayerIndex = LayerMask.NameToLayer("Player");
        Physics.IgnoreLayerCollision(_tempLayerIndex,_tempLayerIndex); 
        Physics.IgnoreLayerCollision(_tempLayerIndex,_tempPlayerIndex); 
        //-----------------
        
        _inventory = GetComponent<Inventory>();
        _playerStats = GetComponent<PlayerStats>();
        _playerRb = GetComponent<Rigidbody>();
        _eyePos = transform.GetChild(0).GetChild(0).GetComponent<Transform>();
        _armPos = transform.GetChild(0).GetChild(1).GetComponent<Transform>();
        
        _playerStates[(int)PState.IDLE] = new PlayerIdle(this);
        _playerStates[(int)PState.WALK] = new PlayerWalk(this);
        _playerStates[(int)PState.RUN] = new PlayerRun(this);
        _playerStates[(int)PState.JUMP] = new PlayerJump(this);
         
    }

    private void Start()
    {

        
        if (!photonView.IsMine)
        {
            _cam.gameObject.SetActive(false);
            return;
        }
        _cam.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        
        _playerStates[(int)_curState].Enter();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        _playerStates[(int)_curState].Update();
        InputKey();
        InputRotate();
        CameraToItemRay();
        DropItem();
        ItemPositionToArm();
        SelectInventoryInItem();

        if (Input.GetMouseButtonDown(0))
        {
            UseItem();
        }
        
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
        if (_curCarryItem != null)
        {
            if (_curCarryItem.GetHoldingType() == ItemHoldingType.ONEHANDED)
            {
                _curCarryItem.gameObject.SetActive(false);
                
                //변경할 슬롯의 아이템이 있는지 확인
                if (_inventory.SelectedItem(index) != null)
                {
                    _curCarryItem = _inventory.SelectedItem(index);
                    _curCarryItem.gameObject.SetActive(true); 
                }
                else
                {
                    _curCarryItem = null;
                }
            } 
        }
        else
        {
            if (_inventory.SelectedItem(index) != null)
            {
                _curCarryItem = _inventory.SelectedItem(index);
                _curCarryItem.gameObject.SetActive(true); 
            }
        }  
    }
    
    
    /// <summary>
    /// 현재 아이템의 소유권자를 확인 후 위치 동기화
    /// </summary>
    private void ItemPositionToArm()
    {
        if(_curCarryItem == null) return;
        if (!_curCarryItem.IsOwned) return;
        if (!_curCarryItem.photonView.Owner.Equals(photonView.Owner)) return;
        
        //TODO: 추후 팔 위치 조정 필요
        if (_curCarryItem.AttackItem())
        {
            //공격 아이템인지 아닌지에 따라 잡는 모션 다르게 처리하면 될듯
        }
        else
        {
            
        }
        
        _curCarryItem.transform.position = _armPos.position;
        _curCarryItem.transform.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0);
    }
    
    /// <summary>
    /// 마우스 회전 입력
    /// </summary>
    private void InputRotate()
    {
        _mouseX += Input.GetAxisRaw("Mouse X") * _sensitivity * Time.deltaTime;
        _mouseY += Input.GetAxisRaw("Mouse Y");

        _mouseY = Mathf.Clamp(_mouseY, -90f, 90f);
        
        //캐릭터 몸체 회전
        transform.rotation = Quaternion.Euler(0, _mouseX, 0f);
        photonView.RPC(nameof(SyncCharacterRotate), RpcTarget.AllBuffered, _mouseX);
        //카메라 상하/좌우 회전
        _cam.transform.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0f);
         
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
        Ray ray = new Ray(_cam.transform.position, _cam.transform.forward);
        
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 2f, _itemLayer))
        {  
            _item = hit.collider.GetComponent<Item>();

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

            UIManager.Instance.ItemPickObjActive(!_inventory.IsFull && !_item.IsOwned && _playerStats.CanCarry);
        }
        else
        {
            UIManager.Instance.ItemPickObjActive();
        }
    }
    
    /// <summary>
    /// 들고있는 아이템 드랍
    /// </summary>
    private void DropItem()
    {
        if (_curCarryItem != null && Input.GetKeyDown(KeyCode.G))
        {
            if (_curCarryItem.GetHoldingType() == ItemHoldingType.TWOHANDED)
            {
                _playerStats.CanCarry = true;
            }
            else
            {
                _inventory.DropItem(_curInventoryIndex);
            }
             
            _curCarryItem.Drop(); 
            _playerStats.IsNotHoldingItem(_item.GetItemWeight());
            _curCarryItem = null;
        } 
    }

    /// <summary>
    /// 아이템 소유권 및 잡을 위치 지정
    /// </summary>
    /// <param name="item"></param>
    private void ItemPickUp(Item item)
    {
        if (_curCarryItem == null)
        {
            _curCarryItem = item;
        }
        else
        {
            if (_curCarryItem != item)
            { 
                _curCarryItem.gameObject.SetActive(false);
                _curCarryItem = item;
            }
        }

        if (_curCarryItem.GetHoldingType() == ItemHoldingType.ONEHANDED)
        {
            _inventory.GetItem(_curCarryItem);
        }
        
        _curCarryItem.PickUp(PhotonNetwork.LocalPlayer);
        _playerStats.IsHoldingItem(_item.GetItemWeight());
    }

    private void UseItem()
    {
        if (_curCarryItem == null) return;
        
        _curCarryItem.ItemUse();
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
