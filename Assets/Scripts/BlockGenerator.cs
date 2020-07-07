using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    //Singleton
    public static BlockGenerator Instance { get; set; }
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

    //Cube prefabs
    public GameObject redCube;
    public GameObject blueCube;
    public GameObject greenCube;
    public GameObject yellowCube;
    public GameObject purpleCube;

    //Block prefab
    public GameObject block;

    //Temporary new block reference
    GameObject newBlock;

    //Cube positions in a block
    Vector3[] cubePos = { new Vector3(0, 0, 0), new Vector3(0, 0.5f, 0), new Vector3(0, 1, 0) };

    //Temporary new cube reference
    GameObject newCube;

    //Create a block of cubes
    public GameObject CreateBlock(int playerNum, Vector3 spawnPos, bool isFalling)
    {
        newBlock = Instantiate(block, spawnPos, Quaternion.identity) as GameObject;

        if (isFalling && playerNum == 0)
        {
            newBlock.tag = "MainBlock0";
        }
        else if (isFalling && playerNum == 1)
        {
            newBlock.tag = "MainBlock1";
        }
        else if (!isFalling && playerNum == 0)
        {
            newBlock.tag = "NextBlock0";
        }
        else if (!isFalling && playerNum == 1)
        {
            newBlock.tag = "NextBlock1";
        }

        //newBlock.AddComponent<Block>(); //Add the Block.cs script

        //block.AddComponent<BoxCollider2D>();
        //block.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0.5f);
        //block.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 1.5f);

        //A block has 70% chance to contain 3 cubes, 30% to contain 2 cubes
        int typeRoll = Random.Range(0, 100);

        if (typeRoll < 70)
        {
            newBlock.GetComponent<Block>().type = 3;
        }
        else
        {
            newBlock.GetComponent<Block>().type = 2;
        }

        //A cube type has 20% chance to appear on a cube
        for (int i = 0; i < newBlock.GetComponent<Block>().type; i++)
        {
            int cubeRoll = Random.Range(0, 100);

            if (cubeRoll < 20)
            {
                CreateCube(redCube, cubePos[i], playerNum);
            }
            else if (cubeRoll < 40)
            {
                CreateCube(blueCube, cubePos[i], playerNum);
            }
            else if (cubeRoll < 60)
            {
                CreateCube(greenCube, cubePos[i], playerNum);
            }
            else if (cubeRoll < 80)
            {
                CreateCube(yellowCube, cubePos[i], playerNum);
            }
            else if (cubeRoll < 100)
            {
                CreateCube(purpleCube, cubePos[i], playerNum);
            }
        }

        newBlock.transform.Find("SideCollisionCheck").SetAsLastSibling();
        //newBlock.GetComponent<Block>().InitalizeBlock();

        return newBlock;
    }

    //Create a cube in a block
    void CreateCube(GameObject cube, Vector3 position, int playerNum)
    {
        newCube = Instantiate(cube, newBlock.transform) as GameObject;
        newCube.transform.localPosition = position;
        newCube.name += playerNum.ToString();
    }
}
