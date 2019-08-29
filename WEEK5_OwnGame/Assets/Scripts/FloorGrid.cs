using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGrid : MonoBehaviour
{
    static int m = 23, n = 23;
    // 23*23 그리드 사용 예정
    // Start is called before the first frame update
    GameObject[,] table;//true일 때 비어있음 + 채워넣기 가능
    public GameObject board, tile;
    List<Vector3Int> shape;
    Template template;
    bool[,] temp;
    void Start()
    {
        template = new Template();
        temp = new bool[m, n];
        newTable();
        setTable();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        temp = template.Temp1();
        for (int j = 0; j < m; j++)
        {
            for (int i = 0; i < n; i++)
            {
                if (temp[i, j])
                {
                    table[i, j].transform.GetChild(0).gameObject.SetActive(false);
                    table[i, j].transform.GetChild(1).gameObject.SetActive(false);
                }
                if (!temp[i, j])
                {
                    table[i, j].transform.GetChild(0).gameObject.SetActive(true);
                    table[i, j].transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }


}
