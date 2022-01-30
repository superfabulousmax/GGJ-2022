using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticles : MonoBehaviour
{
    void Start()
    {
        var psMain = GetComponent<ParticleSystem>().main;
        Destroy(gameObject, psMain.duration + 1f);
    }
}
