using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCommand : ScriptableObject
{
    public Sprite icon;
    public abstract IEnumerator Execute(Player player);
}
