using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VContainer;

public class GameplayHUD : MonoBehaviour
{
    [Header("Game Speed Controls")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button normalSpeedButton;
    [SerializeField] private Button fastSpeedButton;
    [SerializeField] private Button superFastSpeedButton;

    [Header("Wave Controls")]
    [SerializeField] private Button spawnNextWaveButton;

    [Header("Player Stats")]
    [SerializeField] private TextMeshProUGUI healthText;

    private WaveManager waveManager;
    private IGameplayManager gameplayManager;

    [Inject]
    private void Construct(WaveManager waveManager, IGameplayManager gameplayManager)
    {
        this.waveManager = waveManager;
        this.gameplayManager = gameplayManager;
    }

    private void Start()
    {
        InitializeButtons();
        UpdateHealthText();
        gameplayManager.OnHealthChanged += OnHealthChanged;
    }

    private void InitializeButtons()
    {
        // Game Speed Buttons
        pauseButton.onClick.AddListener(OnPauseClicked);
        normalSpeedButton.onClick.AddListener(OnNormalSpeedClicked);
        fastSpeedButton.onClick.AddListener(OnFastSpeedClicked);
        superFastSpeedButton.onClick.AddListener(OnSuperFastSpeedClicked);

        // Wave Control Button
        spawnNextWaveButton.onClick.AddListener(OnSpawnNextWaveClicked);
    }

    private void OnPauseClicked()
    {
        if (gameplayManager.IsPaused)
            gameplayManager.ResumeGame();
        else
            gameplayManager.PauseGame();
    }

    private void OnNormalSpeedClicked()
    {
        gameplayManager.SetGameSpeed(1f);
    }

    private void OnFastSpeedClicked()
    {
        gameplayManager.SetGameSpeed(2f);
    }

    private void OnSuperFastSpeedClicked()
    {
        gameplayManager.SetGameSpeed(3f);
    }

    private void OnSpawnNextWaveClicked()
    {
        waveManager.StartNextWave();
    }

    private void OnHealthChanged(int currentHealth)
    {
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = $"Health: {gameplayManager.PlayerHealth}/{gameplayManager.MaxPlayerHealth}";
    }

    private void OnDestroy()
    {
        // Clean up button listeners
        pauseButton.onClick.RemoveListener(OnPauseClicked);
        normalSpeedButton.onClick.RemoveListener(OnNormalSpeedClicked);
        fastSpeedButton.onClick.RemoveListener(OnFastSpeedClicked);
        superFastSpeedButton.onClick.RemoveListener(OnSuperFastSpeedClicked);
        spawnNextWaveButton.onClick.RemoveListener(OnSpawnNextWaveClicked);

        // Clean up event subscriptions
        if (gameplayManager != null)
        {
            gameplayManager.OnHealthChanged -= OnHealthChanged;
        }
    }
} 