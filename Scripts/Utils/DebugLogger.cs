using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DoDoDoIt
{
    public static class DebugLogger
    {
        [Conditional("UNITY_EDITOR")]
        public static void Log(string message, Define.DebugLogColor color)
        {
            string colorStr = color.ToString();
             
            if (colorStr.Equals("yellow") || colorStr.Equals("red"))
            {
                colorStr = "white";
            }
            
            
            
            Debug.Log(FormatMessageWithColor(message,colorStr));
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(string message)
        {
            
            Debug.LogWarning(FormatMessageWithColor(message, "yellow"));
        }

        [Conditional("UNITY_EDITOR")]
        public static void LogError(string message)
        {
            Debug.LogError(FormatMessageWithColor(message, "red"));
        }
        
        
        /*
           지원되는 색상들 (아마도?)
           white
           black
           red
           green
           blue
           yellow
           cyan
           magenta
         */
        private static string FormatMessageWithColor(string message, string color)
        {
            return $"<color={color}>{message}</color>";
        }
    }
}
