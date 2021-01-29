using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorLadder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("InteriorPlayer"))
        {
            InteriorPlayerController pController = other.GetComponent<InteriorPlayerController>();
            if (pController)
            {
                pController.SetCanMooveVertical(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("InteriorPlayer"))
        {
            InteriorPlayerController pController = other.GetComponent<InteriorPlayerController>();
            if (pController)
            {
                pController.SetCanMooveVertical(false);
            }
        }
    }

}
