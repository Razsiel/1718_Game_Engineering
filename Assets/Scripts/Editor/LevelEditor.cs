using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Editor {
    public class LevelEditor : EditorWindow {

        private static LevelEditor _instance;

        private GameObject cam = null;
        private Camera Camera {
            get {
                if (cam == null) {
                    cam = new GameObject("Editor Cam") {
                        hideFlags = HideFlags.HideAndDontSave
                    };
                    cam.transform.position = new Vector3(0, 0, -5f);
                    cam.AddComponent<Camera>();
                }
                return cam.GetComponent<Camera>();
            }
        }

        [MenuItem("Window/Progranimals/LevelEditor")]
        public static void ShowWindow() {
            //Show existing window instance. If one doesn't exist, make one.
            _instance = EditorWindow.GetWindow<LevelEditor>();
        }

        void OnGUI() {
            Handles.DrawCamera(new Rect(0, 0, _instance.position.width, _instance.position.height), Camera, DrawCameraMode.TexturedWire);
        }

        void OnDestroy() {
            DestroyImmediate(cam);
        }
    }
}
