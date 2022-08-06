using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStandStill : MonoBehaviour
{
    [SerializeField]
    float _despawnTimer = 15f;
    float _timer;
    [SerializeField]
    bool onlyInactive = true;
    Vector3 LastPosition;

    private void Update()
    {
        var currentPosition = transform.position;

        if (currentPosition == LastPosition)
        {
            _timer += Time.deltaTime;
        }
        else _timer = 0;

        LastPosition = currentPosition;

        if (!onlyInactive) 
        {
            if (_timer > _despawnTimer || transform.position.y < -5)
                Destroy(this.gameObject);
        }

        if (onlyInactive)
        {
            if (_timer > _despawnTimer || transform.position.y < -5)
                gameObject.SetActive(false);

        }
    }

    private void OnEnable()
    {
        _timer = 0;
    }
}
