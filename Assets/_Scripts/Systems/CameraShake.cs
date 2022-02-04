using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 _originalPos;
    private float _timeAtCurrentFrame;
    private float _timeAtLastFrame;
    private float _fakeDelta;
    public event Action<float> onStartShake = (float duration) => { };
    public event Action onFinishShake = () => { };

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Calculate a fake delta time, so we can Shake while game is paused.
        _timeAtCurrentFrame = Time.realtimeSinceStartup;
        _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
        _timeAtLastFrame = _timeAtCurrentFrame;
    }

    public static void Shake(float duration, float amount)
    {
        Instance._originalPos = Instance.gameObject.transform.localPosition;
        Instance.StopAllCoroutines();
        Instance.StartCoroutine(Instance.ShakeCamera(duration, amount));
    }

    public IEnumerator ShakeCamera(float duration, float amount)
    {
        onStartShake?.Invoke(duration);
        while (duration > 0)
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

            duration -= _fakeDelta;

            yield return null;
        }

        transform.localPosition = _originalPos;
        onFinishShake?.Invoke();
    }
}
