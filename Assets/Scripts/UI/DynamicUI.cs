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
        private GameObject _commandPanel;
        private GameObject _commandListPanel;
        private CommandLibrary _commandLibrary;
        private GameInfo _gameInfo;
        private Player _player;
        private BottomPanelBehaviour _bottomPanelBehaviour;
        private FlowLayoutGroup _cmdFlowLayoutGroup;
        private VerticalLayoutGroup _cmdListPanelVerticalLayout;

        public void Awake()
        {
            EventManager.OnInitializeUi += Initialize;
            EventManager.OnRepaintUi += Repaint;
            //EventManager.OnUserInputDisable += OnUserInputDisable;
            //EventManager.OnUserInputEnable += OnUserInputEnable;
        }

        private void Repaint()
        {
            print("ui repaint to fix flowlayout bug");
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        private void Initialize(GameInfo gameInfo)
        {
            _gameInfo = gameInfo;
            _player = _gameInfo.LocalPlayer.Player;
            _commandLibrary = _gameInfo.AllCommands;
            _bottomPanelBehaviour = BottomPanel.GetComponent<BottomPanelBehaviour>();
            _player.Sequence.OnSequenceChanged += _bottomPanelBehaviour.OnSequenceChanged;

            InitializeCommandPanel();

            InitializeCommandList();

            CreateCommands();
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
            _commandPanelReorderableList.OnElementAdded.AddListener(BottomPanel.GetComponent<BottomPanelBehaviour>().AddDroppedElementToMainSequence);
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
            foreach (var command in _commandLibrary.Commands)
            {
                CreateCommandButton(command, _commandListPanel, () => {
                    _player.Sequence.Add(command.Value);
                });
            }
        }

        private GameObject CreateCommandButton(CommandKVP command, GameObject parent, UnityAction onClick) {
            return CreateCommandButton(command, parent.transform, onClick);
        }

        private GameObject CreateCommandButton(CommandKVP command, Transform parent, UnityAction onClick) {

            var commandObject = new GameObject(command.Value.Name);

            commandObject.transform.SetParent(parent, false);
            commandObject.AddComponent<CanvasRenderer>();

            commandObject.AddComponent<Image>();
            commandObject.GetComponent<Image>().sprite = command.Value.Icon;
            var layoutElement = commandObject.AddComponent<LayoutElement>();
            var button = commandObject.AddComponent<Button>();
            var commandScript = commandObject.AddComponent<CommandPanelCommand>();
            commandScript.CommandType = command.Key;

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
