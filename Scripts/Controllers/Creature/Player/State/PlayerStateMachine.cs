using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DoDoDoIt
{
    public class PlayerStateMachine
    {
        private PlayerController _player;
        public Define.EPlayerState CurrentStateEnum { get; private set; }

        private IPlayerState[] _states = new IPlayerState[(int)Define.EPlayerState.Count];

        public void Init(PlayerController player)
        {
            _states[(int)Define.EPlayerState.Idle] = new PlayerIdleState();
            _states[(int)Define.EPlayerState.Running] = new PlayerRunningState();
            _states[(int)Define.EPlayerState.Jump] = new PlayerJumpState();
            _states[(int)Define.EPlayerState.Falling] = new PlayerFallingState();
            _states[(int)Define.EPlayerState.EnterRightCurve] = new PlayerEnterRightCurveState();
            _states[(int)Define.EPlayerState.EnterLeftCurve] = new PlayerEnterLeftCurveState();
            _states[(int)Define.EPlayerState.EnterGrappling] = new PlayerEnterGrapplingState();
            _states[(int)Define.EPlayerState.EnterRotateGrappling] = new PlayerEnterRotateGrapplingState();
            _states[(int)Define.EPlayerState.Grappling] = new PlayerGrapplingState();
            _states[(int)Define.EPlayerState.RotateGrappling] = new PlayerRotateGrapplingState();
            _states[(int)Define.EPlayerState.ExitGrappling] = new PlayerExitGrapplingState();
            _states[(int)Define.EPlayerState.ExitRotateGrappling] = new PlayerExitRotateGrapplingState();
            _states[(int)Define.EPlayerState.Dead] = new PlayerDeadState();
            _states[(int)Define.EPlayerState.EndingSceneRunning] = new PlayerEndingSceneRunningState();

            _player = player;
        }

        public void TransitionTo(Define.EPlayerState nextState)
        {
            GetState(CurrentStateEnum).ExitState();

            CurrentStateEnum = nextState;

            GetState(CurrentStateEnum).EnterState(_player);
        }

        public void Update()
        {
            GetState(CurrentStateEnum).Update();
        }

        public void FixedUpdate()
        {
            GetState(CurrentStateEnum).FixedUpdate();
        }

        public IPlayerState GetCurrentState()
        {
            return GetState(CurrentStateEnum);
        }

        private IPlayerState GetState(Define.EPlayerState state)
        {
            return _states[(int)state];
        }
    }

}