using UnityEngine;
using System.Collections;

public class WaitCommand : ICommand
{
    public Sprite Icon { get; set; }

    public void Execute()
    {
        //Execute wait command here (most likely nothing)
    }
}
