using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

//class to manage visiblity and position of sprites (12 shapes)
public class ShapePosition : MonoBehaviour
{
    private static GameObject gameobject;

    //placing every of 12 shapes in the same set place
    public static void SetShapes(GameObject ShapeObject, int[] order)
    {
        for (int i = 0; i < 12; i++)
        {
            int shape = order[i] - 1;
            gameobject = ShapeObject.transform.Find("12ksztaltow_" + shape.ToString())?.gameObject;
            Transform transform = gameobject.GetComponent<Transform>();
            transform.position = new Vector3(-29.25f + 4.5f * (float)(i+1), 4f - cameraWidth.mainCamera.orthographicSize , 1f);
            transform.localScale = new Vector3(4f, 4f, 1f);
        }
    }

    //activate (1 from 12) chosen shape (based on currentShape)
    public static void GetCurrentShapes(GameObject ShapeObject, int[] order, int level)
    {
        Vector3 bigShapeScale = new Vector3(10f, 10f, 1f);
        Vector3 smallShapeScale = new Vector3(4f, 4f, 1f);

        int shape = order[level-1] - 1;
        gameobject = ShapeObject.transform.Find("12ksztaltow_" + shape.ToString())?.gameObject;//find gameobject 
        Transform transform = gameobject.GetComponent<Transform>();
        transform.localScale = bigShapeScale;
        transform.position = new Vector3(0f, -10f - cameraWidth.mainCamera.orthographicSize / 2, 1f);

        //if shape is chosen, activate it, if not, deactivate
        if (level > 1)
        {
            shape = order[level - 2] - 1;
            gameobject = ShapeObject.transform.Find("12ksztaltow_" + shape.ToString())?.gameObject;//find gameobject 
            transform = gameobject.GetComponent<Transform>();
            transform.localScale = smallShapeScale;
            transform.position = new Vector3(-29.25f + 4.5f * (float)(level-1), 4f - cameraWidth.mainCamera.orthographicSize, 1f);
        }
                

    }

}
