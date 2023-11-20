using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingEntry : MonoBehaviour
{
    public string sceneName;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var mover = other.GetComponent<Mover>();
        mover.endMoveEvent += TeleportPlayer;
        Debug.Log("testfdqfdq");
    }

    private void TeleportPlayer(Mover mover)
    {
        mover.endMoveEvent -= TeleportPlayer;
        SceneManager.LoadScene(name);
    }
}
