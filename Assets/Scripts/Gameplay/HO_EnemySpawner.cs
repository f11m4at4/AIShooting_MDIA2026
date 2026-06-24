using System.Reflection;
using UnityEngine;

public class HO_EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 2f;
    [SerializeField] private float spawnXMin = -3.5f;
    [SerializeField] private float spawnXMax = 3.5f;
    [SerializeField] private float spawnY = 6f;
    [SerializeField] private Object gameManager;

    private Coroutine spawnRoutine;

    private void OnEnable()
    {
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        if (spawnRoutine == null)
        {
            return;
        }

        StopCoroutine(spawnRoutine);
        spawnRoutine = null;
    }

    private System.Collections.IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float minTime = Mathf.Max(0f, Mathf.Min(minSpawnInterval, maxSpawnInterval));
            float maxTime = Mathf.Max(minTime, Mathf.Max(minSpawnInterval, maxSpawnInterval));
            float waitTime = Random.Range(minTime, maxTime);

            yield return new WaitForSeconds(waitTime);

            if (ShouldStopSpawning())
            {
                continue;
            }

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning($"{nameof(HO_EnemySpawner)} requires an enemy prefab assignment before it can spawn enemies.", this);
            return;
        }

        float randomX = Random.Range(Mathf.Min(spawnXMin, spawnXMax), Mathf.Max(spawnXMin, spawnXMax));
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private bool ShouldStopSpawning()
    {
        if (gameManager == null)
        {
            return false;
        }

        if (TryGetGameManagerBool("IsGameOver", out bool isGameOver))
        {
            return isGameOver;
        }

        if (TryGetGameManagerBool("IsGameRunning", out bool isGameRunning))
        {
            return !isGameRunning;
        }

        return false;
    }

    private bool TryGetGameManagerBool(string memberName, out bool value)
    {
        if (TryReadBoolMember(gameManager, memberName, out value))
        {
            return true;
        }

        if (gameManager is GameObject gameManagerObject)
        {
            Component[] components = gameManagerObject.GetComponents<Component>();

            foreach (Component component in components)
            {
                if (TryReadBoolMember(component, memberName, out value))
                {
                    return true;
                }
            }
        }

        value = false;
        return false;
    }

    private bool TryReadBoolMember(Object target, string memberName, out bool value)
    {
        value = false;

        if (target == null)
        {
            return false;
        }

        System.Type targetType = target.GetType();
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        PropertyInfo property = targetType.GetProperty(memberName, bindingFlags);

        if (property != null && property.PropertyType == typeof(bool) && property.GetIndexParameters().Length == 0)
        {
            value = (bool)property.GetValue(target);
            return true;
        }

        FieldInfo field = targetType.GetField(memberName, bindingFlags);

        if (field != null && field.FieldType == typeof(bool))
        {
            value = (bool)field.GetValue(target);
            return true;
        }

        MethodInfo method = targetType.GetMethod(memberName, bindingFlags, null, System.Type.EmptyTypes, null);

        if (method != null && method.ReturnType == typeof(bool))
        {
            value = (bool)method.Invoke(target, null);
            return true;
        }

        return false;
    }

    private void OnValidate()
    {
        minSpawnInterval = Mathf.Max(0f, minSpawnInterval);
        maxSpawnInterval = Mathf.Max(0f, maxSpawnInterval);
    }
}
