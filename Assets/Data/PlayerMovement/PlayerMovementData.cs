using UnityEngine;

namespace Assets.ScriptableObjects.PlayerMovement
{
    [CreateAssetMenu]
    public class PlayerMovementData : ScriptableObject
    {
        public float StepSize = 1f;
        public float MovementSpeed = 1f;
        public float RotationSpeed = 3f;
        public float OffsetAlmostPosition = 0.0001f;
        public float OffsetAlmostRotation = 1f;
    }
}
