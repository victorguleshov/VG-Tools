using UnityEditor;
using UnityEngine;
using VG.Attributes;

namespace VG.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerAttributeDrawer : PropertyDrawer
    {
        private bool _checked;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                if (!_checked) Warning(property);
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }

        private void Warning(SerializedProperty property)
        {
            Debug.LogWarning(string.Format(
                "Property <color=brown>{0}</color> in object <color=brown>{1}</color> is of wrong type. Expected: Int",
                property.name, property.serializedObject.targetObject));
            _checked = true;
        }
    }
}