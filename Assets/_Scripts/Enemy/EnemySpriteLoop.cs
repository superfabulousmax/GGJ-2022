using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteLoop : MonoBehaviour
{
    [SerializeField] private Sprite[] _frameArray;
    private int _currFrame;
    private float _time;

    private float _fps = 1f/4f;
    private SpriteRenderer _spriteRenderer;
    private bool hasBeenSeen;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currFrame = Random.Range(0, _frameArray.Length);
        _spriteRenderer.sprite = _frameArray[_currFrame];
        hasBeenSeen = false;
    }

    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _fps)
        {
            _time -= _fps;
            _currFrame = (_currFrame + 1) % _frameArray.Length;
            _spriteRenderer.sprite = _frameArray[_currFrame];
        }
    }

}
