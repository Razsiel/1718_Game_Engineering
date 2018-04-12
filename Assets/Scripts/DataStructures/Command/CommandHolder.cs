using System;
using UnityEngine;

[Serializable]
public class CommandHolder
{
    public CommandEnum command;

    public CommandHolder(CommandEnum commandEnum)
    {
        this.command = commandEnum;
    }
}