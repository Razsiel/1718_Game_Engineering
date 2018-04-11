using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ListContainer<T>
{
    public List<T> list;

    public ListContainer(){}

    public ListContainer(List<T> list)
    {
        this.list = list;
    }
}
