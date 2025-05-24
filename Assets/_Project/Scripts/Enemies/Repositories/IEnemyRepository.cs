using System.Collections.Generic;

public interface IEnemyRepository
{
    IReadOnlyList<EnemyProperties> Enemies { get; }
    EnemyProperties GetEnemyByType(EnemyType type);
} 