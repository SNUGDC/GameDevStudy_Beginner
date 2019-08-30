using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    static int m = 23, n = 23;     // 타일 상태 0 = 벽, 1 = 구멍, 2 = 채워짐
    static float wid = 0.1f;

    int total = 0, need = 0, fill = 0;          
    GameObject[,] table;
    public GameObject board, tile, line;
    public Material mat;
    public Block block;

    List<Vector3Int> blockCoord;
    Template template;
    int[,] tileState;

    Stack<List<int[]>> moveIndex;
    Stack<Queue<GameObject>> outlines;


    void Start()
    {
        Debug.Log("Press A, S, D to rotate, Press Space to place, Press LeftShift to undo");

        template = new Template();
        tileState = new int[m, n];
        moveIndex = new Stack<List<int[]>>();
        outlines = new Stack<Queue<GameObject>>();

        newTable();
        setTable();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Place();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && moveIndex.Count > 0)
        {
            Back();
        }
    }

    void newTable()
    {
        table = new GameObject[m, n];
        for(int i = 0; i < m; i++)
        {
            for(int j = 0; j < n; j++)
            {
                table[i, j] = Instantiate(tile);
                table[i, j].transform.SetParent(board.transform);
                table[i, j].transform.position = new Vector3Int(i-11, 0, j-11);
            }
        }
    }

    void setTable()
    {
        tileState = template.Temp1();
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (tileState[i, j] == 0)
                {
                    table[i, j].transform.GetChild(0).gameObject.SetActive(true);
                }
                else need++;
            }
        }
    }

    void Place()
    {
        if (Check())
        {
            List<int[]> blockIndex = new List<int[]>();
            for (int i = 0; i < blockCoord.Count; i++)
            {
                int x = blockCoord[i].x + 11, z = blockCoord[i].z + 11;
                blockIndex.Add(new int[] {x,z});
                table[x,z].transform.GetChild(1).gameObject.SetActive(true);
                tileState[x, z] = 2;
                fill++;
            }
            moveIndex.Push(blockIndex);
            total++;
            Outline(blockIndex);
        }
    }
    bool Check()
    {
        blockCoord = block.bl2;
        bool able = true;
        for(int i = 0; i < blockCoord.Count; i++)
        {
            int state = tileState[blockCoord[i].x + 11, blockCoord[i].z + 11];
            if (state != 1)
            {
                able = false;
                break;
            }
        }
        return able;
    }
    void Outline(List<int[]> index)
    {
        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < index.Count; i++)
        {
            List<Vector3Int> vecNear = new List<Vector3Int>();
            vecNear.Add(new Vector3Int(1, 0, 0));
            vecNear.Add(new Vector3Int(-1, 0, 0));
            vecNear.Add(new Vector3Int(0, 0, 1));
            vecNear.Add(new Vector3Int(0, 0, -1));

            List<Vector3Int> vecLine = new List<Vector3Int>();
            vecLine.Add(new Vector3Int(0, 0, 1));
            vecLine.Add(new Vector3Int(1, 0, 0));

            for (int n = 0; n < 4; n++)
            {
                bool near = false;
                for (int j = 0; j < index.Count; j++)
                {
                    if (Vector2Int.Equals(blockCoord[i] + vecNear[n], blockCoord[j]))
                    {
                        near = true;
                        break;
                    }
                }
                if (!near)
                {
                    GameObject obj = new GameObject();
                    obj.transform.SetParent(line.transform);
                    LineRenderer lr = obj.AddComponent<LineRenderer>();
                    lr.material = mat;
                    lr.startWidth = wid;
                    lr.endWidth = wid;
                    Vector3 vert = new Vector3(Vector3.Dot(vecNear[n], vecLine[0]), 0, Vector3.Dot(vecNear[n], vecLine[1]));
                    lr.SetPositions(new Vector3[] { blockCoord[i] + 0.55f * vert + 0.5f * (Vector3)vecNear[n] + 0.05f * Vector3.up, blockCoord[i] - 0.55f * vert + 0.5f * (Vector3)vecNear[n] + 0.05f * Vector3.up });
                    queue.Enqueue(obj);
                }
            }
            /*
         for (int j = 0; j < index.Count; j++)
            {
                if (index[j][0] == index[i][0] && index[j][1] == index[i][1] - 1)
                {
                    b4 = false;
                    break;
                }
            }
            if (b4)
            {
                GameObject obj = new GameObject();
                obj.transform.SetParent(line.transform);
                LineRenderer lr = obj.AddComponent<LineRenderer>();
                lr.material = mat;
                lr.startWidth = wid;
                lr.endWidth = wid;
                lr.SetPositions(new Vector3[] { (Vector3)blockCoord[i] + new Vector3(0.5f, 0, -0.5f), (Vector3)blockCoord[i] + new Vector3(-0.5f, 0, -0.5f) });
                queue.Enqueue(obj);
            }    
         */
        }
        outlines.Push(queue);
    }

    void Back()
    {
        List<int[]> blockIndex = moveIndex.Pop();
        for (int i = 0; i < blockIndex.Count ; i++)
        {
            int x = blockIndex[i][0], z = blockIndex[i][1];
            table[x, z].transform.GetChild(1).gameObject.SetActive(false);
            tileState[x, z] = 1;
            fill--;
        }
        Queue<GameObject> delLine = outlines.Pop();
        int listLen = delLine.Count;
        for(int i = 0; i < listLen; i++)
        {
            GameObject temp = delLine.Dequeue();
            Destroy(temp);
        }
        total++;
    }
}
