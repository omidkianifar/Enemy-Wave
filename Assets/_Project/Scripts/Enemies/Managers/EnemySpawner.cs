using UnityEngine;
using System.Collections.Generic;
using VContainer;

// TODO: Implement object pooling for better performance
public class EnemySpawner : MonoBehaviour, IEnemySpawner
{

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnParent;

    private IEnemyRepository enemyRepository;

    [Inject]
    public void Construct(IEnemyRepository enemyRepository)
    {
        this.enemyRepository = enemyRepository;
    }

    public EnemyController SpawnEnemy(EnemyType type, Vector3 position)
    {
        var properties = enemyRepository.GetEnemyByType(type);
        if (properties == null)
        {
            Debug.LogError($"No properties found for enemy type: {type}");
            return null;
        }

        var enemyObject = Instantiate(properties.Prefab, position, Quaternion.identity, spawnParent);
        var enemyView = enemyObject.GetComponent<EnemyView>();
        if (enemyView == null)
        {
            Debug.LogError("Enemy prefab is missing EnemyView component!");
            Destroy(enemyObject);
            return null;
        }

        var controller = new EnemyController();
        controller.Initialize(properties);
        enemyView.Initialize(controller, properties);

        return controller;
    }

    public List<EnemyController> SpawnWave(EnemyWave waveConfig)
    {
        var controllers = new List<EnemyController>();
        var waves = waveConfig.Waves;

        foreach (var wave in waves)
        {
            for (int i = 0; i < wave.Count; i++)
            {
                var spawnPosition = GetRandomSpawnPosition();
                var controller = SpawnEnemy(wave.Type, spawnPosition);
                
                if (controller != null)
                {
                    controllers.Add(controller);
                }
            }
        }

        return controllers;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var randomPoint = Random.insideUnitCircle * 10f; // Default spawn radius
        return new Vector3(randomPoint.x, 0f, randomPoint.y);
    }
} 