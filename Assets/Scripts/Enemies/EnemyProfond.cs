using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProfond : MonoBehaviour
{
    enum EnemyState
    {
        MOOVE,
        STOP,
        ATTACK,
        ESCAPE
    }

    private GameController[] playerControllers;
    private Transform podObject;
    private bool PlayerEnoughtCloseToMoove = false;
    public float MaxPlayerDistanceToMoove = 50.0f;

    public float MaxDistanceToAttack = 5.0f;

    private CharacterController characterController;

    private Vector3 initialPos;
    public float MaxDistanceFromInitialPos = 50.0f;


    private bool canAttack = true;
    public float attackReloadTime = 10.0f;


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

        //characterController.
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
                switch (actualState)
                {
                    case EnemyState.STOP:
                        {
                            SwitchToMooveState();
                        }
                        break;

                    case EnemyState.MOOVE:
                        { 
                            SwitchToStopState();
                        }
                        break;

                    case EnemyState.ESCAPE:
                        {
                            SwitchToMooveState();
                        }
                        break;
                }
            }


            if (Vector3.Distance(transform.position, initialPos) > MaxDistanceFromInitialPos && actualState != EnemyState.ESCAPE)
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
            float DistanceA = Vector3.Distance(transform.position, playerControllers[0].transform.position);

            if (playerControllers.Length >= 2 && playerControllers[1])
            {
                float DistanceB = Vector3.Distance(transform.position, playerControllers[1].transform.position);

                PlayerEnoughtCloseToMoove = (DistanceA < MaxPlayerDistanceToMoove || DistanceB < MaxPlayerDistanceToMoove);

                if (DistanceB < MaxDistanceToAttack && canAttack)
                {
                    Vector3 dir = playerControllers[1].transform.position - transform.position;
                    dir.z = 0;
                    dir.Normalize();
                    SetIsAttacking(dir);
                }
            }
            else
            {
                PlayerEnoughtCloseToMoove = DistanceA < MaxPlayerDistanceToMoove;

                if (DistanceA < MaxDistanceToAttack && canAttack)
                {
                    Vector3 dir = playerControllers[0].transform.position - transform.position;
                    dir.z = 0;
                    dir.Normalize();
                    SetIsAttacking(dir);
                }
            }
        }
    }

    public void SetIsEscaping(Vector3 dirToEscape)
    {
        if (actualState == EnemyState.ESCAPE)
        {
            return;
        }

        MooveDirection = dirToEscape;
        actualState = EnemyState.ESCAPE;
        ActionTimeRemaining = 1;
    }

    public void SetIsAttacking(Vector3 dirToAttack)
    {
        if (actualState == EnemyState.ESCAPE || actualState == EnemyState.ATTACK || !canAttack)
        {
            return;
        }

        MooveDirection = dirToAttack;
        actualState = EnemyState.ATTACK;
        ActionTimeRemaining = 1;
    }


    void OnTriggerEnter(Collider other)
    {
        if (!canAttack)
        {
            return;
        }

        GameController player = other.gameObject.GetComponent<GameController>();
        if (player)
        {
            canAttack = false;
            player.TakeDamage();
            Vector3 dir = other.transform.position - transform.position;
            dir.z = 0;
            dir.Normalize();
            SetIsEscaping(dir);
            StartCoroutine(ReloadAttack());
        }

    }

    IEnumerator ReloadAttack()
    {
        yield return new WaitForSeconds(attackReloadTime);
        canAttack = true;
    }
}
