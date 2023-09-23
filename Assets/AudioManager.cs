using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject audioManager = new GameObject("AudioManager");
                    instance = audioManager.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    private AudioSource audioSource;
    public AudioClip mainMenuMusic;        // Background music for MainMenu
    public AudioClip characterSelectMusic; // Background music for CharacterSelect
    public AudioClip gameProperMusic;      // Background music for GameProper

    private void Awake()
    {
        Debug.Log("AudioManager Awake");

        // Check if audioSource is not assigned in the Inspector
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Assign your background music AudioClips in the Inspector
            // No need to load from Resources if assigned in the Inspector
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlayMainMenuMusic()
    {
        PlayBackgroundMusic(mainMenuMusic);
    }

    public void PlayCharacterSelectMusic()
    {
        PlayBackgroundMusic(characterSelectMusic);
    }

    public void PlayGameProperMusic()
    {
        PlayBackgroundMusic(gameProperMusic);
    }

    private void PlayBackgroundMusic(AudioClip music)
    {
        Debug.Log("Playing background music: " + music.name);

        // Check if audioSource is null
        if (audioSource == null)
        {
            Debug.LogError("audioSource is null");
            return;
        }

        if (!audioSource.isPlaying || audioSource.clip != music)
        {
            audioSource.clip = music;
            audioSource.Play();
        }
    }


    public void PlaySound(AudioClip clip)
    {
        Debug.Log("Playing sound: " + clip.name);
        if (clip != audioSource.clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Determine the appropriate background music based on the loaded scene
        if (scene.name == "MainMenu" || scene.name == "Settings" || scene.name == "Credits")
        {
            PlayMainMenuMusic();
        }
        else if (scene.name == "CharacterSelect" || scene.name == "MapSelect" || scene.name == "FlipCoin")
        {
            PlayCharacterSelectMusic();
        }
        else if (scene.name == "GameProper")
        {
            PlayGameProperMusic();
        }
    }
}
