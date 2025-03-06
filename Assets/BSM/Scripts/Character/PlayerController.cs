using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private Camera _cam;
    
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

    private PState _curState = PState.IDLE;

    private bool _isGrab;
    private float _mouseX;
    private float _mouseY;
    private int _itemLayer => LayerMask.GetMask("Item");
    
    private float _sensitivity => DataManager.Instance.UserSettingData.Sensitivity;
    
    private void Awake() => Init();
    
    private void Init()
    {
        if(!photonView.IsMine) return;
        
        //TODO: 임시로 여기서 잠금 추후 PunManager에서 방 입장 시 커서 모드 변경함
        Cursor.lockState = CursorLockMode.Locked;

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
    /// 현재 아이템의 소유권자를 확인 후 위치 동기화
    /// </summary>
    private void ItemPositionToArm()
    {
        if(_item == null) return;
        if (!_item.IsOwned) return;
        if (!_item.photonView.Owner.Equals(photonView.Owner)) return;
        
        //TODO: 추후 팔 위치 조정 필요
        _item.transform.position = _armPos.position;
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
                UIManager.Instance.ItemPickObjActive(!_isGrab);
                
                if (Input.GetKeyDown(KeyCode.E))
                { 
                    _isGrab = true;
                    ItemPickUp(_item);
                    UIManager.Instance.ItemPickObjActive();
                } 
            } 
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
        if (_isGrab && Input.GetKeyDown(KeyCode.G))
        {
            _item.Drop(); 
            _playerStats.IsNotHoldingItem(_item.GetItemWeight());
            _item = null;
            _isGrab = false; 
        } 
    }

    /// <summary>
    /// 아이템 소유권 및 잡을 위치 지정
    /// </summary>
    /// <param name="item"></param>
    private void ItemPickUp(Item item)
    {
        //TODO: 한 손으로 들 수 있는 아이템인 경우 인벤토리에 넣어줘야 함
        _inventory.GetItem(item);
        item.PickUp(PhotonNetwork.LocalPlayer);
        _playerStats.IsHoldingItem(_item.GetItemWeight());
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
