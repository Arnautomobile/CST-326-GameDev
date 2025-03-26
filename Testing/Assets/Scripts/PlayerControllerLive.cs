using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerLive : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    private NavMeshAgent _navMeshAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        _navMeshAgent.destination = _destination.position;
    }
}
