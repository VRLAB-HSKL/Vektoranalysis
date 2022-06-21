using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRKL.MBU;
using System.IO;

public class CockpitTravel : MonoBehaviour
{
    public LineRenderer CurveLine;
    public GameObject TravelObjectParent;
    public GameObject Cockpit;
    public LineRenderer TangentLine;
    public LineRenderer NormalLine;
    public LineRenderer BinormalLine;
    public Transform CockpitRegulator;

    private float _updateTimer;

    private WaypointManager wpm;
    private List<Vector3> curvePoints;
    private List<Vector3> tangentPositions;
    private List<Vector3> normalPositions;
    private List<Vector3> binormalPositions;
    private int index;
    private int size;
    private bool is3D;
    private float timeThreshold;

    private string path = "Assets/Resources/linecoords.txt";
    private float minThreshold = 0.02f; //fastest travel
    private float maxThreshold = 0.1f;  //slowest travel

    // Start is called before the first frame update
    void Start()
    {
        tangentPositions = new List<Vector3>();
        normalPositions = new List<Vector3>();
        binormalPositions = new List<Vector3>();
        curvePoints = new List<Vector3>();
        index = 0;
        timeThreshold = (minThreshold + maxThreshold) / 2;

        using (StreamReader reader = new StreamReader(path)) {
            if(int.Parse(reader.ReadLine()) == 3) {
                is3D = true;
            } else
            {
                is3D = false;
                BinormalLine.gameObject.SetActive(false);   //binormal is only in 3rd dimension
            }

            string str = "";
            size = int.Parse(reader.ReadLine());
            CurveLine.positionCount = size;
            for(int i = 0; i < size; i++)
            {
                //read in current point xyz coordinates
                str = reader.ReadLine();
                string[] xyz = str.Split(' ');
                Vector3 coords = new Vector3(float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2]));
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
            }
        }
        wpm = new WaypointManager(curvePoints.ToArray(), 0.1f);

    }

    // Update is called once per frame
    void Update()
    {
        // Update time since last point step
        _updateTimer += Time.deltaTime;

        changeSpeed();
                
        // If the time threshold has been reached, traverse to next point
        if(_updateTimer >= timeThreshold)
        {
            _updateTimer = 0f;

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

            var pos = wpm.GetWaypoint();
            var target = wpm.GetFollowupWaypoint();
            var dist = Vector3.Distance(pos, target);
            TravelObjectParent.transform.LookAt(target);
            TravelObjectParent.transform.position = wpm.Move(pos, dist);

            //without waypoint manager
            /*Vector3 nextPosition = curvePoints[index];
            TravelObjectParent.transform.LookAt(nextPosition);
            TravelObjectParent.transform.position = nextPosition;
            index++;

            if(index == size) { index = 0; }*/
        }
    }

    void changeSpeed()
    {
        float offset = CockpitRegulator.GetComponent<VRClampDirection>().OffsetX;
        float regulatorPos = CockpitRegulator.localPosition.x;
        //timeThreshold = (regulatorPos - (-1 * offset)) * (maxThreshold - minThreshold) / (offset - (-1 * offset)) + minThreshold;
        //map regulator position to range of speeds for cockpit
        timeThreshold = (regulatorPos - offset) * (maxThreshold - minThreshold) / ((-1 * offset) - offset) + minThreshold;
    }
}
