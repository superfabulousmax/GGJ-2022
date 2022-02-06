/*
 * Utility timer. Has the option to be recurring.
 * Update needs to be called with the deltaTime.
 * TimerCompleteEvent is emitted on completion.
 */
public class CoolDownTimer
{
    public float timer { get; private set; }
    public float TotalTime { get; private set; }
    public bool IsRecurring { get; }
    public bool IsActive { get; private set; }
    public int TimesCounted { get; private set; }
    public float TimeRemaining { get; private set; }

    public float TimeElapsed => timer;
    public float PercentElapsed => TimeElapsed / TotalTime;
    public float PercentRemaining => 1f - timer / TotalTime;
    public bool IsCompleted => timer >= TotalTime;

    public delegate void TimerCompleteHandler();

    /// <summary>
    /// Emits event when timer is completed
    /// </summary>
    public event TimerCompleteHandler TimerCompleteEvent;

    /// <summary>
    /// Create a new CooldownTimer
    /// Must call Start() to begin timer
    /// </summary>
    /// <param name="time">Timer length (seconds)</param>
    /// <param name="recurring">Is this timer recurring</param>
    public CoolDownTimer(float time, bool recurring = false)
    {
        TotalTime = time;
        IsRecurring = recurring;
        timer = 0;
        TimeRemaining = TotalTime;
    }

    /// <summary>
    /// Start timer with existing time
    /// </summary>
    public void Start()
    {
        if (IsActive) { TimesCounted++; }
        timer = 0;
        IsActive = true;
        if (timer >= TotalTime)
        {
            TimerCompleteEvent?.Invoke();
        }
    }

    /// <summary>
    /// Start timer with new time
    /// </summary>
    public void Start(float time)
    {
        TotalTime = time;
        Start();
    }

    public virtual void Update(float timeDelta)
    {
        if (TimeRemaining > 0 && IsActive)
        {
            timer += timeDelta;
            if (timer >= TotalTime)
            {
                if (IsRecurring)
                {
                    timer = 0;
                }
                else
                {
                    IsActive = false;
                    timer = TotalTime;
                }

                TimerCompleteEvent?.Invoke();
                TimesCounted++;
            }
        }
    }

    public void Invoke()
    {
        TimerCompleteEvent?.Invoke();
    }

    public void Pause()
    {
        IsActive = false;
    }

}