using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HM_Gost_Move : MonoBehaviour
{
    NavMeshAgent gost_AI;

    GameObject player;

    HM_Gost_CTL gost_Ctl;

    float dirToPly;

    
    void Start()
    {
        gost_AI = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        gost_Ctl = GetComponent<HM_Gost_CTL>();

        gost_AI.SetDestination(player.transform.position);
    }


    void Update()
    {
        CheckDirToPly();
    }

    void CheckDirToPly()
    {
        dirToPly = Vector3.Distance(player.transform.position, this.transform.position);

        if (gost_Ctl.is_Live)
        {
            if (dirToPly < 5)
            {
                gost_Ctl.is_Chase = false;
                gost_Ctl.is_Arrange = true;
                
                gost_AI.isStopped = true;
            }
            else
            {
                gost_Ctl.is_Chase = true;
                gost_Ctl.is_Arrange = false;

                gost_AI.isStopped = false;
                gost_AI.SetDestination(player.transform.position);
            }
        }
        else
        {
            gost_AI.isStopped = true;
        }


    }

   
}
