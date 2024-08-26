using UnityEngine;

public class DebuffVisualEffect : MonoBehaviour
{
    public float rotationSpeed = 50f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}