using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace DoDoDoIt
{
    
    [System.Serializable]
    public class ObjectReplacementPair
    {
        public string objectNamePrefix; // 오브젝트 이름
        public GameObject prefab; // 교체할 프리팹
    }
    
    [System.Serializable]
    public class ObjectReplacementPairList
    {
        public ObjectReplacementPair[] pairs;

        public ObjectReplacementPairList(ObjectReplacementPair[] pairs)
        {
            this.pairs = pairs;
        }
    }
    
    public class ObjectReplacer : EditorWindow
    {
        public ObjectReplacementPair[] ObjectReplacementPairs;
        private const string prefsKey = "ObjectReplacerPrefs"; // EditorPrefs 저장용 키

       

        [MenuItem("Tools/Replace Objects")]
        public static void ShowWindow()
        {
            GetWindow<ObjectReplacer>("Replace Objects");
        }

        private void OnGUI()
        {
            // 프리팹을 할당할 수 있는 필드
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty prefabsProperty = serializedObject.FindProperty("ObjectReplacementPairs");
            EditorGUILayout.PropertyField(prefabsProperty, true);
            serializedObject.ApplyModifiedProperties();

           
            if (GUILayout.Button("Replace Objects"))
            {
                ReplaceObjects();
            }
        }
        private void OnEnable()
        {
            LoadData(); // 창이 열릴 때 데이터 불러오기
        }

        private void OnDisable()
        {
            SaveData(); // 창이 닫힐 때 데이터 저장
        }
        void ReplaceObjects()
        {
            if (ObjectReplacementPairs == null || ObjectReplacementPairs.Length == 0)
            {
                Debug.LogWarning("No replacement pairs set.");
                return;
            }

            // 씬의 모든 오브젝트 가져오기
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            // 각 ObjectReplacementPair에 대해 처리
            foreach (var pair in ObjectReplacementPairs)
            {
                if (pair.prefab == null)
                {
                    Debug.LogWarning($"Prefab not assigned for {pair.objectNamePrefix}");
                    continue;
                }

                // 씬에 있는 오브젝트 중에서 이름이 objectNamePrefix로 시작하는 오브젝트를 찾음
                foreach (GameObject obj in allObjects)
                {
                    if (obj.name.StartsWith(pair.objectNamePrefix))
                    {
                        // 기존 오브젝트의 위치, 회전, 스케일 저장
                        Vector3 position = obj.transform.position;
                        Quaternion rotation = obj.transform.rotation;
                        //Vector3 scale = obj.transform.localScale;
                        
                        // 부모 트랜스폼 저장
                        Transform parentTransform = obj.transform.parent;

                        // 프리팹 인스턴스화
                        GameObject newObject = PrefabUtility.InstantiatePrefab(pair.prefab) as GameObject;
                        if (newObject != null)
                        {
                            newObject.transform.position = position;
                            newObject.transform.rotation = rotation;
                            //newObject.transform.localScale = scale;

                            // 부모 트랜스폼 다시 설정
                            newObject.transform.SetParent(parentTransform, true);

                            // 기존 오브젝트 삭제
                            DestroyImmediate(obj);
                        }
                    }
                }
            }

            // 씬 변경 사항 저장
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            AssetDatabase.SaveAssets();
        }
        
        private void SaveData()
        {
            if (ObjectReplacementPairs == null) return;

            // ObjectReplacementPairs 배열을 JSON 문자열로 변환
            string jsonData = JsonUtility.ToJson(new ObjectReplacementPairList(ObjectReplacementPairs));

            // JSON 데이터를 EditorPrefs에 저장
            EditorPrefs.SetString(prefsKey, jsonData);
        }

        // 데이터 불러오기 (EditorPrefs 사용)
        private void LoadData()
        {
            // EditorPrefs에서 저장된 JSON 데이터 가져오기
            if (EditorPrefs.HasKey(prefsKey))
            {
                string jsonData = EditorPrefs.GetString(prefsKey);

                // JSON 데이터를 ObjectReplacementPairs 배열로 변환
                ObjectReplacementPairList pairList = JsonUtility.FromJson<ObjectReplacementPairList>(jsonData);
                if (pairList != null)
                {
                    ObjectReplacementPairs = pairList.pairs;
                }
            }
        }
        
    }
    
}