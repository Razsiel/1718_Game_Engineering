using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Data.Command;
using Assets.Data.Levels;
using Assets.Data.Player;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Lib.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public int PlayerNumber;
        public PlayerData Data;
        public Sequence Sequence { get; private set; }
        
        public UnityAction<Vector3> OnMoveTo;
        public UnityAction<Vector3> OnTurn;
        public UnityAction OnWait;
        public UnityAction OnInteract;

        private Coroutine executeCoroutine;

        public CommandLibrary CommandLibrary;

        public bool IsReady = false;
        public bool IsLocalPlayer;

        public CardinalDirection ViewDirection = CardinalDirection.North;
        public Vector2Int GridPosition;

        private GameInfo _gameInfo;

        void Awake()
        {
            Sequence = new Sequence();
            EventManager.OnGameStart += gameInfo => {
                _gameInfo = gameInfo;
            };
            EventManager.OnSimulate += OnSimulate;
        }

        // Use this for initialization
        void Start()
        {
            Sequence.Add(CommandLibrary.MoveCommand);
            Sequence.Add(CommandLibrary.TurnRightCommand);

            EventManager.OnLevelReset += (leveldata, players) =>
            {
                this.IsReady = false;
            };

            OnSimulate();
            //EventManager.Simulate();
        }

        private void OnSimulate() {
            StopCoroutine(executeCoroutine);
            executeCoroutine = StartCoroutine(ExecuteCommands());
        }

        //Press Spacebar to run sequence
        IEnumerator WaitForInput()
        {
            while (true)
            {
                yield return new WaitUntil(() => Input.GetAxis("Jump") != 0);

                yield return StartCoroutine(ExecuteCommands());

                yield return new WaitForSeconds(2);
            }
        }

        public void UpdateSequence(List<CommandEnum> commands)
        {
            this.Sequence.Clear();

            var commandOptions = CommandLibrary.Commands;
            var commandValues = commands.Select(c => commandOptions.GetValue(c)).ToList();

            this.Sequence.AddRange(commandValues);
        }

        public void StartExecution()
        {
            this.executeCoroutine = StartCoroutine(ExecuteCommands());
        }

        IEnumerator ExecuteCommands() {
            yield return Sequence.Run(this, _gameInfo.Level, this);
            
        }

        public void StopExecution()
        {
            if(executeCoroutine != null)
                StopCoroutine(executeCoroutine);
        }
    }
}