using UnityEngine;

public class Boid : MonoBehaviour, IMove
{

    void Start()
    {

    }

    void Update()
    {
        transform.position += transform.forward * 5f * Time.deltaTime;
    }

    public void Move(Vector3 dir)
    {

    }
}
