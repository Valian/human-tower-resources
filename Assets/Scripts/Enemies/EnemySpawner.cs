using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject>     EnemiesPrefabs;
    private List<GameObject>    _spawnedEnemies;
    
    public void SpawnEnemies(int count, int[] enemiesNodesIndexes)
    {
        ClearEnemies();
        _spawnedEnemies = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            var enemy = Instantiate(EnemiesPrefabs[i % EnemiesPrefabs.Count]);
            enemy.transform.parent = transform;
            var enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.Init(GameManager.Instance.GraphManagerInstance.Nodes[enemiesNodesIndexes[i]]);
            _spawnedEnemies.Add(enemy);
        }
    }
    
    public void ClearEnemies()
    {
        if (_spawnedEnemies != null && _spawnedEnemies.Count > 0)
        {
            _spawnedEnemies.ForEach(e => {
                e.gameObject.SetActive(false);
                Destroy(e.gameObject);
            });
        }
    }
}
