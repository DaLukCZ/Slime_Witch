using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSystem : MonoBehaviour
{
    public Transform firePointSides;
    public Transform firePointUp;
    public GameObject bulletPrefab;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire") && Input.GetAxisRaw("Fire") > 0)
        {
            ShootSide();

        }
        if (Input.GetButtonDown("Fire") && Input.GetAxisRaw("Fire") < 0)
        {
            ShootSide();
        }
        if (Input.GetButtonDown("Fire-Up"))
        {
            ShootUp();
        }
    }

    void ShootSide()
    {
        GameObject o = Instantiate(bulletPrefab, firePointSides.position, firePointSides.rotation);
        o.GetComponent<Bullet>().Direction = GetComponent<Player_Movement>().m_FacingRight ? Vector2.right : Vector2.left;
    }
    void ShootUp()
    {
        GameObject o = Instantiate(bulletPrefab, firePointUp.position, firePointUp.rotation);
        o.GetComponent<Bullet>().Direction = Vector2.up;
    }
}
