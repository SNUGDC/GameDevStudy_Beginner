using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Template
{
 /*   FloorGrid floorGrid;
    void Start()
    {
        floorGrid = new FloorGrid();
        floorGrid.
    }
  */
    public bool[,] Temp1()
    {
        bool[,] temp = new bool[23, 23];
        temp[7, 7] = true;
        temp[7, 8] = true;
        temp[8, 8] = true;
        temp[9, 8] = true;
        temp[9, 11] = true;
        temp[9, 12] = true;
        temp[9, 17] = true;
        temp[9, 14] = true;
        temp[9, 15] = true;
        temp[9, 16] = true;
        temp[10, 5] = true;
        temp[10, 6] = true;
        temp[10, 8] = true;
        temp[10, 10] = true;
        temp[10, 11] = true;
        temp[10, 13] = true;
        temp[10, 14] = true;
        temp[10, 15] = true;
        temp[10, 16] = true;
        temp[10, 17] = true;
        temp[10, 18] = true;
        temp[10, 19] = true;
        temp[11, 6] = true;
        temp[11, 8] = true;
        temp[11, 10] = true;
        temp[11, 11] = true;
        temp[11, 14] = true;
        temp[11, 15] = true;
        temp[11, 19] = true;
        temp[12, 6] = true;
        temp[12, 7] = true;
        temp[12, 8] = true;
        temp[12, 10] = true;
        temp[12, 11] = true;
        temp[12, 14] = true;
        temp[12, 15] = true;
        temp[12, 16] = true;
        temp[12, 18] = true;
        temp[12, 19] = true;
        temp[13, 7] = true;
        temp[13, 8] = true;
        temp[13, 10] = true;
        temp[13, 12] = true;
        temp[13, 13] = true;
        temp[13, 14] = true;
        temp[13, 15] = true;
        temp[13, 16] = true;
        temp[13, 17] = true;
        temp[13, 18] = true;
        temp[13, 19] = true;
        for(int i = 4; i < 19; i++)
        {
            for (int j = 14; j < 16; j++)
            {
                temp[j, i] = true;
            }
        }
        temp[16, 16] = true;
        temp[16, 18] = true;
        return temp;
    }

}