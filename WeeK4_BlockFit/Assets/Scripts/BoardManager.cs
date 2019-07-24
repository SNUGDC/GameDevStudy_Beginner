using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blocks
{
    public List<Vector2> block;
}

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    public int boardSize;
    public GameObject tilePrefab;
    public GameObject blockPrefab;
    public List<Sprite> blockSprite;
    public List<Blocks> type;

    [HideInInspector] public GameObject holdingBlock;
    private GameObject[,] tilesObject;
    private int[,] tilesState; //0이면 아무것도 없는 상태, 1이면 뭔가 있는 상태

    private void Awake()
    {
        //static 변수를 선언
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
                int randomType = Random.Range(0, type.Count);
                int randomColor = Random.Range(0, blockSprite.Count);

                GameObject blocks = CreateBlock(randomType, randomColor);
                blocks.transform.position = new Vector2(-5 + i * 5, -4);
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
                Vector2 spawnPos = new Vector2(i - (boardSize - 1) / 2, j - (boardSize - 1) / 2);
                tilesObject[i, j] = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);

                tilesState[i, j] = 0;
            }
        }
    }

    private GameObject CreateBlock(int typeNum, int color)
    {
        GameObject block = new GameObject { };
        List<Vector2> targetBlock = type[typeNum].block;

        for (int i = 0; i < targetBlock.Count; i++)
        {
            GameObject singleBlock = Instantiate(blockPrefab, targetBlock[i], Quaternion.identity, block.transform);
            singleBlock.GetComponent<SpriteRenderer>().sprite = blockSprite[color];
            singleBlock.GetComponent<BlockController>().myType = typeNum;
        }

        return block;
    }

    public bool CheckFit()
    {
        if (holdingBlock == null) return false;

        int blockType = holdingBlock.transform.GetComponentInChildren<BlockController>().myType;
        List<Vector2> targetBlock = type[blockType].block;

        for (int i = 0; i < targetBlock.Count; i++)
        {
            int targetX = (int)(mousePosInt().x + targetBlock[i].x);
            int targetY = (int)(mousePosInt().y + (int)targetBlock[i].y);

            if (!(targetX >= 0 && targetX < boardSize && targetY >= 0 && targetY < boardSize)) return false;
            if (tilesState[targetX, targetY] == 1) return false;
        }

        return true;
    }

    private Vector2 mousePosInt()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(Mathf.RoundToInt(mousePos.x + (boardSize - 1) / 2), Mathf.RoundToInt(mousePos.y + (boardSize - 1) / 2));
    }

    public void FitBlocks()
    {
        int blockType = holdingBlock.transform.GetComponentInChildren<BlockController>().myType;
        List<Vector2> targetBlock = type[blockType].block;

        for (int i = 0; i < targetBlock.Count; i++)
        {
            int targetX = (int)(mousePosInt().x + targetBlock[i].x);
            int targetY = (int)(mousePosInt().y + (int)targetBlock[i].y);

            tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = holdingBlock.GetComponentInChildren<SpriteRenderer>().sprite;
            tilesState[targetX, targetY] = 1;
        }

        Destroy(holdingBlock);
        holdingBlock = null;
    }
}