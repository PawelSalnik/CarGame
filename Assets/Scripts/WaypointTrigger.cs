using UnityEngine;
using UnityEngine.AI;

public class WaypointTrigger : MonoBehaviour
{
    public float newSpeed = 10f; // Nowa pr�dko�� po wej�ciu w obszar
    private NavMeshAgent navMeshAgent;
    private bool hasEntered = false;

    void Start()
    {
        // Pobierz komponent NavMeshAgent z tego samego obiektu
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("Brakuje komponentu NavMeshAgent na tym obiekcie.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Sprawd�, czy obiekt wej�cia to bot z komponentem NavMeshAgent
        if (other.CompareTag("Bot") && !hasEntered)
        {
            // Zmie� pr�dko�� agenta nawigacji
            navMeshAgent.speed = newSpeed;
            hasEntered = true; // Ustaw flag�, aby unikn�� wielokrotnego wykonania tego kodu
        }
    }
}
