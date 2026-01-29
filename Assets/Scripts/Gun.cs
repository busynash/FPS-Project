using UnityEngine;
using System.Collections;
using Unity.Mathematics;

public class Gun : MonoBehaviour
{
    public float reloadTime = 1f;
    public float fireRate = 0.15f;
    public int magSize = 20;

    public GameObject bullet;
    public Transform bulletSpawnPoint;
    public float recoilDistance = 0.1f;
    public float recoilSpeed = 15f;

    public GameObject weaponFlash;
    public GameObject droppedWeapon;

    private int currnetAmmo;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;

    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private Vector3 reloadRotationOffset = new Vector3(66,50,50);



    void Start()
    {
        currnetAmmo = magSize;
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;
    }

    public void Shoot()
    {
        if (isReloading) return;
        if (Time.time < nextTimeToFire) return;

        if (currnetAmmo <= 0)
        {
            StartCoroutine(reload());
            return;
        }

        nextTimeToFire = Time.time + fireRate;
        currnetAmmo--;

        Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Instantiate(weaponFlash, bulletSpawnPoint.position, bulletSpawnPoint.rotation);


        StopCoroutine(nameof(Recoil));
        StartCoroutine(nameof(Recoil));
    }

    IEnumerator reload()
    {
        isReloading = true;
        Quaternion targetRotation = Quaternion.Euler(initialRotation.eulerAngles + reloadRotationOffset);
        float halfReload = reloadTime / 2f;
        float t = 0f;
        while (t < halfReload)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, t / halfReload);
            yield return null;
        }

        t = 0f;

        while (t < halfReload)
        {
            t += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(targetRotation, initialRotation, t / halfReload);
            yield return null;
        }

        currnetAmmo = magSize;
        isReloading = false;

    }
    public void tryReload()
    {
        if (isReloading) return;
        if (currnetAmmo == magSize) return;
        StartCoroutine(reload());

    }

   private IEnumerator Recoil()
   {
       Vector3 recoilTarget = initialPosition + new Vector3(recoilDistance,0, 0);
       float t = 0f;
       while (t < 1f)
       {
           t += Time.deltaTime * recoilSpeed;
           transform.localPosition = Vector3.Lerp(initialPosition, recoilTarget, t);
           yield return null;
       }
   
       t = 0f;
   
       while (t < 1f)
       {
           t += Time.deltaTime * recoilSpeed;
           transform.localPosition = Vector3.Lerp(recoilTarget, initialPosition, t);
           yield return null;
       }
       transform.localPosition = initialPosition;
   }

    public void Drop()
    {
        Instantiate(droppedWeapon, transform.position , transform.rotation);
        Destroy(gameObject);
    }
}
