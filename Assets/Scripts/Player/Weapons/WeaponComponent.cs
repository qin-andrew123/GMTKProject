using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponComponent : MonoBehaviour
{
    [SerializeField] private WeaponComponentData componentData;
    [SerializeField] private GameObject firePoint;
    private float weaponCooldown;
    private int numProjectilesFired;
    private GameObject projectile;
    private bool bCanShoot = true;
    // Start is called before the first frame update
    void Start()
    {
        weaponCooldown = componentData.fireCooldown;
        numProjectilesFired = componentData.numProjectilesShot;
        projectile = componentData.ProjectilePrefab;
    }

    // Update is called once per frame
    void Update()
    {
        if(bCanShoot && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(FireProjectile());
        }
    }

    private IEnumerator FireProjectile()
    {
        bCanShoot = false;
        GameObject newProjectile = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
        ProjectileComponent projComp = newProjectile.GetComponent<ProjectileComponent>();
        Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.transform.position;
        mouseDirection.z = 0;
        mouseDirection.Normalize();
        if(projComp)
        {
            projComp.MoveDirection = mouseDirection;
        }
        yield return new WaitForSeconds(weaponCooldown);

        bCanShoot = true;
    }
}
