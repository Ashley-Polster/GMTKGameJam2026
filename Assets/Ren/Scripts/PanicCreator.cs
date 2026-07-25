using UnityEngine;

public class PanicCreator : MonoBehaviour
{
    private AudioSource audioSource;
    public float targetPitch = 1.5f; // Example: 50% higher
    public float duration = 30f;     // Duration in seconds

    private float startTime;
    private float currentPitch;
    private float elapsedTime = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Start at original pitch
        currentPitch = audioSource.pitch;
        startTime = Time.time;
    }

    void Update()
    {
        // Calculate elapsed time
        elapsedTime = Time.time - startTime;
        if (elapsedTime < 3.5)
        {
            return;
        }

        // Clamp elapsed time to duration
        if (elapsedTime < duration)
        {
            // Interpolate pitch toward target
            float t = elapsedTime / duration;
            audioSource.pitch = Mathf.Lerp(currentPitch, targetPitch, t);
        }
        else
        {
            // Stop ramping after duration
            audioSource.pitch = targetPitch;
        }
    }
}
