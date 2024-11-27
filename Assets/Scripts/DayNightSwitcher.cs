using UnityEngine;

public class MoonLightSwitcher : MonoBehaviour
{
    public Light directionalLight; // La luz direccional principal (asígnala en el Inspector)
    public KeyCode toggleKey = KeyCode.N; // Tecla para alternar entre día y noche

    // Configuración de día
    public Vector3 dayRotation = new Vector3(179.1f, -30f, 0f); // Rotación de la luz direccional para el día
    public Color dayLightColor = Color.white;
    public float dayIntensity = 1.0f;

    // Configuración de noche (luz de luna)
    public Vector3 nightRotation = new Vector3(15f, -40f, 0f); // Luz baja y suave, como la luna
    public Color moonLightColor = new Color(0.4f, 0.5f, 0.8f); // Azul frío tenue
    public float moonIntensity = 0.5f; // Intensidad media, suficiente para ver pero manteniendo la atmósfera nocturna

    private bool isDay = true; // Indica si es de día o de noche

    public Light[] streetLights; // Array de luces de los postes (asígnalas en el Inspector)

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleDayNight();
        }
    }

    void ToggleDayNight()
    {
        if (directionalLight != null)
        {
            if (isDay)
            {
                // Cambiar a noche (luz de luna)
                directionalLight.transform.rotation = Quaternion.Euler(nightRotation);
                directionalLight.color = moonLightColor;
                directionalLight.intensity = moonIntensity;

                // Encender las luces de los postes
                SetStreetLightsState(true);
            }
            else
            {
                // Cambiar a día
                directionalLight.transform.rotation = Quaternion.Euler(dayRotation);
                directionalLight.color = dayLightColor;
                directionalLight.intensity = dayIntensity;

                // Apagar las luces de los postes
                SetStreetLightsState(false);
            }

            isDay = !isDay; // Alternar el estado
        }
    }

    void SetStreetLightsState(bool state)
    {
        if (streetLights != null)
        {
            foreach (Light streetLight in streetLights)
            {
                if (streetLight != null)
                {
                    streetLight.enabled = state; // Encender o apagar la luz
                }
            }
        }
    }
}
