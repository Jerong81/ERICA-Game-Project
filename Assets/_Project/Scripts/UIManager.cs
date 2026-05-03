using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI 연결")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI fatigueText;
    public GameObject clearPanel;

    private int collectedCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
        if (clearPanel != null) clearPanel.SetActive(false);
    }

    public void AddPiece(int index)
    {
        collectedCount++;
        UpdateScoreUI();

        if (collectedCount >= 8)
        {
            ShowCompleteUI();
        }
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"조각 수집: {collectedCount} / 8";
        }
    }

    public void UpdateFatigueUI(int fatigue)
    {
        if (fatigueText != null)
        {
            fatigueText.text = $"피로도: {fatigue}";
        }
    }

    public void ShowCompleteUI()
    {
        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
            UnityEngine.Debug.Log("학생증 완성 패널 활성화!");
        }
    }
}