using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using DG.Tweening;
using Utils;

public class GamePlayManager : MonoBehaviour
{
    // Abilities
    Abilities primaryAbilities;
    Abilities secondaryAbilities;
    AbilitySet fireAbilitySet;
    AbilitySet waterAbilitySet;
    AbilitySet airAbilitySet;
    AbilitySet earthAbilitySet;

    private AbilitySet currentAbilities;

    // Player
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject fireDamageVFX;
    [SerializeField]
    private GameObject waterDamageVFX;
    [SerializeField]
    private GameObject airDamageVFX;
    [SerializeField]
    private GameObject earthDamageVFX;
    [SerializeField]
    private GameObject healVFX;
    [SerializeField]
    private SoundFX sound;
    private PlayerContext _playerContext;
    private FireState fireState;
    private WaterState waterState;
    private AirState airState;
    private EarthState earthState;

    private GameObject _playerPrefab;
    [SerializeField] private SceneLoadingManager _sceneLoadingManager;
    [SerializeField] private DataManager _DataManager;

    private EnemySpawner enemySpawner;

    // Units

    // Events
    public event Action<GameObject> onPlayerInstantiated;
    public event Action<AbilitySet, AbilityState> changeAbility;
    public event Action<Elements> selectIcon;
    private bool isGameOver;
    public event Action onGameOver;
    public AbilitySet CurrentAbilities { get => currentAbilities; }
    public bool IsGameOver { get => isGameOver; }

    public static GamePlayManager Instance;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void CallChangeAbilityEvent(AbilitySet abilities, AbilityState newState)
    {
        Debug.Log("CallChangeAbilityEvent");
        currentAbilities = abilities;
        _playerContext.TransitionTo(newState);
    }
    void Start()
    {
        Debug.Log("Initializing the GamePlayManager");
        changeAbility = CallChangeAbilityEvent;
        selectIcon = UIManager.Instance.SelectIcon;
        onPlayerInstantiated = OnPlayerInstantiated;
        onGameOver = SetGameOver;
        currentAbilities = fireAbilitySet;

        // Load
        LoadPrimaries();
        _playerPrefab = Resources.Load<GameObject>("Prefabs/Player");

        // Initialization code
        LoadSecondaries();
        InitAbilityStates();
        SetUpPlayer();
        SetupEnemy();
    }

    private void SetGameOver()
    {
        isGameOver = true;
        UIManager.Instance.OnGameOver();
        _player.GetComponent<PlayerMovementController>().enabled = false;
        PlayerPrefs.SetInt("totalKilled", enemySpawner.GetTotalKilled);
        PlayerPrefs.SetInt("totalHealed", enemySpawner.GetTotalHealed);
        PlayerPrefs.SetInt("totalDamage", enemySpawner.GetTotalDamage);


        DOTween.Sequence()
            .AppendInterval(2f)
            .OnComplete(() => _sceneLoadingManager.OnLoadScene(3));
    }

    public void OnPlayerInstantiated(GameObject player)
    {

    }

    internal void SendGameOver()
    {
        onGameOver?.Invoke();
    }

    private void LoadPrimaries()
    {
        primaryAbilities = new Abilities();
        primaryAbilities.fire = Resources.Load<FireAbility>($"{Constants.PrimariesFolder}Fire Primary");
        primaryAbilities.water = Resources.Load<WaterAbility>($"{Constants.PrimariesFolder}Water Primary");
        primaryAbilities.earth = Resources.Load<EarthAbility>($"{Constants.PrimariesFolder}Earth Primary");
        primaryAbilities.air = Resources.Load<AirAbility>($"{Constants.PrimariesFolder}Air Primary");
    }

    private void LoadSecondaries()
    {
        secondaryAbilities = new Abilities();
        secondaryAbilities.fire = Resources.Load<FireAbility>($"{Constants.SecondariesFolder}Fire Secondary");
        secondaryAbilities.water = Resources.Load<WaterAbility>($"{Constants.SecondariesFolder}Water Secondary");
        secondaryAbilities.earth = Resources.Load<EarthAbility>($"{Constants.SecondariesFolder}Earth Secondary");
        secondaryAbilities.air = Resources.Load<AirAbility>($"{Constants.SecondariesFolder}Air Secondary");
    }

    private void InitAbilityStates()
    {
        InitFireState();
        InitWaterState();
        InitAirState();
        InitEarthState();
    }

    private void SetUpPlayer()
    {
        if(_player == null)
            _player = Instantiate(_playerPrefab);
        _playerContext = new PlayerContext(fireState, _player.transform, this, sound.audioSource);
        onPlayerInstantiated?.Invoke(_player);
    }

    private void InitFireState()
    {
        fireState = new FireState();
        fireAbilitySet = new AbilitySet() { primary = primaryAbilities.fire, secondary = secondaryAbilities.fire };
        fireState.SetAbilities(fireAbilitySet);
        fireState.SetAudio(sound.audioSource, sound.firePrimaryDamage);
    }

    private void InitWaterState()
    {
        waterState = new WaterState();
        waterAbilitySet= new AbilitySet() { primary = primaryAbilities.water, secondary = secondaryAbilities.water };
        waterState.SetAbilities(waterAbilitySet);
        fireState.SetAudio(sound.audioSource, sound.waterPrimaryDamage);
    }
    private void InitEarthState()
    {
        earthState = new EarthState();
        earthAbilitySet = new AbilitySet() { primary = primaryAbilities.earth, secondary = secondaryAbilities.earth };
        earthState.SetAbilities(earthAbilitySet);
        fireState.SetAudio(sound.audioSource, sound.earthPrimaryDamage);
    }
    private void InitAirState()
    {
        airState = new AirState();
        airAbilitySet = new AbilitySet() { primary = primaryAbilities.air, secondary = secondaryAbilities.air };
        airState.SetAbilities(airAbilitySet);
        fireState.SetAudio(sound.audioSource, sound.airPrimaryDamage);
    }

    public void Update()
    {
        if (isGameOver)
            return;
        // todo select via scroll wheel y
        var y = Input.mouseScrollDelta.y;
        if(y != 0)
        {

        }
        // fire
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeAbility?.Invoke(fireAbilitySet, fireState);
            selectIcon?.Invoke(Elements.Fire);
        }
        // water
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeAbility?.Invoke(waterAbilitySet, waterState);
            selectIcon?.Invoke(Elements.Water);
        }
        // air
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeAbility?.Invoke(airAbilitySet, airState);
            selectIcon?.Invoke(Elements.Air);
        }
        // earth
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            changeAbility?.Invoke(earthAbilitySet, earthState);
            selectIcon?.Invoke(Elements.Earth);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        _playerContext.Handle();
    }

    private void SetupEnemy()
    {
        var spawnerObject = new GameObject("Enemy Spawner");
        enemySpawner = spawnerObject.AddComponent<EnemySpawner>();
        enemySpawner.Init(this, _player, fireDamageVFX, waterDamageVFX, airDamageVFX, earthDamageVFX, healVFX, sound);
    }
}