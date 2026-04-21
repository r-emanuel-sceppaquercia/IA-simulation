using UnityEngine;

public class HunterModel : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private float energy;
    [SerializeField] private int ammo;

    [SerializeField] private float maxEnergy = 100;
    [SerializeField] private int maxAmmo = 2;

    public HunterModel(float hp, float energy)
    {
        this.hp = hp;
        this.energy = energy;
        ammo = maxAmmo;
    }

    public bool HasAmmo() => ammo > 0;
    public void UseAmmo() => ammo--;
    public void Reload() => ammo = maxAmmo;

    public bool IsAlive() => hp > 0;
    public void TakeDamage(float damage) => hp -= damage;

    public bool OutOfEnergy() => energy <= 0;
    public bool FullEnergy() => energy == maxEnergy;
    public bool HasEnergy() => energy > 0f;

    public void ConsumeEnergy(float energy)
    {
        this.energy -= energy * Time.deltaTime;

        //Debug.Log("Energy: " + this.energy);

        if (this.energy <= 0)
            this.energy = 0;
    }

    public void RecoverEnergy(float energy)
    {
        this.energy += energy * Time.deltaTime;

        //Debug.Log("Energy: " + this.energy);

        if (this.energy > maxEnergy)
            this.energy = maxEnergy;
    }

}
