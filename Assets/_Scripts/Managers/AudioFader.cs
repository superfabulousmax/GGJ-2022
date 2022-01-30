using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    public AudioSource source;

}

public class AudioFader
{
    public static IEnumerator FadeOut(Sound sound, float fadingTime, Func<float, float, float, float> Interpolate, IEnumerator fadeIn)
    {
        float startVolume = sound.source.volume;
        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;

        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            sound.source.volume = Interpolate(startVolume, 0, t);
            yield return null;
        }

        sound.source.volume = 0;
        sound.source.Stop();
        CoroutineRunner.Instance.Run(fadeIn);
    }
    public static IEnumerator FadeIn(Sound sound, float fadingTime, Func<float, float, float, float> Interpolate)
    {
        sound.source.clip = sound.clip;
        sound.source.Play();
        sound.source.volume = 0;

        float resultVolume = sound.volume;
        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;

        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            sound.source.volume = Interpolate(0, resultVolume, t);
            yield return null;
        }

        sound.source.volume = resultVolume;
    }
}