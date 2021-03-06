using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public static WeaponUI instance; //중복 생성을 막기 위한 제한선
    private void Awake() // 시작 할때
    {
        WeaponUI.instance = this;
    }

    #region 주무장
    int remain_main; //주무장 잔여
    public Text mainUI; //주무장 잔탄 연동
    public bool isReload; //재장전 여부
    
    public int REMAINMAIN 
    {
        get { return remain_main; }
        set
        {
            remain_main = value;
            mainUI.text = "주무기 장탄 수 - " + remain_main;
        }
    }
    public bool ISRELOAD 
    {
        get { return isReload; }
        set 
        {
            isReload = value;

            if (isReload == true) 
            {
                mainUI.text = "재 장전 중...";
            }
        }
    }
    #endregion

    #region 부무장
    int remain_sub; //부무장 잔여
    public Text subUI; //부무장 잔탄 연동

    public float delay; //부 무장 딜레이
    public Text subCoolUI; //부무장 쿨다운 연동 
    
    int subMAX; //부무장 스톡수 
    
    public float isCharge;//부 무장 충전여부
    public Text SubCharge; //부무장 충전 연동


    public int REMAINSUB
    {
        get { return remain_sub; }
        set
        {
            remain_sub = value;
            subUI.text = "부무장 장탄 수 - " + remain_sub;
        }
    }


    public int SUBMAX //부 무장 최대 스톡
    {
        get { return subMAX; }
        set
        {
            subMAX = value;
        }
    }
    public float DELAY //부 무장 재사용시간
    {
        get { return delay; }
        set
        {
            delay = value;

            if (delay == 0)
            {
                subCoolUI.text = "보조무기 준비";
            }
            else
            {
                subCoolUI.text = "쿨다운 중 : " + DELAY;
                if (DELAY < 0)
                {
                    DELAY = 0; //마이너스 방지
                }
            }
        }
    }

    public float CHARGE 
    {
        get { return isCharge; }
        set 
        {
            isCharge = value;

            if ((REMAINSUB == SUBMAX))
            {
                SubCharge.text = "최대 충전 상태";
            }
            else if ((REMAINSUB != SUBMAX) && (isCharge > 0))
            {
                SubCharge.text = "현재 충전 중";
            }
        }
    }

    public float CHARGETIME 
    {
        get { return isCharge; }
        set
        {
            isCharge = value;
        }
    }

    #endregion
    private void Start()
    {
        DELAY = 0;
    }

    void Update()
    {
        DELAY -= Time.deltaTime;
        CHARGE -= Time.deltaTime;
    }

}
