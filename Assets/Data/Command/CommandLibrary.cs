using System;
using System.Collections.Generic;
using Assets.Scripts.DataStructures.Command;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Data.Command {
    [Serializable]
    [CreateAssetMenu(fileName = "CommandLibrary", menuName = "Data/Commands/CommandLibrary")]
    public class CommandLibrary : ScriptableObject {
        [SerializeField] public MoveCommand MoveCommand;
        [SerializeField] public TurnCommand TurnRightCommand;
        [SerializeField] public TurnCommand TurnLeftCommand;
        [SerializeField] public WaitCommand WaitCommand;
        [SerializeField] public InteractCommand InteractCommand;
        [SerializeField] public LoopCommand LoopCommand;

        void OnEnable() {
            AssertCommandsAreSet();
            Commands = new List<CommandKVP> {
                new CommandKVP() {Key = CommandEnum.MoveCommand, Value = MoveCommand},
                new CommandKVP() {Key = CommandEnum.TurnLeftCommand, Value = TurnLeftCommand},
                new CommandKVP() {Key = CommandEnum.TurnRightCommand, Value = TurnRightCommand},
                new CommandKVP() {Key = CommandEnum.WaitCommand, Value = WaitCommand},
                new CommandKVP() {Key = CommandEnum.InteractCommand, Value = InteractCommand},
                new CommandKVP() {Key = CommandEnum.LoopCommand, Value = LoopCommand}
            };
        }

        [SerializeField] public List<CommandKVP> Commands;

        public void AssertCommandsAreSet() {
            Assert.IsNotNull(MoveCommand);
            Assert.IsNotNull(TurnLeftCommand);
            Assert.IsNotNull(TurnRightCommand);
            Assert.IsNotNull(WaitCommand);
            Assert.IsNotNull(InteractCommand);
        }
    }
}