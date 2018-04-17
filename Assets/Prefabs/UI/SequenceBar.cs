using System;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using Assets.Data.Command;
using Assets.Scripts.Lib.Helpers;
using UnityEngine;
using UnityEngine.UI;

public class SequenceBar : MonoBehaviour
{
    public int AmountOfCommandsAllowed;
    public GameObject[] CommandSlots;
    public Player Player;
    public GameObject Slots;
    private List<GameObject> _commandGameObjects;
    [SerializeField] private CommandLibrary commandLibrary;

    public GameObject InteractCommand;
    public GameObject MoveCommand;
    public GameObject TurnLeftCommand;
    public GameObject TurnRightCommand;
    public GameObject WaitCommand;

    private void Awake()
    {
        EventManager.InitializeUi += Initialize;
    }

    private void Initialize()
    {
        //Initialize the list of all command GameObjects
        _commandGameObjects = new List<GameObject>
        {
            MoveCommand,
            TurnLeftCommand,
            TurnRightCommand,
            WaitCommand,
            InteractCommand
        };
        
        CommandSlots = new GameObject[AmountOfCommandsAllowed];

        for (var i = 0; i < AmountOfCommandsAllowed; i++)
        {
            var slot = Instantiate(Slots);
            slot.name = i.ToString();
            CommandSlots[i] = slot;
            var button = slot.GetComponent<Button>();
            if (button)
            {
                //temp used to ensure the right parameter is set
                var temp = i;
                button.onClick.AddListener(() => { HasChanged(temp, true); });
            }

            slot.transform.SetParent(gameObject.transform, false);
        }

        var manager = GameManager.GetInstance();
        Player = manager.Players[0].Player;
    }

    public void UnShowDropInPoint(int slotIndex)
    {
        for (int i = slotIndex; i < CommandSlots.Length - 1; i++)
        {
            //CommandSlots[i+1].transform.GetChild(0).SetParent(CommandSlots[i].transform);
            //if (CommandSlots[i].transform.childCount > 0)
            //{
            //    CommandSlots[i].transform.SetParent(CommandSlots[i - 1].transform.GetChild(0));
            //}
        }
    }

    public void ShowDropInPoint(int slotIndex)
    {
        for (int i = CommandSlots.Length - 1; i >= slotIndex; i--)
        {
            if(CommandSlots[i].transform.childCount > 0)
                CommandSlots[i].transform.GetChild(0).SetParent(CommandSlots[i+1].transform);
            //GameObject currentChild = null;
            //if(CommandSlots[i].transform.childCount > 0)
            //    currentChild = CommandSlots[i].transform.GetChild(0).gameObject;

            //if (currentChild && CommandSlots[i-1].transform.childCount > 0)
            //{
            //    print("move");
            //    currentChild.transform.SetParent(CommandSlots[i+1].transform);
            //}
        }
    }

    public int GetNextEmptySlotIndex()
    {
        for (var i = 0; i < AmountOfCommandsAllowed; i++)
            if (CommandSlots[i].transform.childCount < 1)
                return i;

        return 999;
    }

    public IEnumerator ClearAllCommands()
    {
        for (var i = 0; i < AmountOfCommandsAllowed; i++)
            if (CommandSlots[i].transform.childCount > 0)
                 DestroyImmediate(CommandSlots[i].transform.GetChild(0).gameObject);

        yield return new WaitForEndOfFrame();
    }

    public void GetCount()
    {
        int count = 0;
        for (int i = 0; i < AmountOfCommandsAllowed; i++)
        {
            if (CommandSlots[i].transform.childCount > 0)
            {
                count++;
            }
        }

        print(count);
    }

    public void HasChanged(int commandSlotIndex, bool destroy)
    {
        if (destroy)
        {
            //Destroy the child of the slot
            var command = CommandSlots[commandSlotIndex].transform.GetChild(0).gameObject;
            Destroy(command);
            MoveCommandsToLeftFromIndex(commandSlotIndex);

            //Remove the command at that index from the player
            Player.RemoveCommand(commandSlotIndex);
        }
    }

    public void HasChanged(int commandSlotIndex, GameObject command)
    {
        int slotToFill = GetNextEmptySlotIndex();
        InsertCommandAtIndex(command, slotToFill);
        GameObject commandCopy = Instantiate(command);
        commandCopy.transform.SetParent(CommandSlots[slotToFill].transform, false);
    }

    public void HasChangedInsert(int commandSlotIndex, GameObject command)
    {
        //Move slots from index one to the right
        for (int i = CommandSlots.Length -1; i >= commandSlotIndex; i--)
        {
            if (CommandSlots[i].transform.childCount <= 0) continue;

            GameObject currentChild = CommandSlots[i].transform.GetChild(0).gameObject;
            if (i == CommandSlots.Length - 1 && currentChild)
            {
                Destroy(currentChild);
            }
            else if (currentChild)
            {
                currentChild.transform.SetParent(CommandSlots[i + 1].transform, false);
            }
        }
        //Insert itemBeingDragged into the index slot
        GameObject commandCopy = Instantiate(command);
        commandCopy.transform.SetParent(CommandSlots[commandSlotIndex].transform, false);

        //Insert the command in the player sequence
        InsertCommandAtIndex(command, commandSlotIndex);
    }

    private void InsertCommandAtIndex(GameObject command, int slotToFill)
    {
        foreach (var libraryCommand in commandLibrary.Commands)
        {
            if (libraryCommand.Key.ToString().Equals(command.name))
            {
                Player.AddOrInsertCommandAt(libraryCommand.Value, slotToFill);
                return;
            }
        }
    }

    private void MoveCommandsToLeftFromIndex(int commandSlotIndex)
    {
        for (var i = commandSlotIndex; i < CommandSlots.Length; i++)
            //Set command parent to the previous slot
            if (CommandSlots[i].transform.childCount > 0 && i != 0)
            {
                var command = CommandSlots[i].transform.GetChild(0).gameObject;

                command.transform.SetParent(CommandSlots[i - 1].transform);
            }
    }

    public void UpdateSequenceBarFromList(List<CommandEnum> commands)
    {
        //Methode om met een list van CommandEnum een sequence bar te vullen
        List<string> commandNames = new List<string>();
        foreach (CommandEnum command in commands)
        {
            commandNames.Add(Enum.GetName(typeof(CommandEnum), command));
        }
        StartCoroutine(ClearAllCommands());
        foreach (var commandName in commandNames)
        {
            int slotIndex = GetNextEmptySlotIndex();
            if(slotIndex == 999) return;

            foreach (var commandGameObject in _commandGameObjects)
            {
                string commandGameObjectName = commandGameObject.name;
                if (commandGameObjectName.Equals(commandName))
                {
                    Instantiate(commandGameObject, CommandSlots[slotIndex].transform);
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}