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
        public SequenceBar OtherSequencebar;
        Player _player;
        GameManager _gameManager;

        public override void Awake() {
            EventManager.InitializeUi += Initialize;
        }

    public void OnMoveButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        player.AddCommand(commandLibrary.MoveCommand);
        GameObject moveCommand = Instantiate(sequenceBar.moveCommand);
        moveCommand.transform.parent = sequenceBar.commandSlots[nextFreeSlot].transform;
        moveCommand.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnTurnLeftButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        player.AddCommand(commandLibrary.TurnLeftCommand);
        GameObject moveCommand = Instantiate(sequenceBar.turnLeftCommand);
        moveCommand.transform.parent = sequenceBar.commandSlots[nextFreeSlot].transform;
        moveCommand.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnTurnRightButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        player.AddCommand(commandLibrary.TurnRightCommand);
        GameObject moveCommand = Instantiate(sequenceBar.turnRightCommand);
        moveCommand.transform.parent = sequenceBar.commandSlots[nextFreeSlot].transform;
        moveCommand.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnWaitButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        player.AddCommand(commandLibrary.WaitCommand);
        GameObject moveCommand = Instantiate(sequenceBar.waitCommand);
        moveCommand.transform.parent = sequenceBar.commandSlots[nextFreeSlot].transform;
        moveCommand.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnInteractButtonClicked()
    {
        int nextFreeSlot = sequenceBar.GetNextEmptySlotIndex();
        player.AddCommand(commandLibrary.InteractCommand);
        GameObject moveCommand = Instantiate(sequenceBar.interactCommand);
        moveCommand.transform.parent = sequenceBar.commandSlots[nextFreeSlot].transform;
        moveCommand.transform.localScale = new Vector3(1, 1, 1);
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
    }
}