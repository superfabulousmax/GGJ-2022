using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;
using Utils;

[CreateAssetMenu(fileName = "GameManager", menuName = "Singletons/GameManager")]
public class GamePlayManager : yaSingleton.Singleton<GamePlayManager>
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
    private GameObject player;
    private PlayerContext playerContext;
    private FireState fireState;
    private WaterState waterState;
    private AirState airState;
    private EarthState earthState;
    // Units

    // Events
    public event Action<AbilitySet, AbilityState> changeAbility;
    public AbilitySet CurrentAbilities { get => currentAbilities; }

    public void CallChangeAbilityEvent(AbilitySet abilities, AbilityState newState)
    {
        Debug.Log("CallChangeAbilityEvent");
        currentAbilities = abilities;
        playerContext.TransitionTo(newState);
    }
    protected override void Initialize()
    {
        Debug.Log("Initializing the GamePlayManager");
        base.Initialize();
        changeAbility = CallChangeAbilityEvent;
        LoadPrimaries();
        LoadSecondaries();
        InitAbilityStates();
        currentAbilities = fireAbilitySet;
        SetUpPlayer();
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
        player = Resources.Load<GameObject>("Prefabs/Player");
        var playerInstance = Instantiate(player);
        playerContext = new PlayerContext(fireState, playerInstance.transform, this);
    }

    private void InitFireState()
    {
        fireState = new FireState();
        fireAbilitySet = new AbilitySet() { primary = primaryAbilities.fire, secondary = secondaryAbilities.fire };
        fireState.SetAbilities(fireAbilitySet);
    }

    private void InitWaterState()
    {
        waterState = new WaterState();
        waterAbilitySet= new AbilitySet() { primary = primaryAbilities.water, secondary = secondaryAbilities.water };
        waterState.SetAbilities(waterAbilitySet);
    }
    private void InitEarthState()
    {
        earthState = new EarthState();
        earthAbilitySet = new AbilitySet() { primary = primaryAbilities.earth, secondary = secondaryAbilities.earth };
        earthState.SetAbilities(earthAbilitySet);
    }
    private void InitAirState()
    {
        airState = new AirState();
        airAbilitySet = new AbilitySet() { primary = primaryAbilities.air, secondary = secondaryAbilities.air };
        airState.SetAbilities(airAbilitySet);
    }

    public override void OnUpdate()
    {
        // fire
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeAbility?.Invoke(fireAbilitySet, fireState);
        }
        // water
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeAbility?.Invoke(waterAbilitySet, waterState);
        }
        // air
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeAbility?.Invoke(airAbilitySet, airState);
        }
        // earth
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            changeAbility?.Invoke(earthAbilitySet, earthState);
        }
        playerContext.Handle();
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
    }
}