using UnityEngine;
using System.Collections.Generic;

public interface IEnemySpawner
{
    EnemyController SpawnEnemy(EnemyType type, Vector3 position);
    List<EnemyController> SpawnWave(EnemyWave waveConfig);
} 