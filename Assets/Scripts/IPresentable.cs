using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public interface IPresentable {
        List<PresentableConfiguration> PresentableConfigurations { get; }
        GameObject Present(Transform parent, bool hideInHierarchy = false, string name = "");
    }

    [Serializable]
    public class PresentableConfiguration {
        [SerializeField] public Mesh Mesh;
        [SerializeField] public Material Material;
        [SerializeField] public bool IsPlayerColored;
        [SerializeField] public Vector3 LocalPosition;
        [SerializeField] public Vector3 LocalRotation;
        [SerializeField] public Vector3 LocalScale = new Vector3(1, 1, 1);
    }

    public class PresentablePresenter : BasePresenter {
        public static GameObject Present(IPresentable presentable, GameObject parent, bool hideInHierarchy = false)
        {
            return Present(presentable, parent.transform, hideInHierarchy);
        }

        public static GameObject Present(IPresentable presentable, Transform parent, bool hideInHierarchy = false, string name = "")
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = presentable.GetType().Name.Replace("Data", "");
            }
            var gameObject = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer))
            {
                hideFlags = hideInHierarchy ? HideFlags.HideAndDontSave : HideFlags.NotEditable
            };
            gameObject.transform.parent = parent.transform;
            
            foreach (var presentableConfiguration in presentable.PresentableConfigurations) {
                var meshedGameObject = PresentMeshedGameObject(presentableConfiguration.Mesh, presentableConfiguration.Material, gameObject,
                                        hideInHierarchy, $"RenderConfiguration");
                var meshedTransform = meshedGameObject.transform;
                meshedTransform.localPosition = presentableConfiguration.LocalPosition;
                meshedTransform.localRotation = Quaternion.Euler(presentableConfiguration.LocalRotation);
                meshedTransform.localScale = presentableConfiguration.LocalScale;

                if (presentableConfiguration.IsPlayerColored) {
                    var colorBehaviour = gameObject.AddComponent<PlayerGoalMaterialBehaviour>();
                    colorBehaviour.GoalToColor = meshedGameObject;
                }
            }

            return gameObject;
        }
    }

    public class BasePresenter {
        public static GameObject PresentMeshedGameObject(Mesh mesh, Material material, GameObject parent,
                                                         bool hideInHierarchy = false, string name = "") {
            return PresentMeshedGameObject(mesh, material, parent.transform, hideInHierarchy, name);
        }
        public static GameObject PresentMeshedGameObject(Mesh mesh, Material material, Transform parent, bool hideInHierarchy = false, string name = "") {
            
            var gameObject = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer))
            {
                hideFlags = hideInHierarchy ? HideFlags.HideAndDontSave : HideFlags.NotEditable
            };
            gameObject.transform.parent = parent.transform;

            gameObject.transform.localScale = Vector3.one;

            var meshFilter = gameObject.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                meshFilter.mesh = mesh;
            }
            var meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.sharedMaterial = material;
            }

            return gameObject;
        }
    }
}