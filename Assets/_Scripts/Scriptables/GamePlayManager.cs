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
    // Player
    private GameObject player;
    private PlayerContext playerContext;
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
        player = Resources.Load<GameObject>("Prefabs/Player");
        LoadPrimaries();
        SetUpPlayer();
    }

    private void LoadPrimaries()
    {
        primaryAbilities = new PrimaryAbilities();
        primaryAbilities.firePrimary = Resources.Load<FireAbility>($"{Constants.PrimariesFolder}Fire Primary");
        primaryAbilities.waterPrimary = Resources.Load<WaterAbility>($"{Constants.PrimariesFolder}Water Primary");
        primaryAbilities.earthPrimary = Resources.Load<EarthAbility>($"{Constants.PrimariesFolder}Earth Primary");
        primaryAbilities.airPrimary = Resources.Load<AirAbility>($"{Constants.PrimariesFolder}Air Primary");
    }

    private void SetUpPlayer()
    {
        playerContext = new PlayerContext(new FireState());
        Instantiate(player);
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
    }
}