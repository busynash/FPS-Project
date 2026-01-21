using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public Gun gun;
    public bool isholdinShoot = false;

    void OnShoot()
    {
        isholdinShoot =true;
    }

    void OnShootRelease()
    {
        isholdinShoot =false;
    }

    void OnReload()
    {
        if(gun != null)
        {
            gun.tryReload();
        }
    }
    void Update()
    {
        if (isholdinShoot && gun != null)
        {
            gun.Shoot();
        }
    }
}
