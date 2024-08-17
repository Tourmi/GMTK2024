using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GoToPointComponent _enemy;

    [SerializeField]
    public Transform Target;

    [SerializeField]
    [Range(0.1f, 5f)]
    private float _timeBetweenSpawns = 1f;

    [SerializeField]
    private bool _enabled = true;

    private float _currTime;

    // Start is called before the first frame update
    void Start()
    {
        _currTime = _timeBetweenSpawns;
    }

    // Update is called once per frame
    void Update()
    {
        _currTime -= Time.deltaTime;
        if (_currTime < 0)
        {
            var enemy = Instantiate(_enemy, transform);
            enemy.Destination = Target;
            _currTime = _timeBetweenSpawns;
        }
    }
}
