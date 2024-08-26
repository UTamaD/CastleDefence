using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage;
    public GameObject target;
    public float area = 0.25f;

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);

            if (Vector3.Distance(transform.position, target.transform.position) < area)
            {
                Hit();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Hit()
    {
        Monster1 monster = target.GetComponent<Monster1>();
        if (monster != null && monster.IsAlive())
        {
            monster.TakeDamage((int)damage,"lightning");
        }
        else
        {
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}