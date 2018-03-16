using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class InteractCommand : ScriptableObject, ICommand
{
    public Sprite Icon { get; set; }
    private GameObject other;

    public IEnumerator Execute(Player player)
    {
        // MATTHIJS TODO: interact with tile

        // Get current tile
        // Get trigger on tile
        // Play interactAnimation
        // trigger.Execute()
        yield break;
    }
}
