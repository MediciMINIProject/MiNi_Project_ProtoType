using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HM_MagicMon_CTL_IP : MonoBehaviour
{
    #region MagicMon 스탯 { Health, Attack_Stat }
    
    [SerializeField] int magic_Helath = 80;

    int magic_AttackStat = 20;

    public int MAGIC_HELATH // MagicMon 체력 프로퍼티
    {
        get { return magic_Helath; }
        set { magic_Helath = value; }
    }
    public int MAGIC_ATTACKSTAT
    {
        get { return magic_AttackStat; }
        set { magic_AttackStat = value; }
    }
    #endregion

    #region Bool값 { 생존, 추적, 사거리, 딜레이 }
    public bool is_Live = true;            // 살아있는가?
    public bool is_Chase = true;          // 추적중인가?
    public bool is_Arrange = false;        // 사거리가 닿는가?
    public bool is_Attack = false;
    #endregion

    public GameObject magic_prefeb;
    Transform magic_Pos;

    float attack_Timer = 1.5f;
    [SerializeField ]int attack_Delay = 3;

    void Start()
    {
        magic_Pos = this.transform.GetChild(2).gameObject.transform;
    }

    void Update()
    {
        Check_MagicMonLife();

        DelayAttack();
    }

    void DelayAttack()
    {
        if (is_Live && is_Arrange)
        {
            attack_Timer += Time.deltaTime;

            if (attack_Timer > attack_Delay)
            {
                is_Attack = true;

                GameObject magic = Instantiate(magic_prefeb);

                magic.transform.position = magic_Pos.position;

                attack_Timer = 0;
            }
            else
            {
                is_Attack = false;
            }
        }
    }

    void Check_MagicMonLife()
    {
        if (magic_Helath <= 0)
        {
            is_Live = false;

            Destroy(this.gameObject, 3);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            MAGIC_HELATH -= 40;
        }
    }
}
