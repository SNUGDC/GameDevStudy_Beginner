using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreBoard : MonoBehaviour
{
    public Text ReRollUsage;
    public Text BlockUsage;
    public Text FinalScore;

    public void Start()
    {
        
    }
    public void GotoStageSelect()
    {
        SceneManager.LoadScene(0);
    }
}
