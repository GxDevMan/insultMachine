using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class PlayerHealthManager : MonoBehaviour
{
    public Slider player1HealthBar;
    public Slider player2HealthBar;

    private float player1Health = 100f;
    private float player2Health = 100f;

    MatchManager matchInstance;
    private bool gameOver = false; 
    public GameObject player1WinBanner; 
    public GameObject player2WinBanner; 

    public AudioSource gameoverAudioSource; 
    public AudioClip winSound; 

    void Start()
    {
        player1HealthBar.maxValue = player1Health;
        player2HealthBar.maxValue = player2Health;

        player1HealthBar.value = player1Health;
        player2HealthBar.value = player2Health;

        matchInstance = MatchManager.instance;
    }

    void Update()
    {
        player1HealthBar.value = matchInstance.player1.health;
        player2HealthBar.value = matchInstance.player2.health;
        sceneSwitch();
    }

    private void sceneSwitch()
    {
        if (!gameOver && (matchInstance.player1.health <= 0 || matchInstance.player2.health <= 0))
        {
            
            gameOver = true;

            
            gameoverAudioSource.Play();

            int winnerID;

            
            if (matchInstance.player1.health <= 0)
            {
                
                player2WinBanner.SetActive(true);
                winnerID = 2;
            }
            else
            {
                
                player1WinBanner.SetActive(true);
                winnerID = 1;
            }

            PlayerPrefs.SetInt("WinnerID", winnerID);
            if (winSound != null)
            {
                AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position);
            }
            StartCoroutine(DelayedTransition());
        }
    }

    IEnumerator DelayedTransition()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("WinBanner");
    }
}
