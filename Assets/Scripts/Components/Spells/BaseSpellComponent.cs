using System;
using UnityEngine;

public class BaseSpellComponent : MonoBehaviour
{
    public event Action<BoltComponent> OnProjectileDestroyed;
    public event Action<BoltComponent> OnProjectileUpdated;
    public event Action<BaseSpellComponent> OnSpellEnded;
    public event Action<BoltComponent, GameObject> OnEnemyHit;

    [SerializeField]
    protected Transform _spellRoot;

    [SerializeField]
    private SpellAttributes _spellAttributes;

    public SpellAttributes EffectiveSpellAttributes;

    protected virtual void Start()
    {
        EffectiveSpellAttributes = Instantiate(_spellAttributes);
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
    }

    public virtual void ActivateTowards(Vector3 direction)
    {
    }

    protected void RaiseOnProjectileDestroyed(BoltComponent bolt)
    {
        OnProjectileDestroyed?.Invoke(bolt);
    }

    protected void RaiseOnProjectileUpdated(BoltComponent bolt)
    {
        OnProjectileUpdated?.Invoke(bolt);
    }

    protected void RaiseOnSpellEnded()
    {
        OnSpellEnded?.Invoke(this);
    }

    protected void RaiseOnEnemyHit(BoltComponent bolt, GameObject enemy)
    {
        OnEnemyHit?.Invoke(bolt, enemy);
    }
}
