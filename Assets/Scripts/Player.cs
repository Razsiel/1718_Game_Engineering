using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Data.Command;
using Assets.Data.Grids;
using Assets.Data.Levels;
using Assets.Data.Player;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Lib.Extensions;
using Assets.Scripts.Lib.Helpers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Assets.Scripts {
    public class Player : TGEMonoBehaviour {
        public int PlayerNumber;
        public PlayerData Data;
        public Sequence Sequence { get; private set; }

        public UnityAction<Vector3> OnMoveTo;
        public UnityAction<Vector3> OnFailMoveTo;
        public UnityAction<Vector3> OnTurn;
        public UnityAction OnWait;
        public UnityAction OnInteract;
        public UnityAction OnReset;

        private Coroutine _executeCoroutine;

        public bool IsReady = false;
        public bool IsLocalPlayer;

        public CardinalDirection ViewDirection = CardinalDirection.North;
        public Vector2Int GridPosition;

        public override void Awake() {
            Sequence = new Sequence();

            EventManager.OnLevelReset += (gameInfo, players) => {
                Assert.IsNotNull(gameInfo);
                this.GameInfo = gameInfo;

                this.IsReady = false;
            };

            // Generate head
            Data.GenerateGameObject(this.gameObject);
        }

        // Use this for initialization
        public override void Start() { }

        public void UpdateSequence(List<BaseCommand> commands, bool sendEvent) {
            this.Sequence.Clear(sendEvent);

            //var commandOptions = GameInfo.AllCommands.Commands;
            //var commandValues = commands.Select(c => commandOptions.GetValue(c)).ToList();

            this.Sequence.AddRange(commands, sendEvent);
        }

        public void StopExecution() {
            if (_executeCoroutine != null)
                StopCoroutine(_executeCoroutine);
        }

        public void Reset() {
            OnReset?.Invoke();
        }
    }
}