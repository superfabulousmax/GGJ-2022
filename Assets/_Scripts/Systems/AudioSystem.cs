using UnityEngine;

/// <summary>
/// Insanely basic audio system which supports 3D sound.
/// Ensure you change the 'Sounds' audio source to use 3D spatial blend if you intend to use 3D sounds.
/// </summary>
public class AudioSystem : MonoBehaviour{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundsSource;
    private const float secondaryCoolDown = 1.5f;
    private const float waitTime = 0.6f;
    private float secondaryFxTimer;

    public static AudioSystem Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Update()
    {
        secondaryFxTimer += Time.deltaTime;
    }
    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlaySecondarySound(AudioClip clip, Vector3 pos, float vol = 1)
    {
        if(secondaryFxTimer >= secondaryCoolDown)
        {
            _soundsSource.transform.position = pos;
            PlaySound(clip, vol);
            secondaryFxTimer = 0;
        }

    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1) {
        _soundsSource.transform.position = pos;
        PlaySound(clip, vol);
    }

    public void PlaySound(AudioClip clip, float vol = 1) {
        _soundsSource.PlayOneShot(clip, vol);
    }
}