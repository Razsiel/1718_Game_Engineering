using Assets.Data.Command;
using Assets.Scripts;
using UnityEngine.Assertions;
using Assets.Scripts.Lib.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Prefabs.UI {

    public class CommandController : TGEMonoBehaviour {

        private CommandLibrary _commandLibrary;
        public SequenceBar SequenceBar;       
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
            moveCommand.transform.SetParent(SequenceBar.commandSlots[nextFreeSlot].transform, false);
            moveCommand.GetComponent<Button>().enabled = false;
        }

        public void OnTurnLeftButtonClicked()
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            _player.AddCommand(_commandLibrary.TurnLeftCommand);
            GameObject turnLeftCommand = Instantiate(SequenceBar.turnLeftCommand);
            turnLeftCommand.transform.SetParent(SequenceBar.commandSlots[nextFreeSlot].transform, false);
        }

        public void OnTurnRightButtonClicked()
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            _player.AddCommand(_commandLibrary.TurnRightCommand);
            GameObject turnRightCommand = Instantiate(SequenceBar.turnRightCommand);
            turnRightCommand.transform.SetParent(SequenceBar.commandSlots[nextFreeSlot].transform, false);
        }

        public void OnWaitButtonClicked()
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            _player.AddCommand(_commandLibrary.WaitCommand);
            GameObject waitCommand = Instantiate(SequenceBar.waitCommand);
            waitCommand.transform.SetParent(SequenceBar.commandSlots[nextFreeSlot].transform, false);
        }

        public void OnInteractButtonClicked()
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            _player.AddCommand(_commandLibrary.InteractCommand);
            GameObject interactCommand = Instantiate(SequenceBar.interactCommand);
            interactCommand.transform.SetParent(SequenceBar.commandSlots[nextFreeSlot].transform, false);
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

        public void OnHover()
        {
            EventManager.OnPlaySoundEffect(SFX.ButtonHover);
        }
    }
}