using System.Collections.Generic;
using System.Linq;
using Assets.Data.Command;
using Assets.Scripts.DataStructures.Command;
using UnityEngine.Assertions;

namespace Assets.Scripts.Lib.Helpers {
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
}
