using UnityEngine;
using System.Collections;

public class InteractCommand : ICommand
{
    public Sprite Icon { get; set; }
    private GameObject other;

    public void Execute()
    {
        //Do interact on current tile for example
    }
}
