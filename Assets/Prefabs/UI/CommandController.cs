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
        public SequenceBar OtherSequenceBar;
        public GameObject ReadyButton;

        public enum ReadyButtonState
        {
            ReadyButton,
            UnReadyButton,
            StopButton
        };

        private ReadyButtonState readyButtonState = ReadyButtonState.ReadyButton;

        public bool _isReadyButton;

        Player _player;
        GameManager _gameManager;

        public override void Awake() {
            EventManager.InitializeUi += Initialize;
            _gameManager = GameManager.GetInstance();
            _commandLibrary = _gameManager.CommandLibrary;

            Assert.IsNotNull(_commandLibrary);
            _gameManager.PlayersInitialized += /*(_player _playerInitialized)*/ () =>
            {
                this._player = _gameManager.Players.GetLocalPlayer().Player;
                print("_player shoudl be filled");
                Assert.IsNotNull(_player);
            };
        }

        void Start()
        {
        }

        public void Initialize()
        {
            //Initialize the ready button and add listener
            _isReadyButton = true;
            ReadyButton.GetComponent<Button>().onClick.AddListener(OnReadyButtonClicked);
            ReadyButton.GetComponent<Image>().sprite = _gameManager.PrefabContainer.PlayButton;
        }

        public void OnMoveButtonClicked()
        {
            _player.AddCommand(_commandLibrary.MoveCommand);
            GameObject moveCommand = Instantiate(SequenceBar.MoveCommand);
            AddCommandToRightSlotAndFixSettings(moveCommand);
        }

        public void OnTurnLeftButtonClicked()
        {
            _player.AddCommand(_commandLibrary.TurnLeftCommand);
            GameObject turnLeftCommand = Instantiate(SequenceBar.TurnLeftCommand);
            AddCommandToRightSlotAndFixSettings(turnLeftCommand);
        }

        public void OnTurnRightButtonClicked()
        {
            _player.AddCommand(_commandLibrary.TurnRightCommand);
            GameObject turnRightCommand = Instantiate(SequenceBar.TurnRightCommand);
            AddCommandToRightSlotAndFixSettings(turnRightCommand);
        }

        public void OnWaitButtonClicked()
        {
            _player.AddCommand(_commandLibrary.WaitCommand);
            GameObject waitCommand = Instantiate(SequenceBar.WaitCommand);
            AddCommandToRightSlotAndFixSettings(waitCommand);
        }

        public void OnInteractButtonClicked()
        {
            _player.AddCommand(_commandLibrary.InteractCommand);
            GameObject interactCommand = Instantiate(SequenceBar.InteractCommand);
            AddCommandToRightSlotAndFixSettings(interactCommand);
        }

        private void AddCommandToRightSlotAndFixSettings(GameObject command)
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            command.transform.SetParent(SequenceBar.CommandSlots[nextFreeSlot].transform, false);
            command.GetComponent<Button>().enabled = false;
            command.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnClearButtonClicked()
        {
            StartCoroutine(SequenceBar.ClearAllCommands());

            _player.ClearCommands();

            SequenceBar.GetCount();
        }

        public void SetReadyButtonState(ReadyButtonState newReadyButtonState)
        {
            readyButtonState = newReadyButtonState;
        }

        public void OnReadyButtonClicked() {
            Debug.Log(_player);

            if (readyButtonState == ReadyButtonState.ReadyButton)
            {
                if (_gameManager.IsMultiPlayer)
                {
                    SetReadyButtonState(ReadyButtonState.UnReadyButton);
                    ReadyButton.GetComponent<Image>().sprite = _gameManager.PrefabContainer.UnReadyButton;
                }
                else
                {
                    SetReadyButtonState(ReadyButtonState.StopButton);
                    ReadyButton.GetComponent<Image>().sprite = _gameManager.PrefabContainer.StopButton;
                }
                    
                _player.ReadyButtonClicked();

            }
            else if (readyButtonState == ReadyButtonState.UnReadyButton)
            {
                SetReadyButtonState(ReadyButtonState.ReadyButton);
                _player.UnreadyButtonClicked();
                ReadyButton.GetComponent<Image>().sprite = _gameManager.PrefabContainer.PlayButton;
            }
            else
            {
                SetReadyButtonState(ReadyButtonState.ReadyButton);
                _player.StopButtonClicked();
                ReadyButton.GetComponent<Image>().sprite = _gameManager.PrefabContainer.PlayButton;
            }
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