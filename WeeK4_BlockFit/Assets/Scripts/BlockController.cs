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
        
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseDrag()
    {
        
    }

    private void OnMouseUp()
    {
        if (BoardManager.instance.CheckFit() == false)
        {
            
        }
        else
        {
            
        }
    }
}
