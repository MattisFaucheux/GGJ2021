using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLight : MonoBehaviour
{
    public GameObject[] lights;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LightsApparition());
        }
    }

    IEnumerator LightsApparition()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(true);
            yield return null;
        }
        yield return null;
    }
}
