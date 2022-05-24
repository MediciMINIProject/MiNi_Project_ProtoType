using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public GameObject explosionEffect; //폭발 효과 
    public float delay = 3.0f; //생성 되고 폭발 할때까지의 유예시간

    public float explodeForce = 5f; //폭발 위력
    public float radius = 2.0f; //폭발반경
    
    public float damage = 2f; //주는 피해량

    Rigidbody rig;

    void explode() 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius); //폭발 지점으로부터 반경내 있었던 게임오브젝트 체크

        foreach (Collider near in colliders) 
        {
            rig = near.GetComponent<Rigidbody>();
            LivingEntity livingEntity = near.GetComponent<LivingEntity>();

            if (livingEntity != null)
            {
                livingEntity.TakeHit2(damage); //반경내 엔티티에게 데미지      
            }

            if (rig != null) 
            {
                Debug.Log(rig.gameObject);

                rig.AddExplosionForce(explodeForce, transform.position, radius, 1f, ForceMode.Impulse); //반경내 엔티티 밀려남.
            }

        }
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
       
    }
    private void Start()
    {
        Invoke("explode", delay);

    }

    private void Update()
    {
        
    }


}
