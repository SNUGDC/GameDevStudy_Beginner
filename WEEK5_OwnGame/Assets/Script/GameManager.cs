using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Blocks
{
    public List<Vector2> block;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public int boardSize;
    public GameObject tilePrefab;
    public GameObject blockPrefab;
    public List<Sprite> blockSprite;
    public List<Blocks> type;

    [HideInInspector] public GameObject holdingBlock;
    private GameObject[,] tilesObject;
    private int[,] tilesState; //0이면 아무것도 없는 상태, 1이면 뭔가 있는 상태

    private Vector2[] BlockPositionArray = new Vector2[] {new Vector2(-8,8), new Vector2(0,8), new Vector2(8,8), new Vector2(-8,0), new Vector2(8,0), new Vector2(-8,-8), new Vector2(0,-8), new Vector2(8,-8)};
    private bool[,] Blow;
    private int ScoreVariable=0;
    private int ScoreInt = 0;
    private float TimeVariable=10f;
    private bool MusicBool=false;
    private GameObject BlockParent;

    public Button SettingsButton;
    public Button MusicButton;
    public Canvas SettingsCanvas;
    public GameObject BlockParentPrefab;

    public AudioClip BGMClip;
    public AudioClip LoseClip;
    public AudioSource myAudio;
    public Text ScoreText;
    public Text TimeText;
    public Button GameOverButton;
       

    private void Awake()
    {
        //static 변수를 선언
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GameOverButton.transform.localScale = new Vector2(0, 0);
        BlockParent = Instantiate(BlockParentPrefab);
        CreateBoard();
        for (int i = 0; i < 8; i++)
        {
            CreateBlock(i);
        }
        Blow = new bool[2,boardSize];
        for(int i = 0; i < boardSize; i++)
        {
            Blow[0, i] = false;
            Blow[1, i] = false;
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < 8; i++)
            {
                CreateBlock(i);
            }
        }
        for (int i = 0; i < boardSize; i++)
        {
            Blow[0, i] = true;
            Blow[1, i] = true;
        }
        for (int i = 0; i < boardSize; i++)
        {
            for(int j = 0; j < boardSize; j++)
            {
                if (tilesState[i,j]==0) Blow[1, j] = false;
            }
        }
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (tilesState[j, i] == 0)  Blow[0, j] = false;
            }
        }
        for (int i = 0; i < boardSize; i++)
        {
            if (Blow[0, i])
            {
                ScoreVariable++;
                for(int j = 0; j < boardSize; j++)
                {
                    tilesObject[i, j].GetComponent<SpriteRenderer>().sprite = tilePrefab.GetComponent<SpriteRenderer>().sprite;
                    tilesState[i, j] = 0;
                }
            }
            if (Blow[1, i])
            {
                ScoreVariable++;
                for (int j = 0; j < boardSize; j++)
                {
                    tilesObject[j, i].GetComponent<SpriteRenderer>().sprite = tilePrefab.GetComponent<SpriteRenderer>().sprite;
                    tilesState[j, i] = 0;
                }
            }
        }
        ScoreInt += ScoreVariable* ScoreVariable * 100;
        ScoreText.text = ScoreInt.ToString();
        ScoreVariable=0;
        TimeText.text = Mathf.RoundToInt(TimeVariable).ToString();
        if (TimeVariable > 0) TimeVariable -= Time.deltaTime;
        if (TimeVariable < 0) GameOver();
    }

    private void CreateBlock(int i)
    {
        int randomType = Random.Range(0, type.Count);
        int randomColor = Random.Range(0, blockSprite.Count);

        GameObject blocks = CreateBlock(randomType, randomColor);
        blocks.transform.position = BlockPositionArray[i];
        blocks.name = i.ToString();
        blocks.transform.SetParent(BlockParent.GetComponent<Transform>());
        //blocks.transform.Rotate(0, 0, Random.Range(0, 4)*90);
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
                tilesObject[i, j].name = i.ToString() + "+" + j.ToString();
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
        //Debug.Log(System.Int32.Parse(holdingBlock.name));
        CreateBlock(System.Int32.Parse(holdingBlock.name));
        Destroy(holdingBlock);
        holdingBlock = null;
    }
    public void SettingsButtonFunction()
    {
        SettingsCanvas.transform.localScale = new Vector3(1- SettingsCanvas.transform.localScale.x, 1- SettingsCanvas.transform.localScale.y, 1);
    }
    public void MusicButtonFunction()
    {
        if (MusicBool)
        {
            myAudio.Stop();
            MusicBool = false;
        }
        else
        {
            myAudio.PlayOneShot(BGMClip);
            MusicBool = true;
        }

    }
    void GameOver()
    {
        myAudio.Stop();
        myAudio.PlayOneShot(LoseClip);
        TimeVariable = 0;
        GameOverButton.transform.localScale = new Vector2(0.1f, 0.1f);
        Destroy(BlockParent);
    }
    public void GameOverButtonFunction()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    
}
