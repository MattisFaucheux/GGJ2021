using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        EnemyProfond enemyP = other.GetComponent<EnemyProfond>();
        if (enemyP)
        {
            Vector3 dir = transform.position - other.transform.position;
            dir.z = 0;
            dir.Normalize();
            enemyP.SetIsEscaping(dir);
        }
    }
}
