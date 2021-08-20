using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VRClampDirection : MonoBehaviour
{

    /// <summary>
    /// Boolean to check if the clamping should happen in x-Direction
    /// </summary>
    public bool XDirection;
    /// <summary>
    /// Boolean to check if the clamping should happen in y-Direction
    /// </summary>
    public bool YDirection;
    /// <summary>
    /// Boolean to check if the clamping should happen in z-Direction
    /// </summary>
    public bool ZDirection;
    /// <summary>
    /// Offsetvalue in X-Direction from Startposition
    /// </summary>
    public float OffsetX;
    /// <summary>
    /// Offsetvalue in Y-Direction from Startposition
    /// </summary>
    public float OffsetY;
    /// <summary>
    /// Offsetvalue in Z-Direction from Startposition
    /// </summary>
    public float OffsetZ;
    /// <summary>
    /// Startposition
    /// </summary>
    public Vector3 StartPosition;



    /// <summary>
    ///  Sets the Startposition
    /// </summary>
    /// <remarks> 
    /// </remarks>
    /// <param name="void"></param>
    /// <returns>void</returns>
    void Start()
    {
        StartPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    // Update is called once per frame
    /// <summary>
    /// Checks in which direction the clamping should occur and then applies clamping to the GameObject
    /// </summary>
    /// 


    /// <summary>
    /// Applies Clamping to a GameObject
    /// </summary>
    /// <remarks>
    /// <ul>
    /// <li>Checks in which direction the clamping should occur</li>
    /// <li>Applies clamping to the GameObject considering the Offsetvalue on every Frame</li>
    /// </ul> 
    /// </remarks>
    /// <param name="void"></param>
    /// <returns>void</returns>
    void Update()
    {
        if (XDirection)
        {
            if (this.gameObject.transform.localPosition.x >= StartPosition.x + OffsetX)
            { this.gameObject.transform.localPosition = new Vector3(StartPosition.x + OffsetX, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z); }

            if (this.gameObject.transform.localPosition.x <= StartPosition.x - OffsetX)
            { this.gameObject.transform.localPosition = new Vector3(StartPosition.x - OffsetX, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z); }


        }




        if (YDirection)
        {


            if (this.gameObject.transform.localPosition.y >= StartPosition.y + OffsetY)
            { this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, StartPosition.y + OffsetY, this.gameObject.transform.localPosition.z); }

            if (this.gameObject.transform.localPosition.y <= StartPosition.y - OffsetY)
            { this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, StartPosition.y - OffsetY, this.gameObject.transform.localPosition.z); }

        }




        if (ZDirection)
        {
            if (this.gameObject.transform.localPosition.z >= StartPosition.z + OffsetZ)
            { this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y + OffsetY, StartPosition.z + OffsetZ); }

            if (this.gameObject.transform.localPosition.z <= StartPosition.z - OffsetZ)
            { this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y - OffsetY, StartPosition.z - OffsetZ); }


        }










    }






}

