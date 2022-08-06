using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class Extensions
    {
        public static T Choose<T>(this List<T> list)
        {
            return
                list.Count == 0 ?
                default :
                list[Mathf.RoundToInt(Random.value * (list.Count - 1))];
        }
    }
}