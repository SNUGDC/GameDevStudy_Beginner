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
        blocks.transform.position = mousePos + new Vector2(0, 2f);
        
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

            List<GameObject> loop = new List<GameObject> { gameObject };

            if (BoardManager.instance.FindLoop((Vector2)transform.position + dir[0], transform.position, dir[0], loop))
            {
                BattleManager.instance.PlayerAttack(loop.Count);
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
