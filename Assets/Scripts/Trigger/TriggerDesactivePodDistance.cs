using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDesactivePodDistance : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PodController pod = other.GetComponent<PodController>();
        if (pod)
        {
            pod.SetIsMaxPodDistance(false);
        }
    }
}
