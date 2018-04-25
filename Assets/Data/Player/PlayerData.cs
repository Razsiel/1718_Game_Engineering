using Assets.ScriptableObjects.PlayerMovement;
using UnityEngine;

namespace Assets.Data.Player
{
    [CreateAssetMenu(menuName = "Data/Player")]
    public class PlayerData : ScriptableObject {
        public PlayerMovementData MovementData;
        public PlayerHeadData[] Heads;

        public GameObject GenerateGameObject(GameObject parent) {
            Debug.Log("Creating player customisation");
            if (Heads.Length != 0) {
                var randomHead = Random.Range(0, Heads.Length);
                return Heads[randomHead].GenerateGameObject(parent);
            }
            return null;
        }
    }
}
