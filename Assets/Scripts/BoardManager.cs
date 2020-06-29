using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //Singleton
    public static BoardManager BoardMng { get; set; }
    void Awake()
    {
        if (BoardMng == null)
        {
            BoardMng = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Coroutine
    IEnumerator gameState;

    //Keep track of all cubes except the falling ones
    public List<GameObject> cubes;

    //Keep track of the falling block
    GameObject fallingBlock;

    //Keep track of cubes to be destroyed
    public List<GameObject> toBeDestroyed;

    //Keep track of when the game start
    public bool startGame = false;

    //Check for matching cubes
    [System.NonSerialized] public bool matchChecking = false;
    public bool matched = false;

    // Start is called before the first frame update
    void Start()
    {
        cubes = new List<GameObject>();

        gameState = BlockState();
        StartCoroutine(gameState);
    }

    //State when a block is created
    IEnumerator BlockState()
    {
        while (GameObject.FindWithTag("MainBlock") == null)
        {
            fallingBlock = BlockGenerator.BlockGen.CreateBlock();
            yield return null;
        }
        StopCoroutine(gameState);
        gameState = FallState();
        StartCoroutine(gameState);
    }

    //State when a block is falling
    IEnumerator FallState()
    {
        while (fallingBlock)
        {
            fallingBlock.GetComponent<Block>().Fall();
            yield return null;
        }

        while (GameObject.FindGameObjectsWithTag("TempBlock").Length > 0)
        {
            foreach (GameObject block in GameObject.FindGameObjectsWithTag("TempBlock"))
            {
                block.GetComponent<Block>().Fall();
            }
            yield return null;
        }

        StopCoroutine(gameState);
        gameState = CollisionState();
        StartCoroutine(gameState);
    }

    //State when there are matching cubes
    IEnumerator CollisionState()
    {
        while (matchChecking)
        {
            foreach (GameObject cube in cubes)
            {
                CubeFunction.ClearAdjacents(cube);
                if (!cube.GetComponent<Cube>().didCollisionCheck && !cube.GetComponent<Cube>().gotMatched)
                {
                    CubeFunction.CollisionCheck(cube);
                }
            }

            matchChecking = false;
            yield return null;
        }

        foreach (GameObject cube in cubes)
        {
            cube.GetComponent<Cube>().didCollisionCheck = false;
        }

        if (matched)
        {
            matched = false;

            StopCoroutine(gameState);
            gameState = MatchState();
            StartCoroutine(gameState);
        }
        else
        {
            StopCoroutine(gameState);
            gameState = BlockState();
            StartCoroutine(gameState);
        }
    }

    IEnumerator MatchState()
    {
        while (true)
        {
            int prevDestroyNum = toBeDestroyed.Count;

            foreach (GameObject cube in cubes)
            {
                if (cube.GetComponent<Cube>().gotMatched)
                {
                    CubeFunction.Matched(cube);
                }
            }
            //Debug.Break();
            int currDestroyNum = toBeDestroyed.Count;

            if (prevDestroyNum != currDestroyNum)
            {
                yield return null;
            }
            else
            {
                break;
            }
        }

        StopCoroutine(gameState);
        gameState = TempBlockState();
        StartCoroutine(gameState);
    }

    IEnumerator TempBlockState()
    {
        foreach (GameObject cube in toBeDestroyed)
        {
            CubeFunction.BlockCombine(cube);
        }

        yield return null;

        StopCoroutine(gameState);
        gameState = FallState();
        StartCoroutine(gameState);
    }
}
