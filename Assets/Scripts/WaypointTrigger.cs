using UnityEngine;
using UnityEngine.AI;

public class WaypointTrigger : MonoBehaviour
{
    public float newSpeed = 10f; // Nowa prêdkoœæ po wejœciu w obszar
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
        // SprawdŸ, czy obiekt wejœcia to bot z komponentem NavMeshAgent
        if (other.CompareTag("Bot") && !hasEntered)
        {
            // Zmieñ prêdkoœæ agenta nawigacji
            navMeshAgent.speed = newSpeed;
            hasEntered = true; // Ustaw flagê, aby unikn¹æ wielokrotnego wykonania tego kodu
        }
    }
}
