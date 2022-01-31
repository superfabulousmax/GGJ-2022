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
    private GamePlayManager gamePlayManager;
    private EnemyManager enemyManager;
    private GameObject _player;
    private Enemies enemies;
    private float timer;
    private float coolDown = 0.75f;
    private GameObject _enemyHolder;
    private int sortingOrder = 0;
    private GameObject fireDamageVFX;
    private GameObject waterDamageVFX;
    private GameObject airDamageVFX;
    private GameObject earthDamageVFX;
    private GameObject healVFX;
    private SoundFX sound;
    private bool isPressureMusicPlaying;

    public int GetTotalKilled => enemyManager.totalKilled;
    public int GetTotalHealed => enemyManager.totalHealed;
    public int GetTotalDamage => enemyManager.totalDamage;

    public void Init(GamePlayManager gamePlayManager, GameObject player, GameObject fire, GameObject water, GameObject air, GameObject earth, GameObject heal, SoundFX sound)
    {
        timer = 0;
        formationPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemy Formations");
        _enemyHolder = GameObject.Instantiate(new GameObject());
        _camera = Camera.main;
        _player = player;
        fireDamageVFX = fire;
        waterDamageVFX = water;
        airDamageVFX = air;
        earthDamageVFX = earth;
        healVFX = heal;
        this.sound = sound;
        isPressureMusicPlaying = false;
        enemies = new Enemies() { };
        enemies.fireEnemy = Resources.Load<GameObject>($"{Constants.EnemiesFolder}Fire Enemy");
        enemies.waterEnemy = Resources.Load<GameObject>($"{Constants.EnemiesFolder}Water Enemy");
        enemies.airEnemy = Resources.Load<GameObject>($"{Constants.EnemiesFolder}Air Enemy");
        enemies.earthEnemy = Resources.Load<GameObject>($"{Constants.EnemiesFolder}Earth Enemy");
       
        enemyManager = new EnemyManager(_player, enemies, _enemyHolder, sound);
        enemyManager.onDamage += OnDamage;
        enemyManager.onHeal += OnHeal;
        enemyManager.onKill += OnKill;
        this.gamePlayManager = gamePlayManager;
        gamePlayManager.onGameOver += OnGameOver;
    }

    private void OnGameOver()
    {
    }

    private void OnKill(int totalKilled)
    {
        UIManager.Instance.UpdateDisplayKills(totalKilled);
    }

    private void OnHeal(int totalHealed)
    {
        UIManager.Instance.UpdateDisplayHeals(totalHealed);
    }

    private void OnDamage(int totalDamaged)
    {
        UIManager.Instance.UpdateDisplayDamage(totalDamaged);
    }

    void Update()
    {
        if (gamePlayManager.IsGameOver)
            return;
        timer += Time.deltaTime;
        if(CanSpawnMore() && timer >= coolDown)
        {
            SpawnFormation();
            timer = 0;
        }

        if(!isPressureMusicPlaying && UnderPressure())
        {
            PlayPressureMusic();
        }
        else if(isPressureMusicPlaying && !UnderPressure())
        {
            PlayNormalMusic();
        }
    }

    private bool CanSpawnMore()
    {
        return CountActive() < Constants.MaxEnemies - 10;
    }

    public bool UnderPressure()
    {
        return CountActive() > 100;
    }

    public void PlayPressureMusic()
    {
        isPressureMusicPlaying = true;
        if(sound.audioSource != null)
        {
            var pressureSoundInfo = new Sound() { clip = sound.pressureAudio, source = sound.audioSource, volume = sound.audioSource.volume, loop = sound.audioSource.loop };
            var normalSoundInfo = new Sound() { clip = sound.normalAudio, source = sound.audioSource, volume = sound.audioSource.volume, loop = sound.audioSource.loop };
            var fadeIn = AudioFader.FadeIn(pressureSoundInfo, 3, Mathf.SmoothStep);
            StartCoroutine(AudioFader.FadeOut(normalSoundInfo, 2, Mathf.SmoothStep, fadeIn));
        }
    }

    public void PlayNormalMusic()
    {
        isPressureMusicPlaying = false;
        if (sound.audioSource != null)
        {
            var pressureSoundInfo = new Sound() { clip = sound.pressureAudio, source = sound.audioSource, volume = sound.audioSource.volume, loop = sound.audioSource.loop };
            var normalSoundInfo = new Sound() { clip = sound.normalAudio, source = sound.audioSource, volume = sound.audioSource.volume, loop = sound.audioSource.loop };
            var fadeIn = AudioFader.FadeIn(normalSoundInfo, 3, Mathf.SmoothStep);
            StartCoroutine(AudioFader.FadeOut(pressureSoundInfo, 2, Mathf.SmoothStep, fadeIn));
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
            var enemy = enemyManager.SpawnEnemy(targetPos, randomElement, GetDamageVFX(randomElement), healVFX);
            enemy.GetComponentInChildren<SpriteRenderer>().sortingOrder = sortingOrder;
            sortingOrder++;
        }
        GameObject.Destroy(formation);
    }
    
    private GameObject GetDamageVFX(Elements element)
    {
        if (element == Elements.Fire)
            return fireDamageVFX;
        else if (element == Elements.Water)
            return waterDamageVFX;
        else if (element == Elements.Earth)
            return earthDamageVFX;
        return airDamageVFX;
    }

    public int CountActive()
    {
        return enemyManager.GetTotalActiveEnemies();
    }
}
