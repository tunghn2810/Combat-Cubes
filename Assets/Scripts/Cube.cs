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

    void OnDestroy()
    {
        if (gameObject.name.Contains("0"))
        {
            BoardManager.Instance.toBeDestroyeds[0].Remove(gameObject);
            BoardManager.Instance.cubes[0].Remove(gameObject);
        }
        else
        {
            BoardManager.Instance.toBeDestroyeds[1].Remove(gameObject);
            BoardManager.Instance.cubes[1].Remove(gameObject);
        }
    }
}