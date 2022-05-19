using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HM_MagicMon_Ball_IP : MonoBehaviour
{
    GameObject trPlyer;
    //GameObject ins_Magic;

    Player player;

    Vector3 dir;

    public int magic_Speed;

    public float limit_Time = 1;
    float current_Time = 0;

    // Start is called before the first frame update
    void Start()
    {
        trPlyer = GameObject.FindGameObjectWithTag("Player");
        this.transform.LookAt(trPlyer.transform);

        player = trPlyer.GetComponent<Player>();


        dir = trPlyer.transform.position - this.transform.position;
        dir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += dir * magic_Speed * Time.deltaTime;

        Limit_Destroy();
    }

    void Limit_Destroy()
    {
        current_Time += Time.deltaTime;

        if (current_Time > limit_Time)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);

            player.HEALTH -= 50;

            //플레이어 체력 --


        }
        else if(collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
