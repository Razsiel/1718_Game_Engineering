using UnityEngine;

namespace Assets.ScriptableObjects.PlayerMovement
{
    [CreateAssetMenu]
    public class PlayerMovementData : ScriptableObject
    {
        public float StepSize = 1f;
        public float MovementSpeed = 1f;
        public float OffsetTolerance = 0.001f;
    }
}
