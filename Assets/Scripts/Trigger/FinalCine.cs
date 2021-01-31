using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
public class FinalCine : MonoBehaviour
{
    public CinemachineBrain brain;
    public CinemachineVirtualCamera cam;
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PodPlayer"))
        {
            
        }
    }

    IEnumerator Final()
    {
        cam.Priority = 100;
        brain.m_DefaultBlend.m_Time = 10;
        yield return new WaitForSeconds(12);
        SceneManager.LoadScene(0);
        yield return null;
    }
}
