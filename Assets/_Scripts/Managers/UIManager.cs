using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using System.Linq;

public class UIManager : MonoBehaviour
{
    private GameObject canvasPrefab;
    private GameObject canvas;
    private Transform iconContainer;
    private Transform [] icons;
    private Outline [] outlines;
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

        iconContainer = canvas.transform.GetChild(0).GetChild(0);
        icons = iconContainer.GetComponentsInChildren<Transform>();
        outlines = icons.Select(icon => icon.GetComponent<Outline>()).Where(outline => outline != null).ToArray();
        SelectIcon(Elements.Fire);
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

    internal void UpdateDisplayDamage()
    {
        // todo
    }

    internal void UpdateDisplayHeals()
    {
        // todo
    }

    internal void UpdateDisplayKills()
    {
        // todo
    }
}
