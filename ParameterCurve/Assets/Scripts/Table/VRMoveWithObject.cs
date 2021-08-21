using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Move an GameObject in relation to another GameObject , similar to VRRotateWithObject
/// </summary>
/// 

/// <seealso>
/// <ul>
/// <li>VRRotateWithObject</li>
/// </ul>
/// </seealso>


public class VRMoveWithObject : MonoBehaviour
{
    /// <summary>
    /// First GameObject
    /// </summary>
    public GameObject SourceObject;
    
    /// <summary>
    /// Second GameObject
    /// </summary>
    public GameObject TargetObject;

    /// <summary>
    /// Name of the first GameObject
    /// </summary>
    public string SourceObjectName = string.Empty;
    
    /// <summary>
    /// Name of the second GameObject
    /// </summary>
    public string TargetObjectName = string.Empty;

    ///// <summary>
    ///// Displacementvalue
    ///// </summary>
    //public float Origin = 0;

    ///// <summary>
    ///// Check if the movement is in x-Direction
    ///// </summary>
    //public bool XDirection;
    ///// <summary>
    ///// Check if the movement is in y-Direction
    ///// </summary>
    //public bool YDirection;
    ///// <summary>
    ///// Check if the movement is in z-Direction
    ///// </summary>
    //public bool ZDirection;


    public Vector3 DisplacementVector = Vector3.zero;


    // Start is called before the first frame update


    /// <summary>
    /// Find both Objects in the Scene
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="void"></param>
    /// <returns>void</returns>
    void Start()
    {
        //
        SourceObject = GameObject.Find(SourceObjectName);
        TargetObject = GameObject.Find(TargetObjectName);

    }

    // Update is called once per frame



    /// <summary>
    /// Change the localPosition of the first GameObject in relation of the second GameObject
    /// </summary>
    /// <remarks>
    /// <ul>
    /// <li>Check whether the first Object exists or not</li>
    /// <li>Check in which direction the translation happens</li>
    /// <li>Set the first Object´s localPosition to the position of the second Object (in the correct direction) </li>
    /// <li>Find  the Objects when the first Object doesnt exist (updating Objectreference)  </li>
    /// </ul> 
    /// </remarks>
    /// <param name="void"></param>
    /// <returns>void</returns>
    void Update()
    {
        // obj.transform.position = this.transform.position;
        if (SourceObject != null)
        {
            //if (ZDirection)
            //{
            //    SourceObject.transform.localPosition = new Vector3(0, -(TargetObject.transform.position.z - Origin), 0);
            //}

            //if (XDirection)
            //{
            //    SourceObject.transform.localPosition = new Vector3((TargetObject.transform.position.x - Origin), 0, 0);
            //}

            //if (YDirection)
            //{
            //    SourceObject.transform.localPosition = new Vector3(0, 0, -(TargetObject.transform.position.z - Origin));
            //}

            TargetObject.transform.localPosition = (TargetObject.transform.position - DisplacementVector);
        }
        else
        {
            SourceObject = GameObject.Find(SourceObjectName);
            TargetObject = GameObject.Find(TargetObjectName);
        }
    }



    /// <summary>
    /// Initialize the Variables and Objects
    /// </summary>
    /// <remarks>
    /// This Method is used in the ImportRAWModel-Script to initialize this class
    /// <ul>
    /// <li>Hand over the Objectnames</li>
    /// <li>Set the Direction-Booleans</li>
    /// </ul> 
    /// </remarks>
    /// <param name="name1"></param>
    /// <param name="name2"></param>
    /// <param name="dir"></param>
    /// <returns>void</returns>
    //public void initObj(string name1, string name2, string dir)
    //{
    //    SourceObjectName = name1;
    //    TargetObjectName = name2;


    //    if (dir.Equals("x"))
    //    {
    //        XDirection = true;

    //    }

    //    if (dir.Equals("y"))
    //    {
    //        YDirection = true;

    //    }

    //    if (dir.Equals("z"))
    //    {
    //        ZDirection = true;

    //    }

    //}


}