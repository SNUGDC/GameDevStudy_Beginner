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
        GameManager.instance.holdingBlock = blocks;
    }

    private void OnMouseDrag()
    {
        blocks.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        bool isfit = GameManager.instance.CheckFit();

        if (isfit)
        {
            GameManager.instance.FitBlocks();
        }
        else
        {
            blocks.transform.position = originPos;
        }
        /*
        if (BoardManager.instance.CheckFit() == false)
        {
            
        }
        else
        {
            
        }*/
    }
}
