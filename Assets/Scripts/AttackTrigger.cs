using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       if (GameObject.FindGameObjectWithTag("Tower1"))
       {

            other.gameObject.SetActive(false);
       }
       if ( GameObject.FindGameObjectWithTag("Tower2"))
       {

            other.gameObject.SetActive(false);
        }

    }

}
