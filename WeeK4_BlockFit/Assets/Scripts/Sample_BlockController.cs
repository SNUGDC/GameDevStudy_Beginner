using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_BlockController : MonoBehaviour
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
        Sample_BoardManager.instance.holdingBlock = blocks;
    }

    private void OnMouseDrag()
    {
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        blocks.transform.position = mousePos;
    }

    private void OnMouseUp()
    {
        if (Sample_BoardManager.instance.CheckFit() == false)
        {
            blocks.transform.position = originPos;
            Sample_BoardManager.instance.holdingBlock = null;
        }
        else
        {
            Sample_BoardManager.instance.FitBlocks();
        }
    }
}
