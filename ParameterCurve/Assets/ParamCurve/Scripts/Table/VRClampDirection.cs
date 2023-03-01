using UnityEngine;

//using UnityEngine.XR.OpenXR.Features.Interactions;


namespace ParamCurve.Scripts.Table
{
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


        private float XDirUpperBound;
        private float XDirLowerBound;
    
        private float YDirUpperBound;
        private float YDirLowerBound;

        private float ZDirUpperBound;
        private float ZDirLowerBound;


        /// <summary>
        ///  Sets the Startposition
        /// </summary>
        /// <remarks> 
        /// </remarks>
        /// <param name="void"></param>
        /// <returns>void</returns>
        void Start()
        {
            StartPosition = transform.localPosition; //new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            XDirUpperBound = StartPosition.x + OffsetX;
            XDirLowerBound = StartPosition.x - OffsetX;
            YDirUpperBound = StartPosition.y + OffsetY;
            YDirLowerBound = StartPosition.y - OffsetY;
            ZDirUpperBound = StartPosition.z + OffsetZ;
            ZDirLowerBound = StartPosition.z - OffsetZ;
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
            var currPos = transform.localPosition;
            var x = XDirection ? Mathf.Clamp(currPos.x, XDirLowerBound, XDirUpperBound) : currPos.x;
            var y = YDirection ? Mathf.Clamp(currPos.y, YDirLowerBound, YDirUpperBound) : currPos.y;
            var z = ZDirection ? Mathf.Clamp(currPos.z, ZDirLowerBound, ZDirUpperBound) : currPos.z;

            transform.localPosition = new Vector3(x, y, z);


        
        
            // if (XDirection)
            // {
            //     var x = Mathf.Clamp(transform.localPosition.x, XDirLowerBound, XDirUpperBound);
            // }
            //
            // if (XDirection)
            // {
            //     //if (this.gameObject.transform.localPosition.x >= StartPosition.x + OffsetX)
            //     //{ this.gameObject.transform.localPosition = new Vector3(StartPosition.x + OffsetX, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z); }
            //
            //     //if (this.gameObject.transform.localPosition.x <= StartPosition.x - OffsetX)
            //     //{ this.gameObject.transform.localPosition = new Vector3(StartPosition.x - OffsetX, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z); }
            //
            //     if (transform.localPosition.x >= XDirUpperBound)
            //     {
            //         Debug.Log("XDirUpperBound");
            //         transform.localPosition = new Vector3(XDirUpperBound, transform.localPosition.y, transform.localPosition.z);
            //         
            //     }
            //         
            //     if(transform.localPosition.x <= XDirLowerBound)
            //     {
            //         Debug.Log("XDirLowerBound");
            //         transform.localPosition = new Vector3(XDirLowerBound, transform.localPosition.y, transform.localPosition.z);
            //     }
            // }
            //
            // if (YDirection)
            // {
            //
            //
            //     //if (this.gameObject.transform.localPosition.y >= StartPosition.y + OffsetY)
            //     //{ this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, StartPosition.y + OffsetY, this.gameObject.transform.localPosition.z); }
            //
            //     //if (this.gameObject.transform.localPosition.y <= StartPosition.y - OffsetY)
            //     //{ this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, StartPosition.y - OffsetY, this.gameObject.transform.localPosition.z); }
            //
            //     if (transform.localPosition.y >= YDirUpperBound)
            //     {
            //         transform.localPosition = new Vector3(transform.localPosition.x, YDirUpperBound, transform.localPosition.z);
            //     }
            //
            //     if (transform.localPosition.y <= YDirLowerBound)
            //     {
            //         transform.localPosition = new Vector3(transform.localPosition.x, YDirLowerBound, transform.localPosition.z);
            //     }
            //
            // }
            //
            //
            //
            //
            // if (ZDirection)
            // {
            //     //if (this.gameObject.transform.localPosition.z >= StartPosition.z + OffsetZ)
            //     //{ this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y + OffsetY, StartPosition.z + OffsetZ); }
            //
            //     //if (this.gameObject.transform.localPosition.z <= StartPosition.z - OffsetZ)
            //     //{ this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y - OffsetY, StartPosition.z - OffsetZ); }
            //
            //     if (transform.localPosition.z >= ZDirUpperBound)
            //     {
            //         transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, ZDirUpperBound);
            //     }
            //
            //     if (transform.localPosition.z <= ZDirLowerBound)
            //     {
            //         transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, ZDirLowerBound);
            //     }
            // }
        }
    }
}

