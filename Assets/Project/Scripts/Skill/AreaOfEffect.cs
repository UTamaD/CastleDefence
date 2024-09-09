using System;
using UnityEngine;
using System.Collections.Generic;

public class AreaOfEffect : MonoBehaviour
{
    public float radius = 5f;
    public float damage = 10f;
    public float duration = 10f;
    public float damageInterval = 1f;

    private float timeElapsed = 0f;
    private float lastDamageTime = 0f;

    public string DamageType = "fire";


    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed - lastDamageTime >= damageInterval)
        {
            DealDamageInArea();
            lastDamageTime = timeElapsed;
        }

        if (timeElapsed >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void DealDamageInArea()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            Monster1 monster = hitCollider.GetComponent<Monster1>();
            if (monster != null && monster.IsAlive())
            {
                monster.TakeDamage((int)damage,DamageType);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}