using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int maxEnemies = 10;
    private int numberOfEnemies = 0;
    public int numberOfEnemiesKilled = 0;
    public int numberOfEnemiesTouched = 0;
    void Start()
    {
        Cursor.visible = false;
    }
    void Update()
    {
        numberOfEnemies = (GameObject.FindGameObjectsWithTag("Enemy").Length);
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
    }
    public void killEnemy() {
        numberOfEnemiesKilled += 1;
    }
    public void touchEnemy() {
        numberOfEnemiesTouched += 1;
    }
}