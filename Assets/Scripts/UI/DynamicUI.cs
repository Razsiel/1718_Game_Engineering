using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Command;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class DynamicUI : MonoBehaviour
    {

        public GameObject BottomPanel;
        public GameObject _commandPanel;
        private CommandLibrary _commandLibrary;
        private GameInfo _gameInfo;
        private Player _player;
        private BottomPanelBehaviour _bottomPanelBehaviour;

        public void Awake()
        {
            EventManager.OnInitializeUi += Initialize;
            EventManager.OnUserInputDisable += OnUserInputDisable;
            EventManager.OnUserInputEnable += OnUserInputEnable;
        }

        private void Initialize(GameInfo gameInfo)
        {
            _gameInfo = gameInfo;
            _player = _gameInfo.LocalPlayer.Player;
            _commandLibrary = _gameInfo.AllCommands;
            _bottomPanelBehaviour = BottomPanel.GetComponent<BottomPanelBehaviour>();

            _commandPanel = new GameObject("Command Panel");
            _commandPanel.transform.SetParent(transform.GetChild(0), false);
            var cmdRectTransform = _commandPanel.AddComponent<RectTransform>();
            var cmdVerticalLayoutGroup = _commandPanel.AddComponent<VerticalLayoutGroup>();

            cmdRectTransform.anchorMin = new Vector2(0.8470001f, 0.3742607f);
            cmdRectTransform.anchorMax = new Vector2(1f, 0.8261487f);
            cmdRectTransform.offsetMin = new Vector2(0,0);
            cmdRectTransform.offsetMax = new Vector2(0,0);

            cmdVerticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
            cmdVerticalLayoutGroup.childControlHeight = true;
            cmdVerticalLayoutGroup.childControlWidth = true;
            cmdVerticalLayoutGroup.childForceExpandHeight = true;
            cmdVerticalLayoutGroup.childForceExpandWidth = true;
           
            CreateCommands();

        }

        public void CreateCommands() {
            foreach (var command in _commandLibrary.Commands)
            {
                var uiCommand = CreateCommandButton(command.Value, _commandPanel, () => {
                    print("Pressed " + command.Value.Name);
                    _player.Sequence.Add(command.Value);
                    _player.Sequence.OnSequenceChanged += _bottomPanelBehaviour.OnSequenceChanged;
                });
            }
        }

        private GameObject CreateCommandButton(BaseCommand command, GameObject parent, UnityAction onClick) {
            return CreateCommandButton(command, parent.transform, onClick);
        }

        private GameObject CreateCommandButton(BaseCommand command, Transform parent, UnityAction onClick) {

            var commandObject = new GameObject(command.Name);

            commandObject.transform.SetParent(parent, false);
            commandObject.AddComponent<LayoutElement>();
            var commandHorizontalLayoutGroup = commandObject.AddComponent<HorizontalLayoutGroup>();
            commandObject.AddComponent<CanvasRenderer>();

            commandHorizontalLayoutGroup.padding.left = 10;
            commandHorizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            commandHorizontalLayoutGroup.childControlHeight = true;
            commandHorizontalLayoutGroup.childControlWidth = true;
            commandHorizontalLayoutGroup.childForceExpandHeight = true;
            commandHorizontalLayoutGroup.childForceExpandWidth = true;

            var text = new GameObject();
            text.AddComponent<Text>();
            text.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.GetComponent<Text>().fontSize = 25;
            text.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            text.GetComponent<Text>().text = command.Name;
            text.AddComponent<LayoutElement>();
            text.GetComponent<LayoutElement>().preferredWidth = 120;

            var image = new GameObject();
            image.AddComponent<Image>();
            image.GetComponent<Image>().sprite = command.Icon;
            image.GetComponent<Image>().preserveAspect = true;
            image.AddComponent<LayoutElement>();
            image.AddComponent<Button>();

            var button = image.GetComponent<Button>();
            Assert.IsNotNull(button);
            button.onClick.AddListener(onClick);

            text.transform.SetParent(commandObject.transform, false);
            image.transform.SetParent(commandObject.transform, false);

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
