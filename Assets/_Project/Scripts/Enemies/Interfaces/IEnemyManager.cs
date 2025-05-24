using UnityEngine;

public interface IEnemyManager
{
    void AddEnemy(EnemyController enemy);
    void RemoveEnemy(EnemyController enemy);
} 