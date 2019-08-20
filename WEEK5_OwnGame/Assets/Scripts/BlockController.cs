using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public int myType;
    private GameObject blocks;
    private Vector2 originPos;

    private void Start()
    {
        blocks = transform.parent.gameObject;
        
    }

    private void OnMouseDown()
    {
        if (BoardManager.instance.isPause == false)
        {
            originPos = blocks.transform.position;
            BoardManager.instance.holdingBlock = blocks;
        }
    }

    private void OnMouseDrag()
    {
        if (BoardManager.instance.isPause == false)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            blocks.transform.position = mousePos;
        }
    }

    private void OnMouseUp()
    {
        if (BoardManager.instance.isPause == false)
        {
            if (BoardManager.instance.CheckFit() == false)
            {
                BackToOriginPos();
            }
            else
            {
                BoardManager.instance.FitBlocks((float)originPos.x);
            }
        }
    }

    private void BackToOriginPos()
    {
        blocks.transform.position = originPos;
        BoardManager.instance.holdingBlock = null;
    }
}
