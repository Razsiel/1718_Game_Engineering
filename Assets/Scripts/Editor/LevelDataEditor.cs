using Assets.ScriptableObjects.Levels;
using Assets.ScriptableObjects.Tiles;
using Assets.Scripts.DataStructures;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor {
    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : UnityEditor.Editor {
        private const int MinLevelSize = 2;
        private const int MaxLevelSize = 20;
        private const int ButtonSize = 35;
        private Vector2 _scrollPosition = Vector2.zero;

        private UnityEditor.Editor _levelPreviewer;
        private GameObject _levelObject;

        public override void OnInspectorGUI() {
            var level = (LevelData) target;
            if (level == null) {
                return;
            }
            if (level.GridMap == null) {
                level.GridMap = new GridMap<TileData>(10, 10);
            }
            var map = level.GridMap;

            var newWidth = EditorGUILayout.IntSlider("Width", map.Width, MinLevelSize, MaxLevelSize);
            var newHeight = EditorGUILayout.IntSlider("Height", map.Height, MinLevelSize, MaxLevelSize);

            if (map.Width != newWidth || map.Height != newHeight) {
                level.GridMap = new GridMap<TileData>(newWidth, newHeight);
            }

            EditorGUILayout.LabelField("Map");
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.BeginVertical();
            for (int y = 0; y < map.Height; y++) {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < map.Width; x++) {
                    GUILayout.Button($"{map[x, y] ?? null}", GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize));
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        
        public override bool HasPreviewGUI() {
            var map = ((LevelData) target).GridMap;
            return map != null && map.Count > 0;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background) {
            var map = ((LevelData) target).GridMap;
            if (map != null && map.Count > 0) {
                if (_levelPreviewer == null || _levelObject == null) {
                    _levelObject = CreateLevelPreviewObject(map);
                    _levelPreviewer = CreateEditor(_levelObject);
                }

                _levelPreviewer.OnPreviewGUI(r, background);
            }
        }

        private GameObject CreateLevelPreviewObject(GridMap<TileData> gridMap) {
            var root = EditorUtility.CreateGameObjectWithHideFlags("Level Preview Object", HideFlags.HideAndDontSave);

            foreach (var entry in gridMap) {
                var tile = entry.Value;
                if (tile == null) {
                    continue;
                }
                var tileObject =
                    EditorUtility.CreateGameObjectWithHideFlags(tile.name, HideFlags.HideAndDontSave,
                                                                typeof(MeshFilter), typeof(MeshRenderer));
                var meshFilter = tileObject.GetComponent<MeshFilter>();
                if (meshFilter != null) {
                    meshFilter.sharedMesh = tile.TileMesh;
                }
                var meshRenderer = tileObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null) {
                    meshRenderer.sharedMaterial = tile.TileMaterial;
                }
            }

            return root;
        }
    }
}