using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ScriptableObjects;
using UnityEditor;
using UnityEngine;

[Serializable]
class CommandLibrary : ScriptableObject
{
    [SerializeField]
    public MoveCommand moveCommand;

    [SerializeField]
    public TurnCommand turnRightCommand;

    [SerializeField]
    public TurnCommand turnLeftCommand;

    [SerializeField]
    public WaitCommand waitCommand;

    [SerializeField]
    public InteractCommand interactCommand;
}
