using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        EnemyProfond enemyP = other.GetComponent<EnemyProfond>();
        EnemyPolype enemyP2 = other.GetComponent<EnemyPolype>();
        if (enemyP)
        {
            Vector3 dir = transform.position - other.transform.position;
            dir.z = 0;
            dir.Normalize();
            enemyP.SetIsEscaping(dir);
            //GameManager gm = FindObjectOfType<GameManager>();
            //gm.IncreaseMadness();
        }
        else if (enemyP2)
        {
            //GameManager gm = FindObjectOfType<GameManager>();
            //gm.IncreaseMadness();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyPolype enemyP = other.GetComponent<EnemyPolype>();
        if (enemyP)
        {
            enemyP.SetIsIlluminatedOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyPolype enemyP = other.GetComponent<EnemyPolype>();
        if (enemyP)
        {
            enemyP.SetIsIlluminatedOff();
        }
    }
}
