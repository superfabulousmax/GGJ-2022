using System.Collections;
using UnityEngine;
public class CoroutineRunner : MonoBehaviour
{
    public static CoroutineRunner Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void Run(IEnumerator cor)
    {
        StartCoroutine(cor);
    }
}
