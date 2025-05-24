using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VContainer;

public class WaveManager : MonoBehaviour, IWaveManager
{
    [Header("Wave Configuration")]
    [SerializeField] private List<EnemyWave> waveConfigs = new();
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private bool autoStart = true;

    private int currentWaveIndex = -1;
    private bool isWaveActive;
    private Coroutine waveCoroutine;
    private IEnemySpawner spawner;
    private IPathFinder pathFinder;
    private IEnemyManager enemyManager;
    private readonly List<EnemyController> activeEnemies = new();

    public event System.Action OnWaveStateChanged;

    public IReadOnlyList<EnemyWave> WaveConfigs => waveConfigs;
    public int CurrentWaveIndex => currentWaveIndex;
    public bool IsWaveActive => isWaveActive;
    public IReadOnlyList<EnemyController> ActiveEnemies => activeEnemies;

    [Inject]
    public void Construct(IEnemySpawner spawner, IPathFinder pathFinder, IEnemyManager enemyManager)
    {
        this.spawner = spawner;
        this.pathFinder = pathFinder;
        this.enemyManager = enemyManager;
    }

    private void Start()
    {
        if (autoStart)
        {
            StartNextWave();
        }
    }

    public void StartNextWave()
    {
        if (isWaveActive) return;

        currentWaveIndex++;
        if (currentWaveIndex >= waveConfigs.Count)
        {
            Debug.Log("All waves completed!");
            return;
        }

        waveCoroutine = StartCoroutine(SpawnWave(waveConfigs[currentWaveIndex]));
    }

    private IEnumerator SpawnWave(EnemyWave waveConfig)
    {
        isWaveActive = true;
        OnWaveStateChanged?.Invoke();
        var waves = waveConfig.Waves.ToList();

        if (waveConfig.RandomizeSpawnOrder)
        {
            waves = waves.OrderBy(x => Random.value).ToList();
        }

        foreach (var wave in waves)
        {
            for (int i = 0; i < wave.Count; i++)
            {
                if (!pathFinder.IsValid)
                {
                    Debug.LogError("PathFinder is not valid!");
                    yield break;
                }

                var controller = spawner.SpawnEnemy(wave.Type, pathFinder.StartPoint);
                
                if (controller != null)
                {
                    activeEnemies.Add(controller);
                    enemyManager.AddEnemy(controller);
                    controller.OnDeath += HandleEnemyDeath;
                }

                yield return new WaitForSeconds(wave.SpawnDelay);
            }
        }

        if (waveConfig.WaitForAllEnemiesDefeated)
        {
            yield return new WaitUntil(() => activeEnemies.Count == 0);
        }

        isWaveActive = false;
        OnWaveStateChanged?.Invoke();
        yield return new WaitForSeconds(timeBetweenWaves);
    }

    private void HandleEnemyDeath(EnemyController enemy)
    {
        activeEnemies.Remove(enemy);
        enemy.OnDeath -= HandleEnemyDeath;
        enemyManager.RemoveEnemy(enemy);
    }

    public void StopCurrentWave()
    {
        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
            isWaveActive = false;
        }
    }

    public void ResetWaves()
    {
        StopCurrentWave();
        currentWaveIndex = -1;
        isWaveActive = false;
        activeEnemies.Clear();
    }

    private void OnDestroy()
    {
        // Clean up event subscriptions
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null)
            {
                enemy.OnDeath -= HandleEnemyDeath;
            }
        }
        activeEnemies.Clear();
    }
} 