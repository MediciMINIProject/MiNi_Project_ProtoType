using System.Collections;
using UnityEngine;

//자동 추가
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
    //기초 속도 설정(Unity 수정을 위해 public).
    private float normalspeed; //캐릭터 속도
    public float moveSpeed = 5; //기본 속도

    Camera viewCamera;

    PlayerController controller;
    GunController gunController;

    #region 특수 기동
    //특수 기동 형태 선택
    public enum SelectDodge { SPR, SLD, BLK };
    public SelectDodge selectDodge;

    //순간 이동
    public float blinkDis = 10f; //순간 이동 거리
    private bool can_blink = true; //순간 이동 가능 여부
    public float blinkcooldown = 5.0f;

    //달리기
    public float sprintSpeed = 7f; //달리는 속도
    public float stamina; //스테미나(최대 달리기 유지 시간)
    public float staminaMax = 5f;


    //슬라이딩 구현
    public Rigidbody player; //슬라이딩 할 객체
    public float slidedis = 7f; //슬라이딩 시 이동하는 거리 
    public float slidecooldown = 5.0f; //슬라이딩 재사용 대기 시간
    private bool can_Slide = true; //슬라이딩 활성화 여부
    #endregion

    #region 이동 서포트
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

        stamina = staminaMax; //최대 스테미나 충전
        player = GetComponent<Rigidbody>(); //플레이어 몸 가져오기

        if (moveSpt == MoveSpt.SPD) //가속기 선택시 기본 이동, 특수 기동 1.2배
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
        print(health);

        Vector3 moveInput = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * normalspeed;
        controller.Move(moveVelocity);

        //화면의 마우스 위치 반환
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            controller.LookAt(point);
        }

        //사격 기능
        if (Input.GetMouseButton(0)) //좌 클릭시
        {
            gunController.Shoot();
        }

        //특수 공격
        if (Input.GetMouseButtonDown(1))  //우 클릭시
        {
            gunController.SubShoot();
        }

        if (Input.GetKeyDown(KeyCode.R)) 
        {
            gunController.Reload();
        }


        //특수 기동
        
        if (selectDodge == SelectDodge.SPR) //달리기 선택시
        {
            CoolDownUI.instance.coolDownUi.text = " ";

            if ((Input.GetKey(KeyCode.Space) && stamina > 0) && moveInput != Vector3.zero) //
            {
                sprint(); //달리기 활성화
            }
            else if ((Input.GetKey(KeyCode.Space) && stamina < 0) || !Input.GetKey(KeyCode.Space))
            {
                normalspeed = moveSpeed; //원래 속도대로
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
                case SelectDodge.SLD: //슬라이딩 선택시
                    if (Input.GetKeyDown(KeyCode.Space) && can_Slide == true)
                    {
                        slideAct();
                    } 
                    break;

                case SelectDodge.BLK: //순간이동 선택시
                    if (Input.GetKeyDown(KeyCode.Space) && can_blink == true)
                    {
                        blinkAct();
                    }
                    break;
            }

        }


    }
    #region 순간이동
    private void blinkAct()
    {
        CoolDownUI.instance.COOLDOWN = blinkcooldown; //쿨타임 배정 공유

        can_blink = false;

        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        transform.position += dir * blinkDis; //일정 위치 이동함(순간 이동)

        StartCoroutine(CoolDownBlink());
    }

    IEnumerator CoolDownBlink()
    {
        yield return new WaitForSeconds(blinkcooldown); //5초후 다음 것을 실행;

        can_blink = true;
        Debug.Log("순간 이동 준비 완료!");
    }
    #endregion

    #region 달리기
    //달리기 구현
    private void sprint()
    {
        SteminaUI.instance.STEMINA = stamina;
        SteminaUI.instance.MAXSTEMINA = staminaMax;

        normalspeed = sprintSpeed;
        stamina -= Time.deltaTime; //누르고 있는 동안 지속 소모
    }

    private void recoverStamina() //스테미너 
    {
        stamina += Time.deltaTime; //천천히 회복


        if (stamina > staminaMax)
        {
            stamina = staminaMax; //최대 스테미나양 넘지 않도록 제한
        }
    }
    #endregion

    #region 슬라이딩
    //슬라이딩 구현
    private void slideAct()
    {
        CoolDownUI.instance.COOLDOWN = slidecooldown; //쿨타임 배정 공유

        can_Slide = false;

        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        player.AddForce(dir * slidedis, ForceMode.Impulse); //밀어내듯이 이동

        if (dir == Vector3.zero) 
        {
            player.AddForce(player.transform.forward * slidedis, ForceMode.Impulse); //키보드 입력이 없다면 마우스가 바라보는 방향으로 이동
        }

        StartCoroutine(CoolDownSlide()); //코루틴을 이용한 쿨타임 구현
    }

    IEnumerator CoolDownSlide()
    {
        yield return new WaitForSeconds(3.0f); //2초후 다음 것을 실행;

        can_Slide = true;
        Debug.Log("슬라이딩 준비 완료!");
    }
    #endregion

}
