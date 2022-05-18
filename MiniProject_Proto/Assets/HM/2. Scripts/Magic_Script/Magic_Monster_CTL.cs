using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
//�÷��̾�� ������ �Ÿ����� ������ �� ���缭 ���Ÿ� ���� �߻�

// 1. �÷��̾�� ����
// 2. ������ �Ÿ����� �����ϰ� ����
// 3. �Ѿ��� ���� �� �߻� 
// 4. ���� �� ���� �ݺ�
public class Magic_Monster_CTL : MonoBehaviour
{
    NavMeshAgent magic_Mon;

    Animator magicMon_Anim;

    Vector3 dir;

    GameObject player;
    public GameObject magic_prefeb;
    GameObject magic;
    Transform magic_Pos;

    float dirToPly;

    float current_Time = 0;
    public float limit_Time;

    bool is_Close = false;
    bool is_Attack = false;
    bool is_Move = true;
    bool is_Die = false;
    // Start is called before the first frame update
    void Start()
    {
        magicMon_Anim = GetComponent<Animator>();

        magic_Mon = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player");

        magic_Mon.SetDestination(player.transform.position);

        magic_Pos = this.transform.GetChild(2).gameObject.transform;

        //magic_prefeb = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Test_Scene/Prefeb/Monster/Monster_Magic.prefab",typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {
        dirToPly = Vector3.Distance(player.transform.position, this.transform.position);

        this.transform.LookAt(player.transform);

        Monster_Move();

        Monster_Anim_CTL();
    }

    // �ణ�� �ð����� �ΰ� �Ѿ��� �߻���
    void Shoot_Delay()
    {
        current_Time += Time.deltaTime;

        if (current_Time > limit_Time)
        {

            is_Attack = true;

            current_Time = 0;

            magic = Instantiate(magic_prefeb);

            magic.transform.position = magic_Pos.position;
        }
        else
        {
            is_Attack = false;
        }
    }

    void Monster_Move()
    {
        print(dirToPly);
        if(dirToPly < 15)
        {
            magic_Mon.isStopped = true;
            //is_Attack = true;
            is_Move = false;

            Shoot_Delay();
        }
        else
        {
            magic_Mon.isStopped = false;
            is_Attack = false;
            is_Move = true;
            magic_Mon.SetDestination(player.transform.position);
            
        }
    }

    void Monster_Anim_CTL()
    {
        if(is_Move && !is_Attack)
        {
            magicMon_Anim.SetBool("IsMove", true);
            magicMon_Anim.SetBool("IsAttack", false);
        }
        else if(is_Attack && !is_Move)
        {
            magicMon_Anim.SetBool("IsAttack", true);
            magicMon_Anim.SetBool("IsMove", false);
        }
        else if(is_Die)
        {
            magicMon_Anim.SetBool("IsDie", true);
        }
    }
}
