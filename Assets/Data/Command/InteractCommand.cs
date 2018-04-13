﻿using UnityEngine;
using System.Collections;
using Assets.Data.Command;
using Assets.Scripts;

[CreateAssetMenu(fileName = "InteractCommand", menuName = "Data/Commands/InteractCommand")]
[System.Serializable]
public class InteractCommand : BaseCommand
{
    public override IEnumerator Execute(Player player)
    {
        // MATTHIJS TODO: interact with tile

        // Get current tile
        // Get trigger on tile
        // Play interactAnimation
        // trigger.Execute()
        yield break;
    }

    public override string ToString()
    {
        return this.GetType() + ":" + base.ToString();
    }
}