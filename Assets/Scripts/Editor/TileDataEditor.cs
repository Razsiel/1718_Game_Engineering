using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Tiles;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor
{
    [CustomEditor(typeof(TileData), true)]
    public class TileDataEditor : UnityEditor.Editor
    {
        TileData _instance;
        private GameObject _previewObject;
        private UnityEditor.Editor _preview;

        public void OnEnable()
        {
            _instance = target as TileData;
        }

        public void OnDisable()
        {
            Cleanup();
        }

        /*
        public override void OnInspectorGUI()
        {
            if (_instance == null)
                return;

            this.DrawDefaultInspector();

            if (GUILayout.Button("(Re)create preview"))
            {
                Cleanup();
                _previewObject = CreatePreviewFromData(_instance);
                _preview = CreateEditor(_previewObject);
            }
        }

        private GameObject CreatePreviewFromData(TileData data) {
            var root = EditorUtility.CreateGameObjectWithHideFlags("Preview Object", HideFlags.HideAndDontSave);
            var tile = data.GenerateGameObject(root, 0, 0, true);
            tile.transform.localScale /= 32;
            return tile;
        }*/

        public override bool HasPreviewGUI()
        {
            return _instance != null;
        }

        public void Cleanup() {
            DestroyImmediate(_previewObject);
            DestroyImmediate(_preview);
        }
    }
}
