using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoDoDoIt
{
    public class Define
    {
        public enum EObjectType
        {
            Player,
            Monster,
            Boss,
            Item,
            Hazard
        }

        public enum EMonsterType
        {
            Ghost
        }

        public enum EItemType
        {
            Soul,
            Coin
        }

        public enum EGhostType
        {
            Speed,
            Heal,
            Magnet
        }
        
        public enum Scene
        {
            Unknown,
            TitleScene,
            LobbyScene,
            GameScene,
            EndingScene,
        }
    
        public enum UIEvent
        {
            Click,
            Preseed,
            PointerDown,
            PointerUp,
            BeginDrag,
            Drag,
            EndDrag,
        }

        public enum EPlayerState
        {
            Idle,
            Running,
            Jump,
            Falling,
            EnterRightCurve,
            EnterLeftCurve,
            EnterGrappling,
            EnterRotateGrappling,
            Grappling,
            RotateGrappling,
            ExitGrappling,
            ExitRotateGrappling,
            Dead,
            EndingSceneRunning,
            Count
        }

        //public enum EObstacleType
        //{
        //    DamagingObstacle,
        //}

        public enum EHazardObjectType
        {
            Obstacle,
            Projectile,
            Trap
        }

        //public enum ENonDamagingObstacleType
        //{
        //
        //}

        public enum DebugLogColor
        {
            white,
            black,
            red,
            green,
            blue,
            yellow,
            cyan,
            magenta
        }
    
    
    }

}
