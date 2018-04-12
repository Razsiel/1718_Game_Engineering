using Assets.Data.Command;
using Assets.Scripts;
using UnityEngine.Assertions;
using Assets.Scripts.Lib.Helpers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Prefabs.UI {

    public class CommandController : TGEMonoBehaviour {

        private CommandLibrary _commandLibrary;
        public SequenceBar SequenceBar;
        public SequenceBar OtherSequenceBar;
        Player _player;
        GameManager _gameManager;

        public override void Awake() {
            EventManager.InitializeUi += Initialize;
        }

        public void Initialize()
        {
            _gameManager = GameManager.GetInstance();
            _commandLibrary = _gameManager.CommandLibrary;

            Assert.IsNotNull(_commandLibrary);
            _gameManager.PlayersInitialized += /*(_player _playerInitialized)*/ () => {
                this._player = _gameManager.Players[0].player;
                print("_player shoudl be filled");
                Assert.IsNotNull(_player);
            };

            _player = _gameManager.Players[0].player;
        }

        public void OnMoveButtonClicked()
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            _player.AddCommand(_commandLibrary.MoveCommand);
            GameObject moveCommand = Instantiate(SequenceBar.moveCommand);
            moveCommand.transform.parent = SequenceBar.commandSlots[nextFreeSlot].transform;
            moveCommand.transform.localScale = new Vector3(1, 1, 1);
        }

        public void OnTurnLeftButtonClicked()
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            _player.AddCommand(_commandLibrary.TurnLeftCommand);
            GameObject moveCommand = Instantiate(SequenceBar.turnLeftCommand);
            moveCommand.transform.parent = SequenceBar.commandSlots[nextFreeSlot].transform;
            moveCommand.transform.localScale = new Vector3(1, 1, 1);
        }

        public void OnTurnRightButtonClicked()
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            _player.AddCommand(_commandLibrary.TurnRightCommand);
            GameObject moveCommand = Instantiate(SequenceBar.turnRightCommand);
            moveCommand.transform.parent = SequenceBar.commandSlots[nextFreeSlot].transform;
            moveCommand.transform.localScale = new Vector3(1, 1, 1);
        }

        public void OnWaitButtonClicked()
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            _player.AddCommand(_commandLibrary.WaitCommand);
            GameObject moveCommand = Instantiate(SequenceBar.waitCommand);
            moveCommand.transform.parent = SequenceBar.commandSlots[nextFreeSlot].transform;
            moveCommand.transform.localScale = new Vector3(1, 1, 1);
        }

        public void OnInteractButtonClicked()
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            _player.AddCommand(_commandLibrary.InteractCommand);
            GameObject moveCommand = Instantiate(SequenceBar.interactCommand);
            moveCommand.transform.parent = SequenceBar.commandSlots[nextFreeSlot].transform;
            moveCommand.transform.localScale = new Vector3(1, 1, 1);
        }

        public void ClearButtonClicked() {
            SequenceBar.ClearImages();

            _player.ClearCommands();
        }

        public void ReadyButtonClicked() {
            Debug.Log(_player);
            _player.ReadyButtonClicked();
        }

        //public void UpdateOther_playersSequenceBar(List<BaseCommand> commands)
        //{
        //    foreach(BaseCommand c in commands)
        //    {
        //        int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
        //        Image image = SequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
        //        image.sprite = c.Icon;
        //    }
        //}
    }
}