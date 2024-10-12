using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : MonoBehaviour
{
    public TextMeshProUGUI titleLabel;
    public TextMeshProUGUI enemyKilledLabel;
    public TextMeshProUGUI timeLeftLabel;

    // Start is called before the first frame update
    void Start()
    {
        // 커서 다시 보이게 수정
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        float timeLeft = GameManager.instance.timeLeft;
        int enemyLeft = GameManager.instance.enemyLeft;
        int enemyKilled = GameManager.instance.maxEnemyInField - enemyLeft;

        // Text 
        titleLabel.text = (enemyLeft <= 0) ? "Game Clear!" : "GameOver..";
        enemyKilledLabel.text += enemyKilled.ToString();
        timeLeftLabel.text += timeLeft.ToString("#.##");

        // GameScene으로 다시 넘어가면 GameManager도 다시 생성되기 때문에
        // 이렇게 GameManager를 넘겨받은 후 사용이 끝났으면 제거해준다 . 
        Destroy(GameManager.instance.gameObject);
    }

    public void PlayAgainPressed()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        // 실제 빌드한 실행파일에서만 작동 
        Application.Quit();
    }
}
