using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int maxEnemyInField = 10;
    public int enemyLeft;
    public float timeLeft = 60;

    bool isPlaying = true;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyLeft = maxEnemyInField;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                GameOverScene();
            }
        }
    }

    public void EnemyDied()
    {
        enemyLeft--;
        if (enemyLeft <= 0)
        {
            GameOverScene();
        }
    }

    public void GameOverScene()
    {
        isPlaying = false;
        SceneManager.LoadScene("GameOverScene");
    }
}
