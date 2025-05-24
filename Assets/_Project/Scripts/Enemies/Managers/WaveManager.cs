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

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private float spawnRadius = 10f;

    private int currentWaveIndex = -1;
    private bool isWaveActive;
    private Coroutine waveCoroutine;
    private IEnemySpawner spawner;
    private readonly List<EnemyController> activeEnemies = new();

    public IReadOnlyList<EnemyWave> WaveConfigs => waveConfigs;
    public int CurrentWaveIndex => currentWaveIndex;
    public bool IsWaveActive => isWaveActive;
    public IReadOnlyList<EnemyController> ActiveEnemies => activeEnemies;

    [Inject]
    public void Construct(IEnemySpawner spawner)
    {
        this.spawner = spawner;
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
        var waves = waveConfig.Waves.ToList();

        if (waveConfig.RandomizeSpawnOrder)
        {
            waves = waves.OrderBy(x => Random.value).ToList();
        }

        foreach (var wave in waves)
        {
            for (int i = 0; i < wave.Count; i++)
            {
                var spawnPosition = GetRandomSpawnPosition();
                var controller = spawner.SpawnEnemy(wave.Type, spawnPosition);
                
                if (controller != null)
                {
                    activeEnemies.Add(controller);
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
        yield return new WaitForSeconds(timeBetweenWaves);
    }

    private void HandleEnemyDeath(EnemyController controller)
    {
        activeEnemies.Remove(controller);
        controller.OnDeath -= HandleEnemyDeath;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var center = spawnCenter != null ? spawnCenter.position : transform.position;
        var randomPoint = Random.insideUnitCircle * spawnRadius;
        return new Vector3(center.x + randomPoint.x, center.y, center.z + randomPoint.y);
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
} 