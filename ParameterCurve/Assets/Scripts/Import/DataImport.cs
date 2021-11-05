using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Import;
using Import.NewInitFile;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Unity.Rendering.HybridV2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Interactions;

public static class DataImport
{
    public static float TimeDistanceXAxisLength { get; set; }
    public static float TimeDistanceYAxisLength;

    public static float TimeVelocityXAxisLength;
    public static float TimeVelocityYAxisLength;


    private static NumberFormatInfo nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };


    public static CurveInformationDataset CreatePointDatasetFromCurve(Curve curve)
    {
        CurveInformationDataset pd = new CurveInformationDataset
        {
            id = curve.Info.Id,
            Name = curve.Info.Name,
            DisplayString = curve.Info.Name,
            NotebookURL = string.Empty
        };



        var lineColorValues = curve.CurveSettings.DisplaySettings.LineColor;
        
        Color lineColor = new Color(lineColorValues.Rgba[0], lineColorValues.Rgba[1], 
                                    lineColorValues.Rgba[2], lineColorValues.Rgba[3]);
        
        pd.CurveLineColor = lineColor;

        var travelObjValArr= curve.CurveSettings.DisplaySettings.TravelObjColor.Rgba;
        Color travelObjColor = new Color(travelObjValArr[0], travelObjValArr[1], 
                                         travelObjValArr[2], travelObjValArr[3]);
        pd.TravelObjColor = travelObjColor;
        
        var arcTravelObjValArr= curve.CurveSettings.DisplaySettings.ArcTravelObjColor.Rgba;
        Color arcTravelObjColor = new Color(arcTravelObjValArr[0], arcTravelObjValArr[1], 
                                            arcTravelObjValArr[2], arcTravelObjValArr[3]);
        pd.ArcTravelObjColor = arcTravelObjColor;
        

        // Attempt to load image resource based on curve name        
        string imgResPath = GlobalData.ImageResourcePath + curve.Info.Name; 
        Texture2D imgRes = Resources.Load(imgResPath) as Texture2D;  
                                                             
        if (imgRes != null)                                          
        {                                                            
            pd.MenuButtonImage = imgRes;                            
        }                                          
        
        
        pd.Is3DCurve = curve.Data.Dimension == 3;
        pd.arcLength = curve.Data.ArcLength;
        pd.WorldScalingFactor = curve.Data.WorldScalingFactor;
        pd.TableScalingFactor = curve.Data.TableScalingFactor;
        pd.SelectExercisePillarScalingFactor = curve.Data.SelectExercisePillarScalingFactor;

        pd.paramValues = curve.Data.Data.T;

        pd.arcLength = curve.Data.ArcLength;
        
        for (int j = 0; j < curve.Data.Data.T.Count; j++)
        {
            pd.points.Add(new Vector3(
                curve.Data.Data.PVec[j][0],
                curve.Data.Data.PVec[j][1],
                pd.Is3DCurve ? curve.Data.Data.PVec[j][2] : 0f
                ));

            pd.timeDistancePoints = CalculateTimeDistancePoints(pd.points);
            pd.timeVelocityPoints = CalculateTimeVelocityPoints(pd.points);
            
            FresnetSerretApparatus fsr = new FresnetSerretApparatus();

            fsr.Tangent = new Vector3(
                curve.Data.Data.VelVec[j][0],
                curve.Data.Data.VelVec[j][1],
                pd.Is3DCurve ? curve.Data.Data.VelVec[j][2] : 0f
            ).normalized;
            
            fsr.Normal = new Vector3(
                curve.Data.Data.AccVec[j][0],
                curve.Data.Data.AccVec[j][1],
                pd.Is3DCurve ? curve.Data.Data.AccVec[j][2] : 0f
            ).normalized;

            fsr.Binormal = Vector3.Cross(fsr.Tangent, fsr.Normal);
            
            pd.fresnetApparatuses.Add(fsr);

            pd.arcLengthParamValues = curve.Data.Data.ArcT;
            
            pd.arcLenghtPoints.Add(new Vector3(
                curve.Data.Data.ArcPVec[j][0],
                curve.Data.Data.ArcPVec[j][1],
                pd.Is3DCurve ? curve.Data.Data.ArcPVec[j][2] : 0f
            ));

            FresnetSerretApparatus arcFsr = new FresnetSerretApparatus();
            
            arcFsr.Tangent = new Vector3(
                curve.Data.Data.ArcVelVec[j][0],
                curve.Data.Data.ArcVelVec[j][1],
                pd.Is3DCurve ? curve.Data.Data.ArcVelVec[j][2] : 0f
            );
            
            arcFsr.Normal = new Vector3(
                curve.Data.Data.ArcAccVec[j][0],
                curve.Data.Data.ArcAccVec[j][1],
                pd.Is3DCurve ? curve.Data.Data.ArcAccVec[j][2] : 0f
            );

            arcFsr.Binormal = Vector3.Cross(arcFsr.Tangent, arcFsr.Normal);
            
            pd.arcLengthFresnetApparatuses.Add(arcFsr);

        }
        
        pd.CalculateWorldPoints();

        return pd;
    }
    
    public static ExercisePointDataset CreateExercisePointDatasetFromSubexercise(SubExercise sub)
    {
        ExercisePointDataset epd = new ExercisePointDataset(
            sub.Description,
            CreatePointDatasetFromCurve(sub.LeftCurve),
            CreatePointDatasetFromCurve(sub.MiddleCurve),
            CreatePointDatasetFromCurve(sub.RightCurve)
            
            );

        return epd;
    }
    
    
    private static List<Vector2> CalculateTimeDistancePoints(List<Vector3> points)
    {
        List<Vector2> tdPoints = new List<Vector2>();
        var curvePoints = points;
        int numSteps = points.Count;

        float maxDistance = CalculateRawDistance(curvePoints);
        float currentDistance = 0f;

        //Debug.Log("timeDistanceXAxisLength: " + TimeDistanceXAxisLength);

        for(int i = 0; i < numSteps; i++)
        {
            // Move one step to the right on the horizontal x-axis each iteration
            float x = (i / (float)numSteps) * TimeDistanceXAxisLength; //(i / TimeDistanceXAxisLength); //(float) numSteps);

            Debug.Log("[" + i + "/" + numSteps + "]"+ "Calculated x: " + x);
            
            float y;
            
            // Start at the vertical origin on first iteration
            if (i == 0)
            {
                y = 0f;
            }
            
            // Otherwise calculate vertical distance on y-axis
            else
            {
                // Calculate new vertical offset based on previous point location
                currentDistance += Vector3.Distance(curvePoints[i], curvePoints[i - 1]);
                y = currentDistance;
                y /= maxDistance;
                //y *= TimeDistanceYAxisLength;
            }

            tdPoints.Add(new Vector2(x, y));
        }

        return tdPoints;
    }

    private static List<Vector2> CalculateTimeVelocityPoints(List<Vector3> points)
    {
        List<Vector2> tvPoints = new List<Vector2>();
        var curvePoints = points;
        int numSteps = points.Count;

        //float maxDistance =  CalculateRawDistance(curvePoints);
        //float currentDistance = 0f;
        float maxVelocity = 0f;

        for (int i = 0; i < numSteps; i++)
        {
            float x = (i / (float)numSteps) * TimeVelocityXAxisLength;

            float y;
            if(i == 0)
            {
                y = 0f;
            }
            else
            {
                float distance = Vector3.Distance(curvePoints[i], curvePoints[i - 1]);
                //Debug.Log("VecDist: " + y);
                //currentDistance += d;
                y = distance;

                if(y > maxVelocity)
                {
                    maxVelocity = y;
                }
                //y /= maxDistance;
                //Debug.Log("ScaledVecDist: " + y);
            }

            tvPoints.Add(new Vector2(x, y));
        }

        foreach(Vector2 v in tvPoints)
        {
            v.Set(v.x, v.y / maxVelocity);
        }
        

        return tvPoints;
    }
    
    public static float CalculateRawDistance(List<Vector3> pointList)
    {
        // Only start calculating distance between |n| > 1 points
        if (pointList.Count < 2) return 0f;

        float distance = 0f;
        for(int i = 1; i < pointList.Count; i++)
        {
            // Calculate distance between two points using Unity helper function
            distance += Mathf.Abs(Vector3.Distance(pointList[i - 1], pointList[i]));
        }

        return distance;
    }
    
    
    
    
    // public static PointDataset ImportPointsFromCSVResource(TextAsset txt)
    // {
    //     bool swapYZCoordinates = false;
    //
    //     PointDataset pd = new PointDataset();
    //     pd.Name = txt.name;
    //     pd.DisplayString = txt.name;
    //     pd.NotebookURL = GlobalData.LocalHTMLResourcePath + txt.name + ".html";
    //
    //     string[] lineArr = txt.text.Split('\n'); 
    //
    //     //Skip header
    //     for (int i = 1; i < lineArr.Length; i++)
    //     {
    //         string line = lineArr[i];
    //         if (string.IsNullOrEmpty(line)) continue;
    //
    //         string[] values = line.Split(',');
    //         //Debug.Log("valuesArrSize: " + values.Length);
    //         float t = float.Parse(values[0], nfi);
    //         float x = float.Parse(values[1], nfi);
    //         float y = float.Parse(values[2], nfi);
    //         float z = 0f;
    //         if (values.Length == 4)
    //         {
    //             swapYZCoordinates = true; //Debug.Log("csvIs3dValuesSize: " + values.Length);
    //             z = float.Parse(values[3], nfi);
    //         }
    //         pd.points.Add(new Vector3(x, y, z));
    //         //pd.worldPoints.Add(swapYZCoordinates ?
    //         //    new Vector3(x, z, y) * GlobalData.PointScaleFactor :
    //         //    new Vector3(x, y, z) * GlobalData.PointScaleFactor);
    //         pd.paramValues.Add(t);
    //
    //
    //         FresnetSerretApparatus fsa = new FresnetSerretApparatus();
    //         fsa.Tangent = Vector3.zero;
    //         pd.fresnetApparatuses.Add(fsa);
    //     }
    //     
    //     pd.CalculateWorldPoints();
    //
    //     return pd;
    // }
    //
    //
    //
    // public static PointDataset ImportPointsFromJSONResource(TextAsset json)
    // {
    //     List<string> errors = new List<string>();
    //     ITraceWriter tr = new MemoryTraceWriter();
    //     PointDataJsonRoot jsr = JsonConvert.DeserializeObject<PointDataJsonRoot>(json.text,
    //         new JsonSerializerSettings()
    //         {
    //             Error = delegate(object sender, ErrorEventArgs args)
    //             {
    //                 errors.Add(args.ErrorContext.Error.Message);
    //                 Debug.Log("ErrorOccuredJSON");
    //                 args.ErrorContext.Handled = true;
    //             },
    //             //FloatParseHandling = FloatParseHandling.Double
    //             TraceWriter = tr
    //             //,
    //             //Converters = { new IsoDateTimeConverter()}
    //         }
    //         
    //     );
    //     
    //     //Debug.Log(tr);
    //     
    //     
    //     
    //     
    //     PointDataset pds = new PointDataset();
    //     pds.Name = jsr.Name + "_JSON";
    //     
    //     //Debug.Log("curveName: " + jsr.name);
    //     
    //     pds.DisplayString = jsr.Name;
    //     pds.NotebookURL = GlobalData.LocalHTMLResourcePath + jsr.Name + ".html";
    //
    //     // Temp local host
    //     pds.NotebookURL = "localhost";
    //     
    //     pds.Is3DCurve = jsr.dim == 3;
    //
    //     pds.WorldScalingFactor = jsr.worldScalingFactor;
    //     pds.TableScalingFactor = jsr.tableScalingFactor;
    //     pds.SelectExercisePillarScalingFactor = jsr.selectExercisePillarScalingFactor;
    //     
    //     // Attempt to load image resource based on curve name
    //     string imgResPath = GlobalData.ImageResourcePath + jsr.Name;
    //     Texture2D imgRes = Resources.Load(imgResPath) as Texture2D;
    //
    //     if (imgRes != null)
    //     {
    //         pds.MenuButtonImage = imgRes;
    //     }
    //     
    //     
    //     
    //     
    //     
    //     bool swapYZCoordinates = !pds.Is3DCurve;
    //
    //
    //     for (int i = 0; i < jsr.data.Count; i++)
    //     {
    //         //Debug.Log("pointDataCount: " + jsr.pointData.Count);
    //         
    //         PointData pd = jsr.data[i];
    //         pds.paramValues.Add((float)pd.t);
    //         
    //         pds.points.Add(new Vector3(pd.pVec[0], pd.pVec[1],(pd.pVec.Count == 3 ? pd.pVec[2] : 0f)));
    //
    //         // float tx = float.Parse(pd.tan[0], nfi);
    //         // float ty = float.Parse(pd.tan[1], nfi);
    //         // float tz = float.Parse(pd.tan[2], nfi);
    //         //
    //         // float nx = float.Parse(pd.norm[0], nfi);
    //         // float ny = float.Parse(pd.norm[1], nfi);
    //         // float nz = float.Parse(pd.norm[2], nfi);
    //         //
    //         // float bnx = float.Parse(pd.binorm[0], nfi);
    //         // float bny = float.Parse(pd.binorm[1], nfi);
    //         // float bnz = float.Parse(pd.binorm[2], nfi);
    //
    //         //float t = float.Parse(pd.t, nfi);
    //
    //         
    //         // pds.worldPoints.Add(swapYZCoordinates ?
    //         //     new Vector3(x, z, y) * GlobalData.PointScaleFactor :
    //         //     new Vector3(x, y, z) * GlobalData.PointScaleFactor);
    //         
    //
    //         FresnetSerretApparatus fsp = new FresnetSerretApparatus();
    //         // fsp.Tangent = swapYZCoordinates ?
    //         //     new Vector3(tx, tz, ty) * GlobalData.PointScaleFactor :
    //         //     new Vector3(tx, ty, tz) * GlobalData.PointScaleFactor;
    //
    //         fsp.Tangent = new Vector3((float) pd.velVec[0], (float) pd.velVec[1], (float) (pd.velVec.Count == 3 ? pd.velVec[2] : 0f));
    //         
    //         
    //         // fsp.Normal = swapYZCoordinates ?
    //         //     new Vector3(nx, nz, ny) * GlobalData.PointScaleFactor :
    //         //     new Vector3(nx, ny, nz) * GlobalData.PointScaleFactor;
    //
    //         fsp.Normal = new Vector3((float) pd.accVec[0], (float) pd.accVec[1], (float) (pd.accVec.Count == 3 ? pd.accVec[2] : 0f));
    //         
    //         
    //         // fsp.Binormal = swapYZCoordinates ?
    //         //     new Vector3(bnx, bnz, bny) * GlobalData.PointScaleFactor :
    //         //     new Vector3(bnx, bny, bnz) * GlobalData.PointScaleFactor;
    //
    //         fsp.Binormal = Vector3.Cross(fsp.Tangent, fsp.Normal); 
    //         
    //         pds.fresnetApparatuses.Add(fsp);
    //     }
    //     
    //     pds.CalculateWorldPoints();
    //
    //     return pds;
    // }
    //
    //
    //
    // public static SelectionExercise ImportExerciseFromJSONResource(TextAsset json)
    // {
    //     List<string> errors = new List<string>();
    //     Exercise jsr = JsonConvert.DeserializeObject<Exercise>(json.text,
    //         new JsonSerializerSettings()
    //         {
    //             Error = delegate(object sender, ErrorEventArgs args)
    //             {
    //                 errors.Add(args.ErrorContext.Error.Message);
    //                 Debug.Log("ErrorOccuredJSON");
    //                 args.ErrorContext.Handled = true;
    //             },
    //             
    //             //,
    //             //Converters = { new IsoDateTimeConverter()}
    //         }
    //         
    //         );
    //
    //     
    //     //Debug.Log("ErrorCount: " + errors.Count);
    //
    //     for (int i = 0; i < errors.Count; i++)
    //     {
    //         Debug.Log("JsonError [" + i + "]: " + errors[i]);
    //     }
    //
    //     //Debug.Log("jsonRoot: " + jsr.ToString());
    //     
    //     string id = jsr.id;
    //     string title = jsr.title;
    //     string description = jsr.description;
    //     string type = jsr.type;
    //     var subExercises = jsr.subExercises;
    //     
    //     //Debug.Log("subExercisesCount: " + subExercises.Count);
    //
    //     List<ExercisePointDataset> datasets = new List<ExercisePointDataset>();
    //     List<int> correctAnswers = new List<int>();
    //     
    //     var lds = new PointDataset();
    //     var mds = new PointDataset();
    //     var rds = new PointDataset();
    //     
    //     for (int i = 0; i < subExercises.Count; i++)
    //     {
    //         var subExercise = subExercises[i];
    //         
    //         //Debug.Log("subExercise[" + i + "]: " + subExercise.ToString());
    //
    //         lds = CreatePointDatasetFromCurveData(subExercise.leftCurveData);
    //         mds = CreatePointDatasetFromCurveData(subExercise.middleCurveData);
    //         rds = CreatePointDatasetFromCurveData(subExercise.rightCurveData);
    //
    //         var exercisePds = new ExercisePointDataset(subExercise.description, lds, mds, rds);
    //         datasets.Add(exercisePds);
    //         
    //         correctAnswers.Add(subExercise.correctAnswer);
    //     }
    //     
    //     SelectionExercise selExerc = new SelectionExercise(title, description, datasets, correctAnswers);
    //
    //     return selExerc;
    // }
    //
    // public static PointDataset CreateDatasetFormLocalCalculation(AbstractCurveCalc curveCalc)
    // {
    //     // Local Calculation
    //     PointDataset pdsa = new PointDataset
    //     {
    //         Name = curveCalc.Name,
    //         DisplayString = curveCalc.DisplayString,
    //         NotebookURL = GlobalData.LocalHTMLResourcePath + curveCalc.Name + ".html",
    //         Is3DCurve = curveCalc.Is3DCurve
    //     };
    //
    //     string imgResPath = GlobalData.ImageResourcePath + curveCalc.Name;
    //     Texture2D imgRes = Resources.Load(imgResPath) as Texture2D;
    //
    //     if (imgRes != null)
    //     {
    //         pdsa.MenuButtonImage = imgRes;
    //     }
    //
    //     pdsa.paramValues = new List<float>(curveCalc.ParameterIntervall);
    //     pdsa.points = curveCalc.CalculatePoints();
    //
    //     // for (int i = 0; i < pdsa.points.Count; i++)
    //     // {
    //     //     Vector3 pv = pdsa.points[i];
    //     //     pdsa.worldPoints.Add(curveCalc.Is3DCurve ?
    //     //         new Vector3(pv.x, pv.z, pv.y) * GlobalData.PointScaleFactor :
    //     //         pv * GlobalData.PointScaleFactor);
    //     // }
    //
    //     pdsa.CalculateWorldPoints();
    //     
    //
    //     pdsa.fresnetApparatuses = curveCalc.CalculateFresnetApparatuses();
    //     pdsa.timeDistancePoints = curveCalc.CalculateTimeDistancePoints();        
    //
    //     for(int i = 0; i < pdsa.timeDistancePoints.Count; i++)
    //     {
    //         Vector2 p = pdsa.timeDistancePoints[i];
    //         p.x *= TimeDistanceXAxisLength;
    //         p.y *= TimeDistanceYAxisLength;            
    //         pdsa.timeDistancePoints[i] = new Vector2(p.x, p.y);
    //     }
    //
    //     pdsa.timeVelocityPoints = curveCalc.CalculateTimeVelocityPoints();
    //
    //     for(int i = 0; i < pdsa.timeVelocityPoints.Count; i++)
    //     {
    //         Vector2 p = pdsa.timeVelocityPoints[i];
    //         p.x *= TimeVelocityXAxisLength;
    //         p.y *= TimeVelocityYAxisLength;
    //         pdsa.timeVelocityPoints[i] = new Vector2(p.x, p.y);            
    //     }
    //
    //     pdsa.Distance = AbstractCurveCalc.CalculateRawDistance(pdsa.points);
    //
    //     // Calculate arc length param valus
    //     //var initParamIntervall = curveCalc.ParameterIntervall;
    //     
    //     pdsa.arcLengthParamValues = curveCalc.CalculateArcLengthParamRange();
    //     //curveCalc.ParameterIntervall = pdsa.arcLengthParamValues;
    //     
    //     pdsa.arcLength = pdsa.arcLengthParamValues[pdsa.arcLengthParamValues.Count - 1];      
    //     pdsa.arcLenghtPoints = curveCalc.CalculateArcLengthParameterizedPoints(); //curveCalc.CalculatePoints();
    //
    //     for (int i = 0; i < pdsa.arcLenghtPoints.Count; i++)
    //     {
    //         Vector3 pv = pdsa.arcLenghtPoints[i];
    //         pdsa.arcLengthWorldPoints.Add(curveCalc.Is3DCurve ?
    //             new Vector3(pv.x, pv.z, pv.y) * GlobalData.PointScaleFactor :
    //             pv * GlobalData.PointScaleFactor);
    //     }
    //
    //     pdsa.arcLengthFresnetApparatuses = curveCalc.CalculateFresnetApparatuses();
    //
    //     //curveCalc.ParameterIntervall = initParamIntervall;
    //
    //     return pdsa;
    // }

    
    // private static PointDataset CreatePointDatasetFromCurveData(CurveData curve)
    // {
    //     var pds = new PointDataset
    //     {
    //         Is3DCurve = curve.dim == 3,
    //         arcLength = curve.arcLength
    //     };
    //
    //     for (int l = 0; l < curve.data.Count; ++l)
    //     {
    //         var tData = curve.data[l];
    //         pds.paramValues.Add(tData.t);
    //         pds.points.Add(new Vector3(tData.pVec[0], tData.pVec[1], tData.pVec.Count == 3 ? tData.pVec[2] : 0f));
    //
    //         FresnetSerretApparatus fsa = new FresnetSerretApparatus();
    //         var leftVel = tData.velVec;
    //         fsa.Tangent = new Vector3(leftVel[0], leftVel[1], leftVel.Count == 3 ? leftVel[2] : 0f);
    //
    //         var leftAcc = tData.accVec;
    //         fsa.Normal = new Vector3(leftAcc[0], leftAcc[1], leftAcc.Count == 3 ? leftAcc[2] : 0f);
    //
    //         fsa.Binormal = Vector3.Cross(fsa.Tangent, fsa.Normal);
    //             
    //         pds.fresnetApparatuses.Add(fsa);
    //     
    //         // ToDo: Add arc length parametrization values to json and parse them here
    //         pds.arcLenghtPoints.Add(new Vector3(tData.arcPVec[0], tData.arcPVec[1], tData.arcPVec.Count == 3 ? tData.arcPVec[2] : 0f));
    //
    //         FresnetSerretApparatus arcFsa = new FresnetSerretApparatus
    //         {
    //             Tangent = new Vector3(tData.arcVelVec[0], tData.arcVelVec[1],
    //                 tData.arcVelVec.Count == 3 ? tData.arcVelVec[2] : 0f),
    //             Normal = new Vector3(tData.arcAccVec[0], tData.arcAccVec[1],
    //                 tData.arcAccVec.Count == 3 ? tData.arcAccVec[2] : 0f)
    //         };
    //
    //         arcFsa.Binormal = Vector3.Cross(arcFsa.Tangent, arcFsa.Normal);
    //         
    //         pds.arcLengthFresnetApparatuses.Add(arcFsa);
    //     }
    //     
    //     pds.CalculateWorldPoints();
    //     
    //
    //     return pds;
    // }
}
