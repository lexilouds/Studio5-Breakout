using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private ScoreCounterUI scoreCounter;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private GameObject gameOverScreen;
    private int currentBrickCount;
    private int totalBrickCount;
    private int score;
    private int currentLives;

    private void OnEnable()
    {
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
        currentLives = maxLives; // Initialize lives
        UpdateLivesUI(); // Update UI at start
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        // fire audio here
        // implement particle effect here
        // add camera shake here
        CameraShake.Instance.Shake(0.2f, 0.5f);
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, position, Quaternion.identity);
        }

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, position);
        }


        currentBrickCount--;
        score = totalBrickCount - currentBrickCount;
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");
        scoreCounter.UpdateScore(score);

        if (currentBrickCount == 0) SceneHandler.Instance.LoadNextScene();
    }

    public void KillBall()
    {
        // maxLives--;
        // // update lives on HUD here
        // // game over UI if maxLives < 0, then exit to main menu after delay
        // ball.ResetBall();

        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            ball.ResetBall();
        }

    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0; // Freeze game
        gameOverScreen.SetActive(true); // Show Game Over UI
        StartCoroutine(ReturnToMenu());
    }
    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1; // Reset time
        SceneHandler.Instance.LoadMenuScene();
    }

}
