using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private int[,] tilesState; //0 흰색 1 검은색

    private List<GameObject> BlockinBoard;
    public int Gauge = 3;
    private int ReRollCount = 0;
    public Text ShowCount;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        ShowCount.text = ReRollCount.ToString();
    }

    private void Start()
    {
        switch (StageSelect.instance.StageNumber)
        {
            case 1:
                {
                    CreateBoard();
                    break;
                }
            case 2:
                {
                    CreateChessBoard();
                    break;
                }
            case 3:
                {
                    CreateHeartBoard();
                    boardSize = 9;
                    break;
                }
        }

        BlockinBoard = new List<GameObject>();
        CreateMultipleBlocks();
    }

    private void Update()
    {

        if (Gauge == 0)
        {
            CreateMultipleBlocks();

            Gauge = 3;
        }

        if (Input.GetKeyDown(KeyCode.Q) && holdingBlock != null)
        {
            RotateBlock();
        }
    }

    public void ReRoll()
    {
        for (int i = 0; i < BlockinBoard.Count; i++)
        {
            Destroy(BlockinBoard[i]);
        }

        BlockinBoard.Clear();

        CreateMultipleBlocks();

        ReRollCount++;
        ShowCount.text = ReRollCount.ToString();

        Gauge = 3;
    }

    private void CreateMultipleBlocks()
    {
        for (int i = 0; i < 3; i++)
        {
            int randomType = Random.Range(0, blockType.Count);

            GameObject blocks = CreateBlock(randomType);
            blocks.transform.position = new Vector2(-5 + i * 5, -2);
        }
    }

    private void RotateBlock()
    {
        for (int i = 0; i < holdingBlock.transform.childCount; i++)
        {
            Transform tr = holdingBlock.transform.GetChild(i).gameObject.transform;

            float x = tr.localPosition.x;
            float y = tr.localPosition.y;

            tr.localPosition = new Vector2(y, -x);
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

    private void CreateHeartBoard()
    {
        tilesObject = new GameObject[boardSize, boardSize];
        tilesState = new int[boardSize, boardSize];

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                switch (j)
                {
                    case 0:
                    case 8:
                        {
                            if (i < 7 && i > 3)
                            {
                                Vector2 spawnPos = new Vector2(j, i);
                                tilesObject[i, j] = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                            }
                            break;
                        }
                    case 1:
                    case 7:
                        {
                            if (i > 2)
                            {
                                Vector2 spawnPos = new Vector2(j, i);
                                tilesObject[i, j] = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                            }
                            break;
                        }
                    case 2:
                    case 6:
                        {
                            if (i > 1)
                            {
                                Vector2 spawnPos = new Vector2(j, i);
                                tilesObject[i, j] = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                            }
                            break;
                        }
                    case 3:
                    case 5:
                        {
                            if (i < 7 && i > 0)
                            {
                                Vector2 spawnPos = new Vector2(j, i);
                                tilesObject[i, j] = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                            }
                            break;
                        }
                    case 4:
                        {
                            if (i < 6)
                            {
                                Vector2 spawnPos = new Vector2(j, i);
                                tilesObject[i, j] = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);
                            }
                            break;
                        }
                }
            }
        }

        Camera.main.transform.position += new Vector3((float)(boardSize - 1) / 2, (float)(boardSize - 1) / 2, 0);
    }

    private void CreateChessBoard()
    {
        tilesObject = new GameObject[boardSize, boardSize];
        tilesState = new int[boardSize, boardSize];

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                Vector2 spawnPos = new Vector2(i, j);
                tilesObject[i, j] = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);

                if ((i + j + 1) % 2 == 1)
                {
                    tilesState[i, j] = 1;
                    tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = blockSprite[1];
                }
                else
                {
                    tilesState[i, j] = 0;
                    tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = blockSprite[0];
                }
            }
        }

        Camera.main.transform.position += new Vector3((float)(boardSize - 1) / 2, (float)(boardSize - 1) / 2, 0);
    }

    private GameObject CreateBlock(int typeNum)
    {
        GameObject block = Instantiate(blockType[typeNum]);

        for (int i = 0; i < block.transform.childCount; i++)
        {
            GameObject singleBlock = block.transform.GetChild(i).gameObject;
            singleBlock.GetComponent<SpriteRenderer>().sprite = blockSprite[1];
        }

        BlockinBoard.Add(block);

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
            //if (tilesState[targetX, targetY] == 1) return false;
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

            //tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = holdingBlock.GetComponentInChildren<SpriteRenderer>().sprite;

            tilesState[targetX, targetY] = (tilesState[targetX, targetY] + 1) % 2;

            if(tilesState[targetX, targetY] == 1)
            {
                tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = blockSprite[1];
            }
            else
            {
                tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = blockSprite[0];
            }
        }

        Destroy(holdingBlock);
        holdingBlock = null;
    }

    public void FitMoreBlocks()
    {
        for (int i = 0; i < holdingBlock.transform.childCount; i++)
        {
            Transform tr = holdingBlock.transform.GetChild(i).gameObject.transform;

            int targetX = (int)mousePosInt().x + (int)tr.localPosition.x;
            int targetY = (int)mousePosInt().y + (int)tr.localPosition.y;

            //tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = holdingBlock.GetComponentInChildren<SpriteRenderer>().sprite;

            tilesState[targetX, targetY] = (tilesState[targetX, targetY] + 1) % 2;

            if (tilesState[targetX, targetY] == 1)
            {
                tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = blockSprite[1];
            }
            else
            {
                tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = blockSprite[0];
            }
        }

        Destroy(holdingBlock);
        holdingBlock = null;
    }
}
