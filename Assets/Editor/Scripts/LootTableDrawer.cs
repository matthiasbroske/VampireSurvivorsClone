// using UnityEngine;
// using UnityEditor;

// namespace Vampire
// {
//     [CustomPropertyDrawer(typeof(LootTable<>))]
//     public class LootTableDrawer : PropertyDrawer
//     {
//         private const float FOLDOUT_HEIGHT = 16f;

//         private SerializedProperty lootTable;

//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             EditorGUI.BeginProperty(position, label, property);
//             Rect foldoutRect = new Rect(position.x, position.y, position.width, FOLDOUT_HEIGHT);
//             property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);

//             if (property.isExpanded)
//             {
//                 EditorGUI.indentLevel++;
//                 float addY = FOLDOUT_HEIGHT;
//                 Rect rectt = new Rect(position.x, position.y + addY, position.width, FOLDOUT_HEIGHT);
//                 EditorGUI.PropertyField(rectt, lootTable.FindPropertyRelative("Array.size"));
//                 for (int i = 0; i < lootTable.arraySize; i++)
//                 {
//                     SerializedProperty loot = lootTable.GetArrayElementAtIndex(i);
//                     Rect rect = new Rect(position.x, position.y + addY, position.width, EditorGUI.GetPropertyHeight(loot));
//                     addY += rect.height;
//                     EditorGUI.PropertyField(rect, loot, new GUIContent("Loot"), true);
//                 }
//                 EditorGUI.indentLevel--;
//             }
//             EditorGUI.EndProperty();
//         }

//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             if (lootTable == null)
//                 lootTable = property.FindPropertyRelative("lootTable");

//             float height = FOLDOUT_HEIGHT;
//             if (property.isExpanded)
//             {
//                 for (int i = 0; i < lootTable.arraySize; i++)
//                 {
//                     height += EditorGUI.GetPropertyHeight(lootTable.GetArrayElementAtIndex(i));
//                 }
//             }

//             return height;
//         }
//     }
// }
