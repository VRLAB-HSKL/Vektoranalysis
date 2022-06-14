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

    private float _updateTimer;
    private int index;

    private WaypointManager wpm;

    private string path = "Assets/Resources/linecoords.txt";

    // Start is called before the first frame update
    void Start()
    {
        /*int n = 500;
        float xLength = 10f;
        float yLength = 10f;

        float stepFactor = 1f / n;
        
        float xStep = xLength * stepFactor;
        float yStep = yLength * stepFactor;
        
        
        var list = new List<Vector3>();
        for (int i = 0; i < n; i++)
        {
            float x = xStep * i;
            float y = yStep * i;
            list.Add(new Vector3(x, y, 0f));
        }
        

        CurveLine.positionCount = n;
        CurveLine.SetPositions(list.ToArray());*/

        var list = new List<Vector3>();
        using (StreamReader reader = new StreamReader(path)) {
            string pos = "";
            int size = int.Parse(reader.ReadLine());
            CurveLine.positionCount = size;
            for(int i = 0; i < size; i++)
            {
                pos = reader.ReadLine();
                string[] xyz = pos.Split(' ');
                Vector3 coords = new Vector3(float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2]));
                CurveLine.SetPosition(i, coords);
                list.Add(coords);
            }
        }
        wpm = new WaypointManager(list.ToArray(), 0.1f);

        //Debug.Log(CurveLine.positionCount);
    }

    // Update is called once per frame
    void Update()
    {
        // Update time since last point step
        _updateTimer += Time.deltaTime;
                
        // If the time threshold has been reached, traverse to next point
        if(_updateTimer >= 0.1f)
        {
            _updateTimer = 0f;

            //TravelObjectParent.transform.position = CurveLine.GetPosition(index);
            var pos = wpm.GetWaypoint();
            var target = wpm.GetFollowupWaypoint();
            var dist = Vector3.Distance(pos, target);
            TravelObjectParent.transform.LookAt(target);
            TravelObjectParent.transform.position = wpm.Move(pos, dist);
            
            ++index;
        }
    }
}
