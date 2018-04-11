using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DataStructures;
using UnityEngine;
using UnityEngine.UI;

public class NetPlayerSeqView : MonoBehaviour {

    public SequenceBar SequenceBar;
	
    public void UpdateSequenceBar(List<CommandEnum> commands)
    {
        var commandOptions = GameManager.GetInstance().CommandLibrary.Commands;
        var commandValues = new List<BaseCommand>();
        commandValues = (from c in commands
                         select commandOptions.GetValue(c)).ToList();
        ClearCurrentCommands();
        foreach(BaseCommand c in commandValues)
        {
            int nextFreeSlot = SequenceBar.GetNextEmptySlotIndex();
            print(c.ToString());
            Image image = SequenceBar.commandSlots[nextFreeSlot].transform.GetChild(0).GetComponent<Image>();
            
            Type cType = c.GetType();
            string cName = cType.Name;
            if(c is TurnCommand) cName += ((TurnCommand)c).angle == 90 ? "Right" : "Left";
            var prefabs = GameManager.GetInstance().PrefabContainer;
            var field = prefabs.GetType().GetField(cName);
            var texture = (Texture2D)field.GetValue(prefabs);
            var rect = new Rect(0,0, texture.width, texture.height);
            Sprite s = Sprite.Create(texture, rect, new Vector2(0,0));
            image.sprite = s;
        }
    }

    private void ClearCurrentCommands()
    {
        foreach(var slot in SequenceBar.commandSlots)
        {
            var image = slot.transform.GetChild(0).GetComponent<Image>();
            image.sprite = null;
        }
    }
}
