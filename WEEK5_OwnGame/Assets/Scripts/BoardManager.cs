using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : LoopFinder
{
    public static BoardManager instance;

    [Header("Board Info")]
    public int boardSize;

    [Header("Block Type")]
    public List<GameObject> blockType;

    [Header("Resources")]
    public GameObject tilePrefab;
    public List<Sprite> blockSprite;

    [HideInInspector] public GameObject holdingBlock;
    private GameObject[,] tilesObject;
    private int[,] tilesState; //0이면 아무것도 없는 상태, 1이면 뭔가 있는 상태

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CreateBoard();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < 3; i++)
            {
                int randomType = Random.Range(0, blockType.Count);
                int randomColor = Random.Range(0, blockSprite.Count);

                GameObject blocks = CreateBlock(randomType, randomColor);
                blocks.transform.position = new Vector2(-5 + i * 5, -2);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            List<Pos> loop = LongestLoop(tilesState, 0, 0);

            foreach (Pos pos in loop)
            {
                tilesObject[pos.x, pos.y].GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }

    private void CreateBoard()
    {
        tilesObject = new GameObject[boardSize, boardSize];
        tilesState = new int[boardSize, boardSize];

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                Vector2 spawnPos = new Vector2(i, j);
                tilesObject[i, j] = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);

                tilesState[i, j] = 0;
            }
        }

        Camera.main.transform.position += new Vector3((float)(boardSize - 1) / 2, (float)(boardSize - 1) / 2, 0);   
    }

    private GameObject CreateBlock(int typeNum, int color)
    {
        GameObject block = Instantiate(blockType[typeNum]);

        for (int i = 0; i < block.transform.childCount; i++)
        {
            GameObject singleBlock = block.transform.GetChild(i).gameObject;
            singleBlock.GetComponent<SpriteRenderer>().sprite = blockSprite[color];
        }

        return block;
    }

    public bool CheckFit()
    {
        if (holdingBlock == null) return false;

        for (int i = 0; i < holdingBlock.transform.childCount; i++)
        {
            Transform tr = holdingBlock.transform.GetChild(i).gameObject.transform;

            int targetX = (int)mousePosInt().x + (int)tr.localPosition.x;
            int targetY = (int)mousePosInt().y + (int)tr.localPosition.y;

            //주어진 타일 밖일 경우 false 반환
            if (!(targetX >= 0 && targetX < boardSize && targetY >= 0 && targetY < boardSize)) return false;

            //이미 블럭이 존재할 경우 false 반환
            if (tilesState[targetX, targetY] == 1) return false;
        }

        return true;
    }

    private Vector2 mousePosInt()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
    }

    public void FitBlocks()
    {
        for (int i = 0; i < holdingBlock.transform.childCount; i++)
        {
            Transform tr = holdingBlock.transform.GetChild(i).gameObject.transform;

            int targetX = (int)mousePosInt().x + (int)tr.localPosition.x;
            int targetY = (int)mousePosInt().y + (int)tr.localPosition.y;
            
            tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = holdingBlock.GetComponentInChildren<SpriteRenderer>().sprite;
            tilesState[targetX, targetY] = 1;
        }

        Destroy(holdingBlock);
        holdingBlock = null;
    }
}

public class Pos
{
    public int x;
    public int y;

    public void Set(int a, int b)
    {
        x = a;
        y = b;
    }
}

public class LoopFinder : MonoBehaviour
{
    private int[,] state;

    private int size;

    int[] dx = new int[4] { 1, -1, 0, 0 };
    int[] dy = new int[4] { 0, 0, -1, 1 };

    private List<Pos> longestLoop = new List<Pos>();

    public List<Pos> LongestLoop(int[,] tileState, int startX, int startY)
    {
        state = tileState;
        size = state.Length;
        bool[,] memo = new bool[size, size];

        longestLoop.Clear();

        List<Pos> initial = new List<Pos>();

        Pos start = new Pos();
        start.Set(startX, startY);
        initial.Add(start);

        FindLoop(memo, initial);
        
        return longestLoop;
    }

    private void FindLoop(bool[,] memo, List<Pos> loop)
    {
        int nowX = loop[loop.Count - 1].x;
        int nowY = loop[loop.Count - 1].y;

        for (int i = 0; i < 4; i++)
        {
            int targetX = nowX + dx[i];
            int targetY = nowY + dy[i];

            if (!(targetX >= 0 && targetX < size && targetY >= 0 && targetY < size)) continue;
            if (state[targetX, targetY] == 0) continue;
            if (memo[targetX, targetY] == true) continue;

            if (targetX == loop[0].x && targetY == loop[0].y)
            {
                if (longestLoop.Count < loop.Count)
                {
                    longestLoop.Clear();

                    for(int j = 0; j < loop.Count; j++)
                    {
                        longestLoop.Add(loop[j]);
                    }
                }

                return;
            }

            Pos target = new Pos();
            target.Set(targetX, targetY);

            loop.Add(target);
            memo[targetX, targetY] = true;

            FindLoop(memo, loop);

            loop.Remove(target);
            memo[targetX, targetY] = false;
        }
    }
}