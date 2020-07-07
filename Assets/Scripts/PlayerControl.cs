using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Reference to keep track of the falling block
    GameObject block1;
    GameObject block2;

    //Bounds
    public Transform leftWall1;
    public Transform rightWall1;
    public Transform leftWall2;
    public Transform rightWall2;

    // Update is called once per frame
    void Update()
    {
        block1 = BoardManager.Instance.fallingBlocks[0];
        block2 = BoardManager.Instance.fallingBlocks[1];
        
        //Player 1 Control: JKL - Z
        if (block1 != null)
        {
            //Change place of the cubes in the falling block
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Vector3 temp = new Vector3();
                temp = block1.GetComponent<Block>().cubes[1].localPosition;

                if (block1.GetComponent<Block>().type == 2)
                {
                    block1.GetComponent<Block>().cubes[1].localPosition = block1.GetComponent<Block>().cubes[2].localPosition;
                    block1.GetComponent<Block>().cubes[2].localPosition = temp;
                }
                else if (block1.GetComponent<Block>().type == 3)
                {
                    block1.GetComponent<Block>().cubes[1].localPosition = block1.GetComponent<Block>().cubes[2].localPosition;
                    block1.GetComponent<Block>().cubes[2].localPosition = block1.GetComponent<Block>().cubes[3].localPosition;
                    block1.GetComponent<Block>().cubes[3].localPosition = temp;
                }
            }

            //Move the falling block to the left
            if (Input.GetKeyDown(KeyCode.J) && !block1.GetComponent<Block>().leftCol)
            {
                if (block1.transform.position.x > leftWall1.position.x)
                {
                    block1.transform.position -= new Vector3(0.5f, 0, 0);
                }
            }
            //Move the falling block to the right
            if (Input.GetKeyDown(KeyCode.L) && !block1.GetComponent<Block>().rightCol)
            {
                if (block1.transform.position.x < rightWall1.position.x)
                {
                    block1.transform.position += new Vector3(0.5f, 0, 0);
                }
            }

            //Make the block fall faster
            if (Input.GetKeyDown(KeyCode.K))
            {
                block1.GetComponent<Block>().timer = 0;
                block1.GetComponent<Block>().timeStep = 0.05f;
            }
            //The block falls at normal speed
            else if (Input.GetKeyUp(KeyCode.K))
            {
                block1.GetComponent<Block>().timer = 0.5f;
                block1.GetComponent<Block>().timeStep = 0.5f;
            }
        }

        //Player 2 Control: Num4 Num5 Num6 - End
        if (block2 != null)
        {
            //Change place of the cubes in the falling block
            if (Input.GetKeyDown(KeyCode.End))
            {
                Vector3 temp = new Vector3();
                temp = block2.GetComponent<Block>().cubes[1].localPosition;

                if (block2.GetComponent<Block>().type == 2)
                {
                    block2.GetComponent<Block>().cubes[1].localPosition = block2.GetComponent<Block>().cubes[2].localPosition;
                    block2.GetComponent<Block>().cubes[2].localPosition = temp;
                }
                else if (block2.GetComponent<Block>().type == 3)
                {
                    block2.GetComponent<Block>().cubes[1].localPosition = block2.GetComponent<Block>().cubes[2].localPosition;
                    block2.GetComponent<Block>().cubes[2].localPosition = block2.GetComponent<Block>().cubes[3].localPosition;
                    block2.GetComponent<Block>().cubes[3].localPosition = temp;
                }
            }

            //Move the falling block to the left
            if (Input.GetKeyDown(KeyCode.Keypad4) && !block2.GetComponent<Block>().leftCol)
            {
                if (block2.transform.position.x > leftWall2.position.x)
                {
                    block2.transform.position -= new Vector3(0.5f, 0, 0);
                }
            }
            //Move the falling block to the right
            if (Input.GetKeyDown(KeyCode.Keypad6) && !block2.GetComponent<Block>().rightCol)
            {
                if (block2.transform.position.x < rightWall2.position.x)
                {
                    block2.transform.position += new Vector3(0.5f, 0, 0);
                }
            }

            //Make the block fall faster
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                block2.GetComponent<Block>().timer = 0;
                block2.GetComponent<Block>().timeStep = 0.05f;
            }
            //The block falls at normal speed
            else if (Input.GetKeyUp(KeyCode.Keypad5))
            {
                block2.GetComponent<Block>().timer = 0.5f;
                block2.GetComponent<Block>().timeStep = 0.5f;
            }
        }
    }
}
