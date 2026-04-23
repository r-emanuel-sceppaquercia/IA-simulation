using UnityEngine;

public class Bullet : MonoBehaviour, IMove
{
    public float speed;
    private float damage;

    private Vector3 direction;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 direction, float speed, float damage)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;

        Destroy(gameObject, 5f);
    }

    void Update()
    {
        Move(direction);
    }

    public void Move(Vector3 dir)
    {
        rb.linearVelocity = dir * speed;
    }

    private void OnTriggerEnter(Collider collider)
    {
        print("collision with: " + collider.gameObject.name);

        var boid = collider.GetComponent<Boid>();

        if (boid != null)
        {
            Destroy(gameObject);
            boid.ReceiveDamage(damage);
        }
    }
}
