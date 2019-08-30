using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Template
{
    public int [,] Temp1()
    {
        int[,] temp = new int [23, 23];
        temp[7, 6] = 1;
        temp[7, 7] = 1;
        temp[8, 7] = 1;
        temp[9, 7] = 1;
        temp[9, 10] = 1;
        temp[9, 11] = 1;
        temp[9, 16] = 1;
        temp[9, 13] = 1;
        temp[9, 14] = 1;
        temp[9, 15] = 1;
        temp[10, 4] = 1;
        temp[10, 5] = 1;
        temp[10, 7] = 1;
        temp[10, 9] = 1;
        temp[10, 10] = 1;
        temp[10, 12] = 1;
        temp[10, 13] = 1;
        temp[10, 14] = 1;
        temp[10, 15] = 1;
        temp[10, 16] = 1;
        temp[10, 17] = 1;
        temp[10, 18] = 1;
        temp[11, 5] = 1;
        temp[11, 7] = 1;
        temp[11, 9] = 1;
        temp[11, 10] = 1;
        temp[11, 13] = 1;
        temp[11, 14] = 1;
        temp[11, 18] = 1;
        temp[12, 5] = 1;
        temp[12, 6] = 1;
        temp[12, 7] = 1;
        temp[12, 9] = 1;
        temp[12, 10] = 1;
        temp[12, 13] = 1;
        temp[12, 14] = 1;
        temp[12, 15] = 1;
        temp[12, 17] = 1;
        temp[12, 18] = 1;
        temp[13, 6] = 1;
        temp[13, 7] = 1;
        temp[13, 9] = 1;
        temp[13, 11] = 1;
        temp[13, 12] = 1;
        temp[13, 13] = 1;
        temp[13, 14] = 1;
        temp[13, 15] = 1;
        temp[13, 16] = 1;
        temp[13, 17] = 1;
        temp[13, 18] = 1;
        for(int i = 4; i < 18; i++)
        {
            for (int j = 14; j < 16; j++)
            {
                temp[j, i] = 1;
            }
        }
        temp[16, 15] = 1;
        temp[16, 17] = 1;
        return temp;
    }

}