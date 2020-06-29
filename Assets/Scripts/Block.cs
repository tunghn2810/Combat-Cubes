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
    [SerializeField]
    Transform[] cubes;

    public Transform[] Cubes { get => cubes; set => cubes = value; }

    // Start is called before the first frame update
    void Start()
    {
        timer = timeStep;
        delayTimer = delay;
        cubes = GetComponentsInChildren<Transform>();
    }
    
    //Make the block fall after spawning
    public void Fall()
    {
        timer -= Time.deltaTime;

        //Check for collision below the block
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position + downOffset, Vector2.down, 0.05f);

        if (cubes.Length > 1)
        {
            leftCol = cubes[1].GetComponent<Cube>().LeftCheck();
            rightCol = cubes[1].GetComponent<Cube>().RightCheck();
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
                    if (cube.gameObject != gameObject)
                    {
                        cube.parent = null;
                        if (gameObject.name.Contains("Temp"))
                        {
                            BoardManager.BoardMng.matchChecking = true;
                        }
                        else
                        {
                            BoardManager.BoardMng.cubes.Add(cube.gameObject);
                            BoardManager.BoardMng.matchChecking = true;
                        }
                    }
                }

                Destroy(gameObject);
            }
        }

        //if (timer <= 0)
        //{
        //    //If there is nothing below the block, continue falling
        //    if (!hitInfo.collider)
        //    {
        //        //Falling
        //        gameObject.transform.position -= new Vector3(0, fallStep, 0);
        //
        //        timer = timeStep;
        //    }
        //    else //If there is something below, stop falling
        //    {
        //        foreach (Transform cube in GetComponentsInChildren<Transform>())
        //        {
        //            if (cube.gameObject != gameObject)
        //            {
        //                cube.parent = null;
        //                if (gameObject.name.Contains("Temp"))
        //                {
        //                    BoardManager.BoardMng.matchChecking = true;
        //                }
        //                else
        //                {
        //                    BoardManager.BoardMng.cubes.Add(cube.gameObject);
        //                    BoardManager.BoardMng.matchChecking = true;
        //                }
        //            }
        //        }
        //
        //        Destroy(gameObject);
        //    }
        //}
    }
}
