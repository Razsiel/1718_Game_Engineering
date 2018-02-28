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
        for (int i = 0; i < 10; i++)
        {

            float input = Input.GetAxis("Vertical");
            yield return new WaitUntil(() => Input.GetAxis("Vertical") != 0);

            WalkForward();

            yield return new WaitForSeconds(0.5f);
        }
    }

    void WalkForward()
    {
        transform.Translate(new Vector3(stepDistance, 0, 0));
    }
}
