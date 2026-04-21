using UnityEngine;

public class HunterAttackController : MonoBehaviour
{
    [SerializeField] private float aimTimer = 0f;
    [SerializeField] private float aimTime = 3f;

    [SerializeField] private float aimRotationSpeed;

    [SerializeField] private bool hasShot = false;

    public void Shoot()
    {
        ResetShootStats();
    }

    public void ResetShootStats()
    {
        aimTimer = 0f;
        hasShot = false;
    }

    public bool CanShot() { return hasShot; }
    public void SetCanShot(bool hasShot) => this.hasShot = hasShot;

    public void AimDelay()
    {
        aimTimer += Time.deltaTime;
        if (aimTimer >= aimTime)
            SetCanShot(true);
    }

    public void AimAt(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, aimRotationSpeed * Time.deltaTime);
    }
}
