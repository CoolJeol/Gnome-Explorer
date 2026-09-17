using UnityEngine;

public class EnemyTriggerZone : MonoBehaviour
{
    public enum ZoneType { Detection, Attack }

    [Header("Configuration")]
    public ZoneType zoneType;
    public string playerTag = "Player";

    private EnemyAI mainEnemy;

    void Start()
    {
        // Automatically finds the EnemyAI script on the parent object
        mainEnemy = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            if (zoneType == ZoneType.Detection)
                mainEnemy.SetPlayerTarget(collision.transform);
            else if (zoneType == ZoneType.Attack)
                mainEnemy.SetPlayerInAttackRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            if (zoneType == ZoneType.Detection)
                mainEnemy.SetPlayerTarget(null);
            else if (zoneType == ZoneType.Attack)
                mainEnemy.SetPlayerInAttackRange(false);
        }
    }
}
