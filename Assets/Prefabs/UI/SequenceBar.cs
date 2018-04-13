using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SequenceBar : MonoBehaviour
{
    public int AmountOfCommandsAllowed;
    public GameObject[] CommandSlots;
    public GameObject InteractCommand;
    public GameObject MoveCommand;
    public Player Player;

    public GameObject Slots;
    public GameObject TurnLeftCommand;
    public GameObject TurnRightCommand;
    public GameObject WaitCommand;

    private void Awake()
    {
        EventManager.InitializeUi += Initialize;
    }

    private void Initialize()
    {
        CommandSlots = new GameObject[AmountOfCommandsAllowed];

        for (var i = 0; i < AmountOfCommandsAllowed; i++)
        {
            var slot = Instantiate(Slots);
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
        Player = manager.Players[0].player;
    }

    public int GetNextEmptySlotIndex()
    {
        for (var i = 0; i < AmountOfCommandsAllowed; i++)
            if (CommandSlots[i].transform.childCount < 1)
                return i;
        //slaat nergens op
        return 100;
    }

    public void ClearImages()
    {
        for (var i = 0; i < AmountOfCommandsAllowed; i++)
            CommandSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
    }

    private void HasChanged(int commandSlotIndex, bool destroy)
    {
        print(commandSlotIndex + " " + destroy);
        if (destroy)
        {
            //Destroy the child of the slot
            var command = CommandSlots[commandSlotIndex].transform.GetChild(0).gameObject;
            Destroy(command);
            MoveCommandsToLeftFromIndex(commandSlotIndex);

            //Remove the command at that index from the player
            Player.RemoveCommand(commandSlotIndex);
        }

        //Set the new sequence of the player
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

    // Update is called once per frame
    private void Update()
    {
    }
}