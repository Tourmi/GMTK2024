using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<float, float> HealthChanged;
    public event Action HealthDepleted;

    [SerializeField]
    [Range(0f, 10f)]
    public float MaxHealth;

    [SerializeField]
    public float CurrentHealth;

    private float _lastHealth;

    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    void Update()
    {
        if (_lastHealth != CurrentHealth)
        {
            HealthChanged?.Invoke(_lastHealth, CurrentHealth);
            if (CurrentHealth <= 0)
            {
                HealthDepleted?.Invoke();
            }
        }

        _lastHealth = CurrentHealth;
    }
}
