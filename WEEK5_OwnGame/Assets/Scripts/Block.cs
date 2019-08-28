using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //5*5 3차원 그리드상
    static int frame = 3;
    List<Vector3> bl3;
    List<GameObject> block;
    public GameObject dim3,dim2;
    public GameObject cube, square;
    GameObject coor;
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

        coor = new GameObject();

        bl3 = new List<Vector3>();
        bl3.Add(new Vector3Int(1, 0, 0));
        bl3.Add(new Vector3Int(1, 0, 1));
        bl3.Add(new Vector3Int(1, 0, -1));
        bl3.Add(new Vector3Int(1, 0, -2));
        bl3.Add(new Vector3Int(1, 1, 0));
        bl3.Add(new Vector3Int(0, 1, 0));
        bl3.Add(new Vector3Int(1, -1, -1));
        

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
            Move();
        }
    }

    void Project()
    {
        Transform coo = dim3.transform;
        if (dim2.transform.childCount > 0)
        {
            foreach (Transform x in dim2.transform)
            {
                Destroy(x.gameObject);
            }
        }
        for (int i = 0; i < coo.childCount; i++)
        {
            GameObject temp = Instantiate(square);
            temp.transform.SetParent(dim2.transform);
            temp.transform.position += new Vector3(coo.GetChild(i).position.x,0, coo.GetChild(i).position.z);
        }
    }

    void Rotate()
    {
        coor.transform.rotation = dim3.transform.rotation;
        if (!isSpin)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isSpin = true;
                coor.transform.Rotate(Y, Space.World);
                pos = coor.transform.rotation;
                V = Y;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                isSpin = true;
                coor.transform.Rotate(X, Space.World);
                pos = coor.transform.rotation;
                V = X;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                isSpin = true;
                coor.transform.Rotate(Z, Space.World);
                pos = coor.transform.rotation;
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
    }

    Vector3Int Direction()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) return new Vector3Int(1, 0, 0);
        else if (Input.GetKeyDown(KeyCode.DownArrow)) return new Vector3Int(-1, 0, 0);
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) return new Vector3Int(0, 0, 1);
        else if (Input.GetKeyDown(KeyCode.RightArrow)) return new Vector3Int(0, 0, -1);
        else return new Vector3Int(0, 0, 0);
    }

}
