using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies
{
    public GameObject fireEnemy;
    public GameObject waterEnemy;
    public GameObject airEnemy;
    public GameObject earthEnemy;
}

[CreateAssetMenu(menuName = "Singletons/EnemySpawner", fileName = "EnemySpawner")]
public class EnemySpawner : yaSingleton.Singleton<EnemySpawner>
{
    private GameObject[] formationPrefabs;
    private Camera _camera;
    private EnemyManager enemyManager;
    private GameObject player;
    private Enemies enemies;
    protected override void Initialize()
    {
        base.Initialize();
        formationPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemy Formations");
        _camera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("player " + player == null);
        enemies = new Enemies() { };
        enemies.fireEnemy = Resources.Load<GameObject>($"{Utils.Constants.EnemiesFolder}Fire Enemy");
       
        enemyManager = new EnemyManager(player, enemies);
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
    }

    private void SpawnFormation()
    {
        var newPosition = _camera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1),
                    Random.Range(0, 1), -_camera.transform.position.z));
    }
}
