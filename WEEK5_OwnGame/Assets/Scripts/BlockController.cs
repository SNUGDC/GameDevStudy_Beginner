using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public Vector2[] dir;

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
        
        BoardManager.instance.CheckFit();
    }

    private void OnMouseUp()
    {
        if (BoardManager.instance.CheckFit() == false)
        {
            BackToOriginPos();
        }
        else
        {
            BoardManager.instance.FitBlocks();

            int x = (int)transform.position.x;
            int y = (int)transform.position.y;

            List<GameObject> loop = new List<GameObject>();
            loop.Add(gameObject);
            if (BoardManager.instance.FindLoop(x + (int)dir[0].x, y + (int)dir[0].y, x, y, dir[0], loop))
            {
                foreach (GameObject gameObject in loop)
                {
                    BoardManager.instance.DestroyBlock(gameObject);
                }
            }
        }
    }

    private void BackToOriginPos()
    {
        blocks.transform.position = originPos;
        BoardManager.instance.holdingBlock = null;
    }
}
