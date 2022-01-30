using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScrollingBackgroundController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void Update()
    {
        _spriteRenderer.material.mainTextureOffset = transform.position * 0.05f;
    }
}
