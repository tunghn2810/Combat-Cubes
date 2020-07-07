using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    //Singleton
    public static BoardManager Instance { get; set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Coroutine
    IEnumerator gameState;
    IEnumerator playerState1;
    IEnumerator playerState2;

    //Game mode enum
    enum GameMode { SinglePlayer, HotSeat, Online };
    GameMode gameMode;

    //Keep track of all cubes except the falling ones
    public List<GameObject>[] cubes;
    
    //Main block spawn position
    public GameObject spawn1;
    public GameObject spawn2;
    public Vector3[] spawnPositions;

    //Next block spawn position
    public GameObject nextSpawn1;
    public GameObject nextSpawn2;
    public Vector3[] nextSpawnPositions;

    //Keep track of the falling blocks
    public GameObject[] fallingBlocks;

    //Keep track of next blocks
    public GameObject[] nextBlocks;

    //Keep track of temporary blocks
    public GameObject[][] tempBlocks;

    //Check for matching cubes
    public bool[] matchCheckings;
    public bool[] matcheds;
    public bool[] noMatches;

    //Keep track of cubes to be destroyed
    public List<GameObject>[] toBeDestroyeds;
    
    //Keep track of when the game start
    public bool startGame = false;
    
    void Start()
    {
        cubes = new List<GameObject>[2];
        cubes[0] = new List<GameObject>();
        cubes[1] = new List<GameObject>();

        spawnPositions = new Vector3[2];
        spawnPositions[0] = spawn1.transform.position;
        spawnPositions[1] = spawn2.transform.position;

        nextSpawnPositions = new Vector3[2];
        nextSpawnPositions[0] = nextSpawn1.transform.position;
        nextSpawnPositions[1] = nextSpawn2.transform.position;

        fallingBlocks = new GameObject[2];
        nextBlocks = new GameObject[2];
        tempBlocks = new GameObject[2][];

        matchCheckings = new bool[2];
        matchCheckings[0] = matchCheckings[1] = false;
        matcheds = new bool[2];
        matcheds[0] = matcheds[1] = false;
        noMatches = new bool[2];
        noMatches[0] = noMatches[1] = false;

        toBeDestroyeds = new List<GameObject>[2];
        toBeDestroyeds[0] = new List<GameObject>();
        toBeDestroyeds[1] = new List<GameObject>();

        gameState = StandByState();

        gameMode = GameMode.SinglePlayer;
        StartCoroutine(gameState);
    }

    IEnumerator StandByState()
    {
        playerState1 = BlockState(0);
        StartCoroutine(playerState1);

        if (gameMode == GameMode.HotSeat)
        {
            playerState2 = BlockState(1);
            StartCoroutine(playerState2);
        }

        StopCoroutine(gameState);

        yield return null;
    }

    //State when a block is created
    IEnumerator BlockState(int playerNum)
    {
        string nextBlockTag = "NextBlock" + playerNum.ToString();
        GameObject nextBlock = GameObject.FindWithTag(nextBlockTag);

        string mainBlockTag = "MainBlock" + playerNum.ToString();
        GameObject mainBlock = GameObject.FindWithTag(mainBlockTag);

        if (nextBlock == null) //Only happens once when the game begins
        {
            nextBlocks[playerNum] = BlockGenerator.Instance.CreateBlock(playerNum, nextSpawnPositions[playerNum], false);
        }
        else //Make the next block the main block and then create a new next block
        {
            nextBlock.tag = mainBlockTag;
            nextBlock.transform.position = spawnPositions[playerNum];
            mainBlock = nextBlock;

            nextBlocks[playerNum] = nextBlock =  BlockGenerator.Instance.CreateBlock(playerNum, nextSpawnPositions[playerNum], false);
        }

        if (mainBlock == null) //Only happens once when the game begins
        {
            fallingBlocks[playerNum] = BlockGenerator.Instance.CreateBlock(playerNum, spawnPositions[playerNum], true);
        }
        else
        {
            fallingBlocks[playerNum] = mainBlock;
        }
        fallingBlocks[playerNum].GetComponent<Block>().InitalizeBlock();

        if (playerNum == 0)
        {
            NextState(playerState1, FallState(0));
        }
        else
        {
            NextState(playerState2, FallState(1));
        }

        yield return null;
    }

    //State when a block is falling
    IEnumerator FallState(int playerNum)
    {
        GameObject fallingBlock = fallingBlocks[playerNum];
        while (fallingBlock)
        {
            fallingBlock.GetComponent<Block>().Fall();
            yield return null;
        }

        if (playerNum == 0)
        {
            while (GameObject.FindGameObjectsWithTag("TempBlock0").Length > 0)
            {
                foreach (GameObject block in GameObject.FindGameObjectsWithTag("TempBlock0"))
                {
                    block.GetComponent<Block>().Fall();
                }
                yield return null;
            }
        }
        else
        {
            while (GameObject.FindGameObjectsWithTag("TempBlock1").Length > 0)
            {
                foreach (GameObject block in GameObject.FindGameObjectsWithTag("TempBlock1"))
                {
                    block.GetComponent<Block>().Fall();
                }
                yield return null;
            }
        }
        
        if (playerNum == 0)
        {
            NextState(playerState1, CollisionState(0));
        }
        else
        {
            NextState(playerState2, CollisionState(1));
        }
    }

    //State when there are matching cubes
    IEnumerator CollisionState(int playerNum)
    {
        while (matchCheckings[playerNum])
        {
            foreach (GameObject cube in cubes[playerNum])
            {
                CubeFunction.ClearAdjacents(cube);
                if (!cube.GetComponent<Cube>().didCollisionCheck && !cube.GetComponent<Cube>().gotMatched)
                {
                    CubeFunction.CollisionCheck(cube, playerNum);
                }
            }

            matchCheckings[playerNum] = false;
            yield return null;
        }

        foreach (GameObject cube in cubes[playerNum])
        {
            cube.GetComponent<Cube>().didCollisionCheck = false;
        }
        
        if (playerNum == 0)
        {
            NextState(playerState1, MatchState(0));
        }
        else
        {
            NextState(playerState2, MatchState(1));
        }

        //if (matcheds[playerNum])
        //{
        //    matcheds[playerNum] = false;
        //
        //    if (playerNum == 0)
        //    {
        //        NextState(playerState1, MatchState(0));
        //    }
        //    else
        //    {
        //        NextState(playerState2, MatchState(1));
        //    }
        //}
        //else
        //{
        //    if (playerNum == 0)
        //    {
        //        NextState(playerState1, BlockState(0));
        //    }
        //    else
        //    {
        //        NextState(playerState2, BlockState(1));
        //    }
        //}
    }

    IEnumerator MatchState(int playerNum)
    {
        if (matcheds[playerNum])
        {
            matcheds[playerNum] = false;
            while (true)
            {
                int prevDestroyNum = toBeDestroyeds[playerNum].Count;

                foreach (GameObject cube in cubes[playerNum])
                {
                    if (cube.GetComponent<Cube>().gotMatched)
                    {
                        CubeFunction.Matched(cube, playerNum);
                    }
                }

                int currDestroyNum = toBeDestroyeds[playerNum].Count;

                if (prevDestroyNum != currDestroyNum)
                {
                    yield return null;
                }
                else
                {
                    foreach (GameObject cube in toBeDestroyeds[playerNum])
                    {
                        CubeFunction.BlinkAnimation(cube);
                    }

                    yield return new WaitForSeconds(0.5f);

                    break;
                }
            }
        }
        else
        {
            noMatches[playerNum] = true;
        }
        
        if (playerNum == 0)
        {
            NextState(playerState1, TempBlockState(0));
        }
        else
        {
            NextState(playerState2, TempBlockState(1));
        }
    }
    

    IEnumerator TempBlockState(int playerNum)
    {
        if (!noMatches[playerNum])
        {
            foreach (GameObject cube in toBeDestroyeds[playerNum])
            {
                CubeFunction.BlockCombine(cube, playerNum);
            }
            
            if (playerNum == 0)
            {
                NextState(playerState1, FallState(0));
            }
            else
            {
                NextState(playerState2, FallState(1));
            }
        }
        else
        {
            noMatches[playerNum] = false;

            if (playerNum == 0)
            {
                NextState(playerState1, BlockState(0));
            }
            else
            {
                NextState(playerState2, BlockState(1));
            }
        }

        yield return null;
    }
    
    void NextState(IEnumerator coroutine, IEnumerator state)
    {
        StopCoroutine(coroutine);
        coroutine = state;
        StartCoroutine(coroutine);
    }
}
