using Assets.ScriptableObjects.PlayerMovement;
using UnityEngine;

namespace Assets.ScriptableObjects.Player
{
    [CreateAssetMenu(menuName = "Data/Player")]
    public class PlayerData : ScriptableObject {
        public PlayerMovementData MovementData;
    }
}
