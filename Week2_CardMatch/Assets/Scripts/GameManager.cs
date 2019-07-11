using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] cardObject;
    public Sprite[] cardSprite;
    public AudioClip[] SE;

    private int[] numberArray = new int[8];
    private int firstCardPos = -1;
    private bool isSetting = false;
    private AudioSource audioSource;

    private void Start()
    {
        //카드 배열을 생성

        //카드를 초기화

        //오디오 소스를 할당

    }

    //카드 배치 배열을 생성하는 함수
    private void CreateArray()
    {
        List<int> originNum = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

        for (int i = 0; i < 4; i++)
        {
            int n = Random.Range(0, originNum.Count);

            numberArray[i] = originNum[n];
            numberArray[i + 4] = originNum[n];
            originNum.RemoveAt(n);
        }

        for (int i = 0; i < 8; i++)
        {
            int temp = numberArray[i];
            int target = Random.Range(0, 8);

            numberArray[i] = numberArray[target];
            numberArray[target] = temp;
        }
    }

    private void InitailizeCard()
    {
        //모든 카드를 초기화 상태로

    }

    public void ShowWhoAmI(int myPos)
    {
        //세팅 중이면 조작 불가
        if (isSetting == true) return;

        //누른 카드는 자신의 카드를 보여주고 버튼을 비활성화
        int myNumber = numberArray[myPos];

        //클릭에 대한 판정
        if (firstCardPos == -1)
        {
            //첫번째 카드를 집는 경우
            //내가 집은 첫번째 카드의 위치를 저장해둔다.

        }
        else if (numberArray[firstCardPos] == myNumber)
        {
            //두번째 카드를 열고 그 카드가 동일할 경우
            //정답 반응

        }
        else
        {
            //두번째 카드를 열었지만 다를 경우
            //실패 반응

        }
    }

    private void SameReaction(int secondCardPos)
    {
        Debug.Log("정답!");
        audioSource.clip = SE[0];
        audioSource.Play();

        firstCardPos = -1;
    }

    private IEnumerator DiffReaction(int secondCardPos)
    {
        Debug.Log("오답!");

        audioSource.clip = SE[1];
        audioSource.Play();

        isSetting = true;

        yield return new WaitForSeconds(1);

        cardObject[firstCardPos].GetComponent<Image>().sprite = cardSprite[0];
        cardObject[secondCardPos].GetComponent<Image>().sprite = cardSprite[0];

        cardObject[firstCardPos].GetComponent<Button>().interactable = true;
        cardObject[secondCardPos].GetComponent<Button>().interactable = true;

        firstCardPos = -1;

        isSetting = false;
    }
}