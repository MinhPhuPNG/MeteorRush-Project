using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int maxHealth = 3;
    public Sprite lifeIconSprite;
    public List<GameObject> lifeIcons;
    public TextMeshProUGUI scoreTMP;
    public AudioClip explosionSound;

    public bool IsGameOver { get; private set; }
    public int CurrentHealth { get; private set; }
    public int Score { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        CurrentHealth = maxHealth;
        Score = 0;
        UpdateScoreUI();
        
        // Create life icons if not already in the list
        if (lifeIcons == null || lifeIcons.Count == 0)
        {
            CreateLifeIcons();
        }
        
        UpdateLifeIcons();
    }

    void CreateLifeIcons()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        lifeIcons = new List<GameObject>();
        float spacing = 70f;
        float startX = 50f;

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject icon = new GameObject($"LifeIcon_{i}");
            icon.transform.SetParent(canvas.transform, false);
            
            Image image = icon.AddComponent<Image>();
            image.sprite = lifeIconSprite;
            
            RectTransform rectTransform = icon.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.anchoredPosition = new Vector2(-startX - (i * spacing), -50f);
            rectTransform.sizeDelta = new Vector2(60, 60);

            lifeIcons.Add(icon);
        }
    }

    void Update()
    {
        if (IsGameOver && IsRestartKeyPressed())
        {
            RestartGame();
        }
    }

    bool IsRestartKeyPressed()
    {
        if (Keyboard.current != null)
            return Keyboard.current.spaceKey.wasPressedThisFrame;

        return Input.GetKeyDown(KeyCode.Space);
    }

    void OnGUI()
    {
        if (!IsGameOver)
            return;

        GUIStyle style = new GUIStyle(GUI.skin.box)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 28,
        };
        style.normal.textColor = Color.white;

        Rect rect = new Rect(0, Screen.height * 0.4f, Screen.width, 120);
        GUI.Box(rect, "Game Over\nPress Space to Restart", style);
    }

    public void RegisterEnemyHit()
    {
        if (IsGameOver)
            return;

        Score++;
        UpdateScoreUI();
    }

    public void RegisterPlayerHit()
    {
        if (IsGameOver)
            return;

        CurrentHealth--;
        UpdateLifeIcons();

        if (CurrentHealth <= 0)
        {
            EndGame();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreTMP != null)
            scoreTMP.text = $"Score: {Score}";
    }

    void UpdateLifeIcons()
    {
        if (lifeIcons == null)
            return;

        int visibleCount = Mathf.Clamp(CurrentHealth, 0, lifeIcons.Count);
        for (int i = 0; i < lifeIcons.Count; i++)
        {
            if (lifeIcons[i] != null)
                lifeIcons[i].SetActive(i < visibleCount);
        }
    }

    public void EndGame()
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        Time.timeScale = 0f;
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
