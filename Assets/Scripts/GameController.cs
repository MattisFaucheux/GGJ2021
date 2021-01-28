using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class GameController : MonoBehaviour
{

    public bool isInputActivate = true;

    #region Speed
    [Header("Speed Settings")] 
    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_maxSpeed = 30;
    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_minSpeed = -10, m_acceleration = 3, m_deceleration = 3, m_inputAcceleration = 3, m_inputDeceleration = 3;

    private float m_actualSpeed;
    #endregion


    #region Rotation
    [Header("Rotation Speed Settings")]
    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_maxRSpeed = 10;

    [SerializeField]
    [Tooltip("Speed in Unit/Sec")]
    private float m_minRSpeed = -10, m_rAcceleration = 2, m_rDeceleration = 2, m_rInputAcceleration = 2, m_rInputDeceleration = 2;

    [SerializeField] 
    private float m_maxZRotation = 60, m_minZRotation = -60;

    private float m_rotateSpeed;
    #endregion


    #region Movement
    private Vector3 m_move;
    private CharacterController m_controller;
    #endregion


    private RadarPulse radarPulse;

    void Start()
    {
        radarPulse = transform.GetComponentInChildren<RadarPulse>();
        if (radarPulse)
        {
            radarPulse.SetIsActivate(isInputActivate);
        }

        m_controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        m_move = Vector3.zero;
        ChangeSpeed();
        RotateController();
        MoveForward();
    }

    void ChangeSpeed()
    {
        if (Input.GetAxis("Horizontal") > 0 && isInputActivate)
        {

            if (m_actualSpeed >= m_maxSpeed)
            {
                m_actualSpeed = m_maxSpeed;
            }
            else
            {
                m_actualSpeed += m_inputAcceleration * Time.deltaTime;
            }
        }
        else if (Input.GetAxis("Horizontal") < 0 && m_actualSpeed > m_minSpeed && isInputActivate)
        {
            m_actualSpeed -= m_inputDeceleration * Time.deltaTime;
        }
        else
        {
            if (m_actualSpeed < 0)
            {
                m_actualSpeed += m_acceleration * Time.deltaTime;
            }

            if (m_actualSpeed > 0)
            {
                m_actualSpeed -= m_deceleration * Time.deltaTime;
            }

            if (m_actualSpeed > -0.5f && m_actualSpeed < 0.5f)
            {
                m_actualSpeed = 0;
            }
        }
    }

    void MoveForward()
    {
        m_move = Vector3.zero;
        m_move = transform.right;
        m_controller.Move(m_move * m_actualSpeed * Time.deltaTime);
    }

    private void RotateController()
    {
        float zRotation = transform.eulerAngles.z;

        if (zRotation > m_maxZRotation && zRotation < 360 + m_minZRotation)
        {
            m_rotateSpeed = 0;

            if (zRotation < 180)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, m_maxZRotation - 0.1f);
            }
            else
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 360 + m_minZRotation + 0.1f);
            }
        }
        else if (Input.GetAxis("Vertical") > 0 && isInputActivate)
        {
            if (m_rotateSpeed >= m_maxRSpeed)
            {
                m_rotateSpeed = m_maxRSpeed;
            }
            else
            {
                m_rotateSpeed += m_rInputAcceleration * Time.deltaTime;
            }
        }
        else if (Input.GetAxis("Vertical") < 0 && m_rotateSpeed > m_minRSpeed && isInputActivate)
        {
            m_rotateSpeed -= m_rInputDeceleration * Time.deltaTime;
        }
        else
        {
            if (m_rotateSpeed < 0)
            {
                m_rotateSpeed += m_rAcceleration * Time.deltaTime;
            }

            if (m_rotateSpeed > 0)
            {
                m_rotateSpeed -= m_rDeceleration * Time.deltaTime;
            }

            if (m_rotateSpeed > -0.5f && m_rotateSpeed < 0.5f)
            {
                m_rotateSpeed = 0;
            }
        }

        transform.RotateAround(transform.position, new Vector3(0, 0, 1), m_rotateSpeed * Time.deltaTime);
    }

    public void SetIsInputActivate(bool isActivate)
    {
        this.isInputActivate = isActivate;
        if (radarPulse)
        {
            radarPulse.SetIsActivate(isInputActivate);
        }
    }
}
