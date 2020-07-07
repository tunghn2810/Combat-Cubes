using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollisionCheck
{
    //Offsets for collision detection
    static Vector3 horzOffset = new Vector3(0.251f, 0, 0);
    static Vector3 vertOffset = new Vector3(0, 0.251f, 0);

    //Check for collisions to the left of the cube
    public static bool LeftCheck(GameObject obj)
    {
        RaycastHit2D hitInfo1 = Physics2D.Raycast(obj.transform.position - horzOffset + vertOffset / 2, Vector2.left, 0.05f);
        RaycastHit2D hitInfo2 = Physics2D.Raycast(obj.transform.position - horzOffset - vertOffset / 2, Vector2.left, 0.05f);
        if (hitInfo1.collider || hitInfo2.collider)
        {
            return true;
        }
        return false;
    }

    //Check for collisions to the left of the cube
    public static bool RightCheck(GameObject obj)
    {
        RaycastHit2D hitInfo1 = Physics2D.Raycast(obj.transform.position + horzOffset + vertOffset / 2, Vector2.right, 0.05f);
        RaycastHit2D hitInfo2 = Physics2D.Raycast(obj.transform.position + horzOffset - vertOffset / 2, Vector2.right, 0.05f);
        if (hitInfo1.collider || hitInfo2.collider)
        {
            return true;
        }
        return false;
    }

    ////Check for collisions below the cube
    //public static bool FloorCheck(GameObject obj)
    //{
    //    RaycastHit2D hitInfo = Physics2D.Raycast(obj.transform.position - vertOffset, Vector2.down, 0.05f);
    //    if (hitInfo.collider)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
}
