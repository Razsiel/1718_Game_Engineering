using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player/Customisation/Head", fileName = "New Player Head")]
public class PlayerHeadData : ScriptableObject {
    public Mesh Mesh;
    public Material Material;

    public GameObject GenerateGameObject(GameObject parent, bool hidden = false) {
        return GenerateGameObject(parent.transform, hidden);
    }

    public GameObject GenerateGameObject(Transform parent, bool hidden = false) {
        var head = new GameObject("Head",
                                  typeof(MeshFilter),
                                  typeof(MeshRenderer)) {
            hideFlags = hidden ? HideFlags.HideAndDontSave : HideFlags.None
        };
        head.transform.parent = parent;

        head.transform.localScale = Vector3.one * 0.15f;
        head.transform.localPosition = Vector3.down * 0.75f;

        var meshFilter = head.GetComponent<MeshFilter>();
        if (meshFilter != null) {
            meshFilter.mesh = this.Mesh;
        }
        var meshRenderer = head.GetComponent<MeshRenderer>();
        if (meshRenderer != null) {
            meshRenderer.sharedMaterial = this.Material;
        }

        return head;
    }
}
