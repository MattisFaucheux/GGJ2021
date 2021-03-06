﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interaction") && other.tag.Equals("InteriorPlayer"))
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.EnterDrivingMode();
        }
    }
}
