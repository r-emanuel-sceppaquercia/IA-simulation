using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    public void Init(Vector3 dir, float speed)
    {
        this.direction = dir;
        this.speed = speed;

        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        print("collision with: " + collider.gameObject.name);

        var boidCollision = collider.GetComponent<Boid>();

        if (boidCollision != null)
        {
            print("Colision with boid");

            Destroy(gameObject);
            Destroy(boidCollision);
        }
    }
}
