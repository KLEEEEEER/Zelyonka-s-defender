using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public int enemiesToKill = 30;
    public HUDController hudController;

    public GameObject GameOverTextObject;

    [SerializeField]
    bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        hudController.UpdateEnemiesToKillText(enemiesToKill);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnemyKilled()
    {
        enemiesToKill--;
        if (enemiesToKill <= 0)
        {
            enemiesToKill = 0;
            Win();
        }
        hudController.UpdateEnemiesToKillText(enemiesToKill);
    }

    public void Win()
    {
        gameOver = true;
    }

    public void GameOver()
    {
        GameOverTextObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public bool isGameOver()
    {
        return gameOver;
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level");
    }
}
