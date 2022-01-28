using UnityEngine;
using NaughtyAttributes;
public abstract class Ability : ScriptableObject
{
    [InfoBox("The title of this ability", EInfoBoxType.Normal), SerializeField]
    private string title;
    [SerializeField]
    private Color color;
    [Required, InfoBox("The UI associated with this ability", EInfoBoxType.Normal), ShowAssetPreview, SerializeField]
    private Sprite sprite;
    [Required, ShowAssetPreview, SerializeField]
    private GameObject vfx;
    [SerializeField]
    private AudioClip sound;
    [ValidateInput("IsGreaterThanZero", "Duration must be greater than zero"), SerializeField]
    private float duration = 0;
    [MinMaxSlider(0.1f, 10.0f), SerializeField]
    private Vector2 cooldown;

    public string Title { get => title; }
    public Color Color { get => color; }
    public Sprite Sprite { get => sprite;  }
    public GameObject Vfx { get => vfx;  }
    public AudioClip Sound { get => sound; }
    public float Duration { get => duration; }
    public Vector2 Cooldown { get => cooldown; }

    // Called to create an ability for a player
    public abstract void Initialise(GameObject go);
    // Called when an ability is used
    public abstract void TriggerAbility();
    // Called after ability duration expires (not called if duration is 0)
    public abstract void EndAbility();

    private bool IsGreaterThanZero(float value)
    {
        return value > 0;
    }
}