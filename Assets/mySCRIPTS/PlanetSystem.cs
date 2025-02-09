using UnityEngine;

public class PlanetSystem : MonoBehaviour
{
    public GameObject[] planetPrefabs; // Array to hold different planet prefabs
    public GameObject moonPrefab;
    [SerializeField] private int numberOfPlanets = 7;
    [SerializeField] private float minOrbitRadius = 5f;
    [SerializeField] private float maxOrbitRadius = 25f;
    [SerializeField] private float orbitSpeed = 1f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float moonOrbitRadius = 1f;
    [SerializeField] private float moonOrbitSpeed = 2f;
    [SerializeField] private float orbitHeightVariance = 5f;

    [Header("Planet Sizes")]
    public float[] planetSizes;

    private GameObject[] planets;
    private GameObject moon;
    private float[] distances;
    private float[] angles;
    private Vector3[] rotationAxes;
    private float[] heights;
    private int moonPlanetIndex;

    void Start()
    {
        // Check if we have enough prefabs for all planets
        if (planetPrefabs == null || planetPrefabs.Length < numberOfPlanets)
        {
            Debug.LogError("Not enough planet prefabs provided. Need " + numberOfPlanets);
            return;
        }

        planets = new GameObject[numberOfPlanets];
        distances = new float[numberOfPlanets];
        angles = new float[numberOfPlanets];
        rotationAxes = new Vector3[numberOfPlanets];
        heights = new float[numberOfPlanets];

        // Initialize planetSizes if not set in the inspector
        if (planetSizes == null || planetSizes.Length != numberOfPlanets)
        {
            planetSizes = new float[numberOfPlanets];
            for (int i = 0; i < numberOfPlanets; i++)
            {
                planetSizes[i] = 1f;
            }
        }

        moonPlanetIndex = Random.Range(0, numberOfPlanets);
        Debug.Log("Moon planet index: " + moonPlanetIndex);

        for (int i = 0; i < numberOfPlanets; i++)
        {
            // Instantiate different prefabs for each planet
            planets[i] = Instantiate(planetPrefabs[i], Vector3.zero, Quaternion.identity);
            if (planets[i] == null)
            {
                Debug.LogError("Failed to instantiate planet " + i);
                continue;
            }
            distances[i] = Random.Range(minOrbitRadius, maxOrbitRadius);
            angles[i] = Random.Range(0f, 360f);
            rotationAxes[i] = Random.insideUnitSphere.normalized;
            heights[i] = Random.Range(-orbitHeightVariance, orbitHeightVariance);

            // Set the size of each planet
            planets[i].transform.localScale = Vector3.one * planetSizes[i];

            if (i == moonPlanetIndex)
            {
                moon = Instantiate(moonPrefab, Vector3.zero, Quaternion.identity);
                if (moon == null)
                {
                    Debug.LogError("Moon instantiation failed!");
                }
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < numberOfPlanets; i++)
        {
            if (planets[i] == null) continue;

            angles[i] += Time.deltaTime * orbitSpeed * (i + 1);
            if (angles[i] >= 360f) angles[i] -= 360f;

            float x = Mathf.Sin(Mathf.Deg2Rad * angles[i]) * distances[i];
            float z = Mathf.Cos(Mathf.Deg2Rad * angles[i]) * distances[i];
            Vector3 planetPosition = new Vector3(x, heights[i], z);
            planets[i].transform.position = transform.TransformPoint(planetPosition);

            // Planet self-rotation
            planets[i].transform.Rotate(rotationAxes[i] * rotationSpeed * Time.deltaTime, Space.World);

            // Moon orbit if this planet has one
            if (i == moonPlanetIndex && moon != null)
            {
                Vector3 moonOrbitCenter = planets[i].transform.position;
                Vector3 moonPosition = moonOrbitCenter + Quaternion.Euler(0, angles[i] * moonOrbitSpeed, 0) * Vector3.right * moonOrbitRadius;
                moon.transform.position = moonPosition;
                moon.transform.Rotate(Vector3.up * rotationSpeed * 2 * Time.deltaTime);
            }
        }
    }
}