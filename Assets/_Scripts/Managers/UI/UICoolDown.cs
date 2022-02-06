using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICoolDown : MonoBehaviour
{
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private Slider timerSlider;

    private float cooldownTimeInSeconds;

    private CoolDownTimer _cooldownTimer;

    public float CooldownTimeInSeconds { set => cooldownTimeInSeconds = value; }

    public void InitializeTimer(float duration)
    {
        cooldownTimeInSeconds = duration;
        _cooldownTimer = new CoolDownTimer(cooldownTimeInSeconds);

        // Register handler that will trigger when timer is complete
        _cooldownTimer.TimerCompleteEvent += OnTimerComplete;
    }

    private void Update()
    {
        _cooldownTimer?.Update(Time.deltaTime);
        if (_cooldownTimer != null && _cooldownTimer.IsActive)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        //timerText.text = "Cooldown: " + _cooldownTimer.TimeRemaining;
        timerSlider.value = _cooldownTimer.PercentElapsed;
    }

    private void OnTimerComplete()
    {
        //timerText.text = "Cooldown Completed!";
    }

    public void StartTimer()
    {
        _cooldownTimer.Start();
    }
}
