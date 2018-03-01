using Assets.ScriptableObjects.PlayerMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMovementData data;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(WalkWait());
    }

    // Update is called once per frame
    void Update()
    {
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
        Vector3 destination = transform.position + new Vector3(data.StepSize, 0, 0);
        float offset = Vector3.Distance(transform.position, destination);
        while (offset > data.OffsetTolerance)
        {
            
            offset = Vector3.Distance(transform.position, destination);
            transform.position = Vector3.Lerp(transform.position, destination, data.MovementSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        transform.position = destination;
    }
}
