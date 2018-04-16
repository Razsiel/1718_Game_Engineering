using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Tiles;
using Assets.Scripts.Behaviours;
using Assets.Scripts.DataStructures.Channel;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.DataStructures {
    [Serializable]
    public class DecorationConfiguration {
        public enum DefaultDecorationState {
            InActive = 0,
            Active = 1
        }

        public enum DecorationTrigger {
            Activate,
            Deactivate
        }

        [SerializeField] public DecorationData DecorationData;
        [SerializeField] public Vector3 RelativePosition;
        [SerializeField] public int Scale = 1;
        [SerializeField] public int Rotation;
        [SerializeField] public CardinalDirection Orientation = CardinalDirection.North;
        [SerializeField] public ChannelType Type;
        [SerializeField] public TriggerType TriggerType;
        [SerializeField] public Channel.Channel Channel;
        [SerializeField] public DefaultDecorationState DefaultState = DefaultDecorationState.InActive;

        public GameObject GameObject { get; private set; }

        public bool IsMechanismWithChannel => this.Type == ChannelType.Mechanism && this.Channel != DataStructures.Channel.Channel.None;

        public GameObject GenerateGameObject(GameObject parent, bool hidden = false) {
            GameObject = GenerateGameObject(parent.transform, hidden);
            return GameObject;
        }

        private GameObject GenerateGameObject(Transform parent, bool hidden = false) {
            var decoration = DecorationData?.GenerateGameObject(parent, hidden);
            if (decoration != null) {
                var transform = decoration.transform;
                transform.position = RelativePosition;
                transform.localScale = Vector3.one * Scale;
                transform.eulerAngles = Orientation.ToEuler() + Vector3.up * Rotation;
                
                var behaviour = decoration.GetComponent<DecorationBehaviour>();
                Assert.IsNotNull(behaviour);
                Assert.IsNotNull(this);
                behaviour.Configuration = this;
            }
            GameObject = decoration;
            return decoration;
        }

        public bool IsWalkable(CardinalDirection direction) {
            return (DecorationData?.IsWalkable(direction) ?? false) && direction.ToOppositeDirection() != direction ;
        }
    }
}