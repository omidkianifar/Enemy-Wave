using UnityEngine;
using VContainer;

public interface IGameplayManager
{
    float GameSpeed { get; }
    bool IsPaused { get; }
    int PlayerHealth { get; }
    int MaxPlayerHealth { get; }
    
    event System.Action<float> OnGameSpeedChanged;
    event System.Action<bool> OnPauseStateChanged;
    event System.Action<int> OnHealthChanged;
    
    void SetGameSpeed(float speed);
    void PauseGame();
    void ResumeGame();
    void TakeDamage(int damage);
    void Heal(int amount);
}

public class GameplayManager : MonoBehaviour, IGameplayManager
{
    [Header("Player Settings")]
    [SerializeField] private int maxPlayerHealth = 100;
    [SerializeField] private int initialPlayerHealth = 100;

    private float gameSpeed = 1f;
    private bool isPaused;
    private int playerHealth;

    public float GameSpeed => gameSpeed;
    public bool IsPaused => isPaused;
    public int PlayerHealth => playerHealth;
    public int MaxPlayerHealth => maxPlayerHealth;

    public event System.Action<float> OnGameSpeedChanged;
    public event System.Action<bool> OnPauseStateChanged;
    public event System.Action<int> OnHealthChanged;

    private void Start()
    {
        playerHealth = initialPlayerHealth;
        OnHealthChanged?.Invoke(playerHealth);
    }

    public void SetGameSpeed(float speed)
    {
        if (speed < 0) return;
        
        gameSpeed = speed;
        Time.timeScale = isPaused ? 0 : gameSpeed;
        OnGameSpeedChanged?.Invoke(gameSpeed);
    }

    public void PauseGame()
    {
        if (isPaused) return;
        
        isPaused = true;
        Time.timeScale = 0;
        OnPauseStateChanged?.Invoke(true);
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        
        isPaused = false;
        Time.timeScale = gameSpeed;
        OnPauseStateChanged?.Invoke(false);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        playerHealth = Mathf.Max(0, playerHealth - damage);
        OnHealthChanged?.Invoke(playerHealth);

        if (playerHealth <= 0)
        {
            // TODO: Handle game over
            Debug.Log("Game Over!");
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        playerHealth = Mathf.Min(maxPlayerHealth, playerHealth + amount);
        OnHealthChanged?.Invoke(playerHealth);
    }

    private void OnDestroy()
    {
        // Reset time scale when the game is destroyed
        Time.timeScale = 1f;
    }
} 