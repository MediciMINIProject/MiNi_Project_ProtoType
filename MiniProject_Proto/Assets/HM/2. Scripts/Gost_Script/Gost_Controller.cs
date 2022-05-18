using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gost_Controller : MonoBehaviour
{
    // '��Ʈ' ü��
    int gost_HP = 100;

    // '��Ʈ' ���ݷ�
    int gost_Attack = 10;

    // '��Ʈ'�� �׾����� üũ
    bool IsDead = false;

    // '��Ʈ'�� �̵��ϴ��� üũ
    bool IsMove = false;

    // '��Ʈ'�� �÷��̾��� �Ÿ��� ������� üũ
    bool IsClose = false;

    Animator gost_anim;
    Gost_Move gost_Move;

    // Start is called before the first frame update
    void Start()
    {
        gost_anim = GetComponent<Animator>();
        gost_Move = GetComponent<Gost_Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gost_Move.is_Move == false)
        {
            Debug.Log("attack!");
            gost_anim.SetBool("isClose", true);
            gost_anim.SetBool("isMove", false);
        }
        else 
        {
            Debug.Log("move!");
            gost_anim.SetBool("isClose", false);
            gost_anim.SetBool("isMove", true);
        }
    }
}
