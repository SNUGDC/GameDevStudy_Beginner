using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject myHand;
    public GameObject computerHand;
    public AudioSource SEAudio;
    public Text judgment;
    public Sprite[] hands;
    public AudioClip winSource;
    public AudioClip loseSource;
    public AudioClip drawSource;

    private void Start()
    {
        computerHand.GetComponent<Image>().sprite = null;
        myHand.GetComponent<Image>().sprite = null;
        judgment.text = "";
    }

    //0 : 바위, 1: 가위, 2 : 보
    public void MyHand(int hand)
    {
        myHand.GetComponent<Image>().sprite = hands[hand];
        myHand.GetComponent<Image>().SetNativeSize();
        WhoWin(hand, ComputerHand());
    }

    private int ComputerHand()
    {
        int i = Random.Range(0, 3);
        computerHand.GetComponent<Image>().sprite = hands[i];
        computerHand.GetComponent<Image>().SetNativeSize();
        return i;
    }

    //0 : 바위, 1: 가위, 2 : 보
    private void WhoWin(int myHand, int computerHand)
    {
        if (myHand == 0)
        {
            if (computerHand == 0) DrawReaction();
            else if (computerHand == 1) WinReaction();
            else if (computerHand == 2) LoseReaction();
        }
        else if (myHand == 1)
        {
            if (computerHand == 0) LoseReaction();
            else if (computerHand == 1) DrawReaction();
            else if (computerHand == 2) WinReaction();
        }
        else if (myHand == 2)
        {
            if (computerHand == 0) WinReaction();
            else if (computerHand == 1) LoseReaction();
            else if (computerHand == 2) DrawReaction();
        }
    }

    private void LoseReaction()
    {
        judgment.text = "패배";
        PlayOnce(loseSource);
    }

    private void WinReaction()
    {
        judgment.text = "승리";
        PlayOnce(winSource);
    }

    private void DrawReaction()
    {
        judgment.text = "무승부";
        PlayOnce(drawSource);
    }

    private void PlayOnce(AudioClip clip)
    {
        SEAudio.clip = clip;
        SEAudio.Play();
    }
}
