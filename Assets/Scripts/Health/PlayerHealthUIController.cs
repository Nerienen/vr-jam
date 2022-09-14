using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VRJammies.Framework.Core.Health
{
    public class PlayerHealthUIController : MonoBehaviour
    {
        public float heartDestroyDelayTime = 3.0f;
        public GameObject originalHeart;
        public GameObject deathFlash; //TODO - for blinking on and off for death effect, last blink on to pure black (with potential text to restart)

        [SerializeField]
        private List<GameObject> displayHearts;
        private List<Transform> originalHeartPositions;

        private Damageable _playerDamageable;
        private Player.Player _player;
        private WaitForSeconds _waitForSeconds;

        private int gameOverHeartCount;

        // Start is called before the first frame update
        void Awake()
        {
            originalHeartPositions = new List<Transform>();

            //to cache for performance
            _waitForSeconds = new WaitForSeconds(heartDestroyDelayTime);

            _player = FindObjectOfType<Player.Player>();
            if (_player == null)
            {
                Debug.LogError($"Couldn't find player on awake");
            }
            else
            {
                _playerDamageable = _player.GetComponentInParent<Damageable>();
            }
        }

        private void Start()
        {
            int heartCount = _playerDamageable.GetStartingHealth();
            Renderer heartRend = originalHeart.GetComponent<Renderer>();
            float heartWidth = heartRend.localBounds.size.z;

            //instantiate new hearts based on existing starting health on dameagable
            for (int i = 0; i < heartCount; i++)
            {
                GameObject heart = Instantiate(originalHeart, this.transform);
                Vector3 localSpaceUIPosition = transform.InverseTransformPoint(heart.transform.position);
                localSpaceUIPosition += new Vector3((heartWidth * i), 0, 0);
                heart.transform.localPosition = localSpaceUIPosition;
                displayHearts.Add(heart);
            }

            gameOverHeartCount = displayHearts.Count - 1;

            for (int i = 0; i < displayHearts.Count; i++)
            {
                originalHeartPositions.Add(displayHearts[i].transform);
                displayHearts[i].SetActive(true);
            }

            originalHeart.SetActive(false);
        }

        //adjustment to health UI - can ingest positive or negative values to add or remove health
        public void UpdateHealthUI(int healthMod)
        {
            int heartMod = healthMod;

            for(int i = 0; i < displayHearts.Count; i++)
            {
                if (displayHearts[i].activeSelf)
                {
                    //optional solution: Mathf.Sign(heartMod) == 1

                    if (heartMod > 0)
                    {
                        heartMod--;
                        //TODO- can deactivate, if we dont want some cool effect, will drop them and delayed disable
                        //TODO- cache these in start for optimization (here for speed of development)
                        displayHearts[i].transform.parent = null;
                        displayHearts[i].GetComponent<BoxCollider>().enabled = true;
                        displayHearts[i].GetComponent<Rigidbody>().useGravity = true;

                        if (i == gameOverHeartCount)
                        {
                            //TODO - potential deathFlash
                            //TODO - if gets here instead of return, all hearts deactivated, game over / restart button activation
                        }
                    }
                    else if (heartMod < 0)
                    {
                        heartMod++;
                        ResetHeart(i);
                    }

                    StartCoroutine(DelayedDeactivation(displayHearts[i]));

                    //all modifications have been made and end
                    if(heartMod == 0)
                    {
                        return;
                    }
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

        public void ResetHearts()
        {
            for (int i = 0; i < displayHearts.Count; i++)
            {
                ResetHeart(i);
            }
        }

        private void ResetHeart(int heartIndex)
        {
            displayHearts[heartIndex].transform.parent = this.transform;
            displayHearts[heartIndex].GetComponent<BoxCollider>().enabled = false;
            displayHearts[heartIndex].GetComponent<Rigidbody>().useGravity = false;
            displayHearts[heartIndex].transform.position = originalHeartPositions[heartIndex].transform.position;
            displayHearts[heartIndex].SetActive(true);
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
