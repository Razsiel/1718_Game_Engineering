using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    float stepDistance = 1.0f;

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
        for (int i = 0; i < 99; i++)
        {

            float input = Input.GetAxis("Vertical");
            yield return new WaitUntil(() => Input.GetAxis("Vertical") != 0);

            StartCoroutine(WalkForward());

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator WalkForward()
    {
        Vector3 destination = transform.position + new Vector3(stepDistance, 0, 0);
        Vector3 destinationFake = transform.position + new Vector3(stepDistance * 1.8f, 0, 0);
        float offset = Vector3.Distance(transform.position, destinationFake);
        while (offset > 0.8f)
        {
            offset = Vector3.Distance(transform.position, destinationFake);
            transform.position = Vector3.Lerp(transform.position, destinationFake, stepDistance * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        transform.position = destination;
    }
}
