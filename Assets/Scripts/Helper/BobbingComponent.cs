using UnityEngine;

public class BobbingComponent : MonoBehaviour
{
    // How high the indicator bobs (in Unity units)
    public float amplitude = 0.5f;

    // How fast the indicator bobs up and down
    public float frequency = 2.0f;

    // The starting local position of the indicator (captured on Start)
    private Vector3 initialPosition;

    private void Start()
    {
        // Save the initial local position of the GameObject this script is attached to
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        // Calculate the new y position using a sine wave to create a smooth bobbing effect
        float newY = initialPosition.y + Mathf.Sin(UnityEngine.Time.time * frequency) * amplitude;
        transform.localPosition = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}