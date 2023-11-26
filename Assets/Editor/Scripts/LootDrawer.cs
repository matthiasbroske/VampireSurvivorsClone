using UnityEngine;
using UnityEditor;

namespace Vampire
{
    [CustomPropertyDrawer(typeof(Loot<GameObject>))]
    public class LootDrawer : PropertyDrawer
    {
        private const float FOLDOUT_HEIGHT = 16f;
        private const float PADDING = 2f;

        private SerializedProperty item;
        private SerializedProperty dropChance;
        private SerializedProperty coinType;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            item = property.FindPropertyRelative("item");
            dropChance = property.FindPropertyRelative("dropChance");
            coinType = property.FindPropertyRelative("coinType");

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel++;

            float addY = FOLDOUT_HEIGHT;
            Rect rect = new Rect(position.x, position.y + addY, position.width, EditorGUI.GetPropertyHeight(item));
            EditorGUI.PropertyField(rect, item, new GUIContent("Item"), true);
            addY += rect.height + PADDING;
            rect = new Rect(position.x, position.y + addY, position.width, EditorGUI.GetPropertyHeight(dropChance));
            EditorGUI.PropertyField(rect, dropChance, new GUIContent("Drop Chance"), true);
            GameObject itemObject = item.objectReferenceValue as GameObject;
            if (itemObject != null && itemObject.GetComponent<Coin>() != null)
            {
                addY += rect.height + PADDING;
                rect = new Rect(position.x, position.y + addY, position.width, EditorGUI.GetPropertyHeight(SerializedPropertyType.Enum, GUIContent.none));
                EditorGUI.PropertyField(rect, coinType, new GUIContent("Coin Type"), true);
            }
            
            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            item = property.FindPropertyRelative("item");
            dropChance = property.FindPropertyRelative("dropChance");
            coinType = property.FindPropertyRelative("coinType");

            float height = FOLDOUT_HEIGHT;
            if (property.isExpanded)
            {
                height += EditorGUI.GetPropertyHeight(item);
                height += PADDING;
                height += EditorGUI.GetPropertyHeight(dropChance);
                // Add extra height if adding an additional parameter field
                GameObject itemObject = item.objectReferenceValue as GameObject;
                if (itemObject != null && itemObject.GetComponent<Coin>() != null)
                {
                    height += PADDING;
                    height += EditorGUI.GetPropertyHeight(coinType);
                }
            }

            return height;
        }
    }
}
