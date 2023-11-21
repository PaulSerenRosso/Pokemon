using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStairs : MonoBehaviour
{
    [SerializeField] private Transform teleportPoint;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
       var mover = other.GetComponent<Mover>();
       mover.endMoveEvent += TeleportPlayer;
      
    }

    private void TeleportPlayer(Mover mover)
    {
      
        mover.transform.position = teleportPoint.position;
        mover.endMoveEvent -= TeleportPlayer;
    }
}
