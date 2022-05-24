using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public GameObject explosionEffect; //���� ȿ�� 
    public float delay = 3.0f; //���� �ǰ� ���� �Ҷ������� �����ð�

    public float explodeForce = 5f; //���� ����
    public float radius = 2.0f; //���߹ݰ�
    
    public float damage = 2f; //�ִ� ���ط�

    Rigidbody rig;

    void explode() 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius); //���� �������κ��� �ݰ泻 �־��� ���ӿ�����Ʈ üũ

        foreach (Collider near in colliders) 
        {
            rig = near.GetComponent<Rigidbody>();
            LivingEntity livingEntity = near.GetComponent<LivingEntity>();

            if (livingEntity != null)
            {
                livingEntity.TakeHit2(damage); //�ݰ泻 ��ƼƼ���� ������      
            }

            if (rig != null) 
            {
                Debug.Log(rig.gameObject);

                rig.AddExplosionForce(explodeForce, transform.position, radius, 1f, ForceMode.Impulse); //�ݰ泻 ��ƼƼ �з���.
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
