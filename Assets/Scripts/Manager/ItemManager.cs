using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRJammies.Framework.Core.ItemLifecyle
{
    public enum ItemNames { 
        GasCan, 
        PipeForGasCan, 
        BroomStick, 
        ToxicBottle, 
        WaterGun, 
        CarDoor, 
        MacheteBlade, 
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
        private void OnEnable()
        {
            GameActions.onInitialSpawn += SpawnItem;
            GameActions.onDestroyLimb += DeactiveSpawnZones;
        }
        
        private void OnDisable()
        {
            GameActions.onInitialSpawn -= SpawnItem;
            GameActions.onDestroyLimb -= DeactiveSpawnZones;
        }
         
        // Spawn item depending on item selection
        private void SpawnItem(SpawnableItem item, GameObject spawnZone)
        {
            float angleStep = 360f / item.Amount;
            float angle = 0f;
                
            for (int i = 0; i < item.Amount - 1; i++)
            {
                var startPos = spawnZone.transform.position;
                float itemX = startPos.x + Mathf.Sin((angle * Mathf.PI) / 180) * item.Radius;
                float itemY = startPos.y + Mathf.Sin((angle * Mathf.PI) / 180) * item.Radius;

                Vector3 itemVector = new Vector3(itemX, itemY, 0);
                Vector3 startPoint = new Vector3(startPos.x, startPos.y, startPos.z);
                Vector3 itemMoveDirection = (itemVector - startPoint).normalized;
                
                    
                // need to pick a random position around originPoint but inside spawnRadius
                // must not be too close to another agent inside radiusScale
                Debug.Log(item.Name + " spawned.");

                var newItem = Instantiate(
                    item.Prefab,
                    startPoint,
                    Quaternion.identity);

                newItem.GetComponent<Rigidbody>().velocity = new Vector3(
                    itemMoveDirection.x,
                    itemMoveDirection.y,
                    0);

                angle += angleStep;

                // Add DestroyOnStandStill 
                // Scipt checks if item is still for period X
                // -> If yes it will be destroyed or deactivated. 
                newItem.AddComponent<DestroyOnStandStill>();
                newItem.GetComponent<DestroyOnStandStill>().enabled = false;
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
