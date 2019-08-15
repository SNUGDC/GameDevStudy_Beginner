using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    public void SelectStage(int i)
    {
        SceneManager.LoadScene(i);
    }
}
