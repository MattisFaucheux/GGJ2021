using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraStateTransition : MonoBehaviour
{
    GameManager managerScript;
    GameObject podPlayer;
    public CinemachineVirtualCamera CameraSub, CameraPod, CameraInterior;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (managerScript.pState == GameManager.PlayerState.SUBMARINE)
        {
            CameraSub.m_Priority = 100;
        }
        else
        {
            CameraSub.m_Priority = 0;
        }

        if (managerScript.pState == GameManager.PlayerState.INTERIOR)
        {
            CameraInterior.m_Priority = 100;
        }
        else
        {
            CameraInterior.m_Priority = 0;
        }

        if (managerScript.pState == GameManager.PlayerState.POD)
        {
            podPlayer = GameObject.FindGameObjectWithTag("PodPlayer");
            CameraPod.m_Follow = podPlayer.transform;
            CameraPod.m_Priority = 100;
        }
        else
        {
            podPlayer = null;
            CameraPod.m_Priority = 0;
        }
    }
}
