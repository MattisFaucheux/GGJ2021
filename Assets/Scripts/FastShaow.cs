using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastShaow : MonoBehaviour
{
    public float speed;
    private void Start()
    {
        Transform player = GameObject.Find("Submarine").transform;
        transform.LookAt(player);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
