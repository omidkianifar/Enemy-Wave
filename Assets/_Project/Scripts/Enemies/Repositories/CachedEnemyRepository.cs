using System.Collections.Generic;
using System.Linq;

public class CachedEnemyRepository : IEnemyRepository
{
    private readonly IEnemyRepository repository;
    private readonly Dictionary<EnemyType, EnemyProperties> cache = new();

    public IReadOnlyList<EnemyProperties> Enemies => repository.Enemies;

    public CachedEnemyRepository(IEnemyRepository repository)
    {
        this.repository = repository;
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