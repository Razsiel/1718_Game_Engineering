using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ScriptableObjects;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CommandLibrary", menuName = "Data/Commands/CommandLibrary")]
public class CommandLibrary : ScriptableObject
{
    [SerializeField] public MoveCommand MoveCommand;

    [SerializeField] public TurnCommand TurnRightCommand;

    [SerializeField] public TurnCommand TurnLeftCommand;

    [SerializeField] public WaitCommand WaitCommand;

    [SerializeField] public InteractCommand InteractCommand;
}