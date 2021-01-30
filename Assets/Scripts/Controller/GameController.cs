using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isInputActivate = true;

    #region Speed
    [Header("Speed Settings")]
    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_maxSpeedH = 30;
    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_minSpeedH = -10, m_accelerationH = 3, m_decelerationH = 3, m_inputAccelerationH = 3, m_inputDecelerationH = 3;

    protected float m_actualSpeedH;



    [Header("Speed Settings")]
    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_maxSpeedV = 30;
    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_minSpeedV = -10, m_accelerationV = 3, m_decelerationV = 3, m_inputAccelerationV = 3, m_inputDecelerationV = 3;

    protected float m_actualSpeedV;
    #endregion


    //#region Rotation
    //[Header("Rotation Speed Settings")]
    //[SerializeField]
    //[Tooltip("Speed in Unit/Sec")]
    //private float m_maxRSpeed = 10;

    //[SerializeField]
    //[Tooltip("Speed in Unit/Sec")]
    //private float m_minRSpeed = -10, m_rAcceleration = 2, m_rDeceleration = 2, m_rInputAcceleration = 2, m_rInputDeceleration = 2;

    //[SerializeField]
    //private float m_maxZRotation = 60, m_minZRotation = -60;

    //private float m_rotateSpeed;
    //#endregion


    #region Movement
    protected CharacterController m_controller;
    protected Vector3 startPosition;
    #endregion


    private RadarPulse radarPulse;

    private Transform lightObject;
    public ParticleSystem bubbleParticle;

    public Light spotLight;
    public GameObject spotLightTrigger;

    void Start()
    {
        startPosition = transform.position;
        lightObject = transform.Find("RotativeLight");

        radarPulse = transform.GetComponentInChildren<RadarPulse>();
        //if (radarPulse)
        //{
        //    radarPulse.SetIsActivate(isInputActivate);
        //}

        m_controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        ChangeSpeedHorizontal();
        ChangeSpeedVertical();
        UpdateLightRotation();
        GetLightSwitch();
        UpdateSonarPulse();
        //RotateController();
        MoveForward();
    }

    void UpdateSonarPulse()
    {
        if (Input.GetButtonDown("Sonar") && radarPulse && isInputActivate)
        {
            radarPulse.ActivateRadar();
        }
    }

    void ChangeSpeedHorizontal()
    {
        if (Input.GetAxis("Horizontal") > 0 && isInputActivate)
        {
            m_actualSpeedH += m_inputAccelerationH * Time.deltaTime;

            if (m_actualSpeedH >= m_maxSpeedH)
            {
                m_actualSpeedH = m_maxSpeedH;
            }

        }
        else if (Input.GetAxis("Horizontal") < 0 && m_actualSpeedH > m_minSpeedH && isInputActivate)
        {
            m_actualSpeedH -= m_inputDecelerationH * Time.deltaTime;

            if (m_actualSpeedH <= m_minSpeedH)
            {
                m_actualSpeedH = m_minSpeedH;
            }
        }
        else
        {
            if (m_actualSpeedH < 0)
            {
                m_actualSpeedH += m_accelerationH * Time.deltaTime;
            }
            else if (m_actualSpeedH > 0)
            {
                m_actualSpeedH -= m_decelerationH * Time.deltaTime;
            }

            if (m_actualSpeedH > -0.5f && m_actualSpeedH < 0.5f)
            {
                m_actualSpeedH = 0;
            }
        }
    }

    void ChangeSpeedVertical()
    {
        if (Input.GetAxis("Vertical") > 0 && isInputActivate)
        {
            m_actualSpeedV += m_inputAccelerationV * Time.deltaTime;

            if (m_actualSpeedV >= m_maxSpeedV)
            {
                m_actualSpeedV = m_maxSpeedV;
            }

        }
        else if (Input.GetAxis("Vertical") < 0 && m_actualSpeedV > m_minSpeedV && isInputActivate)
        {
            m_actualSpeedV -= m_inputDecelerationV * Time.deltaTime;

            if (m_actualSpeedV <= m_minSpeedV)
            {
                m_actualSpeedV = m_minSpeedV;
            }
        }
        else
        {
            if (m_actualSpeedV < 0)
            {
                m_actualSpeedV += m_accelerationV * Time.deltaTime;
            }
            else if (m_actualSpeedV > 0)
            {
                m_actualSpeedV -= m_decelerationV * Time.deltaTime;
            }

            if (m_actualSpeedV > -0.5f && m_actualSpeedV < 0.5f)
            {
                m_actualSpeedV = 0;
            }
        }
    }

    protected virtual void MoveForward()
    {
        Vector3 incomingMovement = ((transform.right * m_actualSpeedH) + (transform.up * m_actualSpeedV)) *Time.deltaTime;
        m_controller.Move(incomingMovement);

        if (incomingMovement != Vector3.zero && !bubbleParticle.isPlaying)
        {
            bubbleParticle.Play();
        }
        else if (incomingMovement == Vector3.zero && !bubbleParticle.isStopped)
        {
            bubbleParticle.Stop();
        }
    }

    //private void RotateController()
    //{
    //    float zRotation = transform.eulerAngles.z;

    //    if (zRotation > m_maxZRotation && zRotation < 360 + m_minZRotation)
    //    {
    //        m_rotateSpeed = 0;

    //        if (zRotation < 180)
    //        {
    //            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, m_maxZRotation - 0.1f);
    //        }
    //        else
    //        {
    //            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 360 + m_minZRotation + 0.1f);
    //        }
    //    }
    //    else if (Input.GetAxis("Vertical") > 0 && isInputActivate)
    //    {
    //        if (m_rotateSpeed >= m_maxRSpeed)
    //        {
    //            m_rotateSpeed = m_maxRSpeed;
    //        }
    //        else
    //        {
    //            m_rotateSpeed += m_rInputAcceleration * Time.deltaTime;
    //        }
    //    }
    //    else if (Input.GetAxis("Vertical") < 0 && m_rotateSpeed > m_minRSpeed && isInputActivate)
    //    {
    //        m_rotateSpeed -= m_rInputDeceleration * Time.deltaTime;
    //    }
    //    else
    //    {
    //        if (m_rotateSpeed < 0)
    //        {
    //            m_rotateSpeed += m_rAcceleration * Time.deltaTime;
    //        }

    //        if (m_rotateSpeed > 0)
    //        {
    //            m_rotateSpeed -= m_rDeceleration * Time.deltaTime;
    //        }

    //        if (m_rotateSpeed > -0.5f && m_rotateSpeed < 0.5f)
    //        {
    //            m_rotateSpeed = 0;
    //        }
    //    }

    //    transform.RotateAround(transform.position, new Vector3(0, 0, 1), m_rotateSpeed * Time.deltaTime);
    //}

    public void SetIsInputActivate(bool isActivate)
    {
        this.isInputActivate = isActivate;
        //if (radarPulse)
        //{
        //    radarPulse.SetIsActivate(isInputActivate);
        //}
    }

    void UpdateLightRotation()
    {
        if (isInputActivate)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            worldPos.z = lightObject.transform.position.z;

            lightObject.LookAt(worldPos);
        }

    }

    void GetLightSwitch()
    {
        if (Input.GetButtonDown("LightSwitch") && isInputActivate)
        {
            spotLight.gameObject.SetActive(!spotLight.gameObject.activeSelf);
            spotLightTrigger.SetActive(spotLight.gameObject.activeSelf);
        }
    }

    public virtual void TakeDamage()
    {
        Debug.Log("getOOF");
    }
}
