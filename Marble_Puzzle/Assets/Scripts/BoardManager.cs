using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    [Header("Piece Type")]
    public List<GameObject> pieceType;

    [Header("Resources")]
    public GameObject tilePrefab;
    //public List<Sprite> pieceSprite;
    public List<GameObject> piecePos;

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
        CreateBlock();
    }

    //private void Update()
   // {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
           // for (int i = 0; i < 3; i++)
           // {
              //  int randomType = Random.Range(0, pieceType.Count);
                //int randomType = 0;
                //int randomColor = Random.Range(0, pieceSprite.Count);

              //  GameObject blocks = CreateBlock(randomType);
              //  blocks.transform.position = new Vector2(-5 + i * 5, -2);
           // }
      //  }
   // }

    private void CreateBoard()
    {
        tilesObject = new GameObject[10, 10];
        tilesState = new int[10, 10];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j <= i ; j++)
            {
                Vector2 spawnPos = new Vector2(i, j);
                tilesObject[i, j] = Instantiate(tilePrefab, spawnPos, Quaternion.identity, transform);

                tilesState[i, j] = 0;
            }
        }

        Camera.main.transform.position += new Vector3((float)5, (float)4, 0);
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, -135);
    }

    private void CreateBlock()
    {
        int typeNum;
        for (typeNum = 0; typeNum < 12; typeNum++)
        {
            GameObject block = Instantiate(pieceType[typeNum]);
            block.transform.position = piecePos[typeNum].transform.position;
            
            for (int i = 0; i < block.transform.childCount; i++)
            {
                GameObject singleBlock = block.transform.GetChild(i).gameObject;
                //singleBlock.GetComponent<SpriteRenderer>().sprite = pieceSprite[color];
                
            }

            //return block;
        }
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
            if (!(targetX >= 0 && targetX < 10 && targetY >= 0 && targetY < 10)) return false;

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

            holdingBlock.transform.position = new Vector2(targetX, targetY);

            //tilesObject[targetX, targetY].GetComponent<SpriteRenderer>().sprite = holdingBlock.GetComponentInChildren<SpriteRenderer>().sprite;
            //tilesState[targetX, targetY] = 1;
        }

        //Destroy(holdingBlock);
        holdingBlock = null;
    }
}
