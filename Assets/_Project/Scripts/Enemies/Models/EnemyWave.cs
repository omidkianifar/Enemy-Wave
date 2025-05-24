using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemyWave : MonoBehaviour
{
    private const string FileName = "EnemyWave";

    [Serializable]
    public class Wave
    {
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public int Count { get; private set; }
        [field: SerializeField] public float SpawnDelay { get; private set; } = 1f;
    }

    [Header("Wave Information")]
    [field: SerializeField] public string WaveName { get; private set; }
    [field: SerializeField] public int WaveNumber { get; private set; }
    [field: SerializeField] public float TimeBetweenWaves { get; private set; } = 5f;

    [Header("Enemy Spawn Configuration")]
    [SerializeField] private List<Wave> waves = new();
    public IEnumerable<Wave> Waves => waves;
    
    [Header("Spawn Pattern")]
    [field: SerializeField] public bool RandomizeSpawnOrder { get; private set; }
    [field: SerializeField] public float SpawnRadius { get; private set; } = 10f;
    [field: SerializeField] public Vector3 SpawnCenter { get; private set; } = Vector3.zero;

    [Header("Wave Completion")]
    [field: SerializeField] public float TimeLimit { get; private set; } = 60f;
    [field: SerializeField] public bool WaitForAllEnemiesDefeated { get; private set; } = true;
} 