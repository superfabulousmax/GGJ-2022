using UnityEngine;
using System.Collections;
using UnityEngine.Pool;
using System;
using Utils;

public class EnemyManager
{
    private GameObject _player;
    private ObjectPool<EnemySeekController> _fireEnemyPool;
    private ObjectPool<EnemySeekController> _waterEnemyPool;
    private ObjectPool<EnemySeekController> _earthEnemyPool;
    private ObjectPool<EnemySeekController> _airEnemyPool;
    private GameObject _enemyHolder;
    private Enemies _enemies;
    private int totalKilled;
    private int totalHealed;
    private int totalDamage;
    public event Action<int> onKill = (int amount) => { };
    public event Action<int> onHeal = (int amount) => { };
    public event Action<int> onDamage = (int amount) => { };
    private SoundFX _sound;

    public EnemyManager(GameObject player, Enemies enemies, GameObject enemyHolder, SoundFX sound)
    {
        _player = player;
        _enemies = enemies;
        _enemyHolder = enemyHolder;
        _sound = sound;
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

    public void OnKill(int amount)
    {
        totalKilled += amount;
        onKill?.Invoke(totalKilled);
        Debug.Log($"Total Killed {totalKilled}");
    }

    public void OnDamage(int amount)
    {
        totalDamage += amount;
        onDamage?.Invoke(totalDamage);
        Debug.Log($"Total damaged {totalDamage}");
    }
    public void OnHeal(int amount)
    {
        totalHealed += amount;
        onHeal?.Invoke(totalHealed);
        Debug.Log($"Total healed {totalHealed}");
    }

    public int GetTotalActiveEnemies()
    {
        return _fireEnemyPool.CountActive + _waterEnemyPool.CountActive + _earthEnemyPool.CountActive + _airEnemyPool.CountActive;
    }

    public GameObject SpawnEnemy(Vector3 spawnPosition, Elements element, GameObject damageVFX, GameObject healVFX)
    {
        var enemy = FromElement(element);
        enemy.SpawnAndSeek(this, spawnPosition, _player.transform, element, damageVFX, healVFX, _sound.audioSource, PrimaryDamageClipForElement(element), SecondaryDamageClipForElement(element), _sound.heal);

        return enemy.gameObject;
    }

    public AudioClip PrimaryDamageClipForElement(Elements element)
    {
        switch (element)
        {
            case Elements.Fire:
                return _sound.firePrimaryDamage;
            case Elements.Water:
                return _sound.waterPrimaryDamage;
            case Elements.Earth:
                return _sound.earthPrimaryDamage;
            case Elements.Air:
                return _sound.airPrimaryDamage;
            default:
                return null;
        }
    }

    public AudioClip SecondaryDamageClipForElement(Elements element)
    {
        switch (element)
        {
            case Elements.Fire:
                return _sound.fireSecondaryDamage;
            case Elements.Water:
                return _sound.waterSecondaryDamage;
            case Elements.Earth:
                return _sound.earthSecondaryDamage;
            case Elements.Air:
                return _sound.airSecondaryDamage;
            default:
                return null;
        }
    }


    public EnemySeekController FromElement(Utils.Elements element)
    {
        switch(element)
        {
            case Elements.Fire:
                return _fireEnemyPool.Get();
            case Elements.Water:
                return _waterEnemyPool.Get();
            case Elements.Earth:
                return _earthEnemyPool.Get();
            case Elements.Air:
                return _airEnemyPool.Get();
            default:
                return null;
        }
    }
}