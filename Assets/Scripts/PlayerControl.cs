using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Reference to keep track of the falling block
    GameObject block;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        block = GameObject.FindWithTag("MainBlock");

        if (block != null)
        {
            #region "PC Controls"
            //Change place of the cubes in the falling block
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Vector3 temp = new Vector3();
                temp = block.GetComponent<Block>().Cubes[1].localPosition;

                if (block.GetComponent<Block>().type == 2)
                {
                    block.GetComponent<Block>().Cubes[1].localPosition = block.GetComponent<Block>().Cubes[2].localPosition;
                    block.GetComponent<Block>().Cubes[2].localPosition = temp;
                }
                else if (block.GetComponent<Block>().type == 3)
                {
                    block.GetComponent<Block>().Cubes[1].localPosition = block.GetComponent<Block>().Cubes[2].localPosition;
                    block.GetComponent<Block>().Cubes[2].localPosition = block.GetComponent<Block>().Cubes[3].localPosition;
                    block.GetComponent<Block>().Cubes[3].localPosition = temp;
                }
            }

            //Move the falling block to the left
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !block.GetComponent<Block>().leftCol)
            {
                if (block.transform.position.x > -1.75)
                    block.transform.position -= new Vector3(0.5f, 0, 0);
            }
            //Move the falling block to the right
            if (Input.GetKeyDown(KeyCode.RightArrow) && !block.GetComponent<Block>().rightCol)
            {
                if (block.transform.position.x < 3)
                    block.transform.position += new Vector3(0.5f, 0, 0);
            }

            //Make the block fall faster
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                block.GetComponent<Block>().timer = 0;
                block.GetComponent<Block>().timeStep = 0.05f;
            }
            //The block falls at normal speed
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                block.GetComponent<Block>().timer = 0.5f;
                block.GetComponent<Block>().timeStep = 0.5f;
            }
            #endregion "End of PC Controls"
        }
    }
}
