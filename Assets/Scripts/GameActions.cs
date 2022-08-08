using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRJammies.Framework.Core.ItemLifecyle
{
    public class GameActions : MonoBehaviour
    {
        // spawn actions to subscribe
        public static Action<SpawnableItem, GameObject> onInitialSpawn;
        public static Action<SpawnableItem, GameObject> onItemDestroyed;
        public static Action<SpawnableItem> onDestroyLimb;
    }
}