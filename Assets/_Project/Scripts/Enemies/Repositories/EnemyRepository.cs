using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = FileName, menuName = "Enemies/Enemy Repository")]
public partial class EnemyRepository : ScriptableObject, IEnemyRepository
{
    private const string FileName = "EnemyRepository";
    private static EnemyRepository instance;

    [SerializeField] private List<EnemyProperties> enemies = new();

    public IReadOnlyList<EnemyProperties> Enemies => enemies;

    public static EnemyRepository Load()
    {
        if (instance != null) return instance;

        instance = Resources.Load<EnemyRepository>(FileName);
        if (instance == null)
        {
            Debug.LogError($"EnemyRepository not found in Resources folder at path: {FileName}");
        }
        return instance;
    }

    public EnemyProperties GetEnemyByType(EnemyType type)
    {
        return enemies.FirstOrDefault(e => e.Type == type);
    }

    public IEnumerable<EnemyProperties> GetEnemiesByTypes(params EnemyType[] types)
    {
        return enemies.Where(e => types.Contains(e.Type));
    }

    public IEnumerable<EnemyProperties> GetBossEnemies()
    {
        return enemies.Where(e => e.IsBoss);
    }

    public IEnumerable<EnemyProperties> GetFlyingEnemies()
    {
        return enemies.Where(e => e.CanFly);
    }

    public IEnumerable<EnemyProperties> GetRangedEnemies()
    {
        return enemies.Where(e => e.IsRanged);
    }

    public void AddEnemy(EnemyProperties enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void RemoveEnemy(EnemyProperties enemy)
    {
        enemies.Remove(enemy);
    }

    public void ClearEnemies()
    {
        enemies.Clear();
    }
} 