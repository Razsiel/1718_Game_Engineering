using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

public static class SerializeHelper
{
    public static string Serialize<T>(this List<T> list)
    {
        Assert.IsNotNull(list);

        return JsonUtility.ToJson(list);
    }
}
