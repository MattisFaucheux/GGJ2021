using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarPulse : MonoBehaviour
{
    [SerializeField] 
    private Transform pfRadarPing;

    private Transform pulseTransform;
    private float range;

    public float rangeMax = 300;
    public float rangeSpeed = 20;

    public LayerMask layerToDetect;

    private List<Collider> alreadyPingedColliderList;

    private void Awake()
    {
        pulseTransform = transform.Find("Pulse");
        alreadyPingedColliderList = new List<Collider>();
    }

    private void Update()
    {
        range += rangeSpeed * Time.deltaTime;
        if (range > rangeMax)
        {
            range = 0;
            alreadyPingedColliderList.Clear();
        }
        pulseTransform.localScale = new Vector3(range/2, range/2);

        RaycastHit[] hits = Physics.SphereCastAll(transform.position - (transform.up * range), range, transform.up, 10 * range, layerToDetect);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (!alreadyPingedColliderList.Contains(hit.collider))
                {
                    alreadyPingedColliderList.Add(hit.collider);
                    Transform radarPingTransform = Instantiate(pfRadarPing, new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, -1), Quaternion.identity);
                    RadarPing radarPing = radarPingTransform.GetComponent<RadarPing>();
                    if (radarPing)
                    {
                        radarPing.SetColor(Color.magenta);
                    }
                }
            }
        }
    }

}
