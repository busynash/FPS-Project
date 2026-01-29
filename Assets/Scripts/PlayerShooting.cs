using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public Gun gun;
    public bool isholdinShoot = false;
    public Transform gunHolder;

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
    public void OnDrop()
    {
        if(gun!= null)
        {
            gun.Drop();
            gun = null;
        }
    }
}
