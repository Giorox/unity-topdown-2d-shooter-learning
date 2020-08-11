using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float speed;
    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public int muzzleVelocity;
    public int magSize;
    public int maxAmmo;
    public int ammoCount;
    public int reloadTime;
    public bool fullAuto;

    private float lastfired;
    private float FireRate = 0.125f;

    Animator anim;
    bool IsReloading = false;

    [Command]
    public void CmdAnimate(string trigger)
    {
        RpcAnimate(trigger);
    }

    [ClientRpc]
    public void RpcAnimate(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    /*public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().material.color = Color.blue;
    }*/

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lastfired = Time.time;
    }

    void FixedUpdate ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        Vector3 mousePosition = this.gameObject.GetComponentInChildren<Camera>().ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = -10f;
        Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);

        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

        float inputVertical = Input.GetAxis("Vertical");
        rb.AddForce(transform.up * speed * inputVertical);

        float inputHorizontal = Input.GetAxis("Horizontal");
        rb.AddForce(transform.right * speed * inputHorizontal);

        if (Input.GetKeyDown(KeyCode.Mouse0) && fullAuto == false)
        {
            CmdFire();
        }

        if (fullAuto == true && Input.GetKey(KeyCode.Mouse0) && Time.time - lastfired > FireRate)
        {
            lastfired = Time.time;
            CmdFire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (maxAmmo > 0)
            {
                IsReloading = true;
                StartCoroutine(wait(reloadTime));
                CmdReload();
            }
        }
    }

    [Command]
    void CmdReload()
    {
        if (ammoCount == 0)
        {
            maxAmmo = maxAmmo - magSize;
            ammoCount = magSize;
            CmdAnimate("Reload");
        }
        else
        {
            maxAmmo = maxAmmo + ammoCount;
            maxAmmo = maxAmmo - magSize + 1;
            ammoCount = magSize + 1;
            CmdAnimate("Reload");
        }
    }

    IEnumerator wait(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        if(IsReloading == true)
            IsReloading = false;
    }

    [Command]
    void CmdFire()
    {
        if (ammoCount == 0 || IsReloading == true)
            return;

        CmdAnimate("Attack");
        wait(2);

        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * muzzleVelocity;

        NetworkServer.Spawn(bullet);

        ammoCount--;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}
