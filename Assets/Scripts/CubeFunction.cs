using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeFunction
{
    //Offsets for collision detection
    static Vector3 horzOffset = new Vector3(0.251f, 0, 0);
    static Vector3 vertOffset = new Vector3(0, 0.251f, 0);

    //Check for collisions with other cubes
    public static void CollisionCheck(GameObject cube, int playerNum)
    {
        Cube cubeObj = cube.GetComponent<Cube>();

        //4 Cases: Horizontal, Vertical, Diagonal - Backslash, Diagonal - Forward slash
        //Horizontal - Left
        RaycastHit2D leftInfo = Physics2D.Raycast(cube.transform.position - horzOffset, Vector2.left, 0.05f);
        if (leftInfo && leftInfo.transform.gameObject.CompareTag(cube.tag))
        {
            cubeObj.left = leftInfo.transform.gameObject;
        }

        //Horizontal - Right
        RaycastHit2D rightInfo = Physics2D.Raycast(cube.transform.position + horzOffset, Vector2.right, 0.05f);
        if (rightInfo && rightInfo.transform.gameObject.CompareTag(cube.tag))
        {
            cubeObj.right = rightInfo.transform.gameObject;
        }
        //Check for 3 horizontal adjacents
        if (cubeObj.left && cubeObj.right)
        {
            cubeObj.bingoHorz = true;
        }

        //Vertical - Up
        RaycastHit2D upInfo = Physics2D.Raycast(cube.transform.position + vertOffset, Vector2.up, 0.05f);
        if (upInfo && upInfo.transform.gameObject.CompareTag(cube.tag))
        {
            cubeObj.up = upInfo.transform.gameObject;
        }
        //Vertical - Down
        RaycastHit2D downInfo = Physics2D.Raycast(cube.transform.position - vertOffset, Vector2.down, 0.05f);
        if (downInfo && downInfo.transform.gameObject.CompareTag(cube.tag))
        {
            cubeObj.down = downInfo.transform.gameObject;
        }
        //Check for 3 vertical adjacents
        if (cubeObj.up && cubeObj.down)
        {
            cubeObj.bingoVert = true;
        }

        //Diagonal - Backslash - Up Left
        RaycastHit2D upLeftInfo = Physics2D.Raycast(cube.transform.position + vertOffset - horzOffset, Vector2.up + Vector2.left, 0.05f);
        if (upLeftInfo && upLeftInfo.transform.gameObject.CompareTag(cube.tag))
        {
            cubeObj.upLeft = upLeftInfo.transform.gameObject;
        }
        //Diagonal - Backslash - Down Right
        RaycastHit2D downRightInfo = Physics2D.Raycast(cube.transform.position - vertOffset + horzOffset, Vector2.down + Vector2.right, 0.05f);
        if (downRightInfo && downRightInfo.transform.gameObject.CompareTag(cube.tag))
        {
            cubeObj.downRight = downRightInfo.transform.gameObject;
        }
        //Check for 3 backslash diagonal adjacents
        if (cubeObj.upLeft && cubeObj.downRight)
        {
            cubeObj.bingoBslash = true;
        }

        //Diagonal - Forward slash - Up Right
        RaycastHit2D upRightInfo = Physics2D.Raycast(cube.transform.position + vertOffset + horzOffset, Vector2.up + Vector2.right, 0.05f);
        if (upRightInfo && upRightInfo.transform.gameObject.CompareTag(cube.tag))
        {
            cubeObj.upRight = upRightInfo.transform.gameObject;
        }
        //Diagonal - Forward slash - Down Left
        RaycastHit2D downLeftInfo = Physics2D.Raycast(cube.transform.position - vertOffset - horzOffset, Vector2.down + Vector2.left, 0.05f);
        if (downLeftInfo && downLeftInfo.transform.gameObject.CompareTag(cube.tag))
        {
            cubeObj.downLeft = downLeftInfo.transform.gameObject;
        }
        //Check for 3 backslash diagonal adjacents
        if (cubeObj.upRight && cubeObj.downLeft)
        {
            cubeObj.bingoFslash = true;
        }

        cubeObj.didCollisionCheck = true;

        if (cubeObj.bingoHorz || cubeObj.bingoVert || cubeObj.bingoBslash || cubeObj.bingoFslash)
        {
            cubeObj.gotMatched = true;
            BoardManager.Instance.matcheds[playerNum] = true;

            if (!BoardManager.Instance.toBeDestroyeds[playerNum].Contains(cube))
            {
                BoardManager.Instance.toBeDestroyeds[playerNum].Add(cube);
            }
            //Matched(cube);
        }
    }

    //Check for collisions below the cube
    public static bool FloorCheck(GameObject cube)
    {
        Cube cubeObj = cube.GetComponent<Cube>();

        RaycastHit2D hitInfo = Physics2D.Raycast(cube.transform.position - vertOffset, Vector2.down, 0.05f);
        if (hitInfo.collider)
        {
            return true;
        }
        return false;
    }

    //Check for collisions to the left of the cube
    public static bool LeftCheck(GameObject cube)
    {
        Cube cubeObj = cube.GetComponent<Cube>();

        RaycastHit2D hitInfo1 = Physics2D.Raycast(cube.transform.position - horzOffset + vertOffset / 2, Vector2.left, 0.05f);
        RaycastHit2D hitInfo2 = Physics2D.Raycast(cube.transform.position - horzOffset - vertOffset / 2, Vector2.left, 0.05f);
        if (hitInfo1.collider || hitInfo2.collider)
        {
            return true;
        }
        return false;
    }

    //Check for collisions to the left of the cube
    public static bool RightCheck(GameObject cube)
    {
        RaycastHit2D hitInfo1 = Physics2D.Raycast(cube.transform.position + horzOffset + vertOffset / 2, Vector2.right, 0.05f);
        RaycastHit2D hitInfo2 = Physics2D.Raycast(cube.transform.position + horzOffset - vertOffset / 2, Vector2.right, 0.05f);
        if (hitInfo1.collider || hitInfo2.collider)
        {
            return true;
        }
        return false;
    }

    //Add matched cubes to be destroyed to a list
    public static void Matched(GameObject cube, int playerNum)
    {
        Cube cubeObj = cube.GetComponent<Cube>();

        //Matched horizontally
        if (cubeObj.bingoHorz)
        {
            if (cubeObj.left && cubeObj.left.CompareTag(cube.tag))
            {
                if (!BoardManager.Instance.toBeDestroyeds[playerNum].Contains(cubeObj.left))
                {
                    BoardManager.Instance.toBeDestroyeds[playerNum].Add(cubeObj.left);
                }
                cubeObj.left.GetComponent<Cube>().bingoHorz = true;
            }
            if (cubeObj.right && cubeObj.right.CompareTag(cube.tag))
            {
                if (!BoardManager.Instance.toBeDestroyeds[playerNum].Contains(cubeObj.right))
                {
                    BoardManager.Instance.toBeDestroyeds[playerNum].Add(cubeObj.right);
                }
                cubeObj.right.GetComponent<Cube>().bingoHorz = true;
            }
        }

        //Matched vertically
        if (cubeObj.bingoVert)
        {
            if (cubeObj.up && cubeObj.up.CompareTag(cube.tag))
            {
                if (!BoardManager.Instance.toBeDestroyeds[playerNum].Contains(cubeObj.up))
                {
                    BoardManager.Instance.toBeDestroyeds[playerNum].Add(cubeObj.up);
                }
                cubeObj.up.GetComponent<Cube>().bingoVert = true;
            }
            if (cubeObj.down && cubeObj.down.CompareTag(cube.tag))
            {
                if (!BoardManager.Instance.toBeDestroyeds[playerNum].Contains(cubeObj.down))
                {
                    BoardManager.Instance.toBeDestroyeds[playerNum].Add(cubeObj.down);
                }
                cubeObj.down.GetComponent<Cube>().bingoVert = true;
            }
        }

        //Matched diagonally - Backlash
        if (cubeObj.bingoBslash)
        {
            if (cubeObj.upLeft && cubeObj.upLeft.CompareTag(cube.tag))
            {
                if (!BoardManager.Instance.toBeDestroyeds[playerNum].Contains(cubeObj.upLeft))
                {
                    BoardManager.Instance.toBeDestroyeds[playerNum].Add(cubeObj.upLeft);
                }
                cubeObj.upLeft.GetComponent<Cube>().bingoBslash = true;
            }
            if (cubeObj.downRight && cubeObj.downRight.CompareTag(cube.tag))
            {
                if (!BoardManager.Instance.toBeDestroyeds[playerNum].Contains(cubeObj.downRight))
                {
                    BoardManager.Instance.toBeDestroyeds[playerNum].Add(cubeObj.downRight);
                }
                cubeObj.downRight.GetComponent<Cube>().bingoBslash = true;
            }
        }

        //Matched diagonally - Forward slash
        if (cubeObj.bingoFslash)
        {
            if (cubeObj.upRight && cubeObj.upRight.CompareTag(cube.tag))
            {
                if (!BoardManager.Instance.toBeDestroyeds[playerNum].Contains(cubeObj.upRight))
                {
                    BoardManager.Instance.toBeDestroyeds[playerNum].Add(cubeObj.upRight);
                }
                cubeObj.upRight.GetComponent<Cube>().bingoFslash = true;
            }
            if (cubeObj.downLeft && cubeObj.downLeft.CompareTag(cube.tag))
            {
                if (!BoardManager.Instance.toBeDestroyeds[playerNum].Contains(cubeObj.downLeft))
                {
                    BoardManager.Instance.toBeDestroyeds[playerNum].Add(cubeObj.downLeft);
                }
                cubeObj.downLeft.GetComponent<Cube>().bingoFslash = true;
            }
        }

        cubeObj.didMatchCheck = true;
    }

    public static void BlinkAnimation(GameObject cube)
    {
        cube.GetComponent<Animator>().SetBool("isMatched", true);
    }

    //Combine the cubes above the destroyed cube to move them down
    public static void BlockCombine(GameObject cube, int playerNum)
    {
        Cube cubeObj = cube.GetComponent<Cube>();

        //Check if there is a block directly above
        RaycastHit2D firstBlock = Physics2D.Raycast(cube.transform.position + vertOffset, Vector2.up, 6.0f, LayerMask.GetMask("Cubes"));

        //Make the cube 'disappear'
        cube.GetComponent<Animator>().SetBool("isMatched", false);
        cube.GetComponent<SpriteRenderer>().enabled = false;
        cube.GetComponent<SpriteRenderer>().enabled = false;
        cube.GetComponent<SpriteRenderer>().enabled = false;
        cube.GetComponent<Collider2D>().enabled = false;

        //If there is at least one cube above, group them up into a block
        if (firstBlock)
        {
            bool proceed = false;

            if (cubeObj.bingoVert)
            {
                if (!firstBlock.transform.CompareTag(cube.tag))
                {
                    proceed = true;
                }
            }
            else
            {
                proceed = true;
            }

            if (proceed)
            {
                RaycastHit2D[] movingBlocks = Physics2D.RaycastAll(cube.transform.position + vertOffset, Vector2.up, 6.0f, LayerMask.GetMask("Cubes"));

                GameObject tempBlock = new GameObject("TempBlock");
                if (playerNum == 0)
                {
                    tempBlock.tag = "TempBlock0";
                }
                else
                {
                    tempBlock.tag = "TempBlock1";
                }

                tempBlock.transform.position = firstBlock.transform.position;

                foreach (RaycastHit2D item in movingBlocks)
                {
                    item.transform.parent = tempBlock.transform;

                    //Test cube falling with Rigidbody2D instead of manually
                    //item.transform.gameObject.AddComponent<Rigidbody2D>();
                    //item.transform.gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                    //item.transform.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                }

                //Block falls almost instantly to the bottom
                tempBlock.AddComponent<Block>();
                tempBlock.GetComponent<Block>().timeStep = 0.05f;
                tempBlock.GetComponent<Block>().timer = 0.05f;
                tempBlock.GetComponent<Block>().InitalizeBlock();
            }
        }

        Object.Destroy(cube, 0.5f);
    }

    //Clear all reference to adjacent cubes to check for collision again
    public static void ClearAdjacents(GameObject cube)
    {
        Cube cubeObj = cube.GetComponent<Cube>();

        cubeObj.up = null;
        cubeObj.down = null;
        cubeObj.left = null;
        cubeObj.right = null;
        cubeObj.upLeft = null;
        cubeObj.upRight = null;
        cubeObj.downLeft = null;
        cubeObj.downRight = null;
    }
}
