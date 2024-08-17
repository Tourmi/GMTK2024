using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TowerComponent : MonoBehaviour
{
    [SerializeField]
    public BaseSpellComponent Spell;

    private Collider _towerRange;
    private float _currCooldown;
    private List<GameObject> _enemiesInRange = new();

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
            Spell.ActivateTowards(target.transform.position);
            _currCooldown = Spell.EffectiveSpellAttributes.GetAttributeValue(AttributeTypes.Firerate);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _enemiesInRange.Add(other.gameObject);
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
