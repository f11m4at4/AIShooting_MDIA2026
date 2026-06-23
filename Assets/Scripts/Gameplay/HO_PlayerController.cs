using UnityEngine;

public class HO_PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 minBounds = new Vector2(-3.5f, -4.5f);
    [SerializeField] private Vector2 maxBounds = new Vector2(3.5f, 4.5f);

    [Header("Attack")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("HP")]
    [SerializeField] private int maxHp = 3;

    private int currentHp;
    private bool isDead;

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
    public bool IsDead => isDead;

    private void Awake()
    {
        maxHp = Mathf.Max(1, maxHp);
        currentHp = maxHp;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        HandleMovement();
        HandleAttack();
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
        {
            return;
        }

        currentHp = Mathf.Max(0, currentHp - damage);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void HandleMovement()
    {
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));

        if (input.sqrMagnitude > 1f)
        {
            input.Normalize();
        }

        Vector3 position = transform.position;
        position += new Vector3(input.x, input.y, 0f) * moveSpeed * Time.deltaTime;
        position.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        position.y = Mathf.Clamp(position.y, minBounds.y, maxBounds.y);
        position.z = 0f;

        transform.position = position;
    }

    private void HandleAttack()
    {
        if (!Input.GetButtonDown("Fire1"))
        {
            return;
        }

        Fire();
    }

    private void Fire()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning($"{nameof(HO_PlayerController)} requires a bullet prefab to fire.", this);
            return;
        }

        Transform spawnPoint = firePoint;
        Vector3 spawnPosition;
        Quaternion spawnRotation;

        if (spawnPoint == null)
        {
            Debug.LogWarning($"{nameof(HO_PlayerController)} has no FirePoint assigned. Using player position.", this);
            spawnPosition = transform.position;
            spawnRotation = transform.rotation;
        }
        else
        {
            spawnPosition = spawnPoint.position;
            spawnRotation = spawnPoint.rotation;
        }

        spawnPosition.z = 0f;
        Instantiate(bulletPrefab, spawnPosition, spawnRotation);
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
    }
}
