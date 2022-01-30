using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class Enemies
{
    public GameObject fireEnemy;
    public GameObject waterEnemy;
    public GameObject airEnemy;
    public GameObject earthEnemy;
}
public class EnemySpawner : MonoBehaviour
{
    private GameObject[] formationPrefabs;
    private Camera _camera;
    private EnemyManager enemyManager;
    private GameObject _player;
    private Enemies enemies;
    private float timer;
    private float coolDown = 3;
    private GameObject _enemyHolder;
    private int sortingOrder = 0;
    public void Init(GameObject player)
    {
        timer = 0;
        formationPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemy Formations");
        _enemyHolder = GameObject.Instantiate(new GameObject());
        _camera = Camera.main;
        _player = player;
        enemies = new Enemies() { };
        enemies.fireEnemy = Resources.Load<GameObject>($"{Constants.EnemiesFolder}Fire Enemy");
        enemies.waterEnemy = Resources.Load<GameObject>($"{Constants.EnemiesFolder}Water Enemy");
        enemies.airEnemy = Resources.Load<GameObject>($"{Constants.EnemiesFolder}Air Enemy");
        enemies.earthEnemy = Resources.Load<GameObject>($"{Constants.EnemiesFolder}Earth Enemy");
       
        enemyManager = new EnemyManager(_player, enemies, _enemyHolder);
    }


    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= coolDown)
        {
            SpawnFormation();
            timer = 0;
        }
    }

    private GameObject GetRandomFormation()
    {
        var index = Random.Range(0, formationPrefabs.Length);
        return formationPrefabs[index];
    }

    public Elements GetRandomElement()
    {
        var elements = (Elements[])Enum.GetValues(typeof(Elements));
        var index = Random.Range(0, elements.Length);
        return elements[index];
    }

    private Vector3 GetOuterViewPort()
    {
        var randomNumber = Random.Range(0, 4);
        switch(randomNumber)
        {
            // up
            case 0:
                return Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0, 1), Random.Range(1.2f, 1.4f), 0));
            // down
            case 1:
                return Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0, 1), Random.Range(-1.2f, -1.4f), 0));
            // left
            case 2:
                return Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(-1.2f, -1.4f), Random.Range(0, 1), 0));
            // right
            case 3:
                return Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(1.2f, 1.4f), Random.Range(0, 1) , 0));
            default:
                return Vector3.zero;
        }
    }

    private void SpawnFormation()
    {
        var randomElement = GetRandomElement();
        // In viewport coordinades, the bottom left of the camera view is 0,0 and the top right 1,1
        var newPosition = GetOuterViewPort();

        var formationPrefab = GetRandomFormation();
        var formation = GameObject.Instantiate(formationPrefab, newPosition, Quaternion.identity);
        var positions = new List<Transform>();
        foreach(Transform trans in formation.transform)
        {
            var targetPos = trans.position;
            targetPos.z = 0;
            var enemy = enemyManager.SpawnEnemy(targetPos, randomElement);
            enemy.GetComponentInChildren<SpriteRenderer>().sortingOrder = sortingOrder;
            sortingOrder++;
        }
        GameObject.Destroy(formation);
    }
}
