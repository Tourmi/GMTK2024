using UnityEngine;

public class BoltSpellComponent : BaseSpellComponent
{
    [SerializeField]
    private BoltComponent _boltPrefab;

    protected override void Start() => base.Start();

    protected override void Update() => base.Update();

    protected override void FixedUpdate() => base.FixedUpdate();

    public override void ActivateTowards(Vector3 destination)
    {
        base.ActivateTowards(destination);

        var bolt = Instantiate(_boltPrefab, _spellRoot);
        bolt.SpellAttributes = EffectiveSpellAttributes;
        bolt.transform.position = transform.position;
        destination.y = transform.position.y;
        bolt.transform.LookAt(destination, transform.up);

        bolt.OnEnemyHit += Bolt_OnEnemyHit;
        bolt.OnUpdate += Bolt_OnUpdate;
        bolt.OnProjectileExpired += Bolt_OnProjectileExpired;
    }

    private void Bolt_OnProjectileExpired(BoltComponent obj)
    {
        RaiseOnProjectileDestroyed(obj);
        Destroy(obj.gameObject);
    }

    private void Bolt_OnUpdate(BoltComponent obj)
    {
        RaiseOnProjectileUpdated(obj);
    }

    private void Bolt_OnEnemyHit(BoltComponent bolt, GameObject enemy)
    {
        RaiseOnEnemyHit(bolt, enemy);
    }
}
