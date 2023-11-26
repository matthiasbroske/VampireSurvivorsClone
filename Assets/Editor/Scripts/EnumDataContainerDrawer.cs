using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace Vampire
{
    [CustomPropertyDrawer(typeof(EnumDataContainer<,>))]
    public class EnumDataContainerDrawer : PropertyDrawer
    {
        private const float FOLDOUT_HEIGHT = 16f;
        private const float PADDING = 2f;

        private SerializedProperty content;
        private SerializedProperty enumType;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            content = property.FindPropertyRelative("content");
            enumType = property.FindPropertyRelative("enumType");

            EditorGUI.BeginProperty(position, label, property);
            Rect foldoutRect = new Rect(position.x, position.y, position.width, FOLDOUT_HEIGHT);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);
            
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                float addY = FOLDOUT_HEIGHT;
                for (int i = 0; i < content.arraySize; i++)
                {
                    SerializedProperty value = content.GetArrayElementAtIndex(i);
                    Rect rect = new Rect(position.x, position.y + addY, position.width, EditorGUI.GetPropertyHeight(value));
                    addY += rect.height + PADDING;
                    EditorGUI.PropertyField(rect, value, new GUIContent(Regex.Replace(enumType.enumNames[i], @"([a-zA-Z]+|[0-9]+[0-9/\.]+[0-9]+)", "$& ")), true);
                }
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            content = property.FindPropertyRelative("content");
            enumType = property.FindPropertyRelative("enumType");

            float height = FOLDOUT_HEIGHT;
            if (property.isExpanded)
            {
                if (content.arraySize != enumType.enumNames.Length)
                {
                    content.arraySize = enumType.enumNames.Length;
                }
                for (int i = 0; i < content.arraySize; i++)
                {
                    height += EditorGUI.GetPropertyHeight(content.GetArrayElementAtIndex(i));
                    height += PADDING;
                }
            }

            return height;
        }
    }

    [CustomPropertyDrawer(typeof(EnumDataContainer<,,>))]
    public class EnumDataContainerDrawerLarge : PropertyDrawer
    {
        private const float FOLDOUT_HEIGHT = 16f;
        private const float PADDING = 2f;

        private SerializedProperty content1;
        private SerializedProperty content2;
        private SerializedProperty enumType;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            content1 = property.FindPropertyRelative("content1");
            content2 = property.FindPropertyRelative("content2");
            enumType = property.FindPropertyRelative("enumType");

            EditorGUI.BeginProperty(position, label, property);
            Rect foldoutRect = new Rect(position.x, position.y, position.width, FOLDOUT_HEIGHT);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);
            
            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                float addY = FOLDOUT_HEIGHT;
                for (int i = 0; i < content1.arraySize; i++)
                {
                    SerializedProperty value1 = content1.GetArrayElementAtIndex(i);
                    SerializedProperty value2 = content2.GetArrayElementAtIndex(i);
                    Rect rect1 = new Rect(position.x, position.y + addY, position.width/2, EditorGUI.GetPropertyHeight(value1));
                    Rect rect2 = new Rect(position.x+position.width/2, position.y + addY, position.width/2, EditorGUI.GetPropertyHeight(value2));
                    addY += rect1.height + PADDING;
                    EditorGUI.PropertyField(rect1, value1, new GUIContent(Regex.Replace(enumType.enumNames[i], @"([a-zA-Z]+|[0-9]+[0-9/\.]+[0-9]+)", "$& ")), true);
                    EditorGUI.PropertyField(rect2, value2, new GUIContent(""));
                }
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            content1 = property.FindPropertyRelative("content1");
            content2 = property.FindPropertyRelative("content2");
            enumType = property.FindPropertyRelative("enumType");

            float height = FOLDOUT_HEIGHT;
            if (property.isExpanded)
            {
                if (content1.arraySize != enumType.enumNames.Length)
                {
                    content1.arraySize = enumType.enumNames.Length;
                }
                for (int i = 0; i < content1.arraySize; i++)
                {
                    height += EditorGUI.GetPropertyHeight(content1.GetArrayElementAtIndex(i));
                    height += PADDING;
                }
            }

            return height;
        }
    }
}
