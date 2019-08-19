using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    [Header("Board Info")]
    public int boardSize;

    [Header("Block Type")]
    public List<GameObject> blockType;

    [Header("Resources")]
    public GameObject tilePrefab;

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

                GameObject blocks = CreateBlock(randomType);
                blocks.transform.position = new Vector2(-5 + i * 5, -2);
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

    private GameObject CreateBlock(int typeNum)
    {
        GameObject block = Instantiate(blockType[typeNum]);

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
        int childCnt = holdingBlock.transform.childCount;
        for (int i = 0; i < childCnt; i++)
        {
            Transform tr = holdingBlock.transform.GetChild(0);

            int targetX = (int)mousePosInt().x + (int)tr.localPosition.x;
            int targetY = (int)mousePosInt().y + (int)tr.localPosition.y;

            tr.position = new Vector2(targetX, targetY);
            tr.parent = gameObject.transform;
            tilesObject[targetX, targetY] = tr.gameObject;
            tr.GetComponent<Collider2D>().enabled = false;

            tilesState[targetX, targetY] = 1;
        }

        Destroy(holdingBlock);
        holdingBlock = null;
    }
    
    public bool FindLoop(int x, int y, int startX, int startY, Vector2 comingDir, List<GameObject> loopObject)
    {
        if (startX == x && startY == y) return true;
        if (!(x >= 0 && x < boardSize && y >= 0 && y < boardSize)) return false;
        if (tilesState[x, y] == 0) return false;

        //comingDir = 들어온 방향. Left라면 loop 체크시 right를 체크하지 않기위해
        Vector2 ignoreDir = new Vector2(-1 * comingDir.x, -1 * comingDir.y);

        for(int i = 0; i < 2; i++)
        {
            Vector2 checkDir = tilesObject[x, y].GetComponent<BlockController>().dir[i];
            if (checkDir == ignoreDir) continue;

            List<GameObject> temp = loopObject;
            temp.Add(tilesObject[x, y]);
            return FindLoop(x + (int)checkDir.x, y + (int)checkDir.y, startX, startY, checkDir, temp);
        }
        return false;
    }

    public void DestroyBlock(GameObject block)
    {
        int x = (int)block.transform.position.x;
        int y = (int)block.transform.position.y;

        Destroy(tilesObject[x, y]);
        tilesState[x, y] = 0;
        tilesObject[x, y] = null;
    }
}