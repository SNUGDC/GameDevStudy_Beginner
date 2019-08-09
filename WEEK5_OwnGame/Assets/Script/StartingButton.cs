using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartingButton : MonoBehaviour
{
    public Button StartGameButton;
    // Start is called before the first frame update
    void Start()
    {
        StartGameButton.GetComponent<Button>().onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
