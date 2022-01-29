using UnityEngine;
using System.Collections;
using UnityEngine.Pool;

public class EnemyManager
{
    private GameObject _player;
    private ObjectPool<EnemySeekController> _fireEnemyPool;
    private ObjectPool<EnemySeekController> _waterEnemyPool;
    private ObjectPool<EnemySeekController> _earthEnemyPool;
    private ObjectPool<EnemySeekController> _airEnemyPool;
    private GameObject _enemyHolder;
    private Enemies _enemies;

    public EnemyManager(GameObject player, Enemies enemies, GameObject enemyHolder)
    {
        _player = player;
        _enemies = enemies;
        _enemyHolder = enemyHolder;
        _fireEnemyPool = new ObjectPool<EnemySeekController>(
            createFunc: () => GameObject.Instantiate(_enemies.fireEnemy, _enemyHolder.transform).GetComponent<EnemySeekController>(),
            actionOnGet: enemy => enemy.gameObject.SetActive(true),
            actionOnRelease: enemy => enemy.gameObject.SetActive(false),
            actionOnDestroy: enemy => GameObject.Destroy(enemy),
            collectionCheck: false,
            defaultCapacity: Utils.Constants.MaxEnemies/4,
            maxSize: Utils.Constants.MaxEnemies/4
        );

        _waterEnemyPool = new ObjectPool<EnemySeekController>(
            createFunc: () => GameObject.Instantiate(_enemies.waterEnemy, _enemyHolder.transform).GetComponent<EnemySeekController>(),
            actionOnGet: enemy => enemy.gameObject.SetActive(true),
            actionOnRelease: enemy => enemy.gameObject.SetActive(false),
            actionOnDestroy: enemy => GameObject.Destroy(enemy),
            collectionCheck: false,
            defaultCapacity: Utils.Constants.MaxEnemies/4,
            maxSize: Utils.Constants.MaxEnemies/4
        );
        _earthEnemyPool = new ObjectPool<EnemySeekController>(
            createFunc: () => GameObject.Instantiate(_enemies.earthEnemy, _enemyHolder.transform).GetComponent<EnemySeekController>(),
            actionOnGet: enemy => enemy.gameObject.SetActive(true),
            actionOnRelease: enemy => enemy.gameObject.SetActive(false),
            actionOnDestroy: enemy => GameObject.Destroy(enemy),
            collectionCheck: false,
            defaultCapacity: Utils.Constants.MaxEnemies/4,
            maxSize: Utils.Constants.MaxEnemies/4
        );
        _airEnemyPool = new ObjectPool<EnemySeekController>(
            createFunc: () => GameObject.Instantiate(_enemies.airEnemy, _enemyHolder.transform).GetComponent<EnemySeekController>(),
            actionOnGet: enemy => enemy.gameObject.SetActive(true),
            actionOnRelease: enemy => enemy.gameObject.SetActive(false),
            actionOnDestroy: enemy => GameObject.Destroy(enemy),
            collectionCheck: false,
            defaultCapacity: Utils.Constants.MaxEnemies / 4,
            maxSize: Utils.Constants.MaxEnemies / 4
        );
    }

    public GameObject SpawnEnemy(Vector3 spawnPosition, Utils.Elements element)
    {
        var enemy = FromElement(element);
        enemy.SpawnAndSeek(spawnPosition, _player.transform);

        return enemy.gameObject;
    }


    public EnemySeekController FromElement(Utils.Elements element)
    {
        switch(element)
        {
            case Utils.Elements.Fire:
                return _fireEnemyPool.Get();
            case Utils.Elements.Water:
                return _waterEnemyPool.Get();
            case Utils.Elements.Earth:
                return _earthEnemyPool.Get();
            case Utils.Elements.Air:
                return _airEnemyPool.Get();
            default:
                return null;
        }
    }
}