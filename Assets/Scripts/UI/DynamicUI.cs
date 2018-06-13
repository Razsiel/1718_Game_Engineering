using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Command;
using Assets.Scripts.DataStructures;
using Assets.Scripts.DataStructures.Command;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Assets.Scripts.UI
{
    public class DynamicUI : MonoBehaviour
    {
        public GameObject BottomPanel;
        public GameObject WinScreen;
        public GameObject IngameMenuPanel;
        public GameObject CommandPanel;
        public GameObject CommandsList;
        public GameObject CommandButton;

        private GameObject _winScreenMask;
        private GameObject _commandPanel;
        private GameObject _commandListPanel;
        private GameObject _ingameMenuPanel;
        private GameInfo _gameInfo;
        private Player _player;
        private FlowLayoutGroup _cmdFlowLayoutGroup;
        private VerticalLayoutGroup _cmdListPanelVerticalLayout;

        public void Awake()
        {
            EventManager.OnInitializeUi += Initialize;
        }

        private void OnAllPlayersSpawned() {
            _player = _gameInfo.LocalPlayer.Player;
            Assert.IsNotNull(_player);
        }

        private void Initialize(GameInfo gameInfo)
        {
            EventManager.OnInitializeUi -= Initialize;
            _gameInfo = gameInfo;
            _player = gameInfo.LocalPlayer.Player;

            InitializeIngameMenuPanel();
            InitializeCommandPanel();

            InitializeCommandList();

            CreateCommands();

            AddReorderableListToCommandPanel();

        }

        private void AddReorderableListToCommandPanel()
        {
            var _commandPanelReorderableList = _commandPanel.AddComponent<ReorderableList>();
            _commandPanelReorderableList.IsDraggable = true;
            _commandPanelReorderableList.CloneDraggedObject = true;
            _commandPanelReorderableList.IsDropable = false;
            _commandPanelReorderableList.ContentLayout = _commandListPanel.GetComponent<VerticalLayoutGroup>();
            _commandPanelReorderableList.DraggableArea = transform.GetChild(0).GetComponent<RectTransform>();
            _commandPanelReorderableList.OnElementAdded.AddListener(EventManager.ElementDroppedToMainSequenceBar);
        }

        private void InitializeIngameMenuPanel()
        {
            _ingameMenuPanel = Instantiate(IngameMenuPanel);
            _ingameMenuPanel.transform.SetParent(gameObject.transform.GetChild(0), false);
        }

        private void InitializeCommandList()
        {
            _commandListPanel = Instantiate(CommandsList, _commandPanel.transform, false);
        }

        private void InitializeCommandPanel()
        {
            _commandPanel = Instantiate(CommandPanel, transform.GetChild(0), false);
        }

        public void CreateCommands() {
            foreach (var command in _gameInfo.AllowedCommands)
            {
                CreateCommandButton(command, _commandListPanel, () => {
                    if (command is LoopCommand)
                    {
                        var newCommand = Instantiate(_gameInfo.AllCommands.LoopCommand) as LoopCommand as BaseCommand;
                        newCommand = newCommand.Init();

                        _player.Sequence.Add(newCommand);
                    }
                    else
                    {
                        _player.Sequence.Add(command);
                    }
                });
            }
        }

        public void MenuButtonClicked()
        {
            _ingameMenuPanel.SetActive(true);
        }

        private GameObject CreateCommandButton(BaseCommand command, GameObject parent, UnityAction onClick) {
            return CreateCommandButton(command, parent.transform, onClick);
        }

        private GameObject CreateCommandButton(BaseCommand command, Transform parent, UnityAction onClick)
        { 
            var commandObject = Instantiate(CommandButton, parent, false);

            commandObject.GetComponent<Image>().sprite = command.Icon;
            var button = commandObject.GetComponent<Button>();
            var commandScript = commandObject.GetComponent<CommandPanelCommand>();

            commandScript.command = command;

            Assert.IsNotNull(button);
            button.onClick.AddListener(onClick);

            return commandObject;
        }

        public void OnDestroy()
        {
            EventManager.OnInitializeUi -= Initialize;         
        }
    }
}
