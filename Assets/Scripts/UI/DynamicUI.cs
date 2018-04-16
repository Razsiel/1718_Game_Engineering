using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Command;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class DynamicUI : MonoBehaviour {
        private Player _player;
        private GameManager _gameManager;
        private CommandLibrary _commandLibrary;

        public GameObject CommandPanel;

        public void Start() {
            _gameManager = GameManager.GetInstance();
            _commandLibrary = _gameManager.CommandLibrary;

            Assert.IsNotNull(_commandLibrary);
            _gameManager.PlayersInitialized += /*(_player _playerInitialized)*/ () => {
                this._player = _gameManager.Players[0].player;
                print("_player shoudl be filled");
                Assert.IsNotNull(_player);
            };
            CreateCommands();
        }

        public void CreateCommands() {
            foreach (var command in _commandLibrary.Commands) { 
                var uiCommand = new GameObject(command.Key.ToString(), typeof(Image), typeof(Button));
                uiCommand.transform.parent = CommandPanel.transform;
                var rectTransform = uiCommand.transform as RectTransform;
                rectTransform.localScale = Vector3.one;

                var image = uiCommand.GetComponent<Image>();
                Assert.IsNotNull(image);
                image.sprite = command.Value.Icon;

                var button = uiCommand.GetComponent<Button>();
                Assert.IsNotNull(button);
                button.image = image;
                button.onClick.AddListener(() => {
                    Debug.Log($"pressed a button");
                    _player?.AddCommand(command.Value);
                });
            }
        }
    }
}
