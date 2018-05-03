using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.DataStructures
{
    [Serializable]
    public class ListContainer<T>
    {
        public List<T> List;

        public ListContainer()
        {
        }

        public ListContainer(List<T> list)
        {
            this.List = list;
        }
    }
}
