using UnityEngine;
using System.Collections.Generic;
using VContainer;

// TODO: Implement object pooling for better performance
public class EnemySpawner : MonoBehaviour, IEnemySpawner
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnParent;

    private IEnemyRepository enemyRepository;
    private IEnemyFactory enemyFactory;
    private IPathFinder pathFinder;

    [Inject]
    public void Construct(IEnemyRepository enemyRepository, IEnemyFactory enemyFactory, IPathFinder pathFinder)
    {
        this.enemyRepository = enemyRepository;
        this.enemyFactory = enemyFactory;
        this.pathFinder = pathFinder;
    }

    public EnemyController SpawnEnemy(EnemyType type, Vector3 position)
    {
        var properties = enemyRepository.GetEnemyByType(type);
        if (properties == null)
        {
            Debug.LogError($"No properties found for enemy type: {type}");
            return null;
        }

        return enemyFactory.CreateEnemy(properties, position, spawnParent);
    }

    public List<EnemyController> SpawnWave(EnemyWave waveConfig)
    {
        var controllers = new List<EnemyController>();
        var waves = waveConfig.Waves;

        if (!pathFinder.IsValid)
        {
            Debug.LogError("Cannot spawn wave: PathFinder is not valid!");
            return controllers;
        }

        foreach (var wave in waves)
        {
            for (int i = 0; i < wave.Count; i++)
            {
                var controller = SpawnEnemy(wave.Type, pathFinder.StartPoint);
                
                if (controller != null)
                {
                    controllers.Add(controller);
                }
            }
        }

        return controllers;
    }
} 