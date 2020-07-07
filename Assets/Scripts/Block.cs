using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //Time for each step the block falls
    [System.NonSerialized] public float timeStep = 0.5f; //0.75
    [System.NonSerialized] public float timer;
    float fallStep = 0.25f; //Fall distance per step

    //Delay before the block is destroyed
    public float delay = 0.5f;
    public float delayTimer;

    //Check if there is anything to the side of the block
    public bool leftCol;
    public bool rightCol;

    //Offset for collision detection
    Vector3 downOffset = new Vector3(0, -0.251f, 0);

    //Type of block
    [System.NonSerialized] public int type;

    //Cubes in the falling block
    public Transform[] cubes;

    //Object for side collision checks
    GameObject sideCol;
    
    public void InitalizeBlock()
    {
        timer = timeStep;
        delayTimer = delay;
        cubes = GetComponentsInChildren<Transform>();

        if (!gameObject.name.Contains("Temp"))
        {
            sideCol = transform.Find("SideCollisionCheck").gameObject;
        }
    }
    
    //Make the block fall after spawning
    public void Fall()
    {
        timer -= Time.deltaTime;

        //Check for collision below the block
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position + downOffset, Vector2.down, 0.05f);
        
        if (cubes.Length > 1 && !gameObject.name.Contains("Temp"))
        {
            leftCol = SideCollisionCheck.LeftCheck(sideCol);
            rightCol = SideCollisionCheck.RightCheck(sideCol);
        }
        
        //If there is nothing below the block, continue falling
        if (!hitInfo.collider)
        {
            delayTimer = delay;
            if (timer <= 0)
            {
                gameObject.transform.position -= new Vector3(0, fallStep, 0);

                timer = timeStep;
            }
        }
        else //If there is something below, stop falling
        {
            timer = timeStep;
            delayTimer -= Time.deltaTime;
            
            if (delayTimer <= 0)
            {
                foreach (Transform cube in GetComponentsInChildren<Transform>())
                {
                    if (cube.gameObject != gameObject && cube.gameObject.name != "SideCollisionCheck")
                    {
                        cube.parent = null;
                        if (gameObject.tag.Contains("Temp"))
                        {
                            if (gameObject.tag.Contains("0"))
                            {
                                BoardManager.Instance.matchCheckings[0] = true;
                            }
                            else
                            {
                                BoardManager.Instance.matchCheckings[1] = true;
                            }
                        }
                        else
                        {
                            if (gameObject.tag.Contains("0"))
                            {
                                BoardManager.Instance.cubes[0].Add(cube.gameObject);
                                BoardManager.Instance.matchCheckings[0] = true;
                            }
                            else
                            {
                                BoardManager.Instance.cubes[1].Add(cube.gameObject);
                                BoardManager.Instance.matchCheckings[1] = true;
                            }
                        }
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}
