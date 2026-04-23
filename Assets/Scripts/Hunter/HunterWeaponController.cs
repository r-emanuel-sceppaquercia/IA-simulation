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
    [SerializeField] private float bulletSpeed = 50f;
    [SerializeField] private float bulletDamage = 10f;

    private ISteering pursuit;
    private ISteering seek;
    private ISteering weaponSeek;

    public bool HasAmmo() => ammo > 0;
    public void UseAmmo() => ammo--;
    public void Reload() => ammo = maxAmmo;
    public bool CanShoot() { return hasShot; }
    public void SetCanShoot(bool hasShot) => this.hasShot = hasShot;

    private void Update()
    {
        Debug.DrawRay(firePoint.position, firePoint.forward * 50f, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 50f, Color.magenta);
    }

    public void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().Init(pursuit.GetDir(), bulletSpeed, bulletDamage);

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
        Vector3 bodyDir = seek.GetDir();
        bodyDir.y = 0;

        Quaternion bodyRot = Quaternion.LookRotation(bodyDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, bodyRot, aimRotationSpeed * Time.deltaTime);
    }

    public void CalculateDistance(Transform target)
    {
        float distance = Vector3.Distance(firePoint.position, target.position);
        float timePrediction = distance / bulletSpeed;

        pursuit = new Pursuit(firePoint, target, timePrediction);
        seek = new Seek(transform, target);
    }

}
