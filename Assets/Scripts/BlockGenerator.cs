using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    //Singleton
    public static BlockGenerator BlockGen { get; set; }
    void Awake()
    {
        if (BlockGen == null)
        {
            BlockGen = this;
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

    //The falling block
    GameObject block;

    //Cube positions in a block
    Vector3[] cubePos = { new Vector3(0, 0, 0), new Vector3(0, 0.5f, 0), new Vector3(0, 1, 0) };

    //Spawn position
    public GameObject spawn;
    Vector3 spawnPos;

    //Temporary new cube reference
    GameObject newCube;

    //Testing counter
    int count = 1;

    // Start is called before the first frame update
    void Start()
    {
        spawnPos = spawn.transform.position;
    }

    //Create a cube in a block
    void CreateCube(GameObject cube, Vector3 position)
    {
        newCube = Instantiate(cube, block.transform) as GameObject;
        newCube.transform.localPosition = position;
    }
    
    //Create a block of cubes
    public GameObject CreateBlock()
    {
        block = new GameObject("Block");
        block.tag = "MainBlock";
        block.transform.position = spawnPos;
        block.AddComponent<Block>(); //Add the Block.cs script

        //block.AddComponent<BoxCollider2D>();
        //block.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0.5f);
        //block.GetComponent<BoxCollider2D>().size = new Vector2(0.5f, 1.5f);

        //A block has 70% chance to contain 3 cubes, 30% to contain 2 cubes
        int typeRoll = Random.Range(0, 100);

        if (typeRoll < 70)
        {
            block.GetComponent<Block>().type = 3;
        }
        else
        {
            block.GetComponent<Block>().type = 2;
        }

        //A cube type has 20% chance to appear on a cube
        for (int i = 0; i < block.GetComponent<Block>().type; i++)
        {
            int cubeRoll = Random.Range(0, 100);

            if (cubeRoll < 20)
            {
                CreateCube(redCube, cubePos[i]);
            }
            else if (cubeRoll < 40)
            {
                CreateCube(blueCube, cubePos[i]);
            }
            else if (cubeRoll < 60)
            {
                CreateCube(greenCube, cubePos[i]);
            }
            else if (cubeRoll < 80)
            {
                CreateCube(yellowCube, cubePos[i]);
            }
            else if (cubeRoll < 100)
            {
                CreateCube(purpleCube, cubePos[i]);
            }
        }

        return block;
    }
}
