using UnityEngine;
using System.Collections;
using Assets.Scripts;

[CreateAssetMenu(fileName = "InteractCommand", menuName = "Data/Commands/InteractCommand")]
public class InteractCommand : BaseCommand
{
    public Sprite Icon { get; set; }
    private GameObject other;

    public override IEnumerator Execute(Player player)
    {
        // MATTHIJS TODO: interact with tile

        // Get current tile
        // Get trigger on tile
        // Play interactAnimation
        // trigger.Execute()
        yield break;
    }
}