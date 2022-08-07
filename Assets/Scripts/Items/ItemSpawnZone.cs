using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRJammies.Framework.Core.ItemLifecyle
{
    public class ItemSpawnZone : MonoBehaviour
    {
        [Range(0,1200)]
        public int spawnRadius = 44; // adjust the spawn radius
        
        [Range(0,10)]
        public int spawnAmount = 3; // adjust the spawn amount
        
        public ItemNames itemName; // added in Unity GUI
 
        // Take the selected item and instantiate it with the specified number in the spawn zone.
        private void Start()
        {
            var spawnableItem = CreateSpawnableItem();
            var spawnZone = gameObject;
            
            if (GameActions.onInitialSpawn != null) 
                GameActions.onInitialSpawn.Invoke(spawnableItem, spawnZone);
        }

        // Load the corresponding prefabs of the items and create the 
        private SpawnableItem CreateSpawnableItem ()
        {
            GameObject itemPrefab = Resources.Load("Items/"+itemName) as GameObject;
            SpawnableItem itemToReturn = new SpawnableItem()
            {
                Name = itemName.ToString(), 
                Prefab = itemPrefab,
                Radius = spawnRadius,
                Amount = spawnAmount
            };
            return itemToReturn;
        }


        private void OnTriggerEnter(Collider other)
        {
            var standStillScript = other.transform.gameObject.GetComponent<DestroyOnStandStill>();
            
            if (!standStillScript)
                return;

            standStillScript.enabled = false;
        }

        private void OnTriggerExit(Collider other)
        {
            var standStillScript = other.transform.gameObject.GetComponent<DestroyOnStandStill>();
            
            if (!standStillScript)
                return;
            
            standStillScript.enabled = true;
        }
    }
}