using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HM_Gost_Anim : MonoBehaviour
{
    Animator gost_anim;

    HM_Gost_CTL gost_Ctl;

    // Start is called before the first frame update
    void Start()
    {
        gost_anim = GetComponent<Animator>();
        gost_Ctl = GetComponent<HM_Gost_CTL>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gost_Ctl.is_Live)
        {
            AnimControl();
        }
        else
        {
            gost_anim.SetTrigger("IsLive");
            
        }
    }

    void AnimControl()
    {
        if (gost_Ctl.is_Live == true)
        {
            if (gost_Ctl.is_Chase && !gost_Ctl.is_Arrange)
            {
                gost_anim.SetBool("IsChase", true);
                gost_anim.SetBool("IsArrange", false);
                gost_anim.SetBool("IsIdle", false);
            }
            else if (!gost_Ctl.is_Chase && gost_Ctl.is_Arrange)
            {
                gost_anim.SetBool("IsChase", false);

                if (gost_Ctl.is_delayOff)
                {
                    gost_anim.SetBool("IsIdle", false);
                    gost_anim.SetBool("IsArrange", true);
                }
                else
                {
                    gost_anim.SetBool("IsIdle", true);
                    gost_anim.SetBool("IsArrange", false);
                }
            }
        }
    }
}
