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
    private TMP_Text timer;
    private float timeElapsed;
    public static UIManager Instance;

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
        gameOverText.enabled = false;
        timer = canvas.transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>();
        iconContainer = canvas.transform.GetChild(0).GetChild(0);
        icons = iconContainer.GetComponentsInChildren<Transform>();
        outlines = icons.Select(icon => icon.GetComponent<Outline>()).Where(outline => outline != null).ToArray();
        SelectIcon(Elements.Fire);
        timeElapsed = 0;
    }


    public void OnGameOver()
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

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        timer.text = Math.Round(timeElapsed, 2).ToString();
    }

    internal void UpdateDisplayDamage(int amount)
    {
        // todo
        //totalDamage = amount;
    }

    internal void UpdateDisplayHeals(int amount)
    {
        // todo
        //totalHealed = amount;
    }

    internal void UpdateDisplayKills(int amount)
    {
        // todo
        //totalKilled = amount;
    }
}
