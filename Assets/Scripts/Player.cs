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
        private GameManager _gameManager;
        private List<BaseCommand> _sequence;

        public int PlayerNumber;
        public PlayerData Data;

        public UnityAction<List<BaseCommand>> SequenceChanged;
        public UnityAction OnPlayerReady;
        public UnityAction OnPlayerUnReady;
        public UnityAction StopSequence;
        public UnityAction OnPlayerSequenceRan;
        public UnityAction OnPlayerStop;

        public bool IsReady = false;
        public bool IsLocalPlayer;

        public CardinalDirection ViewDirection = CardinalDirection.North;
        public Vector2Int GridPosition;

        // Use this for initialization
        void Start()
        {
            _gameManager = GameManager.GetInstance();
            _sequence = new List<BaseCommand>();

            if (GameManager.GetInstance().Players.GetLocalPlayer().Player == this)
                OnPlayerSequenceRan += () =>
                {
                    this.IsReady = false;
                };

            /*     
            _sequence = new List<BaseCommand>
            { 
                ScriptableObject.CreateInstance<MoveCommand>(),
                ScriptableObject.CreateInstance<TurnCommand>(),
                ScriptableObject.CreateInstance<MoveCommand>(),
                ScriptableObject.CreateInstance<TurnCommand>(),
                ScriptableObject.CreateInstance<MoveCommand>(),
                ScriptableObject.CreateInstance<TurnCommand>(),
                ScriptableObject.CreateInstance<MoveCommand>(),
                ScriptableObject.CreateInstance<TurnCommand>(),
                ScriptableObject.CreateInstance<MoveCommand>(),
                ScriptableObject.CreateInstance<TurnCommand>(),
            };*/

            //StartCoroutine(WaitForInput());
        }

        // Update is called once per frame
        void Update() { }

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
            this._sequence.Clear();

            var commandOptions = GameManager.GetInstance().CommandLibrary.Commands;
            var commandValues = commands.Select(c => commandOptions.GetValue(c)).ToList();

            this._sequence.AddRange(commandValues);
        }

        public void StartExecution()
        {
            StartCoroutine(ExecuteCommands());
        }

        public void ReadyButtonClicked()
        {
            OnPlayerReady?.Invoke();
            if(!_gameManager.IsMultiPlayer)
                StartCoroutine(ExecuteCommands());
        }

        public void StopButtonClicked()
        {
            EventManager.OnLevelReset(_gameManager.LevelData, _gameManager.Players.Select(x => x.Player).ToList());
            StopAllCoroutines();
        }

        public void UnreadyButtonClicked()
        {
            OnPlayerUnReady?.Invoke();
            IsReady = false;
        }

        public void RemoveCommand(int commandIndex)
        {
            _sequence.RemoveAt(commandIndex);
            SequenceChanged?.Invoke(_sequence);
        }

        IEnumerator ExecuteCommands()
        {
            foreach (BaseCommand command in _sequence)
            {
                DateTime beforeExecute = DateTime.Now;
                yield return StartCoroutine(command.Execute(this));
                DateTime afterExecute = DateTime.Now;

                // A command should take 1.5 Seconds to complete (may change) TODO: Link to some ScriptableObject CONST
                float delay = (1500f - (float)(afterExecute - beforeExecute).TotalMilliseconds) / 1000;

                yield return new WaitForSeconds(delay);
            }
            OnPlayerSequenceRan?.Invoke();
        }

        public void AddOrInsertCommandAt(BaseCommand command, int index)
        {
            _sequence.Insert(index, command);
            SequenceChanged?.Invoke(_sequence);
        }

        public void AddCommand(BaseCommand command)
        {
            _sequence.Add(command);
            SequenceChanged?.Invoke(_sequence);
        }

        public void ClearCommands()
        {
            _sequence.Clear();
            SequenceChanged?.Invoke(_sequence);
        }
        
        public void UpdateCommands(List<BaseCommand> commands)
        {
            _sequence.Clear();
            _sequence.AddRange(commands);
        }

        public void StopExecution()
        {
            StopCoroutine(ExecuteCommands());
        }
    }
}