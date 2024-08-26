using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public Transform target; // The monster to follow
    public float speed = 10f; // Speed of the projectile
    public float rotationSpeed = 5f; // Speed of rotation towards the target
    public float damage = 10f; // Damage dealt to the target

    void Update()
    {
        if (target == null)
        {
            // Destroy the projectile if there's no target
            Destroy(gameObject);
            return;
        }

        // Calculate the direction towards the target
        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        // Calculate the rotation needed to face the target
        Quaternion rotateToTarget = Quaternion.LookRotation(direction);

        // Rotate the projectile over time to face the target
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, rotationSpeed * Time.deltaTime);

        // Move the projectile forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Check if the projectile is close enough to hit the target
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            // Hit the target
            HitTarget();
        }
    }

    void HitTarget()
    {
        // Apply damage to the target (if it has a health component)
        // target.GetComponent<Health>().TakeDamage(damage);

        // Destroy the projectile on impact
        Destroy(gameObject);
    }
}