using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
    }
}
