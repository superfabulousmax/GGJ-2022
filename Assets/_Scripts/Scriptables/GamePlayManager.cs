using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "GameManager", menuName = "Singletons/GameManager")]
public class GamePlayManager : yaSingleton.Singleton<GamePlayManager>
{
    private GameObject _playerPrefab;
    private GameObject _enemyPrefab;

    private GameObject _player;
    private EnemyManager _enemyManager;

    // Units
    // Events
    public event Action sampleEvent = () => { };

    public void CallEvent()
    {
        sampleEvent();
    }
    protected override void Initialize()
    {
        base.Initialize();
        Debug.Log("Initializing the GamePlayManager");
        _playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        _enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");

        // Initialization code
        _player = Instantiate(_playerPrefab);
        SetupEnemy();
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
    }

    private void SetupEnemy()
    {
        _enemyManager = new EnemyManager(_player, _enemyPrefab);

        // Example spawning
        _enemyManager.Spawn(new Vector2(4f, 0));
        _enemyManager.Spawn(new Vector2(-4f, 0));
        _enemyManager.Spawn(new Vector2(0, 4f));
        _enemyManager.Spawn(new Vector2(0, -4f));

        _enemyManager.Spawn(new Vector2(2f, 2f));
        _enemyManager.Spawn(new Vector2(-2f, 2f));
        _enemyManager.Spawn(new Vector2(2f, -2f));
        _enemyManager.Spawn(new Vector2(-2f, -2f));
    }
}