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
    public int Gauge = 5;
    public int ReRollGauge = 0;
    public int HowManyBlock = 0;
    private int ReRollCount = 0;

    public Text ShowHowManyBlock;
    public List<Image> Gaugebar;
    public Image Bar;
    public Button ReRollButton;
    public GameObject OptionPanel;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        ShowHowManyBlock.text = HowManyBlock.ToString();
    }

    private void Start()
    {
        BlockinBoard = new List<GameObject>();
        CreateMultipleBlocks();

        ReRollButton.gameObject.SetActive(false);
        OptionPanel.SetActive(false);

        foreach (Image image in Gaugebar)
        {
            image.gameObject.SetActive(false);
        }

        Bar.color = new Color32(129, 193, 71, 255);
    }

    private void Update()
    {
        if (Gauge == 0)
        {
            CreateMultipleBlocks();

            Gauge = 5;
        }

        if (Input.GetKeyDown(KeyCode.Q) && holdingBlock != null)
        {
            RotateBlock();
        }
    }

    public void ReRoll()
    {
        if(ReRollGauge == 10)
        {
            for (int i = 0; i < BlockinBoard.Count; i++)
            {
                Destroy(BlockinBoard[i]);
            }

            BlockinBoard.Clear();

            CreateMultipleBlocks();

            ReRollCount++;

            Gauge = 5;

            ReRollGauge = 0;

            foreach (Image image in Gaugebar)
            {
                image.gameObject.SetActive(false);
            }

            ReRollButton.gameObject.SetActive(false);

            Bar.color = new Color32(129, 193, 71, 255);
        }
    }

    private void CreateMultipleBlocks()
    {
        for (int i = 0; i < 5; i++)
        {
            int randomType = Random.Range(0, blockType.Count);

            GameObject blocks = CreateBlock(randomType);
            blocks.transform.position = new Vector2(17, i * 3);
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
                    if(hit.transform.GetComponent<SpriteRenderer>().sprite == blockSprite[0])
                    {
                        hit.transform.GetComponent<SpriteRenderer>().sprite = blockSprite[1];
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

    public void ShowGaugebar(int j)
    {
            Gaugebar[j].gameObject.SetActive(true);
    }

    public void ChangeBarColor(int i)
    {
        switch(i)
        {
            case 0:
                Bar.color = new Color32(129, 193, 71, 255);
                break;
            case 2:
                Bar.color = new Color32(255, 215, 0, 255);
                break;
            case 5:
                Bar.color = new Color32(255, 150, 0, 255);
                break;
            case 8:
                Bar.color = new Color32(255, 70, 40, 255);
                break;
            default:
                break;
        }
    }

    public void OptionPanelShow()
    {
        OptionPanel.SetActive(true);
    }

    public void OptionPanelHide()
    {
        OptionPanel.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToStageSelect()
    {
        SceneManager.LoadScene(0);
    }
}
