using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProfond : MonoBehaviour
{
    enum EnemyState
    {
        MOOVE,
        STOP,
        ATTACK
    }

    private GameController[] playerControllers;
    private Transform podObject;
    private bool PlayerEnoughtCloseToMoove = false;
    public float MaxPlayerDistanceToMoove = 50.0f;

    private CharacterController characterController;

    private Vector3 initialPos;
    public float MaxDistanceFromInitialPos = 50.0f;





    public float MooveSpeed = 5;

    public float MinTimeMooving = 2;
    public float MaxTimeMooving = 5;

    public float MinTimeStop = 2;
    public float MaxTimeStop = 5;

    private EnemyState actualState = EnemyState.STOP;
    private float ActionTimeRemaining = 0;

    private Vector3 MooveDirection;


    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        characterController = GetComponent<CharacterController>();
        UpdateControllerDetection();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateControllerDetection();
        UpdatePlayerMovement();
        MooveCharacter();
    }

    void UpdatePlayerMovement()
    {
        if (PlayerEnoughtCloseToMoove)
        {
            ActionTimeRemaining -= Time.deltaTime;

            if (ActionTimeRemaining <= 0)
            {
                if (actualState == EnemyState.STOP)
                {
                    SwitchToMooveState();
                }
                else if (actualState == EnemyState.MOOVE)
                {
                    SwitchToStopState();
                }
            }

            if (Vector3.Distance(transform.position, initialPos) > MaxDistanceFromInitialPos)
            {
                MooveDirection = initialPos - transform.position;
                MooveDirection.Normalize();
            }
        }
    }

    void MooveCharacter()
    {
        if (characterController)
        {
            characterController.Move(MooveDirection * MooveSpeed * Time.deltaTime);
        }

    }

    void SwitchToStopState()
    {
        MooveDirection = Vector3.zero;
        actualState = EnemyState.STOP;
        ActionTimeRemaining = Random.Range(MinTimeStop, MaxTimeStop);
    }

    void SwitchToMooveState()
    {
        MooveDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
        MooveDirection.Normalize();
        actualState = EnemyState.MOOVE;
        ActionTimeRemaining = Random.Range(MinTimeMooving, MaxTimeMooving);
    }

    void UpdateControllerDetection()
    {
        playerControllers = FindObjectsOfType<GameController>();

        if (playerControllers.Length > 0)
        {
            if (playerControllers.Length >= 2 && playerControllers[1])
            {
                PlayerEnoughtCloseToMoove =
                    (Vector3.Distance(transform.position, playerControllers[0].transform.position) < MaxPlayerDistanceToMoove ||
                     Vector3.Distance(transform.position, playerControllers[1].transform.position) < MaxPlayerDistanceToMoove);
            }
            else
            {
                PlayerEnoughtCloseToMoove = Vector3.Distance(transform.position, playerControllers[0].transform.position) < MaxPlayerDistanceToMoove;
            }
        }
    }
}
