using UnityEngine;
using System.Collections.Generic;

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

            ///// <summary>
            ///// Name of the first GameObject
            ///// </summary>
            //public string targetObjectName = "";

            ///// <summary>
            ///// Name of the second GameObject
            ///// </summary>
            //public string ObjectName2 = "";

            /// <summary>
            /// Displacement value
            /// </summary>
            public Vector3 controlObjectOrigin;

            /// <summary>
            /// Displacement value
            /// </summary>
            public Vector3 targetObjectOrigin;

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
            /// Line renderer of target object (if it contains one)
            /// </summary>
            private LineRenderer lr;

            /// <summary>
            /// Original point positions in line renderer of target object (if it contains one)
            /// </summary>
            private List<Vector3> lrPositions;

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
                controlObjectOrigin = controlObject.transform.position;
                targetObjectOrigin = targetObject.transform.localPosition;

                lr = targetObject.GetComponent<LineRenderer>();
                if(lr != null)
                {
                    lrPositions = new List<Vector3>();
                    for(int i = 0; i < lr.positionCount; i++)
                    {
                        lrPositions.Add(lr.GetPosition(i));
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
                if (targetObject != null)
                {
                    if (ZDirection)
                    {
                        targetObject.transform.localPosition = targetObjectOrigin +
                            new Vector3(0f, 0f, (controlObject.transform.position.z - controlObjectOrigin.z));
                    }

                    if (XDirection)
                    {
                        targetObject.transform.localPosition = targetObjectOrigin +
                            new Vector3((controlObject.transform.position.x - controlObjectOrigin.x), 0f, 0f);
                    }

                    if (YDirection)
                    {
                        float changeY = (controlObject.transform.position.y - controlObjectOrigin.y);
                        targetObject.transform.localPosition = targetObjectOrigin +
                            new Vector3(0, changeY, 0);

                        if (lr != null)
                        {
                            for(int i = 0; i < lr.positionCount; i++)
                            {
                                lr.SetPosition(i, lrPositions[i] + new Vector3(0, changeY, 0));
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


        }
    }
}