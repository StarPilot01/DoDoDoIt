using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace DoDoDoIt
{
    [CustomEditor(typeof(Modifier_SO), true)] 
    public class Modifier_SO_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            // 기본 인스펙터 요소를 렌더링합니다.
            DrawDefaultInspector();

            // Modifier_SO의 onCompleteCallback UnityEvent를 강제로 표시합니다.
            Modifier_SO modifier = (Modifier_SO)target;

            // onCompleteCallback을 인스펙터에 표시
            SerializedProperty unityEventProp = serializedObject.FindProperty("onCompleteCallback");
            if (unityEventProp != null)
            {
                EditorGUILayout.LabelField("On Complete Callback");
                EditorGUILayout.PropertyField(unityEventProp);
            }

            // 변경 사항을 적용
            serializedObject.ApplyModifiedProperties();
        }
    }
}
