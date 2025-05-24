using System.Collections.Generic;

public interface IWaveManager
{
    IReadOnlyList<EnemyWave> WaveConfigs { get; }
    int CurrentWaveIndex { get; }
    bool IsWaveActive { get; }
    IReadOnlyList<EnemyController> ActiveEnemies { get; }

    event System.Action OnWaveStateChanged;

    void StartNextWave();
    void StopCurrentWave();
    void ResetWaves();
} 