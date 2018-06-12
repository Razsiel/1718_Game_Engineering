using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

public class SlotScript : MonoBehaviour
{
    //private SequenceBar _sequenceBar;
    public List<int> indices;

    void Awake()
    {
        indices = new List<int>();
    }

    void Start()
    {

    }
}