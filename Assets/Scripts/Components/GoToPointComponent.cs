using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GoToPointComponent : MonoBehaviour
{
    [SerializeField]
    public Transform Destination;

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
        if (Destination == null)
        {
            return;
        }

        if (_currentTarget == Destination.position)
        {
            return;
        }

        if (_agent.SetDestination(Destination.position))
        {
            _currentTarget = Destination.position;
        }
    }
}
