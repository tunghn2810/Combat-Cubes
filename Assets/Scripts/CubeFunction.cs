using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CubeFunction
{
    //Offsets for collision detection
    static Vector3 horzOffset = new Vector3(0.251f, 0, 0);
    static Vector3 vertOffset = new Vector3(0, 0.251f, 0);

    //Check for collisions with other cubes
    public static void CollisionCheck(GameObject cube)
    {
        Cube cubeObj = cube.GetComponent<Cube>();

        //4 Cases: Horizontal, Vertical, Diagonal - Backslash, Diagonal - Forward slash
        //Horizontal - Left
        RaycastHit2D leftInfo = Physics2D.Raycast(cube.transform.position - horzOffset, Vector2.left, 0.05f);
        if (leftInfo && leftInfo.transform.gameObject.tag == cube.tag)
        {
            cubeObj.left = leftInfo.transform.gameObject;
        }

        //Horizontal - Right
        RaycastHit2D rightInfo = Physics2D.Raycast(cube.transform.position + horzOffset, Vector2.right, 0.05f);
        if (rightInfo && rightInfo.transform.gameObject.tag == cube.tag)
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
        if (upInfo && upInfo.transform.gameObject.tag == cube.tag)
        {
            cubeObj.up = upInfo.transform.gameObject;
        }
        //Vertical - Down
        RaycastHit2D downInfo = Physics2D.Raycast(cube.transform.position - vertOffset, Vector2.down, 0.05f);
        if (downInfo && downInfo.transform.gameObject.tag == cube.tag)
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
        if (upLeftInfo && upLeftInfo.transform.gameObject.tag == cube.tag)
        {
            cubeObj.upLeft = upLeftInfo.transform.gameObject;
        }
        //Diagonal - Backslash - Down Right
        RaycastHit2D downRightInfo = Physics2D.Raycast(cube.transform.position - vertOffset + horzOffset, Vector2.down + Vector2.right, 0.05f);
        if (downRightInfo && downRightInfo.transform.gameObject.tag == cube.tag)
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
        if (upRightInfo && upRightInfo.transform.gameObject.tag == cube.tag)
        {
            cubeObj.upRight = upRightInfo.transform.gameObject;
        }
        //Diagonal - Forward slash - Down Left
        RaycastHit2D downLeftInfo = Physics2D.Raycast(cube.transform.position - vertOffset - horzOffset, Vector2.down + Vector2.left, 0.05f);
        if (downLeftInfo && downLeftInfo.transform.gameObject.tag == cube.tag)
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
            BoardManager.BoardMng.matched = true;

            if (!BoardManager.BoardMng.toBeDestroyed.Contains(cube))
            {
                BoardManager.BoardMng.toBeDestroyed.Add(cube);
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
    public static void Matched(GameObject cube)
    {
        Cube cubeObj = cube.GetComponent<Cube>();

        //Matched horizontally
        if (cubeObj.bingoHorz)
        {
            if (cubeObj.left && cubeObj.left.tag == cube.tag)
            {
                if (!BoardManager.BoardMng.toBeDestroyed.Contains(cubeObj.left))
                {
                    BoardManager.BoardMng.toBeDestroyed.Add(cubeObj.left);
                }
                cubeObj.left.GetComponent<Cube>().bingoHorz = true;
                //Matched(cubeObj.left);
            }
            if (cubeObj.right && cubeObj.right.tag == cube.tag)
            {
                if (!BoardManager.BoardMng.toBeDestroyed.Contains(cubeObj.right))
                {
                    BoardManager.BoardMng.toBeDestroyed.Add(cubeObj.right);
                }
                cubeObj.right.GetComponent<Cube>().bingoHorz = true;
                //Matched(cubeObj.right);
            }
        }

        //Matched vertically
        if (cubeObj.bingoVert)
        {
            if (cubeObj.up && cubeObj.up.tag == cube.tag)
            {
                if (!BoardManager.BoardMng.toBeDestroyed.Contains(cubeObj.up))
                {
                    BoardManager.BoardMng.toBeDestroyed.Add(cubeObj.up);
                }
                cubeObj.up.GetComponent<Cube>().bingoVert = true;
                //Matched(cubeObj.up);
            }
            if (cubeObj.down && cubeObj.down.tag == cube.tag)
            {
                if (!BoardManager.BoardMng.toBeDestroyed.Contains(cubeObj.down))
                {
                    BoardManager.BoardMng.toBeDestroyed.Add(cubeObj.down);
                }
                cubeObj.down.GetComponent<Cube>().bingoVert = true;
                //Matched(cubeObj.down);
            }
        }

        //Matched diagonally - Backlash
        if (cubeObj.bingoBslash)
        {
            if (cubeObj.upLeft && cubeObj.upLeft.tag == cube.tag)
            {
                if (!BoardManager.BoardMng.toBeDestroyed.Contains(cubeObj.upLeft))
                {
                    BoardManager.BoardMng.toBeDestroyed.Add(cubeObj.upLeft);
                }
                cubeObj.upLeft.GetComponent<Cube>().bingoBslash = true;
                //Matched(cubeObj.upLeft);
            }
            if (cubeObj.downRight && cubeObj.downRight.tag == cube.tag)
            {
                if (!BoardManager.BoardMng.toBeDestroyed.Contains(cubeObj.downRight))
                {
                    BoardManager.BoardMng.toBeDestroyed.Add(cubeObj.downRight);
                }
                cubeObj.downRight.GetComponent<Cube>().bingoBslash = true;
                //Matched(cubeObj.downRight);
            }
        }

        //Matched diagonally - Forward slash
        if (cubeObj.bingoFslash)
        {
            if (cubeObj.upRight && cubeObj.upRight.tag == cube.tag)
            {
                if (!BoardManager.BoardMng.toBeDestroyed.Contains(cubeObj.upRight))
                {
                    BoardManager.BoardMng.toBeDestroyed.Add(cubeObj.upRight);
                }
                cubeObj.upRight.GetComponent<Cube>().bingoFslash = true;
                //Matched(cubeObj.upRight);
            }
            if (cubeObj.downLeft && cubeObj.downLeft.tag == cube.tag)
            {
                if (!BoardManager.BoardMng.toBeDestroyed.Contains(cubeObj.downLeft))
                {
                    BoardManager.BoardMng.toBeDestroyed.Add(cubeObj.downLeft);
                }
                cubeObj.downLeft.GetComponent<Cube>().bingoFslash = true;
                //Matched(cubeObj.downLeft);
            }
        }

        cubeObj.didMatchCheck = true;
        //BlockCombine(cube);
    }

    //Combine the cubes above the destroyed cube to move them down
    public static void BlockCombine(GameObject cube)
    {
        Cube cubeObj = cube.GetComponent<Cube>();

        //Check if there is a block directly above
        RaycastHit2D firstBlock = Physics2D.Raycast(cube.transform.position + vertOffset, Vector2.up, 6.0f, LayerMask.GetMask("Cubes"));

        //Make the cube 'disappear'
        cube.GetComponent<SpriteRenderer>().enabled = false;
        cube.GetComponent<Collider2D>().enabled = false;

        //If there is at least one cube above, group them up into a block
        if (firstBlock)
        {
            bool proceed = false;

            if (cubeObj.bingoVert)
            {
                if (firstBlock.transform.tag != cube.tag)
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

                GameObject tempBlock = new GameObject("TempBlock ");
                tempBlock.tag = "TempBlock";
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
