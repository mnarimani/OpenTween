using System;
using UnityEditor;
using UnityEngine;

namespace OpenTween
{
    [CustomPropertyDrawer(typeof(OpenAnimation.Anim))]
    public class OpenAnimationEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty method = property.FindPropertyRelative("_method");
            SerializedProperty target = property.FindPropertyRelative("_parameters");

            int index = Array.IndexOf(OpenAnimation.MethodList, method.stringValue);
            if (index < 0)
                index = 0;

            var methodRect = position;
            methodRect.height = EditorGUI.GetPropertyHeight(method);
            
            index = EditorGUI.Popup(methodRect, "Method", index, OpenAnimation.MethodList);
            method.stringValue = OpenAnimation.MethodList[index];

            position.y += EditorGUI.GetPropertyHeight(method);
            position.height = EditorGUI.GetPropertyHeight(target);
            
            Type targetType = OpenAnimation.GetTargetType(method.stringValue);
            if (targetType == null)
            {
                EditorGUI.LabelField(position, $"Unknown target for method {method.stringValue}");
            }
            else
            {
                var realPropertyType = GetRealTypeFromTypename(target.managedReferenceFullTypename);
                if (realPropertyType == null || realPropertyType != targetType)
                {
                    object value = Activator.CreateInstance(targetType);
                    target.managedReferenceValue = value;
                }

                EditorGUI.PropertyField(position, target, new GUIContent("Parameters"), true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty method = property.FindPropertyRelative("_method");
            SerializedProperty target = property.FindPropertyRelative("_parameters");
            return EditorGUI.GetPropertyHeight(method) + EditorGUI.GetPropertyHeight(target, true);
        }


        private static Type GetRealTypeFromTypename(string stringType)
        {
            (string assemblyName, string className) = GetSplitNamesFromTypename(stringType);
            var realType = Type.GetType($"{className}, {assemblyName}");
            return realType;
        }

        private static (string AssemblyName, string ClassName) GetSplitNamesFromTypename(string typename)
        {
            if (string.IsNullOrEmpty(typename))
                return ("", "");

            var typeSplitString = typename.Split(char.Parse(" "));
            var typeClassName = typeSplitString[1];
            var typeAssemblyName = typeSplitString[0];
            return (typeAssemblyName, typeClassName);
        }
    }
}