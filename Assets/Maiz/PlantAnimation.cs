using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantAnimation : MonoBehaviour
{
    public float animationDuration = 2f; // Duración de la animación
    public float elevationHeight = 5f; // Altura máxima a la que se elevará
    private bool isAnimating = false; // Para evitar reiniciar la animación durante la ejecución

    void OnTriggerEnter(Collider other)
    {

        // Detecta colisión con el tag "tractor"
        if (other.CompareTag("tractor") && !isAnimating)
        {
            TractorController tractorController = other.GetComponent<TractorController>();
            if (tractorController != null)
            {
                tractorController.currentLoad++;
            }
            StartCoroutine(ElevateAndDisappear());
        }
    }

    private IEnumerator ElevateAndDisappear()
    {
        isAnimating = true;
        float time = 0;

        Vector3 startPosition = transform.position; // Posición inicial

        while (time < animationDuration)
        {
            float t = time / animationDuration; // Tiempo normalizado (0 a 1)

            // Usar una función matemática explícita (parábola para la elevación)
            float elevation = Mathf.Sin(t * Mathf.PI) * elevationHeight;

            // Ajustar la posición del objeto en el eje Y
            transform.position = startPosition + new Vector3(0, elevation, 0);

            // Ajustar la transparencia (desvanecerse)
            float fade = 1 - t; // Transparencia inversamente proporcional al tiempo
            SetObjectAlpha(fade);

            time += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que el objeto desaparezca al final
        SetObjectAlpha(0);
        Destroy(gameObject); // O usa gameObject.SetActive(false) para desactivarlo
    }

    private void SetObjectAlpha(float alpha)
    {
        // Cambiar la transparencia de todos los materiales del objeto
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            foreach (Material mat in renderer.materials)
            {
                Color color = mat.color;
                color.a = alpha;
                mat.color = color;
            }
        }
    }
}