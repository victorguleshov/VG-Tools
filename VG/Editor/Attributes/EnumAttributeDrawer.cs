using System;
using UnityEditor;
using UnityEngine;

namespace VG.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(Enum), true)]
    public sealed class EnumAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                if (HasEnumFlagsAttribute())
                {
                    var intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumDisplayNames);

                    if (property.intValue != intValue) property.intValue = intValue;
                }
                else
                {
                    EditorGUI.PropertyField(position, property, label);
                }
            }

            bool HasEnumFlagsAttribute()
            {
                var fieldType = fieldInfo.FieldType;

                if (fieldType.IsArray)
                {
                    var elementType = fieldType.GetElementType();

                    return elementType != null && elementType.IsDefined(typeof(FlagsAttribute), false);
                }

                return fieldType.IsDefined(typeof(FlagsAttribute), false);
            }
        }
    }
}