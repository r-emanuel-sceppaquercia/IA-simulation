using System;
using UnityEngine;

public class EntityStats : MonoBehaviour, IDamageable
{
    [Header("HP config")]
    [SerializeField] private float maxLife = 100f;
    public float Life { get; private set; }
    public bool IsAlive { get; private set; }

    [Header("Stamina Config")]
    [SerializeField] private float maxStamina = 100f;
    public float CurrentStamina { get; private set; }
    [SerializeField] private float staminaDrainRate = 15f;
    [SerializeField] private float staminaRegenRate = 20f;

    // Events in case we wanna add UI
    public event Action OnDeath = delegate { };
    public event Action OnDamageTaken = delegate { };

    private void Awake()
    {
        Life = maxLife;
        CurrentStamina = maxStamina;
        IsAlive = true;
    }

    // Life methods, gotta check the IDamageable
    public void ReceiveDamage(float damage)
    {
        if (!IsAlive) return;

        Life -= damage;
        OnDamageTaken();

        if (Life <= 0)
        {
            Life = 0;
            IsAlive = false;
            OnDeath();

            // gameObject.SetActive(false);
        }
    }

    public bool IsHealthLow(float thresholdPercentage = 0.3f)
    {
        return (Life / maxLife) <= thresholdPercentage;
    }

    // Stamina
    public void UseStamina(float deltaTime)
    {
        CurrentStamina -= staminaDrainRate * deltaTime;
        if (CurrentStamina < 0) CurrentStamina = 0;
    }

    public void RecoverStamina(float deltaTime)
    {
        CurrentStamina += staminaRegenRate * deltaTime;
        if (CurrentStamina > maxStamina) CurrentStamina = maxStamina;
    }

    public bool HasStamina()
    {
        return CurrentStamina > 0;
    }

    public bool IsStaminaFull()
    {
        return CurrentStamina >= maxStamina;
    }
}
