using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarPulse : MonoBehaviour
{
    public bool isActivate = true;

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

    private Transform pulseTransform;
    private float range;
    #endregion

    private void Awake()
    {
        pulseTransform = transform.Find("Pulse");
        alreadyPingedColliderList = new List<Collider>();
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
            range = 0;
            alreadyPingedColliderList.Clear();
        }
        pulseTransform.localScale = new Vector3(range / 2, range / 2);
    }

    void UpdateRadarDetection()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position - (transform.forward * range), range, transform.forward, 10 * range, layerToDetect);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (!alreadyPingedColliderList.Contains(hit.collider))
                {
                    alreadyPingedColliderList.Add(hit.collider);
                    Transform radarPingTransform = Instantiate(pfRadarPing, new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, hit.collider.transform.position.z - (hit.collider.transform.localScale.z / 2) - 1), Quaternion.identity);
                    RadarPing radarPing = radarPingTransform.GetComponent<RadarPing>();
                    if (radarPing)
                    {
                        radarPing.SetColor(Color.magenta);
                    }
                }
            }
        }
    }

    public void SetIsActivate(bool isActivated)
    {
        if (!isActivated)
        {
            range = rangeMax;
            UpdatePulse();
        }

        this.isActivate = isActivated;
    }
}
