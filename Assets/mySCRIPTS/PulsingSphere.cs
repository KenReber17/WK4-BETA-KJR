using UnityEngine;

public class PulsingSphere : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private float pulseSpeed = 1f; // Speed of the pulse
    [SerializeField] private float minScale = 0.8f; // Minimum scale during a pulse
    [SerializeField] private float maxScale = 1.2f; // Maximum scale during a pulse

    [Header("Noise Settings")]
    [SerializeField] private float noiseIntensity = 0.1f; // Intensity of the noise affecting the scale
    [SerializeField] private float spikeHeight = 0.2f; // Maximum height of spikes

    private float noiseOffset = 0f;
    private float spikeNoiseOffset = 0f;

    void Update()
    {
        // Time-based pulsing
        float pulse = Mathf.PingPong(Time.time * pulseSpeed, maxScale - minScale) + minScale;

        // Apply noise for random pulsing
        float noise = Mathf.PerlinNoise(Time.time * pulseSpeed, noiseOffset) * 2f - 1f; // Normalize to [-1, 1]
        float noisyPulse = pulse + noise * noiseIntensity;

        // Apply spikes
        float spikeNoise = Mathf.PerlinNoise(Time.time * pulseSpeed * 2f, spikeNoiseOffset) * 2f - 1f; // More frequent noise for spikes
        float spikeScale = Mathf.Max(0, spikeNoise) * spikeHeight; // Only positive spikes
        float finalScale = noisyPulse + spikeScale;

        // Apply the scale to the sphere
        transform.localScale = Vector3.one * finalScale;

        // Update noise offsets for next frame
        noiseOffset += 0.01f; // Small increment to get new noise values
        spikeNoiseOffset += 0.05f; // Larger increment for spikes to appear more randomly
    }
}