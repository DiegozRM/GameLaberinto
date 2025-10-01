using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float detectionRadius = 5f;

    private bool playerInRange = false;

    void Update()
    {
        // Verificar si el jugador está en rango manualmente
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= detectionRadius)
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }

            // Si está en rango, seguir
            if (playerInRange)
            {
                FollowPlayer();
            }
        }
    }

    void FollowPlayer()
    {
        // Mover hacia el jugador
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Mirar hacia el jugador
        transform.LookAt(target);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }

            // Visualizar el radio en el editor
            void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}