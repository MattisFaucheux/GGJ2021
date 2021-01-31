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

    #region Movement
    protected CharacterController m_controller;
    protected Vector3 startPosition;
    #endregion


    private RadarPulse radarPulse;

    private Transform lightObject;
    public float LightRotationMaxAngle = 165;
    public float LightRotationMinAngle = 145;


    public ParticleSystem bubbleParticle;

    public Light spotLight;
    public GameObject spotLightTrigger;


    public int damageFirstBreak = 2;
    public int damageSecondBreak = 4;
    public int damageToGameOver = 6;

    protected int damageTake = 0;
    public int damageRestoreOnRepair = 2;
    private bool isLightActivate = true;
    private bool isSonarActivate = true;

    public RepairLightZoneInt RepairLightTriggerInterior;
    public RepaiRadarZoneInt RepairSonarTriggerInterior;

    private GameManager gameManager;


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        startPosition = transform.position;
        lightObject = transform.Find("RotativeLight");

        radarPulse = transform.GetComponentInChildren<RadarPulse>();

        m_controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        ChangeSpeedHorizontal();
        ChangeSpeedVertical();
        UpdateLightRotation();
        GetLightSwitch();
        UpdateSonarPulse();
        UpdateModelRotation();
        MoveForward();
    }

    void UpdateSonarPulse()
    {
        if (Input.GetButtonDown("Sonar") && radarPulse && isInputActivate && isSonarActivate)
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

    protected virtual void UpdateModelRotation()
    {
        gameManager.RotateSubmarineModel(m_actualSpeedV);
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

    public void SetIsInputActivate(bool isActivate)
    {
        this.isInputActivate = isActivate;
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
        if (Input.GetButtonDown("LightSwitch") && isInputActivate && isLightActivate)
        {
            spotLight.gameObject.SetActive(!spotLight.gameObject.activeSelf);
            spotLightTrigger.SetActive(spotLight.gameObject.activeSelf);
        }
    }

    public virtual void TakeDamage()
    {
        damageTake += 1;

        if (damageTake == damageFirstBreak || damageTake == damageSecondBreak)
        {
            if (isLightActivate && isSonarActivate)
            {
                if (Random.Range(-1, 1) >= 0)
                {
                    BreakLight();
                }
                else
                {
                    BreakSonar();
                }
            }
            else if(isLightActivate)
            {
                BreakLight();
            }
            else
            {
                BreakSonar();
            }

        }
        else if (damageTake == damageToGameOver)
        {
            gameManager.GameOver();
        }

    }

    public void RepairLight()
    {
        isLightActivate = true;

        spotLight.gameObject.SetActive(true);
        spotLightTrigger.SetActive(true);

        RepairLightTriggerInterior.SetActive(false);
        damageTake -= damageRestoreOnRepair;

        RepairLightTriggerInterior.transform.Find("WarningLamp").gameObject.SetActive(false);
        RepairLightTriggerInterior.transform.Find("Flare").gameObject.SetActive(false);

        transform.Find("Warning").gameObject.SetActive(false);

    }

    public void RepairSonar()
    {
        isSonarActivate = true;

        RepairSonarTriggerInterior.SetActive(false);
        damageTake -= damageRestoreOnRepair;

        RepairSonarTriggerInterior.transform.Find("WarningLamp").gameObject.SetActive(false);
        RepairSonarTriggerInterior.transform.Find("Flare").gameObject.SetActive(false);

        transform.Find("Warning").gameObject.SetActive(false);
    }

    void BreakLight()
    {
        isLightActivate = false;
        spotLight.gameObject.SetActive(false);
        spotLightTrigger.SetActive(false);


        RepairLightTriggerInterior.SetActive(true);
        RepairLightTriggerInterior.transform.Find("WarningLamp").gameObject.SetActive(true);
        RepairLightTriggerInterior.transform.Find("Flare").gameObject.SetActive(true);

        transform.Find("Warning").gameObject.SetActive(true);
    }

    void BreakSonar()
    {
        isSonarActivate = false;

        RepairSonarTriggerInterior.SetActive(true);
        RepairSonarTriggerInterior.transform.Find("WarningLamp").gameObject.SetActive(true);
        RepairSonarTriggerInterior.transform.Find("Flare").gameObject.SetActive(true);

        transform.Find("Warning").gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interaction") && other.tag.Equals("PodPlayer"))
        {
            gameManager.EnterInSubmarine();
        }
    }
}
