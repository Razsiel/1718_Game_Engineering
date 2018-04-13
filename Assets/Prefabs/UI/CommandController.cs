using Assets.Data.Command;
using Assets.Scripts;
using Assets.Scripts.Lib.Helpers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Prefabs.UI {
    public class CommandController : TGEMonoBehaviour {
        private CommandLibrary _commandLibrary;
        public SequenceBar SequenceBar;
        public SequenceBar OtherSequencebar;
        Player _player;
        GameManager _gameManager;

        public override void Awake() {
            EventManager.InitializeUi += Initialize;
        }

        public void Initialize() {
            _gameManager = GameManager.GetInstance();
            _commandLibrary = _gameManager.CommandLibrary;

            Assert.IsNotNull(_commandLibrary);
            _gameManager.PlayersInitialized += /*(Player playerInitialized)*/ () => {
                this._player = _gameManager.Players[0].player;
                print("player shoudl be filled");
                Assert.IsNotNull(_player);
            };

            _player = _gameManager.Players[0].player;
        }

        public void OnMoveButtonClicked() {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            Image image = SequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
            image.sprite = SequenceBar.moveCommand;
            Debug.Log(_player + "" + SequenceBar + "" + image);
            _player.AddCommand(_commandLibrary.Commands.GetValue(CommandEnum.MoveCommand));
        }

        public void OnTurnLeftButtonClicked() {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            Image image = SequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
            image.sprite = SequenceBar.turnLeftCommand;
            _player.AddCommand(_commandLibrary.Commands.GetValue(CommandEnum.TurnLeftCommand));
        }

        public void OnTurnRightButtonClicked() {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            Image image = SequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
            image.sprite = SequenceBar.turnRightCommand;
            _player.AddCommand(_commandLibrary.Commands.GetValue(CommandEnum.TurnRightCommand));
        }

        public void WaitCommand() {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            Image image = SequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
            image.sprite = SequenceBar.waitCommand;
            _player.AddCommand(_commandLibrary.Commands.GetValue(CommandEnum.WaitCommand));
        }

        public void InteractCommand() {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            Image image = SequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
            image.sprite = SequenceBar.interactCommand;
            _player.AddCommand(_commandLibrary.Commands.GetValue(CommandEnum.InteractCommand));
        }

        public void ClearButtonClicked() {
            SequenceBar.ClearImages();

            _player.ClearCommands();
        }

        public void ReadyButtonClicked() {
            Debug.Log(_player);
            _player.ReadyButtonClicked();
        }

        //public void UpdateOtherPlayersSequenceBar(List<BaseCommand> commands)
        //{
        //    foreach(BaseCommand c in commands)
        //    {
        //        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        //        Image image = sequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        //        image.sprite = c.Icon;
        //    }
        //}

        public void OnHover()
        {
            EventManager.OnPlaySoundEffect(SFX.ButtonHover);
        }
    }
}