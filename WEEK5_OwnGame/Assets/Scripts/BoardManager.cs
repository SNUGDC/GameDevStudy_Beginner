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
    public List<Sprite> blockSprite;

    [HideInInspector] public GameObject holdingBlock;
    private GameObject[,] tilesObject;
    private int[,] tilesState; //0이면 아무것도 없는 상태, 1이면 가위, 2면 바위, 3이면 보

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
                int randomColor = Random.Range(1, 4);

                GameObject blocks = CreateBlock(randomType, randomColor);
                blocks.transform.position = new Vector2(-5 + i * 5, -2);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //D를 눌렀을 때 어떤 숫자가 제일 많은지 로그로 띄우기
            Debug.Log("D가 눌림.");
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
            singleBlock.GetComponent<BlockController>().myColor = color;
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
            if (tilesState[targetX, targetY] != 0) return false;
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
            tilesState[targetX, targetY] = holdingBlock.GetComponentInChildren<BlockController>().myColor;
        }

        Destroy(holdingBlock);
        holdingBlock = null;
    }

    private int Judgment()
    {
        //tileState
        int redCount = 0;
        int greenCount = 0;
        int yellowCount = 0;

        for (int i = 0; i < 6; i++)
        {
            if (tilesState[0, i] == 1)
            {
                redCount = redCount + 1;
            }
            if (tilesState[0, i] == 2)
            {
                greenCount = greenCount + 1;
            }
            if (tilesState[0, i] == 3)
            {
               yellowCount = yellowCount + 1;
            }

        }
    }
}