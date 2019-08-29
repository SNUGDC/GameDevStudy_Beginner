using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeBlock
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector3Int> Temp1()
    {
        List<Vector3Int> temp = new List<Vector3Int>();
        temp.Add(new Vector3Int(1, 0, 0));
        temp.Add(new Vector3Int(1, 0, 1));
        temp.Add(new Vector3Int(1, 0, -1));
        temp.Add(new Vector3Int(1, 0, -2));
        temp.Add(new Vector3Int(1, 1, 0));
        temp.Add(new Vector3Int(0, 1, 0));
        temp.Add(new Vector3Int(1, -1, -1));
        return temp;
    }

}
