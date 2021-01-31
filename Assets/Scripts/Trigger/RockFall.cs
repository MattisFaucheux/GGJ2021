using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class RockFall : MonoBehaviour
{
    public GameObject rock;
    public CinemachineVirtualCamera cam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            rock.SetActive(true);
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
        yield return new WaitForSeconds(3);
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        yield return null;
    }
}
