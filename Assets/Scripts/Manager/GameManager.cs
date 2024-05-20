
using System;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public float FallSpeedMultiply = 1.0f;

    private int gameLevel = 1;
    public int GameLevel { get => gameLevel; }

    private int combo = 0;
    public int Combo { get => combo; }

    private int score = 0;
    public int Score { get => score; }

    private int b2bstack = 0;
    public int B2BStack { get => b2bstack; }

    private int totalRemovedLine = 0;

    // Reset later
    public void GameOver()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void Update()
    {
        
    }


    public void AddScore(GlobalEnums.ScoreType type, int removeLineAmount)
    {
        int multiple = 0;
        combo++;
        if (type == GlobalEnums.ScoreType.MiniTspin)
        {
            multiple = (int)Mathf.Pow(2, removeLineAmount);
            b2bstack++;
        }
        else if(type == GlobalEnums.ScoreType.Tspin)
        {
            multiple = 4 * (1 + removeLineAmount);
            b2bstack++;
        }
        else if(type == GlobalEnums.ScoreType.Normal)
        {
            if(removeLineAmount == 4)
            {
                multiple = 8;
                b2bstack++;
            }
            else if(removeLineAmount > 0)
            {
                multiple = (removeLineAmount * 2) - 1;
                b2bstack = 0;
            }
            else
            {
                combo = 0;
            }
        }
        var tempScore = (100 * multiple * gameLevel) + (combo * 50 * gameLevel);
        score += (b2bstack > 1) ? (int)(tempScore * 1.5f) : tempScore;
    }

    public void StackTotalRemovedLine(int amount)
    {
        totalRemovedLine += amount;
        if(gameLevel < 20)
        {
            if (4 * (Mathf.Ceil(gameLevel / 2.0f)) <= totalRemovedLine - 4 * (Mathf.Ceil((gameLevel - 1) / 2.0f)))
            {
                gameLevel++;
            }
        }
        Debug.Log(gameLevel);
    }
    public void AddScore(int score) => this.score += score;
}
