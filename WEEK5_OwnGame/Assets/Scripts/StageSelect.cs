using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    public static StageSelect instance;

    public int StageNumber;

    private void Start()
    {
        StageNumber = 0;
        DontDestroyOnLoad(this);
    }

    private void Stage1()
    {
        StageNumber = 1;
        if(StageNumber != 0)
        {
            SceneManager.LoadScene("Main");
        }
    }
    private void Stage2()
    {
        StageNumber = 2;
        if (StageNumber != 0)
        {
            SceneManager.LoadScene("Main");
        }
    }
    private void Stage3()
    {
        StageNumber = 3;
        if (StageNumber != 0)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
