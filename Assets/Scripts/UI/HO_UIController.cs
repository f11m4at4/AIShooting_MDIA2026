using UnityEngine;
using UnityEngine.UI;

public class HO_UIController : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text scoreText;
    [SerializeField] private HO_PlayerController playerController;

    private int currentScore;
    private int lastHp = -1;
    private int lastMaxHp = -1;

    private void Awake()
    {
        ResolvePlayerController();
        RefreshHpFromPlayer();
        ApplyScoreText();
    }

    private void Start()
    {
        if (currentScore == 0)
        {
            HO_GameManager gameManager = FindFirstObjectByType<HO_GameManager>();

            if (gameManager != null)
            {
                UpdateScore(gameManager.Score);
            }
        }

        RefreshHpFromPlayer();
    }

    private void Update()
    {
        RefreshHpFromPlayer();
    }

    public void UpdateHp(int currentHp, int maxHp)
    {
        maxHp = Mathf.Max(1, maxHp);
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        lastMaxHp = maxHp;
        lastHp = currentHp;

        if (hpSlider == null)
        {
            return;
        }

        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
    }

    public void UpdateScore(int score)
    {
        currentScore = Mathf.Max(0, score);
        ApplyScoreText();
    }

    public void SetScore(int score)
    {
        UpdateScore(score);
    }

    private void RefreshHpFromPlayer()
    {
        if (playerController == null)
        {
            ResolvePlayerController();
        }

        if (playerController == null)
        {
            return;
        }

        if (playerController.CurrentHp == lastHp && playerController.MaxHp == lastMaxHp)
        {
            return;
        }

        UpdateHp(playerController.CurrentHp, playerController.MaxHp);
    }

    private void ResolvePlayerController()
    {
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<HO_PlayerController>();
        }
    }

    private void ApplyScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}
