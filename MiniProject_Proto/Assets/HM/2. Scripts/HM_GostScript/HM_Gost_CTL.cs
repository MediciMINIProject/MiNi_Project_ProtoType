using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HM_Gost_CTL : MonoBehaviour
{
    #region GOST 스탯 { Health, Attack_Stat }

    [SerializeField]
    int gost_Health = 100;          // 체력
    int gost_Attack_Stat = 10;      // 공격력

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

    #endregion

    float attack_Timer = 0f;
    int attack_Delay = 2;

    Player player;

    void Start()
    {
    }

    void Update()
    {
        Check_GostLife();
        DelayAttack();
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
            
            Destroy(this.gameObject, 3);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Gost Hit");
        if (collision.gameObject.tag == "Bullet")
        {
            
            GOST_HEALTH -= 50;
        }
        else if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<Player>();

            player.HEALTH -= 30;
        }
    }
}
