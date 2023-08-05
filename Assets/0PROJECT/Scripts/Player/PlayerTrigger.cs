using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    PlayerManager playerManager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Finish":
                EventManager.Broadcast(GameEvent.OnWin);
                break;

            case "Collectable":
                EventManager.Broadcast(GameEvent.OnCollect, other.gameObject);
                break;
        }
    }
}
