using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    //Type of the cube
    string type;

    //Keep track of whether the cube is matched with others
    public bool gotMatched = false;

    //Keep track so that the cube only check for collision once
    public bool didCollisionCheck = false;

    //Keep track so that the cube only match check once
    public bool didMatchCheck = false;
    
    //Offsets for collision detection
    Vector3 horzOffset = new Vector3(0.251f, 0, 0);
    Vector3 vertOffset = new Vector3(0, 0.251f, 0);

    public GameObject firstBlock;

    //Keep track of adjacent cubes
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public GameObject upLeft;
    public GameObject upRight;
    public GameObject downLeft;
    public GameObject downRight;

    //Check when there are 3 adjacent cubes in a row
    public bool bingoHorz = false;
    public bool bingoVert = false;
    public bool bingoBslash = false;
    public bool bingoFslash = false;

    //Check for collisions below the cube
    public bool FloorCheck()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position - vertOffset, Vector2.down, 0.05f);
        if (hitInfo.collider)
        {
            return true;
        }
        return false;
    }

    //Check for collisions to the left of the cube
    public bool LeftCheck()
    {
        RaycastHit2D hitInfo1 = Physics2D.Raycast(transform.position - horzOffset + vertOffset / 2, Vector2.left, 0.05f);
        RaycastHit2D hitInfo2 = Physics2D.Raycast(transform.position - horzOffset - vertOffset / 2, Vector2.left, 0.05f);
        if (hitInfo1.collider || hitInfo2.collider)
        {
            return true;
        }
        return false;
    }

    //Check for collisions to the left of the cube
    public bool RightCheck()
    {
        RaycastHit2D hitInfo1 = Physics2D.Raycast(transform.position + horzOffset + vertOffset / 2, Vector2.right, 0.05f);
        RaycastHit2D hitInfo2 = Physics2D.Raycast(transform.position + horzOffset - vertOffset / 2, Vector2.right, 0.05f);
        if (hitInfo1.collider || hitInfo2.collider)
        {
            return true;
        }
        return false;
    }

    void OnDestroy()
    {
        BoardManager.BoardMng.toBeDestroyed.Remove(gameObject);
        BoardManager.BoardMng.cubes.Remove(gameObject);
    }
}