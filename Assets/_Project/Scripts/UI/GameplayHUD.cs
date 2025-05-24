using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VContainer;

public class GameplayHUD : MonoBehaviour
{
    [Header("Game Speed Controls")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private Button normalSpeedButton;
    [SerializeField] private Button fastSpeedButton;
    [SerializeField] private Button superFastSpeedButton;

    [Header("Wave Controls")]
    [SerializeField] private Button spawnNextWaveButton;
    [SerializeField] private TextMeshProUGUI waveInfoText;

    [Header("Player Stats")]
    [SerializeField] private TextMeshProUGUI healthText;

    private IWaveManager waveManager;
    private IGameplayManager gameplayManager;

    [Inject]
    private void Construct(IWaveManager waveManager, IGameplayManager gameplayManager)
    {
        this.waveManager = waveManager;
        this.gameplayManager = gameplayManager;
    }

    private void Start()
    {
        InitializeButtons();
        UpdateHealthText();
        UpdateWaveInfo();
        UpdateSpawnButtonState();
        UpdatePauseButtonText();
        gameplayManager.OnHealthChanged += OnHealthChanged;
        gameplayManager.OnPauseStateChanged += OnPauseStateChanged;
        waveManager.OnWaveStateChanged += OnWaveStateChanged;
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

    private void OnPauseStateChanged(bool isPaused)
    {
        UpdatePauseButtonText();
    }

    private void UpdatePauseButtonText()
    {
        pauseButtonText.text = gameplayManager.IsPaused ? "Resume" : "Pause";
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
        gameplayManager.SetGameSpeed(4f);
    }

    private void OnSpawnNextWaveClicked()
    {
        waveManager.StartNextWave();
        UpdateSpawnButtonState();
        UpdateWaveInfo();
    }

    private void OnHealthChanged(int currentHealth)
    {
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = $"Health: {gameplayManager.PlayerHealth}/{gameplayManager.MaxPlayerHealth}";
    }

    private void UpdateWaveInfo()
    {
        if (waveManager.WaveConfigs.Count == 0)
        {
            waveInfoText.text = "No waves available";
            return;
        }

        // Check if all waves are completed
        if (waveManager.CurrentWaveIndex >= waveManager.WaveConfigs.Count - 1 && !waveManager.IsWaveActive)
        {
            waveInfoText.text = "Finished";
            return;
        }

        int waveIndex = waveManager.CurrentWaveIndex;
        if (waveIndex < 0) waveIndex = 0; // Show first wave if no wave has started
        else if (!waveManager.IsWaveActive) waveIndex++; // Show next wave if current wave is done

        var wave = waveManager.WaveConfigs[waveIndex];
        string waveInfo = $"Wave {wave.WaveNumber}:\n";

        foreach (var enemyWave in wave.Waves)
        {
            waveInfo += $"{enemyWave.Type}: {enemyWave.Count}\n";
        }

        waveInfoText.text = waveInfo;
    }

    private void UpdateSpawnButtonState()
    {
        spawnNextWaveButton.interactable = !waveManager.IsWaveActive && 
                                         waveManager.CurrentWaveIndex < waveManager.WaveConfigs.Count - 1;
    }

    private void OnWaveStateChanged()
    {
        UpdateWaveInfo();
        UpdateSpawnButtonState();
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
            gameplayManager.OnPauseStateChanged -= OnPauseStateChanged;
        }
        if (waveManager != null)
        {
            waveManager.OnWaveStateChanged -= OnWaveStateChanged;
        }
    }
} 