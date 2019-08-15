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
    public List<Sprite> blockSprite;

    [HideInInspector] public GameObject holdingBlock;

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

        int isFit = 0;

        for (int i = 0; i < holdingBlock.transform.childCount; i++)
        {
            Transform tr = holdingBlock.transform.GetChild(i).gameObject.transform;

            int targetX = (int)mousePosInt().x + (int)tr.localPosition.x;
            int targetY = (int)mousePosInt().y + (int)tr.localPosition.y;

            RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(targetX, targetY), transform.forward);

            foreach (RaycastHit2D hit in hits)
            {
                if(hit.transform.tag == "Tile")
                {
                    isFit++;
                }
            }
        }

        if (isFit == 4) return true;
        else return false;
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

            RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(targetX, targetY), transform.forward);

            foreach (RaycastHit2D hit in hits)
            {
                if(hit.transform.tag == "Tile")
                {
                    Debug.Log("확인");
                    if(hit.transform.GetComponent<SpriteRenderer>().sprite == blockSprite[0])
                    {
                        hit.transform.GetComponent<SpriteRenderer>().sprite = blockSprite[1];
                        Debug.Log("변환");
                    }
                    else
                    {
                        hit.transform.GetComponent<SpriteRenderer>().sprite = blockSprite[0];
                    }
                }

            }
        }

        Destroy(holdingBlock);
        holdingBlock = null;
    }
}
