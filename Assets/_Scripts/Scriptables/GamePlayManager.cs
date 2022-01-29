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
    PrimaryAbilities primaryAbilities;
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
    public event Action<AbilitySet> changeAbility;
    public AbilitySet CurrentAbilities { get => currentAbilities; }

    public void CallChangeAbilityEvent(AbilitySet abilities)
    {
        Debug.Log("CallChangeAbilityEvent");
        currentAbilities = abilities;
    }
    protected override void Initialize()
    {
        Debug.Log("Initializing the GamePlayManager");
        base.Initialize();
        changeAbility = CallChangeAbilityEvent;
        LoadPrimaries();
        InitAbilityStates();
        currentAbilities = fireAbilitySet;
        SetUpPlayer();
       
        //changeAbility?.Invoke(fireAbilitySet);
    }


    private void LoadPrimaries()
    {
        primaryAbilities = new PrimaryAbilities();
        primaryAbilities.firePrimary = Resources.Load<FireAbility>($"{Constants.PrimariesFolder}Fire Primary");
        primaryAbilities.waterPrimary = Resources.Load<WaterAbility>($"{Constants.PrimariesFolder}Water Primary");
        primaryAbilities.earthPrimary = Resources.Load<EarthAbility>($"{Constants.PrimariesFolder}Earth Primary");
        primaryAbilities.airPrimary = Resources.Load<AirAbility>($"{Constants.PrimariesFolder}Air Primary");
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
        playerContext = new PlayerContext(fireState);
        player = Resources.Load<GameObject>("Prefabs/Player");
        Instantiate(player);
    }

    private void InitFireState()
    {
        fireState = new FireState();
        fireAbilitySet = new AbilitySet() { primary = primaryAbilities.firePrimary };
        fireState.SetAbilities(fireAbilitySet);
    }

    private void InitWaterState()
    {
        waterState = new WaterState();
        waterAbilitySet= new AbilitySet() { primary = primaryAbilities.waterPrimary };
        waterState.SetAbilities(waterAbilitySet);
    }
    private void InitEarthState()
    {
        earthState = new EarthState();
        earthAbilitySet = new AbilitySet() { primary = primaryAbilities.earthPrimary };
        earthState.SetAbilities(earthAbilitySet);
    }
    private void InitAirState()
    {
        airState = new AirState();
        airAbilitySet = new AbilitySet() { primary = primaryAbilities.airPrimary };
        airState.SetAbilities(airAbilitySet);
    }

    public override void OnUpdate()
    {
        // Update code
        // fire
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeAbility?.Invoke(fireAbilitySet);
        }
        // water
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeAbility?.Invoke(waterAbilitySet);
        }
        // air
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeAbility?.Invoke(airAbilitySet);
        }
        // earth
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            changeAbility?.Invoke(earthAbilitySet);
        }
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
    }
}