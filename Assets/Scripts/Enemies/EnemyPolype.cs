using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPolype : MonoBehaviour
{
    enum EnemyState
    {
        STOP,
        ATTACK,
        ESCAPE
    }

    private GameController[] playerControllers;
    private CharacterController characterController;

    public float MaxDistanceToAttack = 5.0f;
    private bool canAttack = true;
    public float attackReloadTime = 10.0f;

    public float DistanceEscapePlayer = 5.0f;

    private bool isIlluminated = false;

    private EnemyState actualState = EnemyState.STOP;
    private float ActionTimeRemaining = 0;

    public float MooveSpeed = 5;
    private Vector3 MooveDirection;

    private Vector3 initialPos;

    public AudioClip attackSound;
    public float soundVolume = 1;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
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
        ActionTimeRemaining -= Time.deltaTime;

        if (ActionTimeRemaining <= 0 && actualState == EnemyState.ESCAPE)
        {
            SwitchToStopState();
        }

        if (actualState == EnemyState.STOP)
        {
            if (transform.position.y > initialPos.y + 0.5f || transform.position.y < initialPos.y - 0.5f)
            {
                Vector3 dir = initialPos - transform.position;
                dir.z = 0;
                dir.x = 0;
                dir.Normalize();
                SetIsEscaping(dir);
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

    void UpdateControllerDetection()
    {
        playerControllers = FindObjectsOfType<GameController>();

        if (playerControllers.Length > 0)
        {
            float Distance = Vector3.Distance(transform.position, playerControllers[0].transform.position);

            if (isIlluminated)
            {
                Vector3 dir = playerControllers[0].transform.position - transform.position;
                dir.z = 0;
                dir.Normalize();
                SetIsAttacking(dir);
            }
            else if(Distance < DistanceEscapePlayer)
            {
                Vector3 dir = transform.position - playerControllers[0].transform.position;
                dir.z = 0;
                dir.x = 0;
                dir.Normalize();
                SetIsEscaping(dir);
            }
        }
    }

    void SwitchToStopState()
    {
        MooveDirection = Vector3.zero;
        actualState = EnemyState.STOP;
        ActionTimeRemaining = 0;
    }

    public void SetIsEscaping(Vector3 dirToEscape)
    {
        if (actualState == EnemyState.ATTACK)
        {
            return;
        }

        MooveDirection = dirToEscape;
        actualState = EnemyState.ESCAPE;
        ActionTimeRemaining = 0.1f;
    }

    public void SetIsAttacking(Vector3 dirToAttack)
    {
        if (!isIlluminated)
        {
            return;
        }

        if (actualState != EnemyState.ATTACK)
        {
            if (audioSource && attackSound)
            {
                audioSource.PlayOneShot(attackSound, soundVolume);
            }
        }

        MooveDirection = dirToAttack;
        actualState = EnemyState.ATTACK;
        ActionTimeRemaining = 0.1f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (actualState != EnemyState.ATTACK)
        {
            return;
        }

        GameController player = other.gameObject.GetComponent<GameController>();
        if (player)
        {
            player.TakeDamage();
            //SwitchToStopState();
            MooveDirection = Vector3.zero;
            //Vector3 dir = other.transform.position - transform.position;
            //dir.z = 0;
            //dir.Normalize();
            //SetIsEscaping(dir);
        }

    }


    //IEnumerator ReloadAttack()
    //{
    //    yield return new WaitForSeconds(attackReloadTime);
    //    canAttack = true;
    //}


    public void SetIsIlluminatedOn()
    {
        isIlluminated = true;
    }

    public void SetIsIlluminatedOff()
    {
        isIlluminated = false;
    }


}
