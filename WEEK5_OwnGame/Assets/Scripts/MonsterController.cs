using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    /// <summary>
    /// True일 경우 요괴가 수평 방향으로 움직임.
    /// </summary>
    public bool isHorizontal;
    public float speed;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += MoveDir() * speed * Time.deltaTime;
    }

    private Vector3 MoveDir()
    {
        if (isHorizontal) return new Vector2(-1, 0);
        else return new Vector2(0, -1);
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        Debug.Log(coll.transform.localPosition);
    }
}
