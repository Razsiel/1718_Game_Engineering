using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Tiles;
using Assets.Scripts.DataStructures;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Editor {
    public class TileDataEditor : EditorWindow {
        private static TileConfiguration _instance;
        private GameObject _previewObject;
        private UnityEditor.Editor _preview;

        private bool decorationsFolded;

        public static void ShowWindow(TileConfiguration tileConfig) {
            _instance = tileConfig;
            GetWindow<TileDataEditor>("Tile Config");
        }

        void OnGUI() {
            Assert.IsNotNull(_instance);
            if (_preview == null) {
                _previewObject = EditorUtility.CreateGameObjectWithHideFlags("tile preview", HideFlags.HideAndDontSave);
                _instance.GenerateGameObject(_previewObject, 0, 0, true);
                _preview = UnityEditor.Editor.CreateEditor(_previewObject);
            }

            GUILayout.BeginHorizontal();

            // preview
            GUILayout.BeginVertical();
            //_preview?.OnPreviewGUI(GUILayoutUtility.GetAspectRect(2f), EditorStyles.whiteLabel);
            GUILayout.EndVertical();

            // draw properties here
            GUILayout.BeginVertical();
            // tile
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Tile");
            _instance.Tile = (TileData) EditorGUILayout.ObjectField(_instance.Tile, typeof(TileData), false);
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Add New"))
            {
                var decConfig = new DecorationConfiguration();
                _instance.DecorationConfigs.Add(decConfig);
            }
            decorationsFolded = EditorGUILayout.Foldout(!decorationsFolded, "Decorations");
            if (decorationsFolded) {
                foreach (var decorationConfiguration in _instance.DecorationConfigs)
                {
                    decorationConfiguration.DecorationData =
                        (TileDecorationData)EditorGUILayout.ObjectField(decorationConfiguration.DecorationData,
                                                                        typeof(TileDecorationData), false);
                }
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        public void OnDestroy() {
            Cleanup();
        }

        public void Cleanup() {
            DestroyImmediate(_previewObject);
            DestroyImmediate(_preview);
        }
    }
}