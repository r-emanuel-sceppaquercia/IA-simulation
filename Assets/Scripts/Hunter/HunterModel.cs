using UnityEngine;

public class HunterModel : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private float energy;


    [SerializeField] private float maxEnergy = 100;

    public HunterModel(float hp, float energy)
    {
        this.hp = hp;
        this.energy = energy;
    }

    public bool IsAlive() => hp > 0;
    public void TakeDamage(float damage) => hp -= damage;

    public bool OutOfEnergy() => energy <= 0;
    public bool FullEnergy() => energy == maxEnergy;
    public bool HasEnergy() => energy > 0f;

    public void ConsumeEnergy(float energy)
    {
        ConsumeEnergy(energy, true);
    }

    public void ConsumeEnergy(float energy, bool everyFrame)
    {
        if (everyFrame)
        {
            this.energy -= energy * Time.deltaTime;
        }
        else
        {
            this.energy -= energy;
        }

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
