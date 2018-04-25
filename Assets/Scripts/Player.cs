﻿using System;
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
        public UnityAction<Player> OnPlayerSequenceRan;
        public UnityAction OnPlayerStop;
        public UnityAction OnPlayerUnready;
        private Coroutine coroutine;

        public bool IsReady = false;
        public bool IsLocalPlayer;

        public CardinalDirection ViewDirection = CardinalDirection.North;
        public Vector2Int GridPosition;

        // Use this for initialization
        void Start()
        {
            _gameManager = GameManager.GetInstance();
            _sequence = new List<BaseCommand>();

            //if (GameManager.GetInstance().Players.GetLocalPlayer().Player == this)
            //    OnPlayerSequenceRan += () =>
            //    {
            //        this.IsReady = false;
            //    };

            EventManager.OnLevelReset += (leveldata, players) =>
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
            this.coroutine = StartCoroutine(ExecuteCommands());
        }

        public void ReadyButtonClicked()
        {
            OnPlayerReady?.Invoke();
            if (!_gameManager.IsMultiPlayer)
            {
                coroutine = StartCoroutine(ExecuteCommands());
            }
        }

        public void StopButtonClicked()
        {
            if (_gameManager.IsMultiPlayer)
            {
                OnPlayerStop?.Invoke();
            }
            else
            {
                //Werkt nog niet naar behoren (single player stop)
                StopCoroutine(coroutine);
                //EventManager.LevelReset(_gameManager.LevelData, _gameManager.Players.Select(x => x.Player).ToList());
            }
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
            OnPlayerSequenceRan?.Invoke(this);
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
            if(coroutine != null)
                StopCoroutine(coroutine);
        }
    }
}