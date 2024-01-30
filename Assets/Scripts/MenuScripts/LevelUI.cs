using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Handles the gameplay UI and activates the right canvases when needed. 
/// </summary>
public class LevelUI : MonoBehaviour
{
    [Header ("Canvas Gameobjects")]
    [Tooltip("The canvas that is active during the game.")]
    [SerializeField] GameObject levelCanvas;

    [Tooltip("The canvas that is active when the game is paused.")]
    [SerializeField] GameObject pauseCanvas;

    [Tooltip("The canvas that is active when the game is over.")]
    [SerializeField] GameObject gameOverCanvas;
    public GameObject GameOverCanvas { get { return gameOverCanvas; } }


    [Header("Music and Sound")]
    [Tooltip("The audio source that music comes from while the game is running.")]
    [SerializeField] AudioSource levelMusicAudioSource;

    [Tooltip("These audioSources will be paused and unpaused when the pause menu opens to no sound occurs.")]
    [SerializeField] AudioSource[] audioSourcesToPause;

    [Tooltip("The music that runs while gameplay is happening.")]
    [SerializeField] AudioClip levelClip;

    [Tooltip("The music that runs while the gameplay is paused.")]
    [SerializeField] AudioClip pauseClip;

    [Tooltip("The music that runs when the player has been defeated.")]
    [SerializeField] AudioClip gameOverClip;


    [Header("Level UI")]
    #region Gameplay UI values
    // Enemies left
    [Tooltip("The text indicating how many enemies are left. Changes when a enemy is destroyed.")]
    [SerializeField] TextMeshProUGUI enemiesLeftText;

    // Current level
    [Tooltip("A text showing in what level the player currently is.")]
    [SerializeField] TextMeshProUGUI currentLevelText;

    [Tooltip("A int indicating in what level the player currently is.")]
    [SerializeField] int currentLevel;

    // Health
    [Tooltip("A Scripable Object holding important information of the player like ammunition amount or health amount.")]
    [SerializeField] PlayerStats playerStats;
    HealthSystem healthSystem;

    [Tooltip(" A slider showing how much health is left.")]
    [SerializeField] Slider healthSlider;

    [Tooltip("A text ontop of the healtn slider showing how much health is left.")]
    [SerializeField] TextMeshProUGUI healthText;

    [Tooltip("A text indicating how many madKits the player has.")]
    [SerializeField] TextMeshProUGUI medKitText;

    // Weapons
    [Tooltip("A image indicating which weapon the player has equipped.")]
    [SerializeField] Image weaponImage;

    [Tooltip("A array with different weapon sprites.")]
    [SerializeField] Sprite[] weaponSprites;

    [Tooltip("The slider indicating how much ammunition the player currently has.")]
    [SerializeField] Slider magSlider;

    [Tooltip("A text indicating how many grenades the player has.")]
    [SerializeField] TextMeshProUGUI grenadesText;

    WeaponStats currentWeapon;
    #endregion

    [Header("Menu functionality")]

    // Cameras are activated and deactivated so that the pause menu can be stores in world space
    // and have a dynamic moving camera without messing with the gameplay scene. 

    [Tooltip("The camera active while gameplay is happening.")]
    [SerializeField] GameObject gameCamera;

    [Tooltip("The camera active while the game is paused.")]
    [SerializeField] GameObject pauseCamera;

    [Tooltip("The options menu is a seperate world space canvas that gets actiated once the pause menu is opened.")]
    [SerializeField] GameObject optionsMenu;

    AudioSource audioSource;

    bool initialized; 

    // If the game is paused. 
    bool paused;

    public bool Paused { get => paused; set => paused = value; }

    float currentMagValue;

    private void Start()
    {
        EnemyManager.Instance.enemyAmountUpdated += UpdateEnemiesLeftText;
        healthSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();

        if (CursorHandler.instance)
            CursorHandler.instance.SetGameplayCursor();
    }

    void Update()
    {
        if (!initialized)
        {
            initialized = true;
            Initialization();         
        }

        grenadesText.text = playerStats.Grenades.ToString();
        medKitText.text = playerStats.MedKits.ToString();
        magSlider.value = playerStats.CurrentMagAmmo;

        if(currentMagValue != playerStats.CurrentMagAmmo)
        {
            currentMagValue = playerStats.CurrentMagAmmo;
            magSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.grey;
            StartCoroutine(ChangeSliderColorToWhite(magSlider));
        }


        if(playerStats.CurrentWeaponStats != currentWeapon)
        {
            currentWeapon = playerStats.CurrentWeaponStats;
            magSlider.maxValue = currentWeapon.MagSize;
            
            UpdateWeaponImage();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            HandlePause();
        }
    }

    private void OnDisable()
    {
        EnemyManager.Instance.enemyAmountUpdated -= UpdateEnemiesLeftText;
    }

    #region Methods
    /// <summary>
    /// Sets the timeScale to 1, activates the level canvas and deactivates the pause and game over canvas.
    /// Subscribes to the OnDeathEvent of the player health system.
    /// </summary>
    void Initialization()
    {
        Time.timeScale = 1;

        if (levelCanvas != null)
            levelCanvas.SetActive(true);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        HealthSystem health = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
        if (health)
        {
            healthSystem = health;
            healthSystem.OnDamageEvent.AddListener(UpdateHealthUIOnDamage);
            healthSystem.OnHealEvent.AddListener(UpdateHealthUIOnHealed);
            healthSystem.OnDeathEvent.AddListener(EnableGameOverCanvas);
            healthSlider.maxValue = playerStats.MaxHealth;
            UpdateHealthUIOnHealed();
        }

        if (currentLevelText)
            currentLevelText.text = "Level " + currentLevel.ToString();

        currentWeapon = playerStats.CurrentWeaponStats;
        magSlider.maxValue = currentWeapon.MagSize;
        magSlider.value = playerStats.CurrentMagAmmo;

        UpdateWeaponImage();
        if (GetComponent<AudioSource>())
            audioSource = GetComponent<AudioSource>();
    }

    ///TODO Subscribe to event without making parameters necessary.
    /// <summary>
    /// Sets the timeScale to 1, activates the gameover canvas and deactivates the level and pause canvas.
    /// </summary>
    /// <param name="direction"></param> Not needed 
    /// <param name="knockback"></param> Not needed
    void EnableGameOverCanvas(Vector3 direction, float knockback)
    {
        if (levelMusicAudioSource)
        {
            levelMusicAudioSource.clip = gameOverClip;
            levelMusicAudioSource.Play();
        }

        foreach (AudioSource audioS in audioSourcesToPause)
        {
            if(audioS)
            audioS.Pause();
        }

        Time.timeScale = 0;

        if(levelCanvas != null)
        levelCanvas.SetActive(false);

        if(pauseCanvas != null)
        pauseCanvas.SetActive(false);

        if(gameOverCanvas != null)
        gameOverCanvas.SetActive(true);      

        if(CursorHandler.instance)
        {
            CursorHandler.instance.SetMenuCursor();
        }
    }

    #region Level UI Methods
    /// <summary>
    /// Gets invoked by the enemyDefeated event on the Enemy Manager to change the UI value.
    /// </summary>
    /// <param name="amount"></param> The current enemy amount.
    void UpdateEnemiesLeftText(int amount)
    {
        if(enemiesLeftText) enemiesLeftText.text = new string(amount.ToString() + " Left");
    }

    /// <summary>
    /// Gets invoked by the OnDamage event on the players health system to change the UI values.
    /// </summary>
    /// <param name="hitDIrection"></param> From where the hit came. (Not needed in this method).
    /// <param name="knockBack"></param> The knockback amount of the incoming damage. (´Not needed in this method).
    void UpdateHealthUIOnDamage(Vector3 hitDIrection, float knockBack)
    {
        healthText.text = (healthSystem.GetCurrentHealth() * 10).ToString();
        healthSlider.value = healthSystem.GetCurrentHealth();
        healthSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.red;
        StartCoroutine(ChangeSliderColorToWhite(healthSlider));

    }

    IEnumerator ChangeSliderColorToWhite(Slider slider)
    {
        yield return new WaitForSeconds(.2f);
        slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.white;
    }
    /// <summary>
    /// Gets invoked by the OnHeal event on the players health system to change the UI values.
    /// </summary>
    void UpdateHealthUIOnHealed()
    {
        healthText.text = (healthSystem.GetCurrentHealth() * 10).ToString();
        healthSlider.value = healthSystem.GetCurrentHealth();
    }

    /// <summary>
    /// Changes the weapon image on the level canvas to a image representing the weapon the player currently has equipped.
    /// </summary>
    void UpdateWeaponImage()
    {
        switch(currentWeapon.WeaponID)
        {
            case 0:
                weaponImage.sprite = weaponSprites[0];
                break;
            case 1:
                weaponImage.sprite = weaponSprites[1];
                break;
            case 2:
                weaponImage.sprite = weaponSprites[2];
                break; 
        }
    }
    #endregion

    /// <summary>
    /// Sets the paused state and handles the active state of the level and pause canvas.
    /// Sets the timeScale value to pause or unpause the game.
    /// Sets the cursors visibility.  
    /// </summary>
    public void HandlePause()
    {
        if (gameOverCanvas.activeSelf) return;

        paused = !paused;

        if (paused) // Deactivate the level canvas, activate the pause canvas. 
        {
            gameCamera.SetActive(false);
 

            if (levelMusicAudioSource)
            {
                levelMusicAudioSource.clip = pauseClip;
                levelMusicAudioSource.Play();
            }

            Time.timeScale = 0;
            if (levelCanvas)
                levelCanvas.SetActive(false);
          //  if (pauseCanvas)
           //     pauseCanvas.SetActive(true);       
            
            foreach(AudioSource audioS in audioSourcesToPause)
            {
                if(audioS)
                audioS.Pause();
            }
        }
        else // Deactivate the pause canvas, activate the level canvas. 
        {
            gameCamera.SetActive(true);

            Time.timeScale = 1;
            if (levelCanvas)
                levelCanvas.SetActive(true);
          //  if (pauseCanvas)
             //   pauseCanvas.SetActive(false);

            if (levelMusicAudioSource)
                levelMusicAudioSource.clip = levelClip;
                levelMusicAudioSource.Play();

            foreach (AudioSource audioS in audioSourcesToPause)
            {
                if(audioS)
                audioS.UnPause();
            }
        }
        if(CursorHandler.instance)
        {
            if(Time.timeScale == 1)
            {
                CursorHandler.instance.SetGameplayCursor();
            }
            else
            {
                CursorHandler.instance.SetMenuCursor();
            }
        }
    }

    /// <summary>
    /// Changes the paused state of the game, only when the game is paused.
    /// </summary>
    public void HandlePauseThroughButton()
    {
        if (!paused) return;

        HandlePause();

    }

    /// <summary>
    /// Plays a clip from an audioSource.
    /// </summary>
    /// <param name="clip"></param> The clip to play.
    public void PlaySound(AudioClip clip)
    {
        if (!paused) return;

        if (audioSource)
            audioSource.PlayOneShot(clip);

    }
    #endregion
}
