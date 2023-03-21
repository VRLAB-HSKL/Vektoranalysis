using System.Collections.Generic;
using System.IO;
using ParamCurve.Scripts.Table;
using UnityEngine;
using UnityEngine.UI;
using VRKL.MBU;

namespace ParamCurve.Scripts.Cockpit
{
    public class CockpitTravel : MonoBehaviour
    {
        public LineRenderer CurveLine;
        public GameObject TravelObjectParent;
        public GameObject Cockpit;

        //Vectors
        public LineRenderer TangentLine;
        public LineRenderer NormalLine;
        public LineRenderer BinormalLine;

        //Cockpit UI
        public Transform CockpitRegulator;
        public RawImage CockpitImageDisplay;
        public GameObject CockpitCompassPin;

        //Time-Distance/Velocity Diagrams
        public GameObject TimeDistanceTravelObject;
        public GameObject TimeDistanceLineObject;
        public GameObject TimeVelocityTravelObject;
        public GameObject TimeVelocityLineObject;

        private float _updateTimer;

        private WaypointManager wpm;

        //lists to hold curve data and time/velocity graph data
        private List<Vector3> curvePoints;
        private List<Vector3> tangentPositions;
        private List<Vector3> normalPositions;
        private List<Vector3> binormalPositions;
        private List<Vector2> timeDistPositions;
        private List<Vector2> timeVelPositions;

        private LineRenderer TimeVelocityLR;
        private LineRenderer TimeDistanceLR;

        //use index if not using waypoint manager
        //private int index;

        private int size;
        private bool is3D;
        private float timeThreshold;
        private Vector3 _initTimeDistTravelPos;
        private Vector3 _initTimeVelTravelPos;

        //private string path = "Assets/Resources/linecoords.txt";
        private float minThreshold = 0.02f; //fastest travel
        private float maxThreshold = 0.15f;  //slowest travel

        // Start is called before the first frame update
        void Start()
        {
            tangentPositions = new List<Vector3>();
            normalPositions = new List<Vector3>();
            binormalPositions = new List<Vector3>();
            curvePoints = new List<Vector3>();
            timeDistPositions = new List<Vector2>();
            timeVelPositions = new List<Vector2>();
            TimeDistanceLR = TimeDistanceLineObject.GetComponent<LineRenderer>();
            TimeVelocityLR = TimeVelocityLineObject.GetComponent<LineRenderer>();

            //index = 0;
            timeThreshold = (minThreshold + maxThreshold) / 2;
            _initTimeDistTravelPos = TimeDistanceTravelObject.transform.localPosition;
            _initTimeVelTravelPos = TimeVelocityTravelObject.transform.localPosition;

            // ToDo: Remove this!
            var path = Application.persistentDataPath  + "/linecoords.txt";;
        
            using (StreamReader reader = new StreamReader(path))
            {
                //change center display image on cockpit to current curve
                string imgName = reader.ReadLine();
                Texture2D img = (Texture2D)Resources.Load("/img/" + imgName + ".png", typeof(Texture2D));
                
                //(Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/img/" + imgName + ".png", typeof(Texture2D));
                CockpitImageDisplay.texture = img;

                if (int.Parse(reader.ReadLine()) == 3)
                {
                    is3D = true;
                }
                else
                {
                    is3D = false;
                    BinormalLine.gameObject.SetActive(false);   //binormal is only in 3rd dimension
                }

                string str = string.Empty;
                size = int.Parse(reader.ReadLine());
                CurveLine.positionCount = size;
                TimeDistanceLR.positionCount = size;
                TimeVelocityLR.positionCount = size;

                for (int i = 0; i < size; i++)
                {
                    //read in current point xyz coordinates
                    str = reader.ReadLine();
                    string[] xyz = str.Split(' ');
                    Vector3 coords = new Vector3(
                        float.Parse(xyz[0]), 
                        float.Parse(xyz[is3D ? 1 : 2]), 
                        float.Parse(xyz[is3D ? 2 : 1]));
                    CurveLine.SetPosition(i, coords);
                    curvePoints.Add(coords);

                    //read in current tangent vector
                    str = reader.ReadLine();
                    string[] tXYZ = str.Split(' ');
                    Vector3 tangent = new Vector3(float.Parse(tXYZ[0]), float.Parse(tXYZ[1]), float.Parse(tXYZ[2]));
                    tangentPositions.Add(tangent);

                    //read in current normal vector
                    str = reader.ReadLine();
                    string[] nXYZ = str.Split(' ');
                    Vector3 normal = new Vector3(float.Parse(nXYZ[0]), float.Parse(nXYZ[1]), float.Parse(nXYZ[2]));
                    normalPositions.Add(normal);

                    //read in current binormal vector if 3D
                    if (is3D)
                    {
                        str = reader.ReadLine();
                        string[] bXYZ = str.Split(' ');
                        Vector3 binormal = new Vector3(float.Parse(bXYZ[0]), float.Parse(bXYZ[1]), float.Parse(bXYZ[2]));
                        binormalPositions.Add(binormal);
                    }

                    //read in current time distance point
                    str = reader.ReadLine();
                    string[] tdXY = str.Split(' ');
                    Vector2 td = new Vector2(float.Parse(tdXY[0]), float.Parse(tdXY[1]));
                    timeDistPositions.Add(td);

                    //construct time distance polyline
                    Vector3 newPos = _initTimeDistTravelPos;
                    newPos.x += td[0];
                    newPos.y += td[1];
                    newPos.z -= UnityEngine.Random.Range(0f, 0.005f);
                    TimeDistanceLR.SetPosition(i, newPos);

                    //read in current time velocity point
                    str = reader.ReadLine();
                    string[] tvXY = str.Split(' ');
                    Vector2 tv = new Vector2(float.Parse(tvXY[0]), float.Parse(tvXY[1]));
                    timeVelPositions.Add(tv);

                    //construct time velocity polyline
                    newPos = _initTimeVelTravelPos;
                    newPos.x += tv[0];
                    newPos.y += tv[1];
                    newPos.z -= UnityEngine.Random.Range(0f, 0.005f);
                    TimeVelocityLR.SetPosition(i, newPos);
                }
            }
            wpm = new WaypointManager(curvePoints.ToArray(), 0.1f);

        }

        // Update is called once per frame
        void Update()
        {
            // Update time since last point step
            _updateTimer += Time.deltaTime;

            //update speed based on regulator position
            changeSpeed();

            // If the time threshold has been reached, traverse to next point
            if (_updateTimer >= timeThreshold)
            {
                _updateTimer = 0f;

                updateVectors();
                updateTravelObjects();

                var pos = wpm.GetWaypoint();
                var target = wpm.GetFollowupWaypoint();
                var dist = Vector3.Distance(pos, target);
                TravelObjectParent.transform.LookAt(target);
                //CockpitCompassPin.transform.LookAt(target);
                TravelObjectParent.transform.position = wpm.Move(pos, dist);

                //without waypoint manager
                /*Vector3 nextPosition = curvePoints[index];
            TravelObjectParent.transform.LookAt(nextPosition);
            TravelObjectParent.transform.position = nextPosition;
            index++;

            if(index == size) { index = 0; }*/
            }
        }

        /// <summary>
        /// Draw tan/norm/binorm lines out from cockpit using imported data
        /// </summary>
        private void updateVectors()
        {
            TangentLine.SetPosition(0, Cockpit.transform.position);
            TangentLine.SetPosition(1, Cockpit.transform.position + tangentPositions[wpm.Current]);
            //TangentLine.SetPosition(1, Cockpit.transform.position + tangentPositions[index]);

            NormalLine.SetPosition(0, Cockpit.transform.position);
            NormalLine.SetPosition(1, Cockpit.transform.position + normalPositions[wpm.Current]);
            //NormalLine.SetPosition(1, Cockpit.transform.position + normalPositions[index]);

            if (is3D)
            {
                BinormalLine.SetPosition(0, Cockpit.transform.position);
                BinormalLine.SetPosition(1, Cockpit.transform.position + binormalPositions[wpm.Current]);
                //BinormalLine.SetPosition(1, Cockpit.transform.position + binormalPositions[index]);
            }
        }

        /// <summary>
        /// Move point on time/velocity diagrams
        /// </summary>
        private void updateTravelObjects()
        {
            Vector2 tdPosVec = timeDistPositions[wpm.Current];
            Vector3 tdVec = new Vector3(tdPosVec.x, tdPosVec.y, 0f);
            TimeDistanceTravelObject.transform.localPosition = _initTimeDistTravelPos + tdVec;

            Vector2 tvPosVec = timeVelPositions[wpm.Current];
            Vector3 tvVec = new Vector3(tvPosVec.x, tvPosVec.y, 0f);
            TimeVelocityTravelObject.transform.localPosition = _initTimeVelTravelPos + tvVec;
        }

        /// <summary>
        /// Change travel speed based on regulator position
        /// </summary>
        private void changeSpeed()
        {
            float offset = CockpitRegulator.GetComponent<VRClampDirection>().OffsetX;
            float regulatorPos = CockpitRegulator.localPosition.x;
            //timeThreshold = (regulatorPos - (-1 * offset)) * (maxThreshold - minThreshold) / (offset - (-1 * offset)) + minThreshold;
            //map regulator position to range of speeds for cockpit
            timeThreshold = (regulatorPos - offset) * (maxThreshold - minThreshold) / ((-1 * offset) - offset) + minThreshold;
        }
    }
}
