#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Linq;

public partial class EnemyRepository
{

    [ContextMenu("Find All Enemy Properties")]
    private void FindAllEnemyProperties()
    {
        var guids = AssetDatabase.FindAssets("t:EnemyProperties");
        enemies.Clear();

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var enemyProperties = AssetDatabase.LoadAssetAtPath<EnemyProperties>(path);
            if (enemyProperties != null)
            {
                enemies.Add(enemyProperties);
            }
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    [ContextMenu("Validate Enemy Properties")]
    private void ValidateEnemyProperties()
    {
        var invalidEnemies = enemies.Where(e => e == null).ToList();
        foreach (var invalid in invalidEnemies)
        {
            enemies.Remove(invalid);
        }

        if (invalidEnemies.Count > 0)
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
} 

#endif
