using UnityEngine;

public class HighGrass : MonoBehaviour
{
    [SerializeField] private EnemyFighter enemyFighter;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var mover = other.GetComponent<PlayerCharacter>();
        mover.endMoveEvent += TryTriggerFight;
    }

    private void TryTriggerFight(Mover mover)
    {
        mover.GetComponent<PlayerManager>().fightManager.InitFight(enemyFighter);
    }

    private void TryTriggerFight(PlayerManager playerManager)
    {
      
    }
}
