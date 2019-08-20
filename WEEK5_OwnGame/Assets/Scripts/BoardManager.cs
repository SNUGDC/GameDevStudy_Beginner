using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    public Text timeText;

    [Header("Board Info")]
    public int boardSize;

    [Header("Block Type")]
    public int maxBlockSize;
    public List<GameObject> blockType;
    public GameObject blockParent;

    [Header("Resources")]
    public GameObject tilePrefab;
    public Sprite emptySprite;
    public List<Sprite> blockSprite;
    public List<Sprite> mixSprite;

    [HideInInspector] public GameObject holdingBlock;
    private GameObject[,] tilesObject;
    private int[,] tilesState;
    /* -1이면 빈 상태
     * 0, 1, 2는 마젠타, 노랑, 시안
     * 4, 5, 6, 7은 빨강, 초록, 파랑, 검정
    */
    private struct memoryBlocks {
        public int typeNum, spriteNum;
        public int poSition;
    };
    memoryBlocks[] mBlocks;
    private int[,,] memoryTiles;
    private int memory, state, bState;
    private GameObject[] storageBlocks;

    private int color;
    private float time;
    private int minute;
    private int gameType;
    public bool isPause;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        isPause = true;
        CreateBoard();
        gameType = 0;
        
        time = (float)0.0;
        minute = 0;
        timeText.text = minute.ToString("D2") + ":" + ((int)time).ToString("D2");

        memory = 4;
        state = 0;
        bState = 1;
        
        mBlocks = new memoryBlocks[memory];
        for (int i = 0; i < memory; i++)
        {
            mBlocks[i].poSition = -1;
        }
        memoryTiles = new int[boardSize, boardSize, memory];
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                memoryTiles[i, j, 0] = -1;
            }
        }

        storageBlocks = new GameObject[3];
    }

    private void Update()
    {
        if (isPause == true)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }

        time += Time.deltaTime;
        if (time >= 60)
        {
            minute += 1;
            time %= 60;
        }
        timeText.text = minute.ToString("D2") + ":" + ((int)time).ToString("D2");
    }

    public void setGameType(int type)
    {
        gameType = type;
    }

    public void setStart()
    {
        for (int i = 0; i < 3; i++)
        {
            CreateBlock(i);
        }
    }

    public void blockDestroy()
    {
        for (int i = 0; i < 3; i++)
        {
            Destroy(blockParent.transform.GetChild(i).gameObject);
        }
        if (blockParent.transform.childCount > 0)
        {
            Destroy(blockParent.transform.GetChild(0).gameObject);
        }
    }

    public void reStart()
    {
        ScoreManager.instance.reStart();
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = emptySprite;
                tilesState[i, j] = -1;
            }
        }

        blockDestroy();
        time = (float)0.0;
        minute = 0;
        timeText.text = minute.ToString("D2") + ":" + ((int)time).ToString("D2");

        state = 0;
        bState = 1;

        for (int i = 0; i < memory; i++)
        {
            mBlocks[i].poSition = -1;
        }
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                memoryTiles[i, j, 0] = -1;
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
                Vector2 spawnPos = new Vector2(i, j + 1);
                tilesObject[i, j] = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);

                tilesState[i, j] = -1;
            }
        }

        Camera.main.transform.position += new Vector3((float)(boardSize - 1) / 2, (float)(boardSize - 1) / 2, 0);   
    }

    private int RandomType()
    {
        if (gameType == 1 || gameType == 2 || gameType == 3)
        {
            int random = Random.Range(0, 100);
            int score = ScoreManager.instance.getScore() / 100;
            if (score > 20) score = 20;

            if (random < 30 - score)
            {
                return 0;

            } else if (random < 60 - score * 2)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0) return 1;
                else return 2;

            } else if (random < 80 - score)
            {
                int rand = Random.Range(0, 10);
                if (rand < 4) return (rand + 3);
                else if (rand < 7) return 7;
                else return 8;

            } else
            {
                int rand = Random.Range(0, 20);
                if (rand < 9) return (rand % 3 + 9);
                else
                {
                    rand = Random.Range(0, 12);
                    return (rand + 12);
                }
            }
        }
        return Random.Range(0, blockType.Count);
    }

    private void CreateBlock(int position)
    {
        int randomType = RandomType();
        int randomColor = Random.Range(0, blockSprite.Count);

        GameObject block = Instantiate(blockType[randomType]);
        storageBlocks[position] = block;
        block.transform.SetParent(blockParent.transform);

        for (int i = 0; i < block.transform.childCount; i++)
        {
            GameObject singleBlock = block.transform.GetChild(i).gameObject;
            singleBlock.GetComponent<SpriteRenderer>().sprite = blockSprite[randomColor];
        }
        block.transform.position = new Vector2(-3 + position * 5, -1);

    }

    private void CheckColor()
    {
        string colorName = holdingBlock.GetComponentInChildren<SpriteRenderer>().sprite.name;
        
        for (int i = 0; i < 3; i++)
        {
            if (colorName.Equals(blockSprite[i].name))
            {
                color = i;
                break;
            }
        }
    }

    private void CheckType()
    {
        string typeName = holdingBlock.gameObject.name;
        string[] type = typeName.Split('(');

        for (int i = 0; i < blockType.Count; i++)
        {
            if (type[0].Equals(blockType[i].name))
            {
                mBlocks[state].typeNum = i;
                break;
            }
        }
    }

    public bool CheckFit()
    {
        if (holdingBlock == null) return false;

        CheckColor();
        for (int i = 0; i < holdingBlock.transform.childCount; i++)
        {
            Transform tr = holdingBlock.transform.GetChild(i).gameObject.transform;

            int targetX = (int)mousePosInt().x + (int)tr.localPosition.x;
            int targetY = (int)mousePosInt().y + (int)tr.localPosition.y - 1;

            //주어진 타일 밖일 경우 false 반환
            if (!(targetX >= 0 && targetX < boardSize && targetY >= 0 && targetY < boardSize)) return false;

            int tState = tilesState[targetX, targetY];

           
            if (tState == color || tState == 7) return false;
            if (tState == 4 && color == 0 || tState == 4 && color == 1) return false;
            if (tState == 5 && color == 1 || tState == 5 && color == 2) return false;
            if (tState == 6 && color == 0 || tState == 6 && color == 2) return false;
        }
       
        return true;
    }

    private Vector2 mousePosInt()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
    }

    public void FitBlocks(float pos)
    {
        for (int i = 0; i < holdingBlock.transform.childCount; i++)
        {
            Transform tr = holdingBlock.transform.GetChild(i).gameObject.transform;

            int targetX = (int)mousePosInt().x + (int)tr.localPosition.x;
            int targetY = (int)mousePosInt().y + (int)tr.localPosition.y - 1;
            
            int tState = tilesState[targetX, targetY];

            if (tState == -1)
            {
                tilesState[targetX, targetY] = color;
                tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = holdingBlock.GetComponentInChildren<SpriteRenderer>().sprite;
                
            }
            else if (tState == 0)
            {
                if (color == 1)
                {
                    tilesState[targetX, targetY] = 4;
                    tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = mixSprite[0];
                }
                else
                {
                    tilesState[targetX, targetY] = 6;
                    tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = mixSprite[2];
                }
                
            }
            else if (tState == 1)
            {
                if (color == 0)
                {
                    tilesState[targetX, targetY] = 4;
                    tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = mixSprite[0];
                }
                else
                {
                    tilesState[targetX, targetY] = 5;
                    tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = mixSprite[1];
                }
               
            }
            else if (tState == 2)
            {
                if (color == 0)
                {
                    tilesState[targetX, targetY] = 6;
                    tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = mixSprite[2];
                }
                else
                {
                    tilesState[targetX, targetY] = 5;
                    tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = mixSprite[1];
                }
                
            }
            else if (tState >= 4 && tState <= 6)
            {
                tilesState[targetX, targetY] = 7;
                tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = mixSprite[3];
                
            }
        }

        if (gameType == 1)
        {
            ScoreManager.instance.ScoreChange(boardSize, tilesState);
        } else if (gameType == 2)
        {
            deleteBlack();
        } else if (gameType == 3)
        {
            deleteLine();
        } 

        int position = (int)((pos + 3.0) / 5.0);

        mBlocks[state].poSition = position;
        mBlocks[state].spriteNum = color;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                memoryTiles[i, j, bState] = tilesState[i, j];
            }
        }
        CheckType();

        if (state < memory - 1) state++;
        else state = 0;

        if (bState < memory - 1) bState++;
        else bState = 0;
        
        CreateBlock(position);
        Destroy(holdingBlock);
        holdingBlock = null;
       
        ScoreManager.instance.Changer();
    }

    public bool back()
    {
        if (state == 0) state = memory - 1;
        else state--;

        if (bState == 0) bState = memory - 1;
        else bState--;

        if (mBlocks[state].poSition == -1 || holdingBlock != null)
        {
            if (state < memory - 1) state++;
            else state = 0;

            if (bState < memory - 1) bState++;
            else bState = 0;

            return false;
        }
        
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                tilesState[i, j] = memoryTiles[i, j, state];

                switch(tilesState[i, j])
                {
                    case -1:
                        tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = emptySprite;
                        break;
                    case 0:
                        tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = blockSprite[0];
                        break;
                    case 1:
                        tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = blockSprite[1];
                        break;
                    case 2:
                        tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = blockSprite[2];
                        break;
                    case 4:
                        tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = mixSprite[0];
                        break;
                    case 5:
                        tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = mixSprite[1];
                        break;
                    case 6:
                        tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = mixSprite[2];
                        break;
                    case 7:
                        tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = mixSprite[3];
                        break;
                }
            }
        }
        Destroy(storageBlocks[mBlocks[state].poSition]);

        GameObject block = Instantiate(blockType[mBlocks[state].typeNum]);
        block.transform.SetParent(blockParent.transform);

        for (int i = 0; i < block.transform.childCount; i++)
        {
            GameObject singleBlock = block.transform.GetChild(i).gameObject;
            singleBlock.GetComponent<SpriteRenderer>().sprite = blockSprite[mBlocks[state].spriteNum];
        }
        block.transform.position = new Vector2(-3 + mBlocks[state].poSition * 5, -1);
        mBlocks[state].poSition = -1;

        return true;
    }

    public void deleteBlack()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (tilesState[i, j] == 7)
                {
                    ScoreManager.instance.Score(30);
                    tilesState[i, j] = -1;
                    tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = emptySprite;
                }
            }
        }
    }

    public void deleteLine()
    {
        for (int i = 0; i < boardSize; i++)
        {
            bool line = true;
            int tile = tilesState[i, 0];

            if (tile == -1) continue;

            for (int j = 1; j < boardSize; j++)
            {
                if (tile != tilesState[i, j])
                {
                    line = false;
                    break;
                }
            }

            if (line == true)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    tilesState[i, j] = -1;
                    tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = emptySprite;
                }

                if (tile < 3) ScoreManager.instance.Score(15 * boardSize);
                else if (tile <= 6) ScoreManager.instance.Score(40 * boardSize);
                else if (tile == 7) ScoreManager.instance.Score(80 * boardSize);
            }
        }

        for (int i = 0; i < boardSize; i++)
        {
            bool line = true;
            int tile = tilesState[0, i];

            if (tile == -1) continue;

            for (int j = 1; j < boardSize; j++)
            {
                if (tile != tilesState[j, i])
                {
                    line = false;
                    break;
                }
            }

            if (line == true)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    tilesState[j, i] = -1;
                    tilesObject[j, i].GetComponent<SpriteRenderer>().sprite = emptySprite;
                }

                if (tile < 3) ScoreManager.instance.Score(15 * boardSize);
                else if (tile <= 6) ScoreManager.instance.Score(40 * boardSize);
                else if (tile == 7) ScoreManager.instance.Score(80 * boardSize);
            }
        }

        int dTile = tilesState[0, 0];
        int fTile = tilesState[0, boardSize - 1];
        bool dline = true;
        bool fline = true;

        if (dTile == -1) dline = false;
        if (fTile == -1) fline = false;

        for (int j = 1; j < boardSize; j++)
        {
            if (dTile != tilesState[j, j]) dline = false;
            if (fTile != tilesState[j, boardSize - j - 1]) fline = false;
        }

        if (dline == true)
        {
            for (int j = 0; j < boardSize; j++)
            {
                tilesState[j, j] = -1;
                tilesObject[j, j].GetComponent<SpriteRenderer>().sprite = emptySprite;
            }

            if (dTile < 3) ScoreManager.instance.Score(15 * boardSize);
            else if (dTile <= 6) ScoreManager.instance.Score(40 * boardSize);
            else if (dTile == 7) ScoreManager.instance.Score(80 * boardSize);
        }

        if (fline == true)
        {
            for (int j = 0; j < boardSize; j++)
            {
                tilesState[j, boardSize - j - 1] = -1;
                tilesObject[j, boardSize - j - 1].GetComponent<SpriteRenderer>().sprite = emptySprite;
            }

            if (fTile < 3) ScoreManager.instance.Score(15 * boardSize);
            else if (fTile <= 6) ScoreManager.instance.Score(40 * boardSize);
            else if (fTile == 7) ScoreManager.instance.Score(80 * boardSize);
        }
    }
}
