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
        public GameObject SequenceBar;
        public Player Player;
        public CommandLibrary CommandLibrary;

        public void Start() {
            CreateCommands();
        }

        public void CreateCommands() {
            foreach (var command in CommandLibrary.Commands) { 
                var uiCommand = CreateCommandButton(command.Value, CommandPanel, () => {
                    Debug.Log($"pressed a button");
                    Player.SequenceChanged += OnSequenceChanged;
                    Player.AddCommand(command.Value);
                });
            }
        }

        private void OnSequenceChanged(List<BaseCommand> commands) {
            Debug.Log($"UI Update");
            foreach (Transform child in transform) {
                DestroyImmediate(child);
            }

            foreach (var command in commands) {
                var commandObject = CreateCommandButton(command, SequenceBar, () => {
                    Debug.Log("Removing command from sequence bar...");
                });
            }
        }

        private GameObject CreateCommandButton(BaseCommand command, GameObject parent, UnityAction onClick) {
            return CreateCommandButton(command, parent.transform, onClick);
        }

        private GameObject CreateCommandButton(BaseCommand command, Transform parent, UnityAction onClick) {
            var commandObject = new GameObject(command.ToString(), typeof(Image), typeof(Button));
            commandObject.transform.parent = parent;
            var rectTransform = commandObject.transform as RectTransform;
            rectTransform.localScale = Vector3.one;

            var image = commandObject.GetComponent<Image>();
            Assert.IsNotNull(image);
            image.sprite = command.Icon;

            var button = commandObject.GetComponent<Button>();
            Assert.IsNotNull(button);
            button.image = image;
            button.onClick.AddListener(onClick);

            return commandObject;
        }
    }
}
