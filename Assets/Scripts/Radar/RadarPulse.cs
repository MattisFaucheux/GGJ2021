using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarPulse : MonoBehaviour
{
    private bool isActivate = false;

    #region Radar Ping
    [Header("Radar Ping Prefab")]
    [SerializeField]
    private Transform pfRadarPing;

    private List<Collider> alreadyPingedColliderList;
    #endregion

    #region Radar Settings
    [Header("Radar Settings")]
    public float rangeMax = 300;
    public float rangeSpeed = 20;
    public LayerMask layerToDetect;

    public GameObject pulsePrefab;
    private float range;
    private GameObject pulseObject;

    private Vector3 startPosition;

    public float reloadTime = 5.0f;
    private bool canActivate = true;
    #endregion

    private void Awake()
    {
        alreadyPingedColliderList = new List<Collider>();
    }

    void Start()
    {
        isActivate = false;
        ResetPulse();
    }

    private void Update()
    {
        if (isActivate)
        {
            UpdatePulse();
            UpdateRadarDetection();
        }
    }

    void UpdatePulse()
    {
        range += rangeSpeed * Time.deltaTime;
        if (range > rangeMax)
        {
            ResetPulse();
            isActivate = false;
        }
        else
        {
            DrawPulse();
        }
    }

    void DrawPulse()
    {
        if (pulseObject)
        {
            pulseObject.transform.localScale = new Vector3(range / 2, range / 2);
        }
    }

    void UpdateRadarDetection()
    {
        RaycastHit[] hits = Physics.SphereCastAll(startPosition - (transform.forward * range), range, transform.forward, 10 * range, layerToDetect);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (!alreadyPingedColliderList.Contains(hit.collider))
                {
                    alreadyPingedColliderList.Add(hit.collider);
                    Transform radarPingTransform = Instantiate(pfRadarPing, new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, hit.collider.transform.position.z - (hit.collider.transform.localScale.z / 2) - 0.1f), Quaternion.identity);
                    RadarPing radarPing = radarPingTransform.GetComponent<RadarPing>();
                    if (radarPing)
                    {
                        radarPing.SetColor(Color.white);
                    }
                }
            }
        }
    }

    void ResetPulse()
    {
        if (pulseObject)
        {
            Destroy(pulseObject);
        }
        range = 0.1f;
        alreadyPingedColliderList.Clear();
    }

    public void ActivateRadar()
    {
        if (canActivate)
        {
            ResetPulse();
            pulseObject = Instantiate(pulsePrefab, transform.position, new Quaternion(0, 0, 0, 0));
            startPosition = transform.position;
            isActivate = true;
            canActivate = false;
            StartCoroutine(ResetCanActivate());
        }
    }

    IEnumerator ResetCanActivate()
    {
        Debug.Log(reloadTime);
        yield return new WaitForSeconds(reloadTime);
        canActivate = true;
    }
}
