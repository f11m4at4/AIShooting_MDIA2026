using System.Reflection;
using UnityEngine;

public class HO_GameManager : MonoBehaviour
{
    [SerializeField] private int enemyScore = 100;
    [SerializeField] private bool startGameRunning = true;
    [SerializeField] private Object uiController;

    private static readonly string[] ScoreUpdateMethodNames =
    {
        "UpdateScore",
        "SetScore",
        "RefreshScore",
        "UpdateScoreText"
    };

    public int Score { get; private set; }

    public bool IsGameRunning { get; private set; }

    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        Score = 0;
        IsGameOver = false;
        IsGameRunning = startGameRunning;

        NotifyScoreChanged();
    }

    public void AddEnemyKillScore()
    {
        AddScore(enemyScore);
    }

    public void AddScore(int amount)
    {
        if (amount <= 0 || IsGameOver)
        {
            return;
        }

        Score += amount;
        NotifyScoreChanged();
    }

    public void EndGame()
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;
        IsGameRunning = false;
    }

    public void GameOver()
    {
        EndGame();
    }

    private void NotifyScoreChanged()
    {
        if (uiController == null)
        {
            return;
        }

        if (TryInvokeScoreUpdate(uiController))
        {
            return;
        }

        if (uiController is GameObject gameObject)
        {
            Component[] components = gameObject.GetComponents<Component>();

            foreach (Component component in components)
            {
                if (TryInvokeScoreUpdate(component))
                {
                    return;
                }
            }
        }
    }

    private bool TryInvokeScoreUpdate(Object target)
    {
        if (target == null)
        {
            return false;
        }

        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        System.Type targetType = target.GetType();

        foreach (string methodName in ScoreUpdateMethodNames)
        {
            MethodInfo methodWithInt = targetType.GetMethod(methodName, bindingFlags, null, new[] { typeof(int) }, null);

            if (methodWithInt != null)
            {
                methodWithInt.Invoke(target, new object[] { Score });
                return true;
            }

            MethodInfo methodWithoutParameters = targetType.GetMethod(methodName, bindingFlags, null, System.Type.EmptyTypes, null);

            if (methodWithoutParameters != null)
            {
                methodWithoutParameters.Invoke(target, null);
                return true;
            }
        }

        return false;
    }
}
