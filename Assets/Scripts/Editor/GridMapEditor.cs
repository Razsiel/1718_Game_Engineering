using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ScriptableObjects.Grids;
using Assets.ScriptableObjects.Tiles;
using Assets.Scripts.DataStructures;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor {
    [CustomEditor(typeof(GridMapData))]
    public class GridMapEditor : UnityEditor.Editor {
        GridMapData _instance;
        PropertyField[] _fields;

        private GameObject _previewObject;
        private UnityEditor.Editor _mapEditor;

        public void OnEnable() {
            _instance = target as GridMapData;
            _fields = ExposeProperties.GetProperties(_instance);
        }

        public void OnDisable() {
            DestroyImmediate(_previewObject);
            DestroyImmediate(_mapEditor);
        }

        public override void OnInspectorGUI() {
            if (_instance == null)
                return;

            this.DrawDefaultInspector();
            ExposeProperties.Expose(_fields);

            // draw custom grid editor here
            if (GUILayout.Button("Recreate preview")) {
                _previewObject = CreatePreviewObjectFromGridMap(_instance);
                _mapEditor = UnityEditor.Editor.CreateEditor(_previewObject);
            }
        }

        public override bool HasPreviewGUI() {
            return _instance?.Count > 0;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background) {
            Debug.Log("OnPreviewGUI");
            
            if (_mapEditor != null && _previewObject != null) {
                _mapEditor.OnPreviewGUI(r, background);
            }
        }

        private GameObject CreatePreviewObjectFromGridMap(GridMapData mapData) {
            Debug.Log("Creating preview object");
            DestroyImmediate(GameObject.Find("Level Preview Object")); // clear previous preview
            var root = EditorUtility.CreateGameObjectWithHideFlags("Level Preview Object", HideFlags.HideAndDontSave);
            

            Debug.Log($"Map: {mapData.Width} x {mapData.Height}");

            for (int y = 0; y < mapData.Height; y++) {
                for (int x = 0; x < mapData.Width; x++) {
                    var tile = mapData.DefaultTile;
                    Debug.Log(tile);
                    if (tile == null) {
                        continue;
                    }
                    Debug.Log(tile.TileMesh);
                    var tileObject =
                        EditorUtility.CreateGameObjectWithHideFlags($"Tile ({x},{y})",
                                                                    HideFlags.HideAndDontSave,
                                                                    typeof(MeshFilter),
                                                                    typeof(MeshRenderer));
                    var meshFilter = tileObject.GetComponent<MeshFilter>();
                    if (meshFilter != null) {
                        meshFilter.mesh = tile.TileMesh;
                    }
                    var meshRenderer = tileObject.GetComponent<MeshRenderer>();
                    if (meshRenderer != null) {
                        meshRenderer.sharedMaterial = tile.TileMaterial;
                    }
                    var transform = tileObject.transform;
                    transform.parent = root.transform;
                    transform.position = new Vector3(x, 0, y);
                }
            }

            return root;
        }
    }

    //[CustomPropertyDrawer(typeof(GridMapData))]
    public class GridMapDrawer : PropertyDrawer {
        private Vector2 _scrollPosition = Vector2.zero;
        public readonly int ButtonSize = 50;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.PrefixLabel(position, label);

            var width = property.FindPropertyRelative(nameof(GridMapData.Width));
            var height = property.FindPropertyRelative(nameof(GridMapData.Height));

            //EditorGUILayout.BeginVertical();
            //EditorGUI.indentLevel += 1;
            //EditorGUILayout.IntSlider(width, 2, 20);
            //EditorGUILayout.IntSlider(height, 2, 20);
            //EditorGUILayout.PrefixLabel(new GUIContent("Layout"));
            //EditorGUI.indentLevel -= 1;
            //EditorGUILayout.EndVertical();

            //int controlId = GUIUtility.GetControlID(FocusType.Passive) + 100;

            //_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.MaxHeight(500));
            //EditorGUILayout.BeginVertical();
            //for (int y = 0; y < height.intValue; y++) {
            //    EditorGUILayout.BeginHorizontal();
            //    for (int x = 0; x < width.intValue; x++) {
            //        var button = GUILayout.Button($"", GUILayout.Width(ButtonSize), GUILayout.Height(ButtonSize));
            //        if (button) {
            //            EditorGUIUtility.ShowObjectPicker<TileData>(null, false, "", controlId);
            //        }
            //    }
            //    EditorGUILayout.EndHorizontal();
            //}
            //EditorGUILayout.EndVertical();
            //EditorGUILayout.EndScrollView();

            //string commandName = Event.current.commandName;
            //if (commandName == "ObjectSelectorUpdated" || commandName == "ObjectSelectorClosed") {
            //    var selectedObject = EditorGUIUtility.GetObjectPickerObject();
            //    if (selectedObject is TileData) {
            //        //todo: save to mapData
            //        Debug.Log(selectedObject);
            //    }
            //}
        }
    }
}