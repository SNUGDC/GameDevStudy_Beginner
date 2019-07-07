using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image myHand; //내 손의 이미지
    public Image computerHand; // 컴퓨터 손의 이미지
    public Text judgment; //실제로 판정을 표시하는 텍스트
    public Sprite[] hands; //hands[0] : 바위 이미지, hands[1] : 가위 이미지, hands[2] : 보 이미지
    public AudioClip winSE; //이겼을 때의 사운드 이펙트
    public AudioClip loseSE; //졌을 때의 사운드 이펙트
    public AudioClip drawSE; //비겼을 때의 사운드 이펙트

    private AudioSource SEAudio; //SE를 재생시키는 컴포넌트. private이기 때문에 Inspector에서 할당하지 않고 코드에서 할당한다.

    private void Start()
    {
        judgment.text = "";

        SEAudio = GetComponent<AudioSource>(); //private인 SEAudio를 할당하는 과정
    }

    //0 : 바위, 1: 가위, 2 : 보
    public void MyHand(int handNumber)
    {
        Debug.Log(handNumber + "를 냈습니다.");

        //myHand에 해당하는 이미지를 바꿔준다.
        myHand.sprite = hands[handNumber];

        WhoWin(handNumber, ComputerHand());
    }

    private int ComputerHand()
    {
        int randomNum = Random.Range(0, 3);

        Debug.Log("컴퓨터가 " + randomNum + "를 냈습니다.");

        //computerHand에 해당하는 이미지를 바꿔준다.
        computerHand.sprite = hands[randomNum];

        return randomNum;
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
        PlayOnce(loseSE);
    }

    private void WinReaction()
    {
        judgment.text = "승리";
        PlayOnce(winSE);
    }

    private void DrawReaction()
    {
        judgment.text = "무승부";
        PlayOnce(drawSE);
    }

    //clip에 해당하는 노래를 한 번 재생시키는 함수
    private void PlayOnce(AudioClip clip)
    {
        SEAudio.clip = clip;
        SEAudio.Play();
    }
}
