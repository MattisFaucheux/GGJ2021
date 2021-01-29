using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorPlayerController : MonoBehaviour
{
    public float mooveSpeed = 5;
    public float gravity = -9.81f;

    private Vector3 mooveVector;
    private CharacterController controller;

    private bool canMooveVertical = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        mooveVector = Vector3.zero;

        GetMovementsInput();
        MooveCharacter();
    }

    void GetMovementsInput()
    {
        mooveVector.x = Input.GetAxis("Horizontal");
        
        if (canMooveVertical)
        {
            mooveVector.y = Input.GetAxis("Vertical");
        }
        else if (!controller.isGrounded)
        {
            mooveVector.y = gravity;
        }
    }

    void MooveCharacter()
    {
        controller.Move(mooveVector * mooveSpeed * Time.deltaTime);
    }

    public void SetCanMooveVertical(bool canMooveV)
    {
        canMooveVertical = canMooveV;
    }
}
