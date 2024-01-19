using UnityEngine;

public class SpriteMoverScript : MonoBehaviour
{
    public float speed = 5f;         // Controls the speed of the movement
    public float radius = 10f;       // Controls the radius of the circle
    public Transform centerObject;   // The GameObject to orbit around (optional)

    private Vector3 centerPosition;

    void Start()
    {
        // If a center object is not provided, use the GameObject's current position as the center
        centerPosition = centerObject != null ? centerObject.position : transform.position;
    }

    void Update()
    {
        // Calculate the angle of rotation for smooth circular motion
        float angle = Time.time * speed;

        // Calculate the new position using sin and cos for circular movement
        Vector3 newPosition = centerPosition + new Vector3(Mathf.Sin(angle) * radius, 0, Mathf.Cos(angle) * radius);

        // Set the GameObject's position to the calculated new position
        transform.position = newPosition;
    }
}
