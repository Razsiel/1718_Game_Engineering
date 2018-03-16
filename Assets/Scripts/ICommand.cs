using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    Sprite Icon { get; set; }
    IEnumerator Execute(Player player);
}
