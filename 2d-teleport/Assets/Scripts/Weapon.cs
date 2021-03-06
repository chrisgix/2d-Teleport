﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//By Dayton
public class Weapon : MonoBehaviour
{
    public float offset;

    public GameObject projectile;
    public GameObject player;
    public Transform shotPoint;
    public CameraShake cameraShake;

    private float timeBtwnShots;
    public GameObject lastProjectile;
    public float startTimeBtwShots;

    public Animator gunAnimator; // Dayton

    // Update is called once per frame
    void Update()
    {
        gunAnimator.SetBool("GunShot", false);

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (timeBtwnShots <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                AudioManager.instance.Play("BulletFire");
                gunAnimator.SetBool("GunShot", true);
                GameObject lProjectile = Instantiate(projectile, shotPoint.position, transform.rotation);
                lProjectile.GetComponent<Projectile>().weapon = gameObject;

                lProjectile.GetComponent<Projectile>().prevShot = lastProjectile;
                if (lastProjectile != null)
                {
                    lastProjectile.GetComponent<Projectile>().nextShot = lProjectile;
                }

                lastProjectile = lProjectile;
                timeBtwnShots = startTimeBtwShots;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (lastProjectile != null)
                {
                    AudioManager.instance.Play("TP");
                    StartCoroutine(cameraShake.Shake(0.1f, 0.4f));
                    player.transform.position = lastProjectile.transform.position;
                    Rigidbody2D playerrb = player.GetComponent<Rigidbody2D>();
                    if (playerrb.velocity.y < 0)
                    {
                        float xVel = playerrb.velocity.x;
                        playerrb.velocity = new Vector2(xVel, 0);
                    }
                    lastProjectile.GetComponent<Projectile>().DestroyProjectile();
                }
            }
        }
        else
        {
            timeBtwnShots -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (lastProjectile != null)
            {
                AudioManager.instance.Play("TP");
                StartCoroutine(cameraShake.Shake(0.1f, 0.4f));
                player.transform.position = lastProjectile.transform.position;
                Rigidbody2D playerrb = player.GetComponent<Rigidbody2D>();
                if (playerrb.velocity.y < 0)
                {
                    float xVel = playerrb.velocity.x;
                    playerrb.velocity = new Vector2(xVel, 0);
                    player.GetComponent<PlayerController>().justTeleported = true;
                }
                lastProjectile.GetComponent<Projectile>().DestroyProjectile();
            }
        }

    }
}
