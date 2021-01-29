using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTrigger : MonoBehaviour
{
    public GameObject[] shadowList;
    public float delay;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ShadowApparition());
        }
    }

    IEnumerator ShadowApparition()
    {
        foreach (GameObject shadow in shadowList)
        {
            shadow.SetActive(true);
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }
}
