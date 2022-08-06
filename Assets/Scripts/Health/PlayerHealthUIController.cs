using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VRJammies.Framework.Core.Health
{
    public class PlayerHealthUIController : MonoBehaviour
    {
        public float heartDestroyDelayTime = 3.0f;
        public List<GameObject> displayHearts;

        private Damageable _playerDamageable;
        private Player.Player _player;
        private int _activeHeartCount;
        private WaitForSeconds _waitForSeconds;

        // Start is called before the first frame update
        void Start()
        {
            //to cache for performance
            _waitForSeconds = new WaitForSeconds(heartDestroyDelayTime);

            _player = FindObjectOfType<Player.Player>();
            if (_player == null)
            {
                Debug.LogError($"Couldn't find player on awake");
            }
            else
            {
                _playerDamageable = _player.GetComponent<Damageable>();
            }

            //TODO - instantiate new hearts based on existing hearts on damegable
        }

        // Update is called once per frame
        public void UpdateHealthUI()
        {
            //TODO - potentially add value consideration (if value = 1 of damage, then do, otherwise not enough health to lose heart)
            for(int i = 0; i < displayHearts.Count; i++)
            {
                if (displayHearts[i].activeSelf)
                {
                    //gravity and fall
                    displayHearts[i].GetComponent<Rigidbody>().useGravity = true;

                    StartCoroutine(DelayedDeactivation(displayHearts[i]));

                    if (i == (displayHearts.Count - 1))
                    {
                        //if gets here instead of return, all hearts deactivated, game over / restart button activation
                    }

                    return;
                }
            }
            

            //depreciated
            /*
            List<GameObject> activeHearts = (List<GameObject>)_displayHearts.Where(heart => heart.activeSelf == true);
            if (activeHearts.Count > 0)
            {
               
            }
            //or alternatively just check
            activeHearts.Any(); 
            */
        }

        private IEnumerator DelayedDeactivation(GameObject obj)
        {
            if(obj != null)
            {
                obj.SetActive(false);
            }

            yield return _waitForSeconds;
        }

        private void OnDestroy()
        {
            //incase running delayeddeactivation, stop
            StopAllCoroutines();
        }
    }
}
