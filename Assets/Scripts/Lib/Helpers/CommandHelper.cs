using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

public static class CommandHelper
{
    public static BaseCommand GetValue<T>(this List<T> list, CommandEnum key) where T : CommandKVP
    {
        Assert.IsNotNull(list);
        return list.Single(x => x.Key == key).Value;
    }

    public static CommandEnum GetKey<T>(this List<T> list, BaseCommand value) where T : CommandKVP
    {
        Assert.IsNotNull(list);
        return list.Single(x => x.Value == value).Key;
    }
}
