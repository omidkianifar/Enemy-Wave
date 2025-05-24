using UnityEngine;
using System.Collections.Generic;
using VContainer;

public class EnemyManager : MonoBehaviour, IEnemyManager
{
    private readonly List<EnemyController> activeEnemies = new();
    private IGameplayManager gameplayManager;

    public event System.Action<EnemyController> OnEnemyReachedEnd;

    [Inject]
    private void Construct(IGameplayManager gameplayManager)
    {
        this.gameplayManager = gameplayManager;
    }

    public void AddEnemy(EnemyController enemy)
    {
        if (enemy != null && !activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
            enemy.OnDeath += HandleEnemyDeath;
            enemy.OnReachedEnd += HandleEnemyReachedEnd;
        }
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        if (enemy != null)
        {
            activeEnemies.Remove(enemy);
            enemy.OnDeath -= HandleEnemyDeath;
            enemy.OnReachedEnd -= HandleEnemyReachedEnd;
        }
    }

    private void HandleEnemyDeath(EnemyController enemy)
    {
        RemoveEnemy(enemy);
    }

    private void HandleEnemyReachedEnd(EnemyController enemy)
    {
        gameplayManager.TakeDamage(1);
        OnEnemyReachedEnd?.Invoke(enemy);
        RemoveEnemy(enemy);
    }

    private void FixedUpdate()
    {
        if (gameplayManager.IsPaused) return;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] != null)
            {
                activeEnemies[i].Tick();
            }
        }
    }

    private void OnDestroy()
    {
        // Clean up event subscriptions
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null)
            {
                enemy.OnDeath -= HandleEnemyDeath;
                enemy.OnReachedEnd -= HandleEnemyReachedEnd;
            }
        }
        activeEnemies.Clear();
    }
} 