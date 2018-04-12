using System;
using System.Collections.Generic;
using Assets.Scripts.DataStructures.Command;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Data.Command {
    [Serializable]
    [CreateAssetMenu(fileName = "CommandLibrary", menuName = "Data/Commands/CommandLibrary")]
    public class CommandLibrary : ScriptableObject {
        [SerializeField] private MoveCommand MoveCommand;

        [SerializeField] private TurnCommand TurnRightCommand;

        [SerializeField] private TurnCommand TurnLeftCommand;

        [SerializeField] private WaitCommand WaitCommand;

        [SerializeField] private InteractCommand InteractCommand;

        void OnEnable()
        {
            AssertCommandsAreSet();

            Commands = new List<CommandKVP>();
            Commands.Add(new CommandKVP() {Key = CommandEnum.InteractCommand, Value = InteractCommand});
            Commands.Add(new CommandKVP() {Key = CommandEnum.MoveCommand, Value = MoveCommand});
            Commands.Add(new CommandKVP() {Key = CommandEnum.TurnLeftCommand, Value = TurnLeftCommand});
            Commands.Add(new CommandKVP() {Key = CommandEnum.TurnRightCommand, Value = TurnRightCommand});
            Commands.Add(new CommandKVP() {Key = CommandEnum.WaitCommand, Value = WaitCommand});
        }

        [SerializeField] public List<CommandKVP> Commands;

        public bool AssertCommandsAreSet()
        {
            Assert.IsNotNull(TurnRightCommand);
            Assert.IsNotNull(TurnLeftCommand);
            Assert.IsNotNull(WaitCommand);
            Assert.IsNotNull(InteractCommand);
            Assert.IsNotNull(MoveCommand);

            return true;
        }
    }
}