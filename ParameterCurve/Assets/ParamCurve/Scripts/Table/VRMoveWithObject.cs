using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using HTC.UnityPlugin.Vive;

namespace ImmersiveVolumeGraphics
{
    namespace ModelEdit
    {
        /// <summary>
        /// Move an GameObject in relation to another GameObject , similar to VRRotateWithObject
        /// </summary>
        /// <seealso>
        /// <ul>
        /// <li>VRRotateWithObject</li>
        /// </ul>
        /// </seealso>
        public class VRMoveWithObject : MonoBehaviour
        {
            /// <summary>
            /// First GameObject: object being affected
            /// </summary>
            public GameObject targetObject;

            /// <summary>
            /// Second GameObject: object whose movement affects targetObject
            /// </summary>
            public GameObject controlObject;

            /// <summary>
            /// gameobject providing upper bound for y movement
            /// </summary>
            public GameObject yUpperBoundObject;

            /// <summary>
            /// gameobject providing lower bound for y movement
            /// </summary>
            public GameObject yLowerBoundObject;

            ///// <summary>
            ///// Name of the first GameObject
            ///// </summary>
            //public string targetObjectName = "";

            ///// <summary>
            ///// Name of the second GameObject
            ///// </summary>
            //public string ObjectName2 = "";

            /// <summary>
            /// Check if the movement is in x-Direction
            /// </summary>
            public bool XDirection;

            /// <summary>
            /// Check if the movement is in y-Direction
            /// </summary>
            public bool YDirection;

            /// <summary>
            /// Check if the movement is in z-Direction
            /// </summary>
            public bool ZDirection;

            /// <summary>
            /// Line renderers of target object and children (if it contains one)
            /// </summary>
            private LineRenderer[] lrs;

            /// <summary>
            /// Original point positions in line renderers of target object (if it contains one)
            /// </summary>
            private List<List<Vector3>> lrsPositions;

            /// <summary>
            /// Y position of y upper bound object
            /// </summary>
            private float yUpperBound;

            /// <summary>
            /// Y position of y lower bound object
            /// </summary>
            private float yLowerBound;

            /// <summary>
            /// Original point position (reference) for control object 
            /// </summary>
            private Vector3 controlObjectOrigin;

            /// <summary>
            /// Original point position (reference) for target object 
            /// </summary>
            private Vector3 targetObjectOrigin;

            /// <summary>
            /// Find both Objects in the Scene
            /// </summary>
            /// <remarks>
            /// </remarks>
            /// <returns>void</returns>
            private void Start()
            {
                //targetObject = GameObject.Find(targetObjectName);
                //controlObject = GameObject.Find(ObjectName2);

                //gather initial positions
                controlObjectOrigin = controlObject.transform.position;
                targetObjectOrigin = targetObject.transform.localPosition;
                yLowerBound = yLowerBoundObject.transform.position.y;
                yUpperBound = yUpperBoundObject.transform.position.y;

                //gather initial point positions for line renderer(s) in target (if it has one)
                lrs = targetObject.GetComponentsInChildren<LineRenderer>();
                if (lrs != null)
                {
                    lrsPositions = new List<List<Vector3>>();
                    for(int i = 0; i < lrs.Length; i++)  //for each LR in target
                    {
                        List<Vector3> positions = new List<Vector3>();
                        for(int j = 0; j < lrs[i].positionCount; j++)    //for each point in each LR
                        {
                            positions.Add(lrs[i].GetPosition(j));
                        }
                        lrsPositions.Add(positions);
                    }
                }
            }

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
            /// <returns>void</returns>
            private void Update()
            {
                //if bound is reached, do not move target object and keep control object at bound

                if (controlObject.transform.position.y > yUpperBound)
                {
                    YDirection = false;
                    controlObject.transform.position = new Vector3(controlObject.transform.position.x, yUpperBound, controlObject.transform.position.z);
                } else if (controlObject.transform.position.y < yLowerBound)
                {
                    YDirection = false;
                    controlObject.transform.position = new Vector3(controlObject.transform.position.x, yLowerBound, controlObject.transform.position.z);
                }
                else YDirection = true;

                //only need to update target when control object is being moved by grabbing
                if (controlObject.GetComponent<BasicGrabbable>().isGrabbed)
                {
                    updatePosition();
                }
            }

            private void updatePosition()
            {
                if (targetObject != null)
                {
                    float changeX, changeY, changeZ;

                    if (ZDirection) changeZ = (controlObject.transform.position.z - controlObjectOrigin.z);
                    else changeZ = 0;

                    if (XDirection) changeX = (controlObject.transform.position.x - controlObjectOrigin.x);
                    else changeX = 0;

                    if (YDirection) changeY = (controlObject.transform.position.y - controlObjectOrigin.y);
                    else changeY = 0;

                    //only move if direction is enabled
                    if (XDirection || YDirection || ZDirection)
                    {
                        //new position = origin + change in control object position

                        targetObject.transform.localPosition = targetObjectOrigin + new Vector3(changeX, changeY, changeZ);

                        if (lrs != null)
                        {
                            for (int i = 0; i < lrs.Length; i++)
                            {
                                for (int j = 0; j < lrs[i].positionCount; j++)
                                {
                                    lrs[i].SetPosition(j, lrsPositions[i][j] + new Vector3(changeX, changeY, changeZ));
                                }
                            }
                        }
                    }
                }
                else
                {
                    //targetObject = GameObject.Find(targetObjectName);
                    //controlObject = GameObject.Find(ObjectName2);
                }
            }

            /// <summary>
            /// Initialize the Variables and Objects
            /// </summary>
            /// <remarks>
            /// This Method is used in the ImportRAWModel-Script to initialize this class
            /// <ul>
            /// <li>Hand over the Object names</li>
            /// <li>Set the Direction-Booleans</li>
            /// </ul> 
            /// </remarks>
            /// <param name="name1"></param>
            /// <param name="name2"></param>
            /// <param name="dir"></param>
            /// <returns>void</returns>
            public void InitObj(string name1, string name2, string dir)
            {
                // ToDo: Maybe use enum for direction values instead of raw strings

                //targetObjectName = name1;
                //ObjectName2 = name2;

                if (dir.Equals("x"))
                {
                    XDirection = true;
                }

                if (dir.Equals("y"))
                {
                    YDirection = true;
                }

                if (dir.Equals("z"))
                {
                    ZDirection = true;
                }
            }


            /// <summary>
            /// if the points on the LR change, need to update reference origin positions
            /// </summary>
            public void updateLR(string lrToUpdate)
            {
                if (lrs != null)
                {
                    for (int i = 0; i < lrs.Length; i++)
                    {
                        if (lrs[i].gameObject.name.Equals(lrToUpdate))
                        {
                            for (int j = 0; j < lrs[i].positionCount; j++)
                            {
                                lrsPositions[i][j] = lrs[i].GetPosition(j);
                            }
                        }
                    }
                }                
            }

            public void resetPositions()
            {
                controlObject.transform.position = controlObjectOrigin;
                updatePosition();
            }

        }
    }
}