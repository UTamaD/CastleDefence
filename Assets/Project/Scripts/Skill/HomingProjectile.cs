using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public Transform target; 
    public float speed = 10f; 
    public float rotationSpeed = 5f; 
    public float damage = 10f; 

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Calculate the direction
        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        
        Quaternion rotateToTarget = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, rotationSpeed * Time.deltaTime);

        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Check hit
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            // Hit the target
            HitTarget();
        }
    }

    void HitTarget()
    {
        Destroy(gameObject);
    }
}