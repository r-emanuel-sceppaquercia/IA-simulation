using UnityEngine;

public class LeaderModel : MonoBehaviour
{
    [field: SerializeField] public Herd HerdType { get; private set; }

    [field: SerializeField] public float CurrentHp { get; private set; }
    [field: SerializeField] public float MaxHp { get; private set; }

    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float RoamingSpeed { get; private set; }
    [field: SerializeField] public float FleeSpeed { get; private set; }

    [field: SerializeField] public float AvoidRadius { get; private set; }
    [field: SerializeField] public float AvoidWeight { get; private set; }

    [field: SerializeField] public float AttackRange { get; private set; }

    public bool OnLowHp()
    {
        return CurrentHp < MaxHp / 10;
    }
}
