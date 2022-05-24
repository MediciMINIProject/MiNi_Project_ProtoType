using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update

    public LayerMask collisionMask;
    
    float speed = 10f;
    public float damage = 0.5f; // ÅºÈ¯ °ø°Ý·Â



    void Start()
    {
        Destroy(gameObject, 2f);
    }

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        //CheckCollisions(moveDistance);

        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MagicMon")
        {
            other.gameObject.GetComponent<HM_MagicMon_CTL_IP>().isHit = true;

            print("mmÀÌ ÃÑ¿¡ ¸ÂÀ½");

            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Gost")
        {
            other.gameObject.GetComponent<HM_Gost_CTL>().isHit = true;

            print("gost°¡ ÃÑ¿¡ ¸ÂÀ½");

            Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        print("HIT!!");
        if(collision.gameObject.tag == "MagicMon")
        {
            collision.gameObject.GetComponent<HM_MagicMon_CTL_IP>().isHit = true;

            print("mmÀÌ ÃÑ¿¡ ¸ÂÀ½");

            Destroy(this.gameObject);
        }
        else if(collision.gameObject.tag == "Gost")
        {
            collision.gameObject.GetComponent<HM_Gost_CTL>().isHit = true;

            print("gost°¡ ÃÑ¿¡ ¸ÂÀ½");

            Destroy(this.gameObject);
        }
    }


    void CheckCollisions(float moveDistance) 
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            //OnHitObject(hit);

           
        }
    }

    void OnHitObject(RaycastHit hit) 
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();


        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }
        
        GameObject.Destroy(gameObject);
    }

    
}
