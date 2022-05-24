using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HM_GostCTL_FIXED : LivingEntity
{
    CapsuleCollider col;
    Rigidbody rig;
    NavMeshAgent gost_AI;
    Animator gost_anim;

    LivingEntity livingEntity;

    private bool is_Kuckback = false;
    public bool is_Die = false;

    protected override void Start()
    {
        base.Start();

        col = GetComponent<CapsuleCollider>();
        rig = GetComponent<Rigidbody>();
        gost_AI = GetComponent<NavMeshAgent>();
        gost_anim = GetComponent<Animator>();

        livingEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<LivingEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (base.dead == true)
        {
            Gost_Die();
        }

        print(base.HEALTH);
    }

    public override void TakeHit2(float damage)
    {
        base.TakeHit2(damage);

        if (!dead)
        {
            is_Kuckback = true;
            gost_AI.enabled = false;
            rig.isKinematic = false;
            Invoke("RestoreAI", 1);
        }
    }

    void RestoreAI()
    {
        gost_AI.enabled = true;
        rig.isKinematic = true;
        is_Kuckback = false;
    }

    public void AttackPlayer()
    {
        livingEntity.TakeHit2(1f);
    }

    public void Gost_Die()
    {
        is_Die = true;
        gost_AI.enabled = false;
        rig.isKinematic = true;
        col.enabled = false;
        gost_anim.SetTrigger("IsDie");
    }
}
