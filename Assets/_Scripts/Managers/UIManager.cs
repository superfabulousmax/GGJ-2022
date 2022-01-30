using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using System.Linq;
using TMPro;

public class UIManager : PersistentSingleton<UIManager>
{
    private GameObject canvasPrefab;
    private GameObject canvas;
    private Transform iconContainer;
    private Transform [] icons;
    private Outline [] outlines;
    private TMP_Text gameOverText;
    public static UIManager Instance;
    private int totalKilled;
    private int totalHealed;
    private int totalDamage;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    protected void Start()
    {
        
        canvasPrefab = Resources.Load<GameObject>("Prefabs/Canvases");
        canvas = GameObject.Instantiate(canvasPrefab);
        gameOverText = canvas.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>();
        //gameOverText.enabled = false;
        GamePlayManager.Instance.onGameOver += OnGameOver;
        iconContainer = canvas.transform.GetChild(0).GetChild(0);
        icons = iconContainer.GetComponentsInChildren<Transform>();
        outlines = icons.Select(icon => icon.GetComponent<Outline>()).Where(outline => outline != null).ToArray();
        SelectIcon(Elements.Fire);
    }

    private void OnDisable()
    {
        GamePlayManager.Instance.onGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        gameOverText.enabled = true;
    }

    internal void SelectIcon(Elements obj)
    {
        var index = (int)obj;
        outlines[index].enabled = true;
        for(int i = 0; i < outlines.Length; ++i)
        {
            if(i == index)
            {
                continue;
            }
            outlines[i].enabled = false;
        }
    }

    internal void UpdateDisplayDamage(int amount)
    {
        // todo
        totalDamage = amount;
    }

    internal void UpdateDisplayHeals(int amount)
    {
        // todo
        totalHealed = amount;
    }

    internal void UpdateDisplayKills(int amount)
    {
        // todo
        totalKilled = amount;
    }
}
