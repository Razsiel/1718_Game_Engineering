using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Levels;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(LevelData.AllowedCommand))]
    public class AllowedCommandEditor : PropertyDrawer
    {
        private LevelData.AllowedCommand _target;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty commandName =
                property.FindPropertyRelative(nameof(LevelData.AllowedCommand.CommandType));
            SerializedProperty isAllowedProp =
                property.FindPropertyRelative(nameof(LevelData.AllowedCommand.IsAllowed));
            int indent = EditorGUI.indentLevel;
            isAllowedProp.boolValue =
                EditorGUI.Toggle(position, ((CommandEnum)commandName.enumValueIndex).ToString(), isAllowedProp.boolValue);
            EditorGUI.indentLevel = indent;
        }
    }
}
