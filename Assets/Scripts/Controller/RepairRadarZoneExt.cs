using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairRadarZoneExt : MonoBehaviour
{
    private bool isActivate = false;
    public GameController submarine;

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interaction") && isActivate && other.tag.Equals("PodPlayer"))
        {
            submarine.RepairSonar();
        }
    }

    public void SetActive(bool value)
    {
        isActivate = value;
    }
}
