using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public int totalKilled;
    public int totalHealed;
    public int totalDamage;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
