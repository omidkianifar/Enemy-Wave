using UnityEngine;
using System;
using VContainer;

public class EnemyController
{
    private EnemyProperties properties;
    private float currentHealth;
    private bool isInitialized;
    private Vector3 position;
    private Quaternion rotation;
    private IPathFinder pathFinder;
    private IGameplayManager gameplayManager;
    private int currentPathIndex;
    private Vector3 currentTarget;
    private bool isMoving = false;
    private float rotationSpeed = 10f;
    private float moveSpeedMultiplier = 1f;
    private Vector3 previousPosition;

    public event Action<EnemyController> OnDeath;
    public event Action<EnemyController> OnReachedEnd;
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

    [Inject]
    public void Construct(IPathFinder pathFinder, IGameplayManager gameplayManager)
    {
        this.pathFinder = pathFinder;
        this.gameplayManager = gameplayManager;
    }

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
        position = pathFinder.StartPoint;
        previousPosition = position;
        OnPositionChanged?.Invoke(position);

        if (pathFinder.IsValid)
        {
            currentPathIndex = 0;
            currentTarget = pathFinder.GetPointPosition(0);
            isMoving = true;
        }
    }

    public void Tick()
    {
        if (!isMoving || !pathFinder.IsValid) return;

        // Store previous position for overshoot detection
        previousPosition = position;

        // Calculate direction and distance to target
        Vector3 direction = (currentTarget - position).normalized;
        float distanceToTarget = Vector3.Distance(position, currentTarget);

        // Calculate movement speed (multiply all scalar values first)
        float movementSpeed = MoveSpeed * moveSpeedMultiplier * Time.fixedDeltaTime * gameplayManager.GameSpeed;
        
        // Apply movement to position
        position += direction * movementSpeed;
        
        // Calculate rotation speed (multiply scalar values first)
        float currentRotationSpeed = rotationSpeed * Time.fixedDeltaTime * gameplayManager.GameSpeed;
        
        // Smoothly rotate towards movement direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rotation = Quaternion.Slerp(rotation, targetRotation, currentRotationSpeed);
        }
        
        OnPositionChanged?.Invoke(position);
        OnRotationChanged?.Invoke(rotation);

        // Check if we've reached or passed the current target
        bool hasReachedTarget = distanceToTarget < 0.1f;
        bool hasPassedTarget = Vector3.Dot(currentTarget - previousPosition, currentTarget - position) < 0;

        if (hasReachedTarget || hasPassedTarget)
        {
            currentPathIndex++;
            
            // Check if we've reached the end of the path
            if (currentPathIndex >= pathFinder.PathLength)
            {
                isMoving = false;
                OnReachedEnd?.Invoke(this);
                Die(); // Destroy the enemy when reaching the end
                return;
            }

            // Set next target
            currentTarget = pathFinder.GetPointPosition(currentPathIndex);
        }
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
        isMoving = false;
        OnDeath?.Invoke(this);
    }

    public void SetMoveSpeedMultiplier(float multiplier)
    {
        moveSpeedMultiplier = Mathf.Max(0, multiplier);
    }
} 