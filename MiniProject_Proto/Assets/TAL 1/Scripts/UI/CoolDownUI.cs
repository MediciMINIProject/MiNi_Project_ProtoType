using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownUI : MonoBehaviour
{
    //����
    public static CoolDownUI instance; //�ߺ� ������ ���� ���� ���Ѽ�
    private void Awake() // ���� �Ҷ�
    {
        CoolDownUI.instance = this;
    }

    #region ��Ÿ�� ����

    //��Ÿ�� 
    float coolDownTime;

    public Text coolDownUi;

    public float COOLDOWN 
    {
        get { return coolDownTime; }
        set {
            coolDownTime = value;

            if (coolDownTime == 0)
            {
                coolDownUi.text = "OK";
            }
            else 
            {
                coolDownUi.text = "��Ÿ�� : " + coolDownTime;
                if (COOLDOWN < 0)
                {
                    COOLDOWN = 0; //���̳ʽ� ����
                }
            }
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        COOLDOWN -= Time.deltaTime; // �ڵ� ����
    }
}
