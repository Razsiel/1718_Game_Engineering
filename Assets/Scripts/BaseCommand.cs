﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public abstract class BaseCommand : ScriptableObject
{
    public Sprite Icon;
    public abstract IEnumerator Execute(Player player);
}