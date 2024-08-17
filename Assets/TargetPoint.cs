using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TargetPoint : MonoBehaviour
{
    [SerializeField]
    private Transform _destination;

    private NavMeshAgent _agent;
    private Vector3 _currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentTarget == _destination.position)
        {
            return;
        }

        if (_agent.SetDestination(_destination.position))
        {
            _currentTarget = _destination.position;
        }
    }
}
