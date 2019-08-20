using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public static ButtonControl instance;

    public GameObject startButton;
    public GameObject setPanel, endPanel, selectionPanel, endPanel2;
    private bool isStart;
    private int gameType;

    private int backChance, resetChance;
    private int bChanceRemain, rChanceRemain;
    public Text backRemain, resetRemain;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Start()
    {
        isStart = false;
        gameType = 0;
        startButton.gameObject.SetActive(false);
        setPanel.gameObject.SetActive(false);
        endPanel.gameObject.SetActive(false);
        endPanel2.gameObject.SetActive(false);
        selectionPanel.gameObject.SetActive(true);

        backChance = 3;
        resetChance = 3;
    }

    public void gameStart()
    {
        startButton.gameObject.SetActive(false);
        BoardManager.instance.isPause = false;
        BoardManager.instance.setStart();
        isStart = true;

        if (gameType >= 1)
        {
            bChanceRemain = backChance;
            rChanceRemain = resetChance;
            backRemain.text = bChanceRemain.ToString();
            resetRemain.text = rChanceRemain.ToString();
        }
    }

    private void setGameType()
    {
        ScoreManager.instance.setGameType(gameType);
        BoardManager.instance.setGameType(gameType);
    }

    public void gameFirst()
    {
        gameType = 1;
        startButton.gameObject.SetActive(true);
        selectionPanel.gameObject.SetActive(false);
        this.setGameType();
    }

    public void gameSecond()
    {
        gameType = 2;
        startButton.gameObject.SetActive(true);
        selectionPanel.gameObject.SetActive(false);
        this.setGameType();
    }

    public void gameThird()
    {
        gameType = 3;
        startButton.gameObject.SetActive(true);
        selectionPanel.gameObject.SetActive(false);
        this.setGameType();
    }

    public void getSetting()
    {
        if (isStart == true)
        {
            BoardManager.instance.isPause = true;
            setPanel.gameObject.SetActive(true);
        }
    }

    public void getResume()
    {
        BoardManager.instance.isPause = false;
        setPanel.gameObject.SetActive(false);
    }

    public void getRestart()
    {
        setPanel.gameObject.SetActive(false);
        endPanel.gameObject.SetActive(false);
        isStart = false;
        BoardManager.instance.reStart();
        startButton.gameObject.SetActive(true);
    }

    public void getEnd()
    {
        BoardManager.instance.isPause = true;
        endPanel.gameObject.SetActive(true);
    }

    public void exit()
    {
        endPanel2.gameObject.SetActive(true);
        endPanel.gameObject.SetActive(false);
        setPanel.gameObject.SetActive(false);
    }

    public void gameSelect()
    {
        this.Start();
        BoardManager.instance.reStart();
        ScoreManager.instance.selectGame();
    }

    public void BlockReset()
    {
        if (rChanceRemain > 0)
        {
            BoardManager.instance.blockDestroy();
            BoardManager.instance.setStart();
            rChanceRemain--;
            
            resetRemain.text = rChanceRemain.ToString();
        }
    }

    public void Back()
    {
        if (bChanceRemain > 0)
        {
            if (BoardManager.instance.back() == true)
            {
                bChanceRemain--;
                ScoreManager.instance.back();

                backRemain.text = bChanceRemain.ToString();
            }
        }
    }

    public void gameExit()
    {
        Application.Quit();
    }
}
