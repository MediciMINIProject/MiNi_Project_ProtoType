using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HM_Gost_CTL : LivingEntity
{
    #region GOST 스탯 { Health, Attack_Stat }

    [SerializeField]
    int gost_Health = 100;          // 체력
    int gost_Attack_Stat = 10;      // 공격력

    public bool isHit = false;

    public int GOST_HEALTH // Gost 체력 프로퍼티
    {
        get { return gost_Health; }
        set { gost_Health = value; }
    }
    public int GOST_ATTACK_STAT
    {
        get { return gost_Attack_Stat; }
        set { gost_Attack_Stat = value; }
    }

    #endregion

    #region Bool 값 { 생존, 추적, 사거리, 딜레이 }

    public bool is_Live = true;            // 살아있는가?
    public bool is_Chase = true;          // 추적중인가?
    public bool is_Arrange = false;        // 사거리가 닿는가?
    public bool is_delayOff = true;       // 딜레이 시간이 지났는가?
    public bool is_Kuckback = false;

    #endregion

    float attack_Timer = 0f;
    int attack_Delay = 2;

    Player player;
    CapsuleCollider col;
    Rigidbody rig;
    NavMeshAgent gost_AI;

    protected override void Start()
    {
        base.Start();
        gost_AI = GetComponent<NavMeshAgent>();
        col = GetComponent<CapsuleCollider>();
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Check_GostLife();
        DelayAttack();
        IsHit();
    }

    void DelayAttack()
    {
        if(is_Live && is_Arrange)
        {
            attack_Timer += Time.deltaTime;

            if(attack_Timer > attack_Delay)
            {
                is_delayOff = true;
                
            }
            else
            {
                is_delayOff = false;
            }
        }
    }

    void Check_GostLife()
    {
        if(gost_Health <= 0)
        {
            is_Live = false;
            Destroy(col);
            Destroy(rig);
            Destroy(this.gameObject, 3);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Bullet")
        {
            print("Gost Hit");
            GOST_HEALTH -= 50;
        }
        else if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<Player>();

            player.HEALTH -= 30;
        }
    }

    public override void TakeHit2(float damage) //수류탄을 위한 데미지 계산식 (단순 데미지 받는 공식)
    {
        base.TakeHit2(damage);
        if (!dead)
        {
            base.HEALTH -= damage;
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

    void IsHit()
    {
        if (isHit == true)
        {
            gost_Health -= 30;
            isHit = false;
        }
    }


}
