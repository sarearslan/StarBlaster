using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] AudioClip playerShootingClip;
    [SerializeField] AudioClip enemyShootingClip;
    [SerializeField] AudioClip bossShootingClip;

    [Header("Damage")]
    [SerializeField] AudioClip normalDamageClip;
    [SerializeField] AudioClip bossDamageClip;

    [Header("Explosions")]
    [SerializeField] AudioClip normalExplosionClip;
    [SerializeField] AudioClip bossExplosionClip;

    [Header("Pickups")]
    [SerializeField] AudioClip pickupClip;

    [Header("Music")]
    [SerializeField] AudioClip mainMenuMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip bossFightMusic;
    [SerializeField] AudioClip endScreenMusic;

    [Header("Volumes")]
    [SerializeField, Range(0f, 1f)] float shootingVolume = 0.7f;
    [SerializeField, Range(0f, 1f)] float damageVolume = 0.8f;
    [SerializeField, Range(0f, 1f)] float explosionVolume = 1f;
    [SerializeField, Range(0f, 2f)] float bossExplosionVolume = 1.5f;
    [SerializeField, Range(0f, 1f)] float pickupVolume = 0.8f;
    [SerializeField, Range(0f, 1f)] float musicVolume = 0.5f;

    [Header("UI")]
    [SerializeField] AudioClip buttonClickClip;
    [SerializeField, Range(0f, 1f)] float buttonClickVolume = 0.7f;

    static AudioManager instance;

    AudioSource sfxSource;
    AudioSource musicSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length >= 2)
        {
            sfxSource = sources[0];
            musicSource = sources[1];
        }
        else
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.spatialBlend = 0f;

        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.spatialBlend = 0f;
        musicSource.volume = musicVolume;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                PlayMusic(mainMenuMusic);
                break;

            case "GameScene":
                PlayMusic(gameMusic);
                break;

            case "GameOver":
            case "YouWin":
                PlayMusic(endScreenMusic);
                break;
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null)
        {
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    void PlaySFX(AudioClip clip, float volume)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayPlayerShooting()
    {
        PlaySFX(playerShootingClip, shootingVolume);
    }

    public void PlayEnemyShooting()
    {
        PlaySFX(enemyShootingClip, shootingVolume);
    }

    public void PlayBossShooting()
    {
        PlaySFX(bossShootingClip, shootingVolume);
    }

    public void PlayNormalDamage()
    {
        PlaySFX(normalDamageClip, damageVolume);
    }

    public void PlayBossDamage()
    {
        PlaySFX(bossDamageClip, damageVolume);
    }

    public void PlayNormalExplosion()
    {
        PlaySFX(normalExplosionClip, explosionVolume);
    }

    public void PlayBossExplosion()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }

        PlaySFX(
            normalExplosionClip,
            bossExplosionVolume
        );

        PlaySFX(
            bossExplosionClip,
            bossExplosionVolume
        );
    }

    public void PlayPickup()
    {
        PlaySFX(pickupClip, pickupVolume);
    }

    public void PlayBossFightMusic()
    {
        PlayMusic(bossFightMusic);
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClickClip, buttonClickVolume);
    }
}