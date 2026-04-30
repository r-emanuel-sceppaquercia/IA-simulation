using UnityEngine;

public class Boid : MonoBehaviour, IMove, IDamageable
{
    private Vector3 direction;
    [SerializeField] private float speed = 5f;
    [SerializeField] public float Life { get; private set; }
    [SerializeField] public bool IsAlive { get; private set; }

    void Start()
    {
        IsAlive = true;
        direction = transform.forward;
    }

    public float GetSpeed() => speed;

    public void Move(Vector3 dir)
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    public void ReceiveDamage(float damage)
    {
        IsAlive = false;

        var meshRederers = GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in meshRederers)
        {
            renderer.material.color = Color.red;
        }

        Destroy(gameObject, 2f);
    }
}
