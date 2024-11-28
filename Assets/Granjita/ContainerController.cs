using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContainerController : MonoBehaviour
{
    public Transform target;
    public Transform silo;
    public Rigidbody enemyRigidbody;
    public float followDistance = 4;
    public float speed = 20f;
    public Vector2 direction;

    public float personalSpaceRadius = 1.5f;
    public float repulsionForce = 2f;
    public bool siloFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        target = target.transform;
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (siloFlag)
        {
            speed = 40;
            // Calculate distance
            float distance = Vector3.Distance(silo.position, transform.position);
            Vector3 direction = (silo.position - transform.position).normalized;

            if (distance > followDistance + 0.1f)
            {
                // Follow the silo
                Vector3 targetPosition = Vector3.MoveTowards(transform.position, silo.position, speed * Time.deltaTime);
                enemyRigidbody.MovePosition(targetPosition);
            }

            if (distance < 10)
            {
                siloFlag = false;
            }
        }
        if (!siloFlag)
        {
            // Calculate distance
            float distance = Vector3.Distance(target.position, transform.position);
            Vector3 direction = (target.position - transform.position).normalized;

            if (distance > followDistance + 0.4f)
            {
                // Follow the player
                Vector3 targetPosition = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                enemyRigidbody.MovePosition(targetPosition);
            }
            else if (distance < followDistance - 0.4f)
            {
                float temporalSpeed = 8;
                speed = temporalSpeed;
                // Move away from the player
                Vector3 targetPosition = transform.position + (direction * speed * Time.deltaTime * -1);
                enemyRigidbody.MovePosition(targetPosition);
            }
        }

        //Vector3 separation = BubbleSpace();
        //Separate(separation);
    }

    private Vector3 BubbleSpace()
    {
        Vector3 separation = Vector3.zero;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Contenedor");
        foreach (GameObject enemy in enemies)
        {
            if (enemy == gameObject)
            {
                continue;
            }

            Vector3 directionEnemies = transform.position - enemy.transform.position;
            float enemyDistance = directionEnemies.magnitude;

            if (enemyDistance <= personalSpaceRadius)
            {
                directionEnemies = directionEnemies.normalized;
                separation += directionEnemies / enemyDistance;
            }
        }

        if (enemies.Length > 1) // Avoid division by zero
        {
            separation /= enemies.Length - 1;
            separation = separation.normalized * repulsionForce;
        }

        return separation;
    }

    private void Separate(Vector3 separation)
    {
        Vector3 newPosition = transform.position + separation * Time.deltaTime;
        enemyRigidbody.MovePosition(newPosition);
    }




    private void OnDrawGizmosSelected()
    {
        //color y ubicacion con el radio del circulo
        Gizmos.DrawLine(transform.position, target.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, followDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, personalSpaceRadius);


    }
}
