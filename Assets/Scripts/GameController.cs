using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
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
    GameObject bButton, player, reloadButton, gameOverText, quitButton, titleText, startButton;
    void Start()
    {
        Cursor.visible = true;

        bButton = GameObject.Find("bButton");
        player = GameObject.Find("playerShip");
        reloadButton = GameObject.Find("reloadButton");
        quitButton = GameObject.Find("quitButton");
        gameOverText = GameObject.Find("gameOverText");
        titleText = GameObject.Find("titleText");
        startButton = GameObject.Find("startButton");

        player.SetActive(false);
        bButton.SetActive(false);
        reloadButton.SetActive(false);
        quitButton.SetActive(false);
        gameOverText.SetActive(false);

        instantiateWaypoints();
    }
    private void FixedUpdate()
    {
        pickupChance = Random.Range(0.0f, 10.0f);
        if (pickupChance > 9.0f && !(GameObject.FindGameObjectWithTag("pickup"))
        && Time.time > nextPickupTime)
        {
            instantiatePickup();
            nextPickupTime = Time.time + pickupCooldownTime;
        }
    }
    void Update()
    {
        if (player != null && player.activeSelf)
        {
            if (playerLives <= 0)
            {
                playerDie();
            }
            numberOfEnemies = (GameObject.FindGameObjectsWithTag("Enemy").Length);
            numberOfWaypoints = (GameObject.FindGameObjectsWithTag("waypoint").Length);
            if (Input.GetKeyDown(KeyCode.C))
            {
                Cursor.visible = !(Cursor.visible);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                QuitGame();
            }
            // Instantiate enemies
            if (numberOfEnemies < maxEnemies)
            {
                CameraSupport camSupp = Camera.main.GetComponent<CameraSupport>();
                GameObject enemy = Instantiate(Resources.Load("Prefabs/enemyType1") as GameObject);
                Vector3 pos;
                pos.x = (camSupp.GetWorldBound().min.x + Random.value * camSupp.GetWorldBound().size.x) * 0.9f;
                pos.y = (camSupp.GetWorldBound().min.y + Random.value * camSupp.GetWorldBound().size.y) * 0.9f;
                pos.z = 0;
                enemy.transform.localPosition = pos;
            }
            // Test block to spawn pickup
            if (Input.GetKeyDown(KeyCode.P))
            {
                instantiatePickup();
            }
        }
        // Title/GameOver button behavior
        if (startButton.activeSelf)
        {
            startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        }
        if (reloadButton.activeSelf)
        {
            reloadButton.GetComponent<Button>().onClick.AddListener(ReloadGame);
        }
        if (quitButton.activeSelf)
        {
            quitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
        }
    }
    public void killEnemy()
    {
        numberOfEnemiesKilled += 1;
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
    }
    public void pickupMissile()
    {
        bButton.SetActive(true);
        player.gameObject.GetComponent<playerBehavior>().playerHasMissile = true;
    }
    public void shootMissile()
    {
        bButton.SetActive(false);
        player.gameObject.GetComponent<playerBehavior>().playerHasMissile = false;
    }
    public void pickupShield()
    {

    }
    public void instantiateWaypoints()
    {
        for (int i = 0; i < maxWaypoints; i++)
        {
            CameraSupport camSupp = Camera.main.GetComponent<CameraSupport>();
            GameObject waypoint = Instantiate(Resources.Load("Prefabs/waypoint") as GameObject);
            Vector3 pos;
            pos.x = (camSupp.GetWorldBound().min.x + Random.value * camSupp.GetWorldBound().size.x) * 0.9f;
            pos.y = (camSupp.GetWorldBound().min.y + Random.value * camSupp.GetWorldBound().size.y) * 0.9f;
            pos.z = 0;
            waypoint.transform.localPosition = pos;
        }
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
        Destroy(player, 0.75f);
        Cursor.visible = true;
        reloadButton.SetActive(true);
        quitButton.SetActive(true);
        gameOverText.SetActive(true);
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
        Cursor.visible = false;
    }
}