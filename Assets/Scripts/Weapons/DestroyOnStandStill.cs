using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnStandStill : MonoBehaviour
{
    [SerializeField]
    float _despawnTimer = 3f;
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

        if (_timer > _despawnTimer || transform.position.y < -5 && !onlyInactive)
            Destroy(this.gameObject);

        if (_timer > _despawnTimer || transform.position.y < -5 && onlyInactive)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _timer = 0;
    }
}
