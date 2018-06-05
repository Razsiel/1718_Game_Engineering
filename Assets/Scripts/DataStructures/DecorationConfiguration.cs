using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Tiles;
using Assets.Scripts.Behaviours;
using Assets.Scripts.DataStructures.Channel;
using Assets.Scripts.Lib.Helpers;
using M16h;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.DataStructures {
    [Serializable]
    public class DecorationConfiguration {
        [SerializeField]
        public Material[] ChannelMaterials;
        
        [SerializeField] public DecorationData DecorationData;
        [SerializeField] public Vector3 RelativePosition;
        [SerializeField] public int Scale = 1;
        [SerializeField] public int Rotation;
        [SerializeField] public CardinalDirection Orientation = CardinalDirection.North;
        [SerializeField] public ChannelType Type;
        [SerializeField] public TriggerType TriggerType;
        [SerializeField] public Channel.Channel Channel;
        [SerializeField] public DecorationState DefaultState = DecorationState.Inactive;

        private TinyStateMachine<DecorationState, DecorationTrigger> Fsm; // Finite State Machine

        public GameObject GameObject { get; private set; }

        public bool IsMechanismWithChannel =>
            this.Type == ChannelType.Mechanism && this.Channel != DataStructures.Channel.Channel.None;

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

                if (Type != ChannelType.Decoration && Channel != DataStructures.Channel.Channel.None) {
                    var renderer = decoration.GetComponent<MeshRenderer>();
                    if (renderer != null) {
                        renderer.material = ChannelMaterials[(int)Channel - 1];
                    }
                }

                var behaviour = decoration.GetComponent<DecorationBehaviour>();
                Assert.IsNotNull(behaviour);
                Assert.IsNotNull(this);
                behaviour.Configuration = this;
            }
            GameObject = decoration;
            return decoration;
        }

        public bool IsWalkable(CardinalDirection direction) {
            return IsActivated() || (DecorationData?.IsWalkable(direction) ?? false);
        }

        public bool IsActivated() {
            return Fsm.State == DecorationState.Active;
        }

        public void Init(Action<Player> onActivate, Action onDeactivate) {
            // Setup state machine for triggers and mechanisms
            Fsm = new TinyStateMachine<DecorationState, DecorationTrigger>(this.DefaultState);
            if (this.Type != ChannelType.Decoration) {
                Fsm
                    .Tr(DecorationState.Inactive, DecorationTrigger.Activate, DecorationState.Active)
                    .On(() => onActivate(null))
                    .Tr(DecorationState.Active, DecorationTrigger.Deactivate, DecorationState.Inactive)
                    .On(onDeactivate);
            }
            else {
                Fsm.Tr(DecorationState.Active, DecorationTrigger.Activate, DecorationState.Active)
                   .On((transitionState, trigger, newState, player) => onActivate(player as Player));
            }

            // register self to receive messages from 'interacts' if I'm a mechanism listening to a channel
            if (IsMechanismWithChannel)
            {
                DecorationChannelManager.Instance.RegisterToChannel(Channel, OnInteract);
            }
        }

        public void OnInteract(Channel.Channel channel, Player player) {
            if (this.Type != ChannelType.Decoration) {
                // toggle state for triggers and mechanisms
                if (Fsm.IsInState(DecorationState.Active)) {
                    Fsm.Fire(DecorationTrigger.Deactivate);
                }
                else {
                    Fsm.Fire(DecorationTrigger.Activate, player);
                }
            }
            else {
                Fsm.Fire(DecorationTrigger.Activate, player);
            }
        }

        public enum DecorationState {
            Inactive = 0,
            Active = 1,
        }

        public enum DecorationTrigger {
            Activate,
            Deactivate
        }

        public void Reset() {
            if (Fsm == null || Fsm.State == DefaultState) {
                return;
            }
            Fsm.Fire(Fsm.State == DecorationState.Active ? DecorationTrigger.Deactivate : DecorationTrigger.Activate);
        }

        public void Destroy() {
            Fsm = null;
        }
    }
}