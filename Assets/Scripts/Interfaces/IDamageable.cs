public interface IDamageable
{
    float Life { get; }
    bool IsAlive { get; }

    void ReceiveDamage(float damage);
}
