using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Data.Command;
using Assets.Scripts.Lib.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class NetPlayerSeqView : MonoBehaviour {
        public SequenceBar SequenceBar;

        public void UpdateSequenceBar(List<CommandEnum> commands) {
            SequenceBar.UpdateSequenceBarFromList(commands);

            //var commandOptions = GameManager.GetInstance().CommandLibrary.Commands;
            //var commandValues = commands.Select(c => commandOptions.GetValue(c)).ToList();
            //ClearCurrentCommands();
            //foreach (BaseCommand c in commandValues) {
            //    int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            //    print(c.ToString());
            //    Image image = SequenceBar.CommandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();

            //    Type cType = c.GetType();
            //    string cName = cType.Name;
            //    if (c is TurnCommand)
            //        cName += ((TurnCommand) c).angle == 90 ? "Right" : "Left";
            //    var prefabs = GameManager.GetInstance().PrefabContainer;
            //    var field = prefabs.GetType().GetField(cName);
            //    var texture = (Texture2D) field.GetValue(prefabs);
            //    var rect = new Rect(0, 0, texture.width, texture.height);
            //    Sprite s = Sprite.Create(texture, rect, new Vector2(0, 0));
            //    image.sprite = s;
            //}
        }

        private void ClearCurrentCommands() {
            foreach (var slot in SequenceBar.CommandSlots) {
                var image = slot.transform.GetChild(0).GetComponent<Image>();
                image.sprite = null;
            }
        }
    }
}