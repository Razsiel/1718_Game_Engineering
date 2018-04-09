using Assets.ScriptableObjects.PlayerMovement;
using UnityEngine;

namespace Assets.Data.Player
{
    [CreateAssetMenu(menuName = "Data/Player")]
    public class PlayerData : ScriptableObject {
        public PlayerMovementData MovementData;
    }
}
