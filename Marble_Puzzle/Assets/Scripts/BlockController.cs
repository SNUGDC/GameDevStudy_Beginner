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
        originPos = blocks.transform.position;
        BoardManager.instance.holdingBlock = blocks;
        
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        blocks.transform.position = mousePos;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 0; i < blocks.transform.childCount; i++)
            {
                blocks.transform.GetChild(i).gameObject.transform.Rotate(0, 0, 90);
            }
        }

    }

    private void OnMouseUp()
    {
        if (BoardManager.instance.CheckFit() == false)
        {
            //BackToOriginPos();
        }
        else
        {
            BoardManager.instance.FitBlocks();
        }
    }

    private void BackToOriginPos()
    {
        blocks.transform.position = originPos;
        BoardManager.instance.holdingBlock = null;
    }
}
