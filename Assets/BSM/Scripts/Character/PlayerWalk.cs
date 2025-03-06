using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : PlayerState
{
    
    public PlayerWalk(PlayerController controller) : base(controller) {}

    public override void Enter()
    {
        if (_controller.PlayerStats.Stamina < 100f && _controller.RecoverStaminaCo == null)
        {
            _controller.RecoverStaminaCo = _controller.StartCoroutine(RecoverStaminaRoutine());
        }
    }

    public override void Update()
    {
        if (_controller.MoveDir == Vector3.zero)
        {
            _controller.ChangeState(PState.IDLE);
        }
        else if (_controller.MoveDir != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            _controller.ChangeState(PState.RUN);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _controller.ChangeState(PState.JUMP);
        }
    }
    
    public override void FixedUpdate()
    {
        Vector3 dir = _controller.transform.TransformDirection(_controller.MoveDir);
        
        _controller.PlayerRb.MovePosition(_controller.transform.position + dir.normalized * _controller.PlayerStats.WalkSpeed * Time.fixedDeltaTime);   
    }

    public override void Exit()
    {
        //회복 코루틴이 != null
        if (_controller.RecoverStaminaCo != null)
        {
            _controller.StopCoroutine(_controller.RecoverStaminaCo);
            _controller.RecoverStaminaCo = null;
        }
    }


}
