using UnityEngine;

public class HO_Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float destroyY = -6f;
    [SerializeField] private GameObject enemyExplosionPrefab;

    private HO_GameManager gameManager;
    private bool isRemoving;

    private void Update()
    {
        MoveDown();
        DestroyIfOutOfBounds();
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
        {
            return;
        }

        Remove(true, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryHandlePlayerHit(other.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryHandlePlayerHit(collision.transform);
    }

    private void MoveDown()
    {
        Vector3 position = transform.position;
        position += Vector3.down * moveSpeed * Time.deltaTime;
        position.z = 0f;
        transform.position = position;
    }

    private void DestroyIfOutOfBounds()
    {
        if (transform.position.y <= destroyY)
        {
            Remove(false, false);
        }
    }

    private void TryHandlePlayerHit(Transform hitTransform)
    {
        if (hitTransform == null)
        {
            return;
        }

        HO_PlayerController playerController = hitTransform.GetComponentInParent<HO_PlayerController>();

        if (playerController == null)
        {
            return;
        }

        playerController.TakeDamage(contactDamage);
        Remove(false, true);
    }

    private void Remove(bool awardScore, bool spawnExplosion)
    {
        if (isRemoving)
        {
            return;
        }

        isRemoving = true;

        if (awardScore)
        {
            if (gameManager == null)
            {
                gameManager = FindFirstObjectByType<HO_GameManager>();
            }

            if (gameManager != null)
            {
                gameManager.AddEnemyKillScore();
            }
        }

        if (spawnExplosion)
        {
            SpawnExplosion();
        }

        Destroy(gameObject);
    }

    private void SpawnExplosion()
    {
        if (enemyExplosionPrefab == null)
        {
            return;
        }

        Vector3 explosionPosition = transform.position;
        explosionPosition.z = 0f;

        Instantiate(enemyExplosionPrefab, explosionPosition, Quaternion.identity);
    }
}
