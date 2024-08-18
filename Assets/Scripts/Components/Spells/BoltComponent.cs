using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoltComponent : MonoBehaviour
{
    [SerializeField]
    private Renderer _renderer;

    public event Action<BoltComponent, GameObject> OnEnemyHit;
    public event Action<BoltComponent> OnProjectileExpired;
    public event Action<BoltComponent> OnUpdate;

    public SpellAttributes SpellAttributes;

    public float TimeElapsed;

    private float _piercesLeft;

    // Start is called before the first frame update
    private void Start()
    {
        _renderer.material.color = SpellAttributes.Color;
        transform.localScale = Vector3.one * SpellAttributes.GetAttributeValue(AttributeTypes.Size);
        _piercesLeft = SpellAttributes.GetAttributeValue(AttributeTypes.Pierce);
    }

    private void Update()
    {
        OnUpdate?.Invoke(this);

        TimeElapsed += Time.deltaTime;
        if (TimeElapsed > SpellAttributes.GetAttributeValue(AttributeTypes.TimeToLive))
        {
            OnProjectileExpired?.Invoke(this);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += SpellAttributes.GetAttributeValue(AttributeTypes.Speed) * Time.fixedDeltaTime * transform.forward;
    }

    public void OnCollisionEnter(Collision collision)
    {
        OnEnemyHit?.Invoke(this, collision.gameObject);
        _piercesLeft -= 1f;
        if (_piercesLeft < 0f)
        {
            OnProjectileExpired?.Invoke(this);
        }
    }
}
