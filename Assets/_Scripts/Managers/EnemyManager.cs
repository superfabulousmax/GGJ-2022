using UnityEngine;
using System.Collections;
using UnityEngine.Pool;

public class EnemyManager
{
    private GameObject _player;
    private GameObject _enemyPrefab;
    private ObjectPool<EnemySeekController> _enemyPool;
    private GameObject _enemyHolder;


    public EnemyManager(GameObject player, Enemies enemies)
    {
        _player = player;
        _enemyPrefab = enemies.fireEnemy;
        _enemyHolder = GameObject.Instantiate(new GameObject());

        _enemyPool = new ObjectPool<EnemySeekController>(
            createFunc: () => GameObject.Instantiate(_enemyPrefab, _enemyHolder.transform).GetComponent<EnemySeekController>(),
            actionOnGet: enemy => enemy.gameObject.SetActive(true),
            actionOnRelease: enemy => enemy.gameObject.SetActive(false),
            actionOnDestroy: enemy => GameObject.Destroy(enemy),
            collectionCheck: false,
            defaultCapacity: Utils.Constants.MaxEnemies,
            maxSize: Utils.Constants.MaxEnemies
        );
    }

    public GameObject Spawn(Vector3 spawnPosition)
    {
        var enemy = _enemyPool.Get();
        enemy.SpawnAndSeek(spawnPosition, _player.transform);

        return enemy.gameObject;
    }
}