using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.DataStructures
{
    [Serializable]
    public class ListContainer<T>
    {
        public List<T> list;

        public ListContainer()
        {
        }

        public ListContainer(List<T> list)
        {
            this.list = list;
        }
    }
}
