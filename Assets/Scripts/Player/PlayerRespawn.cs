using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRJammies.Framework.Core.Boss;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField]
    Transform _playerRespawnTransform;
    [SerializeField]
    Transform _bossRespawnTransform;
    [SerializeField]
    ContinuousMoveProviderBase _playerMoveScript;
    [SerializeField]
    GameObject _boss;
    BossLocomotion _bossLocomotion;

    private void Start()
    {
        _playerMoveScript = GetComponent<ContinuousMoveProviderBase>();
        if (!_playerMoveScript) Debug.LogWarning(this + "has no player move script assigned!");

        if (_boss) _bossLocomotion = _boss.GetComponent<BossLocomotion>();
        else Debug.Log("No Boss gameobject assigned!");
    }

    public void OnDeath()
    {
        Debug.Log("You Died!");

        if (_playerMoveScript) _playerMoveScript.enabled = false;
        else Debug.LogWarning(this + "has no player move script assigned!");

        if (_bossLocomotion) _bossLocomotion.enabled = false;
        else Debug.LogWarning(this + "has no player move script assigned!");

    }
    public void OnRespawn()
    {
        Debug.Log("Lucky for you there is a respawn mechanic!");
        ResetPlayer();
        ResetBoss();
    }

    private void ResetPlayer()
    {
        if (_playerRespawnTransform)
        {
            this.transform.position = _playerRespawnTransform.position;
            this.transform.rotation = _playerRespawnTransform.rotation;
        }
        else Debug.LogWarning(this + " has no respawntransform assigned!");

        if (_playerMoveScript) _playerMoveScript.enabled = true;
        else Debug.LogWarning(this + "has no player move script assigned!");
    }

    private void ResetBoss()
    {
        if (_bossRespawnTransform)
        {
            _boss.SetActive(false);
            _boss.transform.position = _bossRespawnTransform.position;
            _boss.transform.rotation = _bossRespawnTransform.rotation;
            _boss.SetActive(true);
        }
        else Debug.LogWarning(this + " has no respawntransform assigned!");


        if (_bossLocomotion) _bossLocomotion.enabled = true;
        else Debug.LogWarning(this + "has no player move script assigned!");
    }

}
