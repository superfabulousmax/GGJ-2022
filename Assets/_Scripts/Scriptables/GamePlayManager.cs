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
    // Player
    private GameObject player;
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
        Instantiate(player);
        // Initialization code
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
    }
}