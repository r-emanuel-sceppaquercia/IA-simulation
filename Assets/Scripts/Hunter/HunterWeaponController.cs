using UnityEngine;

public class HunterWeaponController : MonoBehaviour, IWeapon
{
    [SerializeField] private int ammo;
    [SerializeField] private int maxAmmo = 2;

    [SerializeField] private float aimTimer = 0f;
    [SerializeField] private float aimTime = 3f;

    [SerializeField] private float aimRotationSpeed;

    [SerializeField] private bool hasShot = false;

    [SerializeField] private Transform weapon;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;

    public bool HasAmmo() => ammo > 0;
    public void UseAmmo() => ammo--;
    public void Reload() => ammo = maxAmmo;
    public bool CanShoot() { return hasShot; }
    public void SetCanShoot(bool hasShot) => this.hasShot = hasShot;

    public void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().Init(firePoint.forward, bulletSpeed);

        UseAmmo();
        ResetShootStats();
    }

    public void ResetShootStats()
    {
        aimTimer = 0f;
        hasShot = false;
    }

    public void AimDelay()
    {
        aimTimer += Time.deltaTime;
        if (aimTimer >= aimTime)
            SetCanShoot(true);
    }

    public void AimAt(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion rotateTowards = Quaternion.RotateTowards(transform.rotation, targetRotation, aimRotationSpeed * Time.deltaTime);

        transform.rotation = rotateTowards;
        weapon.rotation = rotateTowards;
    }

}
