using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using System.Linq;

[CreateAssetMenu(menuName = "Singletons/UIManagers", fileName = "UIManager")]
public class UIManager : yaSingleton.Singleton<UIManager>
{
    private GameObject canvasPrefab;
    private GameObject canvas;
    private Transform iconContainer;
    private Transform [] icons;
    private Outline [] outlines;
    protected override void Initialize()
    {
        base.Initialize();
        canvasPrefab = Resources.Load<GameObject>("Prefabs/Canvases");
        canvas = GameObject.Instantiate(canvasPrefab);

        iconContainer = canvas.transform.GetChild(0).GetChild(0);
        icons = iconContainer.GetComponentsInChildren<Transform>();
        outlines = icons.Select(icon => icon.GetComponent<Outline>()).Where(outline => outline != null).ToArray();
        SelectIcon(Elements.Fire);
    }

    protected override void Deinitialize()
    {
        base.Deinitialize();
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
}
