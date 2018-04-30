using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Command;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class DynamicUI : MonoBehaviour {

        public GameObject CommandPanel;
        public CommandLibrary CommandLibrary;

        public void Start() {
            CreateCommands();
        }

        public void CreateCommands() {
            foreach (var command in CommandLibrary.Commands) { 
                var uiCommand = CreateCommandButton(command.Value, CommandPanel, () => {
                    Debug.Log($"pressed a button");
                    //Player.SequenceChanged += OnSequenceChanged;
                    //Player.AddCommand(command.Value);
                });
            }
        }

        private void OnSequenceChanged(List<BaseCommand> commands) {
            Debug.Log($"UI Update");
            foreach (Transform child in transform) {
                Destroy(child);
            }

            //foreach (var command in commands) {
            //    var commandObject = CreateCommandButton(command, SequenceBar, () => {
            //        Debug.Log("Removing command from sequence bar...");
            //    });
            //}
        }

        private GameObject CreateCommandButton(BaseCommand command, GameObject parent, UnityAction onClick) {
            return CreateCommandButton(command, parent.transform, onClick);
        }

        private GameObject CreateCommandButton(BaseCommand command, Transform parent, UnityAction onClick) {

            var commandObject = new GameObject(command.ToString());
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
    }
}
