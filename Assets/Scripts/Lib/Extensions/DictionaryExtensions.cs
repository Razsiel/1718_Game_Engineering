using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Lib.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets a typed value from a dictionary (so you don't have to cast the type you get back anymore)
        /// </summary>
        /// <param name="dictionary">The dictionary to get our value from</param>
        /// <param name="key">The name of the key you want</param>
        /// <param name="value">The out value we get back</param>
        /// <returns>True or false depending on if the conversion succeeded</returns>
        public static bool TryGetTypedValue<TKey, TValue, TActual>
            (this IDictionary<TKey, TValue> dictionary, TKey key, out TActual value)
            where TActual : TValue
        {
            TValue tmp;
            if (dictionary.TryGetValue(key, out tmp))
            {
                value = (TActual) tmp;
                return true;
            }

            value = default(TActual);
            return false;
        }
    }
}
