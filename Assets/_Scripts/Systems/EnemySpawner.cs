using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private float timer;
    private float coolDown = 10;
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

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if(timer >= coolDown)
        {
            SpawnFormation();
        }
    }

    private GameObject GetRandomFormation()
    {
        var index = Random.Range(0, formationPrefabs.Length);
        return formationPrefabs[index];
    }

    private void SpawnFormation()
    {
        var formation = GetRandomFormation();
        var newPosition = _camera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1),
                    Random.Range(0, 1), -_camera.transform.position.z));
    }
}
