using System.Collections;
using UnityEngine;

//�ڵ� �߰�
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
    //���� �ӵ� ����(Unity ������ ���� public).
    private float normalspeed; //ĳ���� �ӵ�
    public float moveSpeed = 5; //�⺻ �ӵ�

    Camera viewCamera;

    PlayerController controller;
    GunController gunController;

    #region Ư�� �⵿
    //Ư�� �⵿ ���� ����
    public enum SelectDodge { SPR, SLD, BLK };
    public SelectDodge selectDodge;

    //���� �̵�
    public float blinkDis = 10f; //���� �̵� �Ÿ�
    private bool can_blink = true; //���� �̵� ���� ����
    public float blinkcooldown = 5.0f;

    //�޸���
    public float sprintSpeed = 7f; //�޸��� �ӵ�
    public float stamina; //���׹̳�(�ִ� �޸��� ���� �ð�)
    public float staminaMax = 5f;


    //�����̵� ����
    public Rigidbody player; //�����̵� �� ��ü
    public float slidedis = 7f; //�����̵� �� �̵��ϴ� �Ÿ� 
    public float slidecooldown = 5.0f; //�����̵� ���� ��� �ð�
    private bool can_Slide = true; //�����̵� Ȱ��ȭ ����
    #endregion

    #region �̵� ����Ʈ
    public enum MoveSpt { NON, SPD };
    public MoveSpt moveSpt;
    #endregion

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;

        stamina = staminaMax; //�ִ� ���׹̳� ����
        player = GetComponent<Rigidbody>(); //�÷��̾� �� ��������

        if (moveSpt == MoveSpt.SPD) //���ӱ� ���ý� �⺻ �̵�, Ư�� �⵿ 1.2��
        {
            normalspeed = moveSpeed * 1.2f;
            sprintSpeed *= 1.2f;
            slidedis *= 1.2f;
            blinkDis *= 1.2f;
        }
        else if (moveSpt == MoveSpt.NON) 
        {
            normalspeed = moveSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * normalspeed;
        controller.Move(moveVelocity);

        //ȭ���� ���콺 ��ġ ��ȯ
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            controller.LookAt(point);
        }

        //��� ���
        if (Input.GetMouseButton(0)) //�� Ŭ����
        {
            gunController.Shoot();
        }

        //Ư�� ����
        if (Input.GetMouseButtonDown(1))  //�� Ŭ����
        {
            gunController.SubShoot();
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            gunController.Reload();
        }


        //Ư�� �⵿
        
        if (selectDodge == SelectDodge.SPR) //�޸��� ���ý�
        {
            CoolDownUI.instance.coolDownUi.text = " ";

            if ((Input.GetKey(KeyCode.Space) && stamina > 0) && moveInput != Vector3.zero) //
            {
                sprint(); //�޸��� Ȱ��ȭ
            }
            else if ((Input.GetKey(KeyCode.Space) && stamina < 0) || !Input.GetKey(KeyCode.Space))
            {
                normalspeed = moveSpeed; //���� �ӵ����
            }
            if (!Input.GetKey(KeyCode.Space))
            {
                recoverStamina();
            }
        }

        if ((selectDodge == SelectDodge.SLD) || (selectDodge == SelectDodge.BLK))
        {
            SteminaUI.instance.SteminaUi.text = " ";
            switch (selectDodge)
            {
                case SelectDodge.SLD: //�����̵� ���ý�
                    if (Input.GetKeyDown(KeyCode.Space) && can_Slide == true)
                    {
                        slideAct();
                    } 
                    break;

                case SelectDodge.BLK: //�����̵� ���ý�
                    if (Input.GetKeyDown(KeyCode.Space) && can_blink == true)
                    {
                        blinkAct();
                    }
                    break;
            }

        }


    }
    #region �����̵�
    private void blinkAct()
    {
        CoolDownUI.instance.COOLDOWN = blinkcooldown; //��Ÿ�� ���� ����

        can_blink = false;

        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        transform.position += dir * blinkDis; //���� ��ġ �̵���(���� �̵�)

        StartCoroutine(CoolDownBlink());
    }

    IEnumerator CoolDownBlink()
    {
        yield return new WaitForSeconds(blinkcooldown); //5���� ���� ���� ����;

        can_blink = true;
        Debug.Log("���� �̵� �غ� �Ϸ�!");
    }
    #endregion

    #region �޸���
    //�޸��� ����
    private void sprint()
    {
        SteminaUI.instance.STEMINA = stamina;
        SteminaUI.instance.MAXSTEMINA = staminaMax;

        normalspeed = sprintSpeed;
        stamina -= Time.deltaTime; //������ �ִ� ���� ���� �Ҹ�
    }

    private void recoverStamina() //���׹̳� 
    {
        stamina += Time.deltaTime; //õõ�� ȸ��


        if (stamina > staminaMax)
        {
            stamina = staminaMax; //�ִ� ���׹̳��� ���� �ʵ��� ����
        }
    }
    #endregion

    #region �����̵�
    //�����̵� ����
    private void slideAct()
    {
        CoolDownUI.instance.COOLDOWN = slidecooldown; //��Ÿ�� ���� ����

        can_Slide = false;

        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        player.AddForce(dir * slidedis, ForceMode.Impulse); //�о���� �̵�

        if (dir == Vector3.zero) 
        {
            player.AddForce(player.transform.forward * slidedis, ForceMode.Impulse); //Ű���� �Է��� ���ٸ� ���콺�� �ٶ󺸴� �������� �̵�
        }

        StartCoroutine(CoolDownSlide()); //�ڷ�ƾ�� �̿��� ��Ÿ�� ����
    }

    IEnumerator CoolDownSlide()
    {
        yield return new WaitForSeconds(3.0f); //2���� ���� ���� ����;

        can_Slide = true;
        Debug.Log("�����̵� �غ� �Ϸ�!");
    }
    #endregion

}