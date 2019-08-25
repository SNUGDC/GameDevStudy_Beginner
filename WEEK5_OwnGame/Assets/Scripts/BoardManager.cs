using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    [Header("Board Info")]
    public int boardSize;
    public List<Transform> blockSpawnPos;

    [Header("Block Type")]
    public List<GameObject> blockType;

    [Header("Resources")]
    public GameObject tilePrefab;

    [HideInInspector] public GameObject holdingBlock;
    private List<GameObject> targetTiles = new List<GameObject>();
    private List<GameObject> createdBlockList = new List<GameObject>();
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
            RerollBlocks();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateBlock();
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

        Camera.main.transform.position = new Vector3((float)(boardSize - 1) / 2, (float)(boardSize - 1) / 2 + 2, -10);
    }

    private void RerollBlocks()
    {
        foreach (GameObject block in createdBlockList)
        {
            if (block != null) Destroy(block);
        }
        createdBlockList = new List<GameObject>();

        for (int i = 0; i < 3; i++)
        {
            int randomType = Random.Range(0, blockType.Count);

            GameObject blocks = CreateBlock(randomType);
            blocks.transform.position = blockSpawnPos[i].position;
        }

        BattleManager.instance.EnemyAttack();
    }

    private GameObject CreateBlock(int typeNum)
    {
        GameObject block = Instantiate(blockType[typeNum]);
        createdBlockList.Add(block);

        return block;
    }

    private void RotateBlock()
    {
        if (holdingBlock == null) return;

        for(int i = 0; i < holdingBlock.transform.childCount; i++)
        {
            Transform block = holdingBlock.transform.GetChild(i);

            float originX = block.localPosition.x;
            float originY = block.localPosition.y;

            //블럭의 위치 회전
            block.localPosition = new Vector2(originY, -originX);
            //블럭의 고유 회전
            block.localRotation = Quaternion.Euler(0, 0, -90f + block.localEulerAngles.z);
            //블럭의 dir 회전
            for (int j = 0; j < block.GetComponent<BlockController>().dir.Length; j++)
            {
                Vector2 originDir = block.GetComponent<BlockController>().dir[j];

                block.GetComponent<BlockController>().dir[j] = new Vector2(originDir.y, -originDir.x);
            }
        }
    }

    public bool CheckFit()
    {
        if (holdingBlock == null) return false;

        ChangeTargetTilesColor(Color.white);
        targetTiles = new List<GameObject>();
        int isFit = 0;

        for (int i = 0; i < holdingBlock.transform.childCount; i++)
        {
            Transform tr = holdingBlock.transform.GetChild(i).gameObject.transform;

            float targetX = tr.position.x;//(int)mousePosInt().x + (int)tr.localPosition.x;
            float targetY = tr.position.y;//(int)mousePosInt().y + (int)tr.localPosition.y;

            RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(targetX, targetY), transform.forward);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.tag == "Tile")
                {
                    isFit += 1;
                    targetTiles.Add(hit.transform.gameObject);
                }
            }
        }
        
        if (isFit < holdingBlock.transform.childCount) return false;

        ChangeTargetTilesColor(new Color(0.6f, 0.6f, 0.6f));
        return true;
    }

    private void ChangeTargetTilesColor(Color color)
    {
        foreach (GameObject tile in targetTiles)
        {
            tile.GetComponent<SpriteRenderer>().color = color;
        }
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

            int targetX = (int)targetTiles[i].transform.position.x;
            int targetY = (int)targetTiles[i].transform.position.y;

            tr.position = targetTiles[i].transform.position;
            tr.parent = gameObject.transform;
            tilesObject[targetX, targetY] = tr.gameObject;
            tr.GetComponent<Collider2D>().enabled = false;

            tilesState[targetX, targetY] = 1;
        }

        Destroy(holdingBlock);
        holdingBlock = null;

        ChangeTargetTilesColor(Color.white);
        targetTiles = new List<GameObject>();
    }
    
    public bool FindLoop(Vector2 nowPos, Vector2 startPos, Vector2 comingDir, List<GameObject> loopObject)
    {
        if (startPos.x == nowPos.x && startPos.y == nowPos.y) return true;
        if (!(nowPos.x >= 0 && nowPos.x < boardSize && nowPos.y >= 0 && nowPos.y < boardSize)) return false;
        if (tilesState[(int)nowPos.x, (int)nowPos.y] == 0) return false;

        //comingDir = 들어온 방향. Left라면 loop 체크시 right를 체크하지 않기위해
        Vector2 ignoreDir = new Vector2(-1 * comingDir.x, -1 * comingDir.y);

        for(int i = 0; i < 2; i++)
        {
            Vector2 checkDir = tilesObject[(int)nowPos.x, (int)nowPos.y].GetComponent<BlockController>().dir[i];
            if (checkDir == ignoreDir) continue;

            List<GameObject> temp = loopObject;
            temp.Add(tilesObject[(int)nowPos.x, (int)nowPos.y]);
            return FindLoop(nowPos + checkDir, startPos, checkDir, temp);
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