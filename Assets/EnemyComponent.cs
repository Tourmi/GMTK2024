using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class EnemyComponent : MonoBehaviour
{
    HealthComponent _health;

    void Start()
    {
        _health = GetComponent<HealthComponent>();
        _health.HealthDepleted += HealthDepleted;
    }

    void Update()
    {

    }

    private void HealthDepleted() => Destroy(gameObject);

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.TryGetComponent<BoltComponent>(out var bolt))
        {
            _health.CurrentHealth -= bolt.SpellAttributes.GetAttributeValue(AttributeTypes.Power);
        }
    }
}
