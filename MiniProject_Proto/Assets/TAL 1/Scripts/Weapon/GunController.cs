using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform weaponHold;
    public Gun startingGun;
    Gun equippedGun;

    // Start is called before the first frame update
    void Start()
    {
        if (startingGun != null)
        {
            EquipGun(startingGun);
        }
    }

    public void EquipGun(Gun gunToEquip) 
    {
        if (equippedGun != null) 
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation);
        equippedGun.transform.parent = weaponHold;
    }

    public void Shoot() 
    {
        if (equippedGun != null)
        {
            equippedGun.Shoot();
        }
    }

    public void SubShoot()
    {
        if (equippedGun != null)
        {          
            equippedGun.SubShoot();
        }
    }

    public void Reload() 
    {
        if (equippedGun != null) 
        {
            equippedGun.Reload();
        }
    }
}
