using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRJammies.Framework.Core.ItemLifecyle
{
    public enum ItemNames { 
        GasCan, 
        FlamePipe, 
        BroomStickVar1,
        BroomStickVar2,
        ToxicTank, 
        WaterGun, 
        CarDoor, 
        ButcherBlade, 
        Machete,
        TrashCanCover 
    }

    public class SpawnableItem
    {
        public String Name;
        public GameObject Prefab;
        public int Radius;
        public int Amount;
    }
    
    public class ItemManager : Singleton<ItemManager>
    {
[Range(-200,200)]
        public float nudgePower = 70f;
        
        private void OnEnable()
        {
            GameActions.onInitialSpawn += SpawnItem;
            GameActions.onItemDestroyed += SpawnItem;
            GameActions.onDestroyLimb += DeactiveSpawnZones;
        }
        
        private void OnDisable()
        {
            GameActions.onInitialSpawn -= SpawnItem;
            GameActions.onItemDestroyed -= SpawnItem;
            GameActions.onDestroyLimb -= DeactiveSpawnZones;
        }
         
        // Spawn item depending on item selection
        private void SpawnItem(SpawnableItem item, GameObject spawnZone)
        { 
            // Radial spawn of the selected items 
            for (int i = 0; i < item.Amount; i++)
            {
                float angle = i * Mathf.PI * 2f / item.Amount;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * item.Radius, 0, Mathf.Sin(angle) * item.Radius);
                Vector3 itemPosition = spawnZone.transform.position + newPos;
                Quaternion itemRotation = item.Prefab.transform.rotation.normalized;
                
                GameObject itemToInstantiate = Instantiate(
                    item.Prefab, 
                    itemPosition, 
                    itemRotation);
                 
                // need to pick a random position around originPoint but inside spawnRadius
                // must not be too close to another agent inside radiusScale
                Debug.Log(item.Name + " spawned.");
  
                // Add DestroyOnStandStill 
                // Scipt checks if item is still for period X
                // -> If yes it will be destroyed or deactivated. 
                // newItem.AddComponent<DestroyOnStandStill>(); 

                // To prevent the items in the drop zone from disappearing, the DestroyOnStandStill scipt is disabled inside the boundary.
                itemToInstantiate.GetComponent<DestroyOnStandStill>().enabled = false;
                
                // So that some items can lean against a wall or the like, the item is given a little nudge ;)
                itemToInstantiate.GetComponent<Rigidbody>().AddForce(-transform.right * 70f);
            }
        }
        
        // Disable the spawn zones that were passed.
        // Can be used for example when the boss suffers damage and a spawn point is to be destroyed.
        private void DeactiveSpawnZones(SpawnableItem item)
        {
            var spawnZonesInScene = GameObject.FindGameObjectsWithTag("SpawnZone");
            foreach (var spawnZone in spawnZonesInScene)
            {
                var selectedItemName = spawnZone.GetComponent<ItemSpawnZone>().itemName.ToString();
                if (selectedItemName == item.Name)
                {
                    spawnZone.SetActive(false);
                }
            }
        }
    }
}
