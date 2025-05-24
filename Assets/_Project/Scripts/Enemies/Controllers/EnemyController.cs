using UnityEngine;
using System;

public class EnemyController
{
    private EnemyProperties properties;
    private float currentHealth;
    private bool isInitialized;
    private Vector3 position;
    private Quaternion rotation;

    public event Action<EnemyController> OnDeath;
    public event Action<float> OnHealthChanged;
    public event Action<Vector3> OnPositionChanged;
    public event Action<Quaternion> OnRotationChanged;

    public EnemyType Type => properties?.Type ?? EnemyType.Basic;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => properties?.MaxHealth ?? 0f;
    public float MoveSpeed => properties?.MoveSpeed ?? 0f;
    public bool IsInitialized => isInitialized;
    public Vector3 Position => position;
    public Quaternion Rotation => rotation;

    public void Initialize(EnemyProperties enemyProperties)
    {
        if (enemyProperties == null)
        {
            Debug.LogError("Cannot initialize enemy with null properties!");
            return;
        }

        properties = enemyProperties;
        currentHealth = properties.MaxHealth;
        isInitialized = true;
    }

    public void Move(Vector3 targetPosition)
    {
        if (!isInitialized) return;

        var direction = (targetPosition - position).normalized;
        position += direction * (properties.MoveSpeed * Time.deltaTime);
        rotation = Quaternion.LookRotation(direction);

        OnPositionChanged?.Invoke(position);
        OnRotationChanged?.Invoke(rotation);
    }

    public void TakeDamage(float damage)
    {
        if (!isInitialized) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke(this);
    }
} 