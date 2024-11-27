using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 startRotationAngles;
    public Vector3 targetRotationAngles;
    public float rotationSpeed = 1f;
    private float t = 0f;

    void Update()
    {
        Quaternion startRotation = Quaternion.Euler(startRotationAngles);
        Quaternion targetRotation = Quaternion.Euler(targetRotationAngles);

        t += rotationSpeed * Time.deltaTime;
        Quaternion interpolatedRotation = Quaternion.Lerp(startRotation, targetRotation, t);

        // Apply the interpolated rotation to the object's transform
        transform.rotation = interpolatedRotation;

        // Log the interpolated euler angles
		Vector3 interpolatedEulerAngles = interpolatedRotation.eulerAngles;
        Debug.Log("Interpolated Euler Angles: " + interpolatedEulerAngles);
    }
}