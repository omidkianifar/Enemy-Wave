using UnityEngine;

public class EnemyView : MonoBehaviour
{
    private EnemyController controller;
    private GameObject visualInstance;

    public void Initialize(EnemyController enemyController, EnemyProperties properties)
    {
        controller = enemyController;
        
        // Subscribe to controller events
        controller.OnPositionChanged += HandlePositionChanged;
        controller.OnRotationChanged += HandleRotationChanged;
        controller.OnDeath += HandleDeath;

        // Create visual representation
        if (properties.Prefab != null)
        {
            visualInstance = Instantiate(properties.Prefab, transform);
            visualInstance.transform.localPosition = Vector3.zero;
            visualInstance.transform.localRotation = Quaternion.identity;
        }

        transform.localScale = Vector3.one * properties.Scale;
    }

    private void HandlePositionChanged(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private void HandleRotationChanged(Quaternion newRotation)
    {
        transform.rotation = newRotation;
    }

    private void HandleDeath(EnemyController _)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (controller != null)
        {
            controller.OnPositionChanged -= HandlePositionChanged;
            controller.OnRotationChanged -= HandleRotationChanged;
            controller.OnDeath -= HandleDeath;
        }
    }
} 