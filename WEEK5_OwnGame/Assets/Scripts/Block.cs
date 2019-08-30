using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //5*5 3차원 그리드상
    static int frame = 3;
    public List<Vector3Int> bl3, bl2;
    List<GameObject> block;
    public GameObject dim3,dim2, cube, square, board;
    GameObject point3;
    makeBlock make;
    bool isSpin = false;
    int cd = 0;
    Quaternion pos;
    Vector3Int Y, X, Z;
    Vector3 V;

    // Start is called before the first frame update
    void Start()
    {
        Y = new Vector3Int(0, 90, 0);
        X = new Vector3Int(90, 0, 0);
        Z = new Vector3Int(0, 0, 90);

        make = new makeBlock();
        point3 = new GameObject();
        board.GetComponent<FloorGrid>().block = this;

        bl3 = setBlock();
        
        

        for (int i = 0; i < bl3.Count; i++)
        {
            GameObject temp = Instantiate(cube);
            temp.transform.SetParent(dim3.transform);
            temp.transform.position += bl3[i]+new Vector3(0,10,0);
        }
        Project();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        if (!isSpin){
            Place();
            Move();
        }
    }

    List<Vector3Int> setBlock()
    {
        return make.Temp1();
    }

    void Project()
    {
        Transform up = dim3.transform;
        bl2 = new List<Vector3Int>();
        if (dim2.transform.childCount > 0)
        {
            foreach (Transform x in dim2.transform)
            {
                Destroy(x.gameObject);
            }
        }
        for (int i = 0; i < up.childCount; i++)
        {
            Vector3Int down = Vector3Int.RoundToInt(new Vector3(up.GetChild(i).position.x, 0, up.GetChild(i).position.z));
            if (!bl2.Contains(down))
            {
                GameObject temp = Instantiate(square);
                temp.transform.SetParent(dim2.transform);
                temp.transform.position += down + Vector3.up * 0.1f;
                bl2.Add(down);
            }
        }
    }

    void Rotate()
    {
        point3.transform.rotation = dim3.transform.rotation;
        if (!isSpin)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isSpin = true;
                point3.transform.Rotate(Y, Space.World);
                pos = point3.transform.rotation;
                V = Y;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                isSpin = true;
                point3.transform.Rotate(X, Space.World);
                pos = point3.transform.rotation;
                V = X;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                isSpin = true;
                point3.transform.Rotate(Z, Space.World);
                pos = point3.transform.rotation;
                V = Z;
            }
        }
        if (isSpin)
        {
            dim3.transform.Rotate(V / frame, Space.World);
            cd++;
            if (cd == frame)
            {
                isSpin = false;
                cd = 0;
                dim3.transform.rotation = pos;
                Project();
            }
        }
    }

    void Move()
    {
        Vector3Int Mv = Direction();
        transform.position += Mv;
        for(int i = 0; i < bl2.Count; i++) bl2[i] += Mv;
    }
    Vector3Int Direction()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && transform.position.x < 9) return new Vector3Int(1, 0, 0);
        else if (Input.GetKeyDown(KeyCode.DownArrow) && transform.position.x > -9) return new Vector3Int(-1, 0, 0);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.z < 9) return new Vector3Int(0, 0, 1);
        else if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.z > -9) return new Vector3Int(0, 0, -1);
        else return new Vector3Int(0, 0, 0);
    }

    void Place()
    {

    }
}
