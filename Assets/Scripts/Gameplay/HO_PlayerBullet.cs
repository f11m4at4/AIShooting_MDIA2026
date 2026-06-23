using System.Reflection;
using UnityEngine;

public class HO_PlayerBullet : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float destroyY = 6f;

    [Header("Damage")]
    [SerializeField] private int damage = 1;

    private const string EnemyClassName = "HO_Enemy";
    private const string TakeDamageMethodName = "TakeDamage";

    private void Update()
    {
        Move();
        DestroyIfOutOfBounds();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TryDamageEnemy(other))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (TryDamageEnemy(collision.collider))
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        Vector3 position = transform.position;
        position += Vector3.up * speed * Time.deltaTime;
        position.z = 0f;

        transform.position = position;
    }

    private void DestroyIfOutOfBounds()
    {
        if (transform.position.y >= destroyY)
        {
            Destroy(gameObject);
        }
    }

    private bool TryDamageEnemy(Collider other)
    {
        if (other == null)
        {
            return false;
        }

        Component enemy = GetEnemyComponent(other.transform);

        if (enemy == null)
        {
            return false;
        }

        MethodInfo takeDamageMethod = enemy.GetType().GetMethod(
            TakeDamageMethodName,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null,
            new[] { typeof(int) },
            null);

        if (takeDamageMethod == null)
        {
            return false;
        }

        takeDamageMethod.Invoke(enemy, new object[] { damage });
        return true;
    }

    private Component GetEnemyComponent(Transform startTransform)
    {
        Transform currentTransform = startTransform;

        while (currentTransform != null)
        {
            Component[] components = currentTransform.GetComponents<Component>();

            foreach (Component component in components)
            {
                if (component != null && component.GetType().Name == EnemyClassName)
                {
                    return component;
                }
            }

            currentTransform = currentTransform.parent;
        }

        return null;
    }
}
