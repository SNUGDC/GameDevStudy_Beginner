using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Template
{
    public int [,] Temp1()
    {
        int[,] temp = new int [23, 23];
        temp[7, 7] = 1;
        temp[7, 8] = 1;
        temp[8, 8] = 1;
        temp[9, 8] = 1;
        temp[9, 11] = 1;
        temp[9, 12] = 1;
        temp[9, 17] = 1;
        temp[9, 14] = 1;
        temp[9, 15] = 1;
        temp[9, 16] = 1;
        temp[10, 5] = 1;
        temp[10, 6] = 1;
        temp[10, 8] = 1;
        temp[10, 10] = 1;
        temp[10, 11] = 1;
        temp[10, 13] = 1;
        temp[10, 14] = 1;
        temp[10, 15] = 1;
        temp[10, 16] = 1;
        temp[10, 17] = 1;
        temp[10, 18] = 1;
        temp[10, 19] = 1;
        temp[11, 6] = 1;
        temp[11, 8] = 1;
        temp[11, 10] = 1;
        temp[11, 11] = 1;
        temp[11, 14] = 1;
        temp[11, 15] = 1;
        temp[11, 19] = 1;
        temp[12, 6] = 1;
        temp[12, 7] = 1;
        temp[12, 8] = 1;
        temp[12, 10] = 1;
        temp[12, 11] = 1;
        temp[12, 14] = 1;
        temp[12, 15] = 1;
        temp[12, 16] = 1;
        temp[12, 18] = 1;
        temp[12, 19] = 1;
        temp[13, 7] = 1;
        temp[13, 8] = 1;
        temp[13, 10] = 1;
        temp[13, 12] = 1;
        temp[13, 13] = 1;
        temp[13, 14] = 1;
        temp[13, 15] = 1;
        temp[13, 16] = 1;
        temp[13, 17] = 1;
        temp[13, 18] = 1;
        temp[13, 19] = 1;
        for(int i = 4; i < 19; i++)
        {
            for (int j = 14; j < 16; j++)
            {
                temp[j, i] = 1;
            }
        }
        temp[16, 16] = 1;
        temp[16, 18] = 1;
        return temp;
    }

}