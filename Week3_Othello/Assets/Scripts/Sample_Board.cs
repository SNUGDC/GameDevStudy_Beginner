using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_Board : MonoBehaviour
{
    public int myX;
    public int myY;
    public int myState; //-1 : 아무것도 없는 상태, 0 : 검은색, 1 : 흰색

    public Sprite[] stoneImage;

    private SpriteRenderer stoneSprite;

    private void Start()
    {
        stoneSprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (myState != -1) stoneSprite.sprite = stoneImage[myState];
    }

    private void OnMouseDown()
    {
        //Debug.Log("위치 (" + myX + ", " + myY + "), 상태는 " + myState);
        gameObject.transform.parent.GetComponent<Sample_BoardManager>().FlipStone(myX, myY);
    }
}
