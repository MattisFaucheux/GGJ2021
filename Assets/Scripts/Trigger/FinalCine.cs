using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FinalCine : MonoBehaviour
{
    public CinemachineBrain brain;
    public CinemachineVirtualCamera cam;
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PodPlayer"))
        {
            cam.Priority = 100;
            brain.m_DefaultBlend.m_Time = 10;
        }
    }
}
