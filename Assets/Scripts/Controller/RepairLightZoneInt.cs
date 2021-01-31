using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairLightZoneInt : MonoBehaviour
{
    private bool isActivate = false;
    public GameController submarine;

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interaction") && isActivate && other.tag.Equals("InteriorPlayer"))
        {
            submarine.RepairLight();
        }
    }

    public void SetActive(bool value)
    {
        isActivate = value;
    }
}
