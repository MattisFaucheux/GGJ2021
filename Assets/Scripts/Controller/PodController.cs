using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodController : GameController
{
    public bool isMaxPodDistance = true;
    public float MaxPodDistance = 20.0f;

    protected override void MoveForward()
    {
        Vector3 incomingMovement = ((transform.right * m_actualSpeedH) + (transform.up * m_actualSpeedV)) * Time.deltaTime;

        if (isMaxPodDistance)
        {
            if (Vector3.Distance(startPosition, transform.position + incomingMovement) > MaxPodDistance)
            {
                m_actualSpeedV = 0;
                m_actualSpeedH = 0;
                incomingMovement = Vector3.zero;
            }
        }

        m_controller.Move(incomingMovement);

    }

    public void SetIsMaxPodDistance(bool isMaxDistance)
    {
        isMaxPodDistance = isMaxDistance;
    }
}
