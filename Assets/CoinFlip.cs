using UnityEngine;
using System.Collections;

public class CoinFlip : MonoBehaviour
{
    public Transform coinTransform;
    public GameObject player1Checkmark;
    public GameObject player2Checkmark;
    public Sprite headsSprite;
    public Sprite tailsSprite;
    public float spinDuration = 2f;
    public int spins = 2;
    public float headsSpinSpeed = 900f; // Spin speed for heads.
    public float tailsSpinSpeed = 500f; // Spin speed for tails.

    private bool isHeads;
    private SpriteRenderer coinRenderer;

    private void Start()
    {
        coinRenderer = GetComponent<SpriteRenderer>();
        player1Checkmark.SetActive(false);
        player2Checkmark.SetActive(false);
        StartCoroutine(SpinCoin());
    }

    private IEnumerator SpinCoin()
    {
        int spinsRemaining = spins;

        // Determine if it's heads or tails based on spinSpeed.
        isHeads = Random.value < (headsSpinSpeed / (headsSpinSpeed + tailsSpinSpeed));

        while (spinsRemaining > 0)
        {
            // Simulate a single spin.
            float spinProgress = 0f;
            float spinSpeed = isHeads ? headsSpinSpeed : tailsSpinSpeed;

            while (spinProgress < spinDuration)
            {
                coinRenderer.sprite = isHeads ? headsSprite : tailsSprite;

                coinTransform.Rotate(Vector3.up, spinSpeed * Time.deltaTime); // Rotate around the Y-axis (upwards).
                spinProgress += Time.deltaTime;

                yield return null;
            }

            spinsRemaining--;
        }

        // Ensure the coin completes a full rotation cycle.
        float fullRotationTime = 360f / (isHeads ? headsSpinSpeed : tailsSpinSpeed);
        yield return new WaitForSeconds(fullRotationTime);

        // Show the checkmark for the respective player.
        if (isHeads)
        {
            Debug.Log("Player 1 (Heads) goes first.");
            player1Checkmark.SetActive(true);
            Debug.Log("Currently showing: " + (coinRenderer.sprite == headsSprite ? "Heads" : "Tails"));
        }
        else
        {
            Debug.Log("Player 2 (Tails) goes first.");
            player2Checkmark.SetActive(true);
            Debug.Log("Currently showing: " + (coinRenderer.sprite == headsSprite ? "Heads" : "Tails"));
        }

        // Transition to the "GameProper" scene after a delay (e.g., 2 seconds).
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameProper");
    }
}
