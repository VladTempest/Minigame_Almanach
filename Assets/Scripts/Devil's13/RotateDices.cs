using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDices : MonoBehaviour
{
    public float tumble = 5;
    private Rigidbody rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.insideUnitSphere * tumble; 
    }
}