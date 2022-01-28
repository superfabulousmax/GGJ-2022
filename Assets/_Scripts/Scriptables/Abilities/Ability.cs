using UnityEngine;
using NaughtyAttributes;
public abstract class Ability : ScriptableObject
{
    [InfoBox("The title of this ability", EInfoBoxType.Normal), SerializeField]
    private string title;
    [SerializeField]
    private Color color;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private AudioClip sound;
    [SerializeField]
    private float duration = 0;
    [SerializeField]
    private float cooldown = 1.0f;

    public string Title { get => title; }
    public Color Color { get => color; }
    public Sprite Sprite { get => sprite;  }
    public AudioClip Sound { get => sound; }
    public float Duration { get => duration; }
    public float Cooldown { get => cooldown; }

    // Called to create an ability for a player
    public abstract void Initialise(GameObject go);
    // Called when an ability is used
    public abstract void TriggerAbility();
    // Called after ability duration expires (not called if duration is 0)
    public abstract void EndAbility();
}