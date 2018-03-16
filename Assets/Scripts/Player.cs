using Assets.ScriptableObjects.PlayerMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovementData data;

    List<Func<IEnumerator>> commands;

    // Use this for initialization
    void Start()
    {
        commands = new List<Func<IEnumerator>>();


        commands.Add(() => WalkForward());
        commands.Add(() => Turn(TurnDirection.Right));
        commands.Add(() => WalkForward());
        commands.Add(() => Turn(TurnDirection.Right));
        commands.Add(() => WalkForward());
        commands.Add(() => Turn(TurnDirection.Right));
        commands.Add(() => WalkForward());

        StartCoroutine(ExecuteCommands());

        //StartCoroutine(WalkWait());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator ExecuteCommands()
    {
        foreach (Func<IEnumerator> command in commands)
        {
            StartCoroutine(command.Invoke());
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator WalkWait()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetAxis("Vertical") != 0);

            yield return StartCoroutine(WalkForward());

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator WalkForward()
    {
        Vector3 destination = transform.position + (transform.forward * data.StepSize);
        float offset = Vector3.Distance(transform.position, destination);
        while (offset > data.OffsetTolerance)
        {
            
            offset = Vector3.Distance(transform.position, destination);
            transform.position = Vector3.Lerp(transform.position, destination, data.MovementSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        transform.position = destination;
    }

    IEnumerator Turn(TurnDirection dir)
    {
        if (dir == TurnDirection.Right)
            transform.Rotate(0, 90, 0);
        else
            transform.Rotate(0, -90, 0);

        yield return new WaitForEndOfFrame();
    }

    enum TurnDirection
    {
        Right, Left
    }
}
