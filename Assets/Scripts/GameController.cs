using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    private bool gameRunning = false;
    private int maxEnemies = 10;
    private int numberOfEnemies = 0;
    private int maxWaypoints = 6;
    private int numberOfWaypoints = 0;
    public int numberOfEnemiesKilled = 0;
    public int numberOfEnemiesTouched = 0;
    public int playerLives = 3;
    private float pickupChance;
    private float nextPickupTime = 0f;
    private float pickupCooldownTime = 15f;
    bool forceEnemyPatrolType = false;
    GameObject bButton, tButton, player, reloadButton, gameOverText, quitButton, titleText, startButton, controlsPanel, missileReadyMsg, evalPanel;
    public GameObject[] wps = null;
    public GameObject textScore;
    public AudioSource sfxPickup, sfxLaser, sfxExplosion;
    public bool turboReady = false;
    public int score = 0;
    string textEP = "";
    void Start()
    {
        Cursor.visible = true;

        evalPanel = GameObject.Find("evalPanel");
        textScore = GameObject.Find("scoreNum");
        bButton = GameObject.Find("bButton");
        tButton = GameObject.Find("tButton");
        player = GameObject.Find("playerShip");
        reloadButton = GameObject.Find("reloadButton");
        quitButton = GameObject.Find("quitButton");
        gameOverText = GameObject.Find("gameOverText");
        titleText = GameObject.Find("titleText");
        startButton = GameObject.Find("startButton");
        controlsPanel = GameObject.Find("controlsPanel");
        missileReadyMsg = GameObject.Find("missileReadyMsg");
        sfxPickup = GameObject.Find("sfxPickupSource").GetComponent<AudioSource>();
        sfxLaser = GameObject.Find("sfxLaserSource").GetComponent<AudioSource>();
        sfxExplosion = GameObject.Find("sfxExplosionSource").GetComponent<AudioSource>();

        evalPanel.SetActive(false);
        player.SetActive(false);
        bButton.SetActive(false);
        reloadButton.SetActive(false);
        quitButton.SetActive(false);
        gameOverText.SetActive(false);
        missileReadyMsg.SetActive(false);
        tButton.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (gameRunning)
        {
            pickupChance = Random.Range(0.0f, 10.0f);
            if (pickupChance > 9.0f && !(GameObject.FindGameObjectWithTag("pickup")) && Time.time > nextPickupTime)
            {
                instantiatePickup();
                nextPickupTime = Time.time + pickupCooldownTime;
            }
        }
    }
    void Update()
    {
        if (evalPanel.activeSelf)
        {
            textEP = buildEPText();
            evalPanel.GetComponentInChildren<Text>().text = textEP;
        }
        if (score >= 500 && score % 1000 == 0)
        {
            turboReady = true;
        }
        textScore.GetComponent<TMPro.TextMeshProUGUI>().text = (score + "");
        // Title/GameOver button behavior
        if (startButton.activeSelf)
        {
            startButton.GetComponent<Button>().onClick.AddListener(StartGame);
            startButton.GetComponent<Button>().onClick.AddListener(instantiateWaypoints);
        }
        if (reloadButton.activeSelf)
        {
            reloadButton.GetComponent<Button>().onClick.AddListener(ReloadGame);
        }
        if (quitButton.activeSelf)
        {
            quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cursor.visible = !(Cursor.visible);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
        if (Input.GetKeyDown(KeyCode.E) && gameRunning)
        {
            evalPanel.SetActive(!(evalPanel.activeSelf));
        }
        if (Input.GetKeyDown(KeyCode.K) && gameRunning)
        {
            ShowControls();
        }
        if (Input.GetKeyDown(KeyCode.J) && gameRunning)
        {
            toggleEnemyPatrolType();
        }
        if (Input.GetKeyDown(KeyCode.H) && gameRunning)
        {
            foreach (GameObject w in wps)
            {
                w.GetComponent<WaypointBehavior>().toggleVis();
            }
        }
        if (playerLives <= 0)
        {
            playerDie();
        }
        if (gameRunning)
        {
            wps = GameObject.FindGameObjectsWithTag("waypoint");
            numberOfEnemies = (GameObject.FindGameObjectsWithTag("Enemy").Length);
            numberOfWaypoints = wps.Length;
        }
        // Instantiate enemies
        if (numberOfEnemies < maxEnemies && gameRunning)
        {
            CameraSupport camSupp = Camera.main.GetComponent<CameraSupport>();
            GameObject enemy = Instantiate(Resources.Load("Prefabs/enemyType1") as GameObject);
            Vector3 pos;
            pos.x = (camSupp.GetWorldBound().min.x + Random.value * camSupp.GetWorldBound().size.x) * 0.9f;
            pos.y = camSupp.GetWorldBound().max.y + Random.Range(0.5f, 1.0f);
            // pos.y = (camSupp.GetWorldBound().min.y + Random.value * camSupp.GetWorldBound().size.y) * 0.9f;
            pos.z = 0;
            enemy.transform.localPosition = pos;
        }
        // Test block to spawn pickup
        if (Input.GetKeyDown(KeyCode.P))
        {
            instantiatePickup();
        }
        if (turboReady) tButton.SetActive(true);
        if (Input.GetKeyDown(KeyCode.T) && turboReady)
        {
            player.GetComponent<playerBehavior>().turboEnabled = true;
            player.GetComponent<playerBehavior>().turboTime = Time.time + 5f;
        }
    }
    public void killEnemy()
    {
        numberOfEnemiesKilled += 1;
        score += 25;
    }
    public void touchEnemy()
    {
        numberOfEnemiesTouched += 1;
    }
    public void takePlayerLife()
    {
        playerLives -= 1;
    }
    public void givePlayerLife()
    {
        playerLives += 1;
        player.GetComponent<playerBehavior>().playerHealthTopOff();
    }
    public void pickupMissile()
    {
        if (!(player.gameObject.GetComponent<playerBehavior>().playerHasMissile))
        {
            bButton.SetActive(true);
            missileReadyMessage();
            player.gameObject.GetComponent<playerBehavior>().playerHasMissile = true;
            Invoke("missileReadyMessage", 0.5f);
        }

    }
    public void shootMissile()
    {
        bButton.SetActive(false);
        player.gameObject.GetComponent<playerBehavior>().playerHasMissile = false;
    }
    public void pickupShield()
    {
        player.gameObject.GetComponent<playerBehavior>().activateShield();
    }
    public void instantiateWaypoints()
    {
        while (GameObject.FindGameObjectsWithTag("waypoint").Length < maxWaypoints && gameRunning)
        {
            CameraSupport camSupp = Camera.main.GetComponent<CameraSupport>();
            GameObject waypoint = Instantiate(Resources.Load("Prefabs/waypoint") as GameObject);
            Vector3 pos;
            pos.x = (camSupp.GetWorldBound().min.x + Random.value * camSupp.GetWorldBound().size.x) * 0.9f;
            pos.y = ((camSupp.GetWorldBound().min.y + 1f) + Random.value * camSupp.GetWorldBound().size.y) * 0.9f;
            pos.z = 0;
            waypoint.transform.localPosition = pos;
        }
        string[] wpNames = { "A", "B", "C", "D", "E", "F" };
        GameObject[] wpArray = GameObject.FindGameObjectsWithTag("waypoint");
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("waypoint").Length; i++)
        {
            wpArray[i].name = "waypoint" + wpNames[i];
        }
        wps = wpArray;
    }
    public void instantiatePickup()
    {
        CameraSupport camSupp = Camera.main.GetComponent<CameraSupport>();
        GameObject pickup = Instantiate(Resources.Load("Prefabs/pickup") as GameObject);
        Vector3 pos;
        pos.x = (camSupp.GetWorldBound().min.x + Random.value * camSupp.GetWorldBound().size.x) * 0.9f;
        pos.y = 5.4f;
        pos.z = 0;
        pickup.transform.localPosition = pos;
    }
    public void playerDie()
    {
        player.gameObject.GetComponent<playerBehavior>().playerExplode();
        Cursor.visible = true;
        reloadButton.SetActive(true);
        quitButton.SetActive(true);
        gameOverText.SetActive(true);
        gameRunning = false;
        gameOver();
    }
    private void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    private void StartGame()
    {
        player.SetActive(true);
        titleText.SetActive(false);
        controlsPanel.SetActive(false);
        Cursor.visible = false;
        gameRunning = true;
    }
    private void ShowControls()
    {
        controlsPanel.SetActive(!controlsPanel.activeSelf);
    }
    public void relocateWaypoint(Transform xform, string name)
    {
        CameraSupport camSupp = Camera.main.GetComponent<CameraSupport>();
        GameObject waypoint = Instantiate(Resources.Load("Prefabs/waypoint") as GameObject);
        Vector3 pos;
        pos.x = (xform.position.x + Random.Range(-0.75f, 0.75f));
        pos.y = (xform.position.y + Random.Range(-0.75f, 0.75f));
        pos.z = 0;
        waypoint.transform.localPosition = pos;
        waypoint.name = name;
    }
    public bool isGameRunning()
    {
        return gameRunning;
    }
    void gameOver()
    {
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(e);
        }
        foreach (GameObject w in GameObject.FindGameObjectsWithTag("waypoint"))
        {
            Destroy(w);
        }
    }
    void missileReadyMessage()
    {
        missileReadyMsg.SetActive(!(missileReadyMsg.activeSelf));
    }
    void toggleEnemyPatrolType()
    {
        char type = ' ';
        if (forceEnemyPatrolType)
        {
            type = 'A';
        }
        else
        {
            type = 'B';
        }
        foreach (GameObject e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            e.GetComponent<EnemyBehavior>().patrolType = type;
        }
        forceEnemyPatrolType = !forceEnemyPatrolType;
    }
    string buildEPText()
    {
        string text = "";
        text += "Control mode: ";
        if (player.GetComponent<playerBehavior>().controlScheme)
        {
            text += "mouse";
        }
        else text += "keyboard";
        text += "\n";
        text += "Enemies killed: " + numberOfEnemiesKilled + "\n";
        text += "Enemies touched: " + numberOfEnemiesTouched + "\n";
        text += "Waypoints: " + numberOfWaypoints + "\n";
        text += "Enemies alive: " + numberOfEnemies + "\n";
        text += "Bullet cooldown: " + player.GetComponent<playerBehavior>().shootCooldown + "\n";
        text += "Bullets in world: " + GameObject.FindGameObjectsWithTag("projectile").Length + "\n";
        return text;
    }
}