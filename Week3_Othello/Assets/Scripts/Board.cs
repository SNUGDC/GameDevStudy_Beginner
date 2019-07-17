using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int myX;
    public int myY;
    public int myState; //-1 : 아무것도 없는 상태, 0 : 검은색, 1 : 흰색

    public Sprite[] stoneImage;

    private SpriteRenderer stoneSprite;

    private void Start()
    {
    }

    private void Update()
    {
    }

    private void OnMouseDown()
    {

    }
}
