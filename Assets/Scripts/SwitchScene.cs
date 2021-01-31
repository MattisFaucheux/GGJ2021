using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public string sceneNameToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("PodPlayer") || other.gameObject.tag.Equals("PodPlayer"))
        {
            SceneManager.LoadScene(sceneNameToLoad, LoadSceneMode.Single);
        }
    }
}
