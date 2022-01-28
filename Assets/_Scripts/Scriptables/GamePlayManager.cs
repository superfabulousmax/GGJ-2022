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
    protected override void Initialize()
    {
        base.Initialize();

        // Initialization code
    }
}