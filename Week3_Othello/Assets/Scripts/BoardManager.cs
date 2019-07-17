using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int boardSize;

    public GameObject boardPrefab;

    private GameObject[,] board;
    public int nowTurn;
    private bool isNext = false;

    private void Start()
    {
        CreateBoard();
        BoardInitialize();

        nowTurn = 0;
    }

    private void CreateBoard()
    {
        board = new GameObject[boardSize, boardSize];

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                Vector3 spawnPos = new Vector3(i, j, 0);
                //spawnPos에 boardPrefab을 생성

                board[i, j].GetComponent<Board>().myX = i;
                board[i, j].GetComponent<Board>().myY = j;
                board[i, j].GetComponent<Board>().myState = -1;
            }
        }

        transform.position = new Vector3(-0.5f * (boardSize - 1), -0.5f * (boardSize - 1), 0);
    }

    private void BoardInitialize()
    {
        ChangeStone(boardSize / 2 - 1, boardSize / 2 - 1, 0);
        ChangeStone(boardSize / 2, boardSize / 2, 0);
        ChangeStone(boardSize / 2 - 1, boardSize / 2, 1);
        ChangeStone(boardSize / 2, boardSize / 2 - 1, 1);
    }

    private void ChangeStone(int x, int y, int state)
    {
        //(x, y)의 상태를 state로 변환
    }

    //(x,y)에 돌을 놓았을때 바뀌는 돌들의 리스트를 반환
    private List<Vector2> GetChangeList(int x, int y)
    {
        List<Vector2> changeList = new List<Vector2> { };

        if (board[x, y].GetComponent<Board>().myState != -1)
        {
            Debug.Log("이미 돌이 있는 곳에는 놓을 수 없습니다.");
            return changeList;
        }

        int[] dx = new int[8] { 1, 1, 0, -1, -1, -1, 0, 1 };
        int[] dy = new int[8] { 0, 1, 1, 1, 0, -1, -1, -1 };

        for (int i = 0; i < 8; i++)
        {
            List<Vector2> tempList = new List<Vector2> { };
            int targetX = x + dx[i];
            int targetY = y + dy[i];
            if (IsValid(targetX, targetY) == false) continue;

            while (board[targetX, targetY].GetComponent<Board>().myState != nowTurn)
            {
                int targetState = board[targetX, targetY].GetComponent<Board>().myState;
                if (targetState == -1)
                {
                    tempList = new List<Vector2> { };
                    break;
                }

                tempList.Add(new Vector2(targetX, targetY));
                targetX += dx[i];
                targetY += dy[i];

                if (IsValid(targetX, targetY) == false)
                {
                    tempList = new List<Vector2> { };
                    break;
                }
            }
            changeList.AddRange(tempList);
        }
        return changeList;
    }

    public void FlipStone(int x, int y)
    {
        //(x, y)를 클릭했을 때 해당하는 돌들을 뒤집자.        
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && x < boardSize && y >= 0 && y < boardSize;
    }
}
