﻿using Assets.Data.Grids;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Editor {
    [CustomEditor(typeof(GridMapData))]
    public class GridMapEditor : UnityEditor.Editor {
        GridMapData _instance;

        private GameObject _previewObject;
        private UnityEditor.Editor _mapEditor;

        public void OnEnable() {
            _instance = target as GridMapData;
        }

        public void OnDisable() {
            DestroyImmediate(_previewObject);
            DestroyImmediate(_mapEditor);
        }

        public override void OnInspectorGUI() {
            if (_instance == null)
                return;

            this.DrawDefaultInspector();

            if (GUILayout.Button("Recalculate")) {
                _instance.RecalculateGrid();
                CreatePreview();
            }

            // draw custom grid editor here
            if (GUILayout.Button("Refresh preview")) {
                CreatePreview();
            }

            if (_previewObject == null) {
                CreatePreview();
            }

            /*
            for (int row = 0; row < _instance.Height; row++) {
                GUILayout.BeginHorizontal();
                for (int column = 0; column < _instance.Width; column++) {
                    GUILayout.BeginVertical();

                    if (GUILayout.Button($"{row}, {column}", GUILayout.ExpandWidth(true),
                                         GUILayout.ExpandHeight(true))) {
                        TileDataEditor.ShowWindow(_instance[row, column]);
                    }

                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            */
        }

        private void CreatePreview() {
            DestroyImmediate(_previewObject); // clear previous preview
            DestroyImmediate(_mapEditor);
            _previewObject = CreatePreviewObjectFromGridMap(_instance);
            _mapEditor = UnityEditor.Editor.CreateEditor(_previewObject);
        }

        public override bool HasPreviewGUI() {
            return _instance?.Count > 0;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background) {
            if (_mapEditor != null && _previewObject != null) {
                _mapEditor.OnPreviewGUI(r, background);
            }
        }

        private GameObject CreatePreviewObjectFromGridMap(GridMapData mapData) {
            var root = EditorUtility.CreateGameObjectWithHideFlags("Level Preview Object", HideFlags.HideAndDontSave);

            for (int y = 0; y < mapData.Height; y++) {
                for (int x = 0; x < mapData.Width; x++) {
                    var tileConfiguration = mapData[x, y];
                    if (tileConfiguration == null) {
                        continue;
                    }

                    var tileObject = tileConfiguration.GenerateGameObject(root, x, y, true);

                    var transform = tileObject.transform;
                    transform.position = GridHelper.GridToWorldPosition(mapData, new Vector2Int(x, y));
                    transform.localScale /= 32;
                }
            }

            return root;
        }
    }
}