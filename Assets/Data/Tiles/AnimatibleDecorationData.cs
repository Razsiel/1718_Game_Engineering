using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Data.Tiles;
using Assets.Scripts.DataStructures;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Data/Tiles/AnimatedDecoration")]
public class AnimatibleDecorationData : DecorationData {
    
    [SerializeField] public Mesh MeshToAnimate;
    [SerializeField] public Material MaterialOfMeshToAnimate;
    [SerializeField] public Vector3 AnimatePosition;

    public override bool IsWalkable(CardinalDirection direction) {
        return false;
    }

    public override GameObject GenerateGameObject(Transform parent, bool hidden = false) {
        var baseDeco = base.GenerateGameObject(parent, hidden);

        var animationDeco = new GameObject("AnimatedDecorationPart", typeof(MeshRenderer), typeof(MeshFilter))
        {
            hideFlags = hidden ? HideFlags.HideAndDontSave : HideFlags.NotEditable
        };
        animationDeco.transform.parent = baseDeco.transform;

        var meshFilter = animationDeco.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.mesh = this.MeshToAnimate;
        }
        var meshRenderer = animationDeco.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.sharedMaterial = this.MaterialOfMeshToAnimate;
        }

        return baseDeco;
    }
}
