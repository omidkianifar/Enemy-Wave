using UnityEngine;

[CreateAssetMenu(fileName = FileName, menuName = "Enemies/Enemy Properties")]
public class EnemyProperties : ScriptableObject
{
    private const string FileName = "EnemyProperties";

    [Header("Basic Properties")]
    [field: SerializeField] public EnemyType Type { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; } = 100f;
    [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
    [field: SerializeField] public float Damage { get; private set; } = 10f;
    [field: SerializeField] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField] public float AttackCooldown { get; private set; } = 1f;

    [Header("Reward Properties")]
    [field: SerializeField] public int ScoreValue { get; private set; } = 100;
    [field: SerializeField] public int CurrencyValue { get; private set; } = 10;

    [Header("Visual Properties")]
    [field: SerializeField] public Color Color { get; private set; } = Color.white;
    [field: SerializeField] public float Scale { get; private set; } = 1f;

    [Header("Special Properties")]
    [field: SerializeField] public bool IsBoss { get; private set; } = false;
    [field: SerializeField] public bool CanFly { get; private set; } = false;
    [field: SerializeField] public bool IsRanged { get; private set; } = false;
} 