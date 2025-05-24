using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SceneContainer : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // Register repositories
        builder.Register<IEnemyRepository, CachedEnemyRepository>(Lifetime.Singleton)
            .As<IEnemyRepository>();

        // Register components with interfaces
        builder.RegisterComponentInHierarchy<EnemySpawner>()
            .As<IEnemySpawner>();
            
        builder.RegisterComponentInHierarchy<WaveManager>()
            .As<IWaveManager>();
    }
}