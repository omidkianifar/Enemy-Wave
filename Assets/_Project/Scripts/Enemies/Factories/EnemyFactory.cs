using UnityEngine;
using VContainer;

public interface IEnemyFactory
{
    EnemyController CreateEnemy(EnemyProperties properties, Vector3 position, Transform parent);
}

public class EnemyFactory : IEnemyFactory
{
    private readonly IObjectResolver container;

    [Inject]
    public EnemyFactory(IObjectResolver container)
    {
        this.container = container;
    }

    public EnemyController CreateEnemy(EnemyProperties properties, Vector3 position, Transform parent)
    {
        if (properties.Prefab == null)
        {
            Debug.LogError($"Enemy prefab is missing in properties for type: {properties.Type}");
            return null;
        }

        var enemyObject = Object.Instantiate(properties.Prefab, position, Quaternion.identity, parent);
        var enemyView = enemyObject.GetComponent<EnemyView>();
        
        if (enemyView == null)
        {
            Debug.LogError("Enemy prefab is missing EnemyView component!");
            Object.Destroy(enemyObject);
            return null;
        }

        // Resolve dependencies for the controller
        var controller = container.Resolve<EnemyController>();
        controller.Initialize(properties);
        enemyView.Initialize(controller, properties);

        return controller;
    }
} 