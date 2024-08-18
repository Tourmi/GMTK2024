using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class TowerComponent : MonoBehaviour
{
    [SerializeField]
    public BaseSpellComponent Spell;

    private Collider _towerRange;
    private float _currCooldown;
    private List<GameObject> _enemiesInRange = new();
    private Vector3 targetShot;
    private Vector3 _targetEnemy;

    // Start is called before the first frame update
    void Start()
    {
        _towerRange = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        _currCooldown -= Time.deltaTime;
        if (_currCooldown > 0)
        {
            return;
        }
        var target = GetNearestEnemy();
        if (target != null)
        {
            var enemyComponent = target.GetComponent<NavMeshAgent>();
            var bulletSpeed = Spell.EffectiveSpellAttributes.GetAttributeValue(AttributeTypes.Speed);
            _targetEnemy = target.transform.position;
            var horizontalPos = transform.position.WithY(target.transform.position.y);
            targetShot = (enemyComponent.velocity / bulletSpeed * Vector3.Distance(horizontalPos, _targetEnemy)) + _targetEnemy;
            Spell.ActivateTowards(targetShot);
            _currCooldown = Spell.EffectiveSpellAttributes.GetAttributeValue(AttributeTypes.Firerate);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, targetShot - transform.position);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, _targetEnemy - transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _enemiesInRange.Remove(other.gameObject);
    }

    private GameObject GetNearestEnemy()
    {
        GameObject nearest = null;
        var nearestDistance = float.PositiveInfinity;
        foreach (var enemy in _enemiesInRange.ToArray())
        {
            if (enemy == null)
            {
                _enemiesInRange.Remove(enemy);
                continue;
            }

            var distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < nearestDistance)
            {
                nearest = enemy;
                nearestDistance = distance;
            }
        }

        return nearest;
    }
}
