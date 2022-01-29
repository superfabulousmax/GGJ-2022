using UnityEngine;
public class MoveCamera : MonoBehaviour
{

    public Vector3 zOffset = new Vector3(0, 0, -10);
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(player == null)
        {
            Debug.Log("IJhskjvskdjvndslkj");
            player = GameObject.FindGameObjectWithTag("Player");
        }
        transform.position = player.transform.position + zOffset;
    }
}