using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject explosionEffect; //폭발 효과   
    public LayerMask collisionMask; //부딧침 제한.

    float speed = 10f; //날아가는 속도

    public float explodeForce = 10f; //폭발 위력
    public float radius = 2.0f; //폭발반경

    private float directdamage = 30f; //직격시 주는 피해량
    
    private float explodedamage = 50f; //폭발 피해량
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);

        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    #region 탄환 부분
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        { 
            OnHitObject(hit);
            explode();
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();


        if (damageableObject != null)
        {
            damageableObject.TakeHit(directdamage, hit);
        }

        GameObject.Destroy(gameObject);
    }
    #endregion 직격탄 부분

    #region 폭발 부분
    void explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius); //폭발 지점으로부터 반경내 있었던 게임오브젝트 체크

        foreach (Collider near in colliders)
        {
            Rigidbody rig = near.GetComponent<Rigidbody>();
            LivingEntity livingEntity = near.GetComponent<LivingEntity>();

            if (rig != null)
            {
                print("TakeHit2!! : " + near.name);
                livingEntity.TakeHit2(explodedamage); //반경내 엔티티에게 데미지      
            }

            if (livingEntity != null)
            {
                print("AddExplosionForce!! : " + near.name);
                rig.AddExplosionForce(explodeForce, transform.position, radius, 1f, ForceMode.Impulse); //반경내 엔티티 밀려남.
            }

        }
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    #endregion 
}
