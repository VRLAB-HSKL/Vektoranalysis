using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using HTC.UnityPlugin.Vive;
using Model;
using ImmersiveVolumeGraphics.ModelEdit;

public class DrawTangentNormal : MonoBehaviour
{
    public GameObject drawCurveDisplay;
    public LineRenderer worldCurve;
    public GameObject tangentSphereParent;
    public GameObject normalSphereParent;
    public LineRenderer tangentSolutionLine;
    public LineRenderer normalSolutionLine;
    public GameObject heightAdjustmentObject;

    private GameObject pointSphere;
    private GameObject tangentSphere;
    private GameObject normalSphere;

    private LineRenderer drawCurveLR;
    private LineRenderer tangentSphereLR;
    private LineRenderer normalSphereLR;

    private BasicGrabbable tangentGrab;
    private BasicGrabbable normalGrab;

    private bool tangentDrawn, normalDrawn;
    private int pointIndex = 200;
    private VRMoveWithObject heightAdjustment;

    // Start is called before the first frame update
    void Start()
    {
        drawCurveLR = drawCurveDisplay.GetComponent<LineRenderer>();
        heightAdjustment = heightAdjustmentObject.GetComponent<VRMoveWithObject>();
        tangentDrawn = false;
        normalDrawn = false;
        generateCurve();
    }

    // Update is called once per frame
    void Update()
    {
        //show solution on grip press
        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Grip))
        {
            compareVectors();
        }

        //calculate distance from tan/norm spheres to point sphere
        float tangentDistance = Vector3.Distance(pointSphere.transform.position, tangentSphere.transform.position);
        float normalDistance = Vector3.Distance(pointSphere.transform.position, normalSphere.transform.position);
        float thresholdDistance = 0.55f;

        //if calculated distance is below threshold, draw the line from the point sphere to the tan/norm sphere
        if (tangentDistance < thresholdDistance)
        {
            //override position from controller to make sure line connects to sphere instead for 2D curves
            if (tangentGrab.isGrabbed && !GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].Is3DCurve)
                tangentSphere.transform.position = new Vector3(tangentSphere.transform.position.x, tangentSphere.transform.position.y, 0);
            
            tangentSphereLR.SetPosition(0, pointSphere.transform.position + new Vector3(0, 0, -0.005f));
            tangentSphereLR.SetPosition(1, tangentSphere.transform.position + new Vector3(0, 0, -0.005f));
            tangentDrawn = true;
        } else
        {
            tangentSphereLR.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
            tangentDrawn = false;
        }

        if (normalDistance < thresholdDistance)
        {
            //override position from controller to make sure line connects to sphere instead for 2D curves
            if (normalGrab.isGrabbed && !GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].Is3DCurve)
                normalSphere.transform.position = new Vector3(normalSphere.transform.position.x, normalSphere.transform.position.y, 0);

            normalSphereLR.SetPosition(0, pointSphere.transform.position + new Vector3(0, 0, 0.005f));
            normalSphereLR.SetPosition(1, normalSphere.transform.position + new Vector3(0, 0, 0.005f));
            normalDrawn = true;
        } else
        {
            normalSphereLR.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
            normalDrawn = false;
        }
    }

    /// <summary>
    /// called when the curve changes. will update the drawing curve display and generate new spheres for new curve
    /// </summary>
    public void generateCurve()
    {
        for (int i = 0; i < worldCurve.positionCount; i++)
        {
            //half the size of the world curve
            float x = worldCurve.GetPosition(i).x / 2f + 4.5f;  //x offset from world curve
            float y = worldCurve.GetPosition(i).y / 2f + 0.25f; //y offset for axes height
            float z = worldCurve.GetPosition(i).z / 2f;
            drawCurveLR.SetPosition(i, new Vector3(x, y, z));
        }

        //update reference positions for new curve
        heightAdjustment.updateLR(drawCurveDisplay.name);
        heightAdjustment.resetPositions();
        
        //create spheres for new curve
        generateSpheres(pointIndex);

        //update reference positions for new tan/normal line solutions
        heightAdjustment.updateLR(tangentSolutionLine.name);
        heightAdjustment.updateLR(normalSolutionLine.name);
    }

    /// <summary>
    /// Calculate difference between user and data tan/norm lines then show solution
    /// </summary>
    private void compareVectors()
    {
        FresnetSerretApparatus fsr = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].FresnetApparatuses[pointIndex];
        Vector3 tangent = fsr.Tangent.normalized;
        Vector3 normal = fsr.Normal.normalized;
        //Vector3 binormal = fsr.Binormal;

        Vector3 userTangent = tangentSphereLR.GetPosition(1) - tangentSphereLR.GetPosition(0);
        Vector3 userNormal = normalSphereLR.GetPosition(1) - normalSphereLR.GetPosition(0);

        //calculate angle difference between user generated line and the tan/norm lines from the data model
        float tanAngle = Vector3.Angle(userTangent, tangent);
        float normAngle = Vector3.Angle(userNormal, normal);

        Debug.Log("angle b/t tangents: " + tanAngle);
        Debug.Log("angle b/t normals: " + normAngle);

        //also allow wrong direction tangent
        if (!tangentDrawn) Debug.Log("tangent not drawn");
        else if (tanAngle < 20 || (tanAngle > 160 && tanAngle < 200)) Debug.Log("tangent: correct");
        else Debug.Log("tangent: incorrect");

        //allow wrong direction normal also
        if (!normalDrawn) Debug.Log("normal not drawn");
        else if (normAngle < 20 || (normAngle > 160 && normAngle < 200)) Debug.Log("normal: correct");
        else Debug.Log("normal: incorrect");

        //before solution is shown, reset height adjustment
        heightAdjustment.resetPositions();
        showSolution(tangent, normal);
    }

    /// <summary>
    /// Create point sphere and tan/norm spheres for that point
    /// </summary>
    /// <param name="pointIndex">position on line renderer to render point sphere</param>
    private void generateSpheres(int pointIndex)
    {
        //destroy spheres for old curve if there are any
        if(pointSphere != null) Destroy(pointSphere);
        if (tangentSphere != null) Destroy(tangentSphere);
        if (normalSphere != null) Destroy(normalSphere);

        //reset solution lines
        tangentSolutionLine.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
        normalSolutionLine.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });

        //bounding box dimensions
        Vector3 bbDimensions = new Vector3(2.5f, 2.5f, 2.5f);

        //generate sphere on curve
        pointSphere = Utility.DrawingUtility.DrawSphereOnLine(drawCurveDisplay, pointIndex, bbDimensions);
        //make sure sphere cannot be grabbed with tangent/normal spheres
        pointSphere.GetComponent<SphereCollider>().enabled = false;

        //generate tangent and normal spheres
        float pointSphereX = pointSphere.transform.position.x - 4.5f;   //account for x offset
        float pointSphereY = pointSphere.transform.position.y - 1.25f;  //account for height of axes
        float spawnOffset = 0.6f;
        Vector3 tangentSpherePos, normalSpherePos;

        //sphere positions spawn depending on which XY quadrant they are in
        //1st quadrant: up and right, 2nd quadrant: up and left, 3rd quadrant: down and left, 4th quadrant: down and right
        if (pointSphereX >= 0 && pointSphereY >= 0)
        {
            //Debug.Log(pointSphereX + ", " + pointSphereY + ": both pos");
            tangentSpherePos = pointSphere.transform.position + new Vector3(0, spawnOffset, 0);
            normalSpherePos = pointSphere.transform.position + new Vector3(spawnOffset, 0, 0);
        }
        else if (pointSphereX <= 0 && pointSphereY >= 0)
        {
            //Debug.Log(pointSphereX + ", " + pointSphereY + ": x neg, y pos");
            tangentSpherePos = pointSphere.transform.position + new Vector3(-spawnOffset, 0, 0);
            normalSpherePos = pointSphere.transform.position + new Vector3(0, spawnOffset, 0);
        }
        else if (pointSphereX <= 0 && pointSphereY <= 0)
        {
            //Debug.Log(pointSphereX + ", " + pointSphereY + ": both neg");
            tangentSpherePos = pointSphere.transform.position + new Vector3(0, -spawnOffset, 0);
            normalSpherePos = pointSphere.transform.position + new Vector3(-spawnOffset, 0, 0);
        }
        else
        {
            //Debug.Log(pointSphereX + ", " + pointSphereY + ": y neg, x pos");
            tangentSpherePos = pointSphere.transform.position + new Vector3(spawnOffset, 0, 0);
            normalSpherePos = pointSphere.transform.position + new Vector3(0, -spawnOffset, 0);
        }

        //create spheres at parent object positions
        tangentSphere = Utility.DrawingUtility.DrawSphere(tangentSpherePos, tangentSphereParent.transform, Color.red, bbDimensions);
        normalSphere = Utility.DrawingUtility.DrawSphere(normalSpherePos, normalSphereParent.transform, Color.blue, bbDimensions);

        //make the new spheres grabbable
        tangentGrab = tangentSphere.AddComponent<BasicGrabbable>();
        normalGrab = normalSphere.AddComponent<BasicGrabbable>();

        //get line renderers to give them lines to connect to pointSphere
        tangentSphereLR = tangentSphereParent.GetComponent<LineRenderer>();
        normalSphereLR = normalSphereParent.GetComponent<LineRenderer>();

        //if 2d curve, restrict curve movement in z direction
        if (!GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].Is3DCurve)
        {
            ConstraintSource cs = new ConstraintSource();
            cs.sourceTransform = pointSphere.transform;
            cs.weight = 1;

            PositionConstraint tanPC = tangentSphere.AddComponent<PositionConstraint>();
            tanPC.translationAxis = Axis.Z;
            tanPC.AddSource(cs);
            tanPC.locked = true;
            tanPC.constraintActive = true;

            PositionConstraint normPC = normalSphere.AddComponent<PositionConstraint>();            
            normPC.translationAxis = Axis.Z;
            normPC.AddSource(cs);
            normPC.locked = true;
            normPC.constraintActive = true;
        }
    }

    /// <summary>
    /// Show tan/norm line solution
    /// </summary>
    /// <param name="tangent">Normalized tangent vector for current point sphere location from global data model</param>
    /// <param name="normal">Normalized normal vector for current point sphere location from global data model</param>
    private void showSolution(Vector3 tangent, Vector3 normal)
    {
        tangentSolutionLine.SetPosition(0, pointSphere.transform.position);
        tangentSolutionLine.SetPosition(1, pointSphere.transform.position + new Vector3(tangent.x, tangent.y, tangent.z));

        normalSolutionLine.SetPosition(0, pointSphere.transform.position);
        normalSolutionLine.SetPosition(1, pointSphere.transform.position + new Vector3(normal.x, normal.y, normal.z));

        heightAdjustment.updateLR(tangentSolutionLine.name);
        heightAdjustment.updateLR(normalSolutionLine.name);
    }
}
