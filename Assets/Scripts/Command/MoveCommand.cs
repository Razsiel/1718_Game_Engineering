using UnityEngine;
using System.Collections;

public class MoveCommand : ICommand
{
    public Sprite Icon { get; set; }

    public void Execute()
    {
        //Execute logic of the move command here.
    }
}
