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
    GameObject bButton, player;
    GameObject reloadButton;
    void Start()
    {
        bButton = GameObject.Find("bButton");
        player = GameObject.Find("playerShip");
        bButton.SetActive(false);
        Cursor.visible = false;
        reloadButton = GameObject.Find("reloadButton");
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
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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
        if (reloadButton.activeSelf) {
            reloadButton.GetComponent<Button>().onClick.AddListener(ReloadGame);
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
        Destroy(player,0.75f);
        Cursor.visible = true;
        reloadButton.SetActive(true);
    }
    private void ReloadGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}