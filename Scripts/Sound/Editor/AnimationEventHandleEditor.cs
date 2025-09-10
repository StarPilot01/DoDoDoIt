using DoDoDoIt.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace DoDoDoIt
{
    [CustomEditor(typeof(AnimationEventHandle))]
    public class AnimationEventHandleEditor : Editor
    {

        private VisualElement _root;
        
        private SerializedProperty _stringByEventsProperty;
        
        private void FindProperties()
        {
            _stringByEventsProperty = serializedObject.FindProperty("StringByEvents");
            
        }
        
        private void Init()
        {
            _root = new VisualElement();

            var ussPath = AssetDatabase.GUIDToAssetPath("6eaf9f650f3964093bc7a14b56013395");
            var uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
            _root.styleSheets.Add(uss);
            
            var title = new Label();
            title.text = "Animation Event Handle";
            title.AddToClassList("TitleStyle");
            _root.Add(title);
            
            var group = new VisualElement();
            group.name = "GroupBox";
            group.AddToClassList("GroupBoxStyle");
            _root.Add(group);
            
            var stringByEventsField = new PropertyField();
            stringByEventsField.name = "StringByEvents";
            stringByEventsField.BindProperty(_stringByEventsProperty);
            group.Add(stringByEventsField);

            var button = new Button();
            button.text = "Copy Handler Name";
            button.AddToClassList("ButtonStyle");
            button.clicked += CopyHandlerName;
            _root.Add(button);
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            FindProperties();
            Init();
            
            return _root;
        }
        
        private void CopyHandlerName()
        {
            TextEditor te = new()
            {
                text = "OnAnimationEvent"
            };
            
            te.SelectAll();
            te.Copy();
            
            Debug.Log("OnAnimationEvent 이벤트 이름이 복사되었습니다.");
        }
    }
}
