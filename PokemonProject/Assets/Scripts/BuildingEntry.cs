using UnityEngine;

public class BuildingEntry : MonoBehaviour
{
    [SerializeField] private Transform teleportPoint;
    [SerializeField] private GameObject spaceToActivate;
    private void OnTriggerEnter2D(Collider2D other)
    {
       var mover = other.GetComponent<Mover>();
       mover.endMoveEvent += TeleportPlayer;
    }

    private void TeleportPlayer(Mover mover)
    {
        WorldManager.instance.ChangeSpace(spaceToActivate);
        mover.transform.position = teleportPoint.position;
        mover.endMoveEvent -= TeleportPlayer;
    }
}
