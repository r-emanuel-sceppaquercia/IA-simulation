using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 180f;

    private float moveInput;
    private float turnInput;

    [field: SerializeField] public float Life { get; private set; }
    [field: SerializeField] public bool IsAlive { get; private set; }

    private void Update()
    {
        transform.Rotate(0f, turnInput * rotationSpeed * Time.deltaTime, 0f);

        transform.position += transform.forward * -moveInput * moveSpeed * Time.deltaTime;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<float>();
    }

    public void OnTurn(InputValue value)
    {
        turnInput = value.Get<float>();
    }

    public void ReceiveDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
}
