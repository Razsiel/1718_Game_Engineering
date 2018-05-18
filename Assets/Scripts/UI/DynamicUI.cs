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
        public GameObject WinScreenMask;
        private GameObject _winScreenMask;
        private GameObject _commandPanel;
        private GameObject _commandListPanel;
        private GameInfo _gameInfo;
        private Player _player;
        private FlowLayoutGroup _cmdFlowLayoutGroup;
        private VerticalLayoutGroup _cmdListPanelVerticalLayout;

        [SerializeField] private GameObject _threeStarsImage;
        [SerializeField] private GameObject _twoStarsImage;
        [SerializeField] private GameObject _oneStarImage;

        public void Awake()
        {
            EventManager.OnInitializeUi += Initialize;
            //EventManager.OnUserInputDisable += OnUserInputDisable;
            //EventManager.OnUserInputEnable += OnUserInputEnable;
        }

        private void Initialize(GameInfo gameInfo)
        {
            EventManager.OnInitializeUi -= Initialize;
            _gameInfo = gameInfo;
            _player = _gameInfo.LocalPlayer.Player;

            InitializeCommandPanel();

            InitializeCommandList();

            CreateCommands();
            InitializeWinScreen();
        }

        private void InitializeCommandList()
        {
            _commandListPanel = new GameObject("Commands List");
            _commandListPanel.transform.SetParent(_commandPanel.transform, false);

            var _cmdListPanelVerticalLayout = _commandListPanel.AddComponent<VerticalLayoutGroup>();

            _cmdListPanelVerticalLayout.childAlignment = TextAnchor.LowerRight;
            _cmdListPanelVerticalLayout.spacing = 5;
            _cmdListPanelVerticalLayout.childControlHeight = true;
            _cmdListPanelVerticalLayout.childControlWidth = true;
            _cmdListPanelVerticalLayout.childForceExpandHeight = true;
            _cmdListPanelVerticalLayout.childForceExpandWidth = true;

            var commandListContentSizeFitter = _commandListPanel.AddComponent<ContentSizeFitter>();
            commandListContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            commandListContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var _commandPanelReorderableList = _commandPanel.AddComponent<ReorderableList>();
            _commandPanelReorderableList.CloneDraggedObject = true;
            _commandPanelReorderableList.IsDraggable = true;
            _commandPanelReorderableList.IsDropable = false;
            _commandPanelReorderableList.ContentLayout = _cmdListPanelVerticalLayout;
            _commandPanelReorderableList.DraggableArea = transform.GetChild(0).GetComponent<RectTransform>();
            _commandPanelReorderableList.OnElementAdded.AddListener(EventManager.ElementDroppedToMainSequenceBar);
        }

        private void InitializeCommandPanel()
        {
            _commandPanel = new GameObject("Command Panel");
            _commandPanel.transform.SetParent(transform.GetChild(0), false);
            var image = _commandPanel.AddComponent<Image>();
            image.color = new Color(0,0,0,0);

            _cmdFlowLayoutGroup = _commandPanel.AddComponent<FlowLayoutGroup>();

            _cmdFlowLayoutGroup.childAlignment = TextAnchor.UpperRight;
            _cmdFlowLayoutGroup.horizontal = false;

            var cmdRectTransform = _commandPanel.GetComponent<RectTransform>();
            _commandPanel.transform.localPosition = new Vector3(-150,0,0);

            cmdRectTransform.anchorMin = new Vector2(1, 0.5f);
            cmdRectTransform.anchorMax = new Vector2(1f, 0.5f);
            cmdRectTransform.sizeDelta = new Vector2(300, 700);
        }

        public void CreateCommands() {
            foreach (var command in _gameInfo.AllowedCommands)
            {
                CreateCommandButton(command, _commandListPanel, () => {
                    if (command is LoopCommand)
                    {
                        BaseCommand newCommand = ScriptableObject.CreateInstance<LoopCommand>();
                        newCommand.Icon = _gameInfo.AllCommands.LoopCommand.Icon;
                        newCommand.Name = _gameInfo.AllCommands.LoopCommand.Name;
                        newCommand.Priority = _gameInfo.AllCommands.LoopCommand.Priority;
                        _player.Sequence.Add(newCommand);
                    }
                    else
                    {
                        _player.Sequence.Add(command);
                    }
                });
            }
        }

        private void InitializeWinScreen()
        {
            GameObject _winScreenMask = Instantiate(WinScreenMask);

            WinScreenBehaviour winScreenBehaviour = _winScreenMask.GetComponent<WinScreenBehaviour>();
            winScreenBehaviour.Initialize();
            _winScreenMask.transform.SetParent(transform, false);
        }

        private GameObject CreateCommandButton(BaseCommand command, GameObject parent, UnityAction onClick) {
            return CreateCommandButton(command, parent.transform, onClick);
        }

        private GameObject CreateCommandButton(BaseCommand command, Transform parent, UnityAction onClick) {

            var commandObject = new GameObject(command.Name);

            commandObject.transform.SetParent(parent, false);
            commandObject.AddComponent<CanvasRenderer>();

            commandObject.AddComponent<Image>();
            commandObject.GetComponent<Image>().sprite = command.Icon;
            var layoutElement = commandObject.AddComponent<LayoutElement>();
            var button = commandObject.AddComponent<Button>();
            var commandScript = commandObject.AddComponent<CommandPanelCommand>();

            commandScript.command = command;

            layoutElement.preferredWidth = 95;
            layoutElement.preferredHeight = 95;
            Assert.IsNotNull(button);
            button.onClick.AddListener(onClick);

            return commandObject;
        }

        private void OnUserInputEnable()
        {
            throw new NotImplementedException();
        }

        private void OnUserInputDisable()
        {
            _commandPanel.SetActive(false);
        }
    }
}
