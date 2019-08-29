using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DottedLine { }

public class drawLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DottedLine.DottedLine.Instance.DrawDottedLine(new Vector3(2.5f, 0, 2.5f), new Vector3(2.5f, 12.5f, 2.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
