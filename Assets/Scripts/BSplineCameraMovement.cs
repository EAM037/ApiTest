using UnityEngine;

public class BSplineCameraMovement : MonoBehaviour
{
    public Transform[] controlPoints; // Puntos de control (checkpoints)
    public Transform target; // Punto al que la cámara debe mirar
    public float duration = 5f; // Duración de la trayectoria completa
    private float t = 0f;
    private int degree = 3; // Grado de la B-spline
    private float[] knotVector;
    private int numControlPoints;

    private void Start()
    {
        numControlPoints = controlPoints.Length;
        knotVector = GenerateUniformKnotVector(numControlPoints, degree);
    }

    private void Update()
    {
        t += Time.deltaTime / duration;
        if (t > 1f) t -= 1f; // Reiniciar cuando se complete el trayecto

        // Calcular posición de la cámara
        Vector3 newPosition = CalculateBSplinePosition(t);
        transform.position = newPosition;

        // Hacer que la cámara mire al objetivo
        if (target != null)
        {
            transform.LookAt(target);
        }
    }

    private Vector3 CalculateBSplinePosition(float t)
    {
        float clampedT = Mathf.Clamp(t, 0f, 1f);
        float u = clampedT * (knotVector[numControlPoints] - knotVector[degree]) + knotVector[degree];
        Vector3 position = Vector3.zero;

        for (int i = 0; i < numControlPoints; i++)
        {
            float basis = DeBoorCox(i, degree, u);
            position += basis * controlPoints[i].position;
        }

        return position;
    }

    private float[] GenerateUniformKnotVector(int numControlPoints, int degree)
    {
        int n = numControlPoints + degree + 1;
        float[] knotVector = new float[n];

        for (int i = 0; i < n; i++)
        {
            if (i < degree)
            {
                knotVector[i] = 0f;
            }
            else if (i > n - degree - 1)
            {
                knotVector[i] = 1f;
            }
            else
            {
                knotVector[i] = (float)(i - degree) / (n - 2 * degree - 1);
            }
        }

        return knotVector;
    }

    private float DeBoorCox(int i, int k, float u)
    {
        if (k == 0)
        {
            return (u >= knotVector[i] && u < knotVector[i + 1]) ? 1f : 0f;
        }

        float denom1 = knotVector[i + k] - knotVector[i];
        float denom2 = knotVector[i + k + 1] - knotVector[i + 1];
        float term1 = denom1 > 0 ? ((u - knotVector[i]) / denom1) * DeBoorCox(i, k - 1, u) : 0f;
        float term2 = denom2 > 0 ? ((knotVector[i + k + 1] - u) / denom2) * DeBoorCox(i + 1, k - 1, u) : 0f;

        return term1 + term2;
    }
}