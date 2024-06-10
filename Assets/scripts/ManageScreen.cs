using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ManageScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup; // CanvasGroup to control the fade effect
    public float fadeDuration = 2f; // Duration for the fade effect
    public AudioSource buttonPressSound;
    public AudioSource titleScreenMusic;
    public TMP_Text pressAnyButtonText;
    public float blinkInterval = 0.5f;
    public Animator transitionAnimator; // Animator for the circle transition

    void Start()
    {
        StartCoroutine(BlinkText());
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            pressAnyButtonText.enabled = !pressAnyButtonText.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            buttonPressSound.Play(); // Play the sound effect when a button is pressed
            StopCoroutine(BlinkText()); // Stop the blinking effect
            StartCoroutine(StartGame("GameScene")); // Start the fade-out transition
        }
    }

    IEnumerator StartGame(string sceneName)
    {
        float currentTime = 0f;
        float initialVolume = titleScreenMusic.volume;

        // Start the circle expansion animation
        transitionAnimator.SetTrigger("TriggerTransition");

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            titleScreenMusic.volume = Mathf.Lerp(initialVolume, 0f, currentTime / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration); // Fade out
            yield return null;
        }

        // Ensure music is completely silent and canvas is invisible
        titleScreenMusic.volume = 0f;
        canvasGroup.alpha = 0f;

        // Wait a bit longer if the animation is still playing
        yield return new WaitForSeconds(0.5f);

        // Load the game scene
        SceneManager.LoadScene(sceneName);
    }
}
