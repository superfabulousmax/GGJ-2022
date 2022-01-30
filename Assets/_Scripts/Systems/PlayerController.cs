using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private AbilitySet currentAbilities;
    [SerializeField]
    private Transform fire;
    private Collider2D _collider;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
    }

    private void ChangeAbility(AbilitySet abilities)
    {
        Debug.Log($"Change Ability to {abilities.primary.Title}");
        currentAbilities = abilities;
    }


    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
    }
}
