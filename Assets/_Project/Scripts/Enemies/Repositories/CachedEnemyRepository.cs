using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CachedEnemyRepository : IEnemyRepository
{
    private readonly IEnemyRepository repository;
    private readonly Dictionary<EnemyType, EnemyProperties> cache = new();

    public IReadOnlyList<EnemyProperties> Enemies => repository.Enemies;

    public CachedEnemyRepository()
    {
        repository = EnemyRepository.Load();
        if (repository == null)
        {
            Debug.LogError("Failed to load EnemyRepository!");
            return;
        }
        InitializeCache();
    }

    private void InitializeCache()
    {
        foreach (var enemy in repository.Enemies)
        {
            cache[enemy.Type] = enemy;
        }
    }

    public EnemyProperties GetEnemyByType(EnemyType type)
    {
        if (cache.TryGetValue(type, out var properties))
        {
            return properties;
        }

        var enemy = repository.GetEnemyByType(type);
        if (enemy != null)
        {
            cache[type] = enemy;
        }

        return enemy;
    }
} 