using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private GameObject _barFill;
    [SerializeField] private ParticleSystem _hurtVFX;

    private bool _alive;
    private float _maxHealth;
    private float _initFillScaleX;
    private Vector3 _fillScale;
    private Vector3 _fillPosition;
    private GamePlayManager gamePlayManager;

    private void Awake()
    {
        _alive = true;
        _maxHealth = _health;
        _initFillScaleX = _barFill.transform.localScale.x;
        _fillScale = _barFill.transform.localScale;
        _fillPosition = _barFill.transform.localPosition;
    }
    private void Start()
    {
        gamePlayManager = FindObjectOfType<GamePlayManager>();
        if(gamePlayManager)
        {
            gamePlayManager.onGameOver += OnGameOver;
        }
    }

    private void OnDisable()
    {
        gamePlayManager.onGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        _alive = false;
    }

    public void TakeDamage(float damage)
    {
        if (!_alive)
        {
            return;
        }

        _health -= damage;
        var healthFactor = _health / _maxHealth;

        _fillScale.x = healthFactor * _initFillScaleX;
        _fillPosition.x = -(_initFillScaleX - _fillScale.x) * 0.5f;

        _barFill.transform.localPosition = _fillPosition;
        _barFill.transform.localScale = _fillScale;

        _hurtVFX.Emit(1);

        if (_health <= 0)
        {
            _alive = false;
            gamePlayManager.SendGameOver();
        }
    }
}
