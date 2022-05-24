using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HM_Gost_FSM_FIXED : MonoBehaviour
{
    public enum State
    {
        CHASE,
        ATTACK,
        DELAY,
    }

    public State state;

    GameObject player;
    Transform playerPos;

    Animator gost_anim;
    NavMeshAgent gost_AI;

    HM_GostCTL_FIXED hm_GostCtl;

    
    public bool is_Chase = true;
    public bool is_Attack = false;
    public bool is_Delay = true;

    float disToPly;

    float attack_Timer = 0f;
    public int attack_Delay = 2;

    // Start is called before the first frame update
    void Start()
    {
        state = State.CHASE;

        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform;

        gost_anim = GetComponent<Animator>();
        gost_AI = GetComponent<NavMeshAgent>();
        hm_GostCtl = GetComponent<HM_GostCTL_FIXED>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Gost FSM 스위치문
        if (player != null)
        {
            disToPly = Vector3.Distance(playerPos.position, transform.position);

            switch (state)
            {
                case State.CHASE:
                    SFM_ChaseUpdate();
                    break;

                case State.ATTACK:
                    SFM_AttackUpdate();
                    break;

                case State.DELAY:
                    SFM_DelayUpdate();
                    break;
            }
        }
        #endregion
    }

    private void SFM_DelayUpdate()                  // SFM 딜레이
    {

        attack_Timer += Time.deltaTime;

        if (attack_Timer > attack_Delay)
        {
            state = State.ATTACK;

            gost_anim.SetBool("IsAttack", true);
        }
    }

    private void SFM_AttackUpdate()                 // SFM 공격
    {
        is_Chase = false;
        is_Attack = true;

        if (disToPly > 3f)
        {
            gost_AI.isStopped = false;
            gost_AI.SetDestination(playerPos.position);

            state = State.CHASE;
            gost_anim.SetBool("IsChase", true);
            gost_anim.SetBool("IsAttack", false);
        }
       
    }

    private void SFM_ChaseUpdate()                  // SFM 추격
    {
        is_Chase = true;
        is_Attack = false;

        gost_AI.SetDestination(playerPos.position);
        gost_AI.isStopped = false;

        if (disToPly < 3)
        {
            gost_AI.isStopped = true;

            state = State.ATTACK;
            gost_anim.SetBool("IsAttack", true);
            gost_anim.SetBool("IsChase", false);
        }
    }

    public void OnAttackHit()                       // 공격 애니메이션 이벤트
    {
        if (disToPly < 3)
        {
            hm_GostCtl.AttackPlayer();

            state = State.DELAY;
            gost_anim.SetTrigger("IsDelay");
            gost_anim.SetBool("IsAttack", false);
        }
        else
        {
            state = State.CHASE;

            gost_anim.SetBool("IsChase", true);
            gost_anim.SetBool("IsAttack", false);
        }
    }
}
