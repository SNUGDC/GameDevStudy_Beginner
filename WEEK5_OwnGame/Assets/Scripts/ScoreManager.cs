using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText, highText;
    public Text blockNum;

    private int[] scoreMemory;
    private int[] highScores;
    private int memory, state;
    
    private int score, highscore, blockCounter;
    private int gameType;

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        score = 0;
        highscore = 0;
        blockCounter = 0;
        gameType = 0;
        memory = 4;
        state = 0;

        highScores = new int[3];
        for (int i = 0; i < 3; i++)
        {
            highScores[i] = 0;
        }

        scoreMemory = new int[memory];
        for (int i = 0; i < memory; i++)
        {
            scoreMemory[i] = 0;
        }

        scoreText.text = "Score\n0000";
        highText.text = "HighScore\n0000";
        blockNum.text = "블록 수 : 0";
    }

    public void selectGame()
    {
        highScores[gameType - 1] = highscore;
        highscore = 0;
        gameType = 0;
        highText.text = "HighScore\n0000";
        
        state = 0;

        for (int i = 0; i < memory; i++)
        {
            scoreMemory[i] = 0;
        }
    }

    public void reStart()
    {
        score = 0;
        blockCounter = 0;
        scoreText.text = "Score\n0000";
        blockNum.text = "블록 수 : 0";

        state = 0;

        for (int i = 0; i < memory; i++)
        {
            scoreMemory[i] = 0;
        }
    }

    public void setGameType(int type)
    {
        gameType = type;
        highscore = highScores[gameType - 1];
        highText.text = "HighScore\n" + highscore.ToString("D4");
    }
        
    public int getScore()
    {
        return score;
    }

    public void ScoreChange(int boardSize, int [,] tilesState)
    {
        scoreMemory[state] = score;

        score = 0;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int tile = tilesState[i, j];
                if (tile == -1) continue;
                else if (tile < 3) score += 15;
                else if (tile <= 6) score += 40;
                else if (tile == 7) score += 80;
            }
        }
    }

    public void Score(int number)
    {
        scoreMemory[state] = score;

        score += number;
    }

    public void Changer()
    {
        blockCounter++;
        scoreText.text = "Score\n" + score.ToString("D4");
        blockNum.text = "블록 수: " + blockCounter.ToString();
        
        if (state < memory - 1) state++;
        else state = 0;

        if (score > highscore)
        {
            highscore = score;
            highText.text = "HighScore\n" + highscore.ToString("D4");
        }
    }

    public void back()
    {
        blockCounter--;

        if (state == 0) state = memory - 1;
        else state--;

        score = scoreMemory[state];
       
        scoreText.text = "Score\n" + score.ToString("D4");
        blockNum.text = "블록 수: " + blockCounter.ToString();
       
    }
}
