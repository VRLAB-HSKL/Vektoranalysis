using System.Collections;
using System.IO;
using Model;
using ParamCurve.Scripts.Controller;
using ParamCurve.Scripts.Model;
using ParamCurve.Scripts.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ParamCurve.Scripts.Behaviour.Button
{
    /// <summary>
    /// Button Behaviour used to move from main display room to cockpit view
    /// </summary>
    public class CockpitButtonBehavior : AbstractButtonBehaviour
    {
        /// <summary>
        /// Single world state controller instance <see cref="WorldStateController"/>
        /// </summary>
        public WorldStateController world;

        /// <summary>
        /// Line renderer for current curve on table display
        /// </summary>
        public LineRenderer line;

        public TubeMesh lineMesh;
        
        
        /// <summary>
        /// Panel confirming that the user wants to enter cockpit mode, contains yes and cancel buttons
        /// </summary>
        public GameObject ExitConfirmationPanel;
        public UnityEngine.UI.Button YesButton;
        public UnityEngine.UI.Button CancelButton;

        /// <summary>
        /// World Curve Display, to be hidden when confirming travel to cockpit mode
        /// </summary>
        public GameObject DisplayViewParent;


        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update
        /// </summary>
        protected new void Start()
        {
            base.Start();
            //gameObject.SetActive(GlobalDataModel.InitFile.ApplicationSettings.TableSettings.ShowNavButtons);
            YesButton.onClick.AddListener(Exit);
            CancelButton.onClick.AddListener(Cancel);
        }

        #region Clicked
        protected override void HandleButtonEvent()
        {
            ExitConfirmationPanel.SetActive(true);
            DisplayViewParent.SetActive(false);
        }

        IEnumerator WriteCoordsData()
        {
            // ToDo: Remove this!
            var path = Application.persistentDataPath  + "/linecoords.txt";
            
            Debug.Log("BehaviourPath: " + path);
            
            bool is3D = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].Is3DCurve;
            using (StreamWriter writer = new StreamWriter(path, append: false))
            {
                string name = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].Name;
                writer.WriteLine(name);
        
                if (is3D)
                {
                    writer.WriteLine("3");
                }
                else
                {
                    writer.WriteLine("2");
                }                
        
                writer.WriteLine(lineMesh.polyline.Count);
        
                //write points of current linerenderer to text file to be read by new line renderer
                for (int i = 0; i < lineMesh.polyline.Count; i++)
                {
                    var x = lineMesh.polyline[i].x;
                    //subtract 1 to offset parent TableCurve height
                    var y = lineMesh.polyline[i].y - 1;
                    //add 2.5 to offset table position 
                    var z = lineMesh.polyline[i].z + 2.5f;
        
                    //writer.WriteLine(x + " " + y + " " + z);
                    //table display is already flat, no need to rotate
                    //scale up size to match cockpit
                    var scaleFactor = 1f;//35f;
                    writer.WriteLine(scaleFactor * x + " " + scaleFactor * y + " " + scaleFactor * z);
        
                    FresnetSerretApparatus fsr = 
                        GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].FresnetApparatuses[i];
                    var tangentX = fsr.Tangent.normalized.x;
                    var tangentY = fsr.Tangent.normalized.y;
                    var tangentZ = fsr.Tangent.normalized.z;
                    var normalX = fsr.Normal.normalized.x;
                    var normalY = fsr.Normal.normalized.y;
                    var normalZ = fsr.Normal.normalized.z;
                    var binormalX = fsr.Binormal.normalized.x;
                    var binormalY = fsr.Binormal.normalized.y;
                    var binormalZ = fsr.Binormal.normalized.z;
        
                    var coordsScaler = 2.5f;
        
                    //write tan/norm/binorm data
                    if (!is3D)
                    {
                        //Debug.Log("not 3D");
                        writer.WriteLine(coordsScaler * tangentX + " " + coordsScaler * tangentZ + " " + coordsScaler * tangentY);
                        writer.WriteLine(coordsScaler * normalX + " " + coordsScaler * normalZ + " " + coordsScaler * normalY);
        
                        //for 2D curves, binormal is in 3rd dimension so do not write it 
                        //writer.WriteLine(coordsScaler * binormalX + " " + coordsScaler * binormalZ + " " + coordsScaler * binormalY);
                    }
                    else
                    {
                        //Debug.Log("3D");
                        writer.WriteLine(coordsScaler * tangentX + " " + coordsScaler * tangentY + " " + coordsScaler * tangentZ);
                        writer.WriteLine(coordsScaler * normalX + " " + coordsScaler * normalY + " " + coordsScaler * normalZ);
                        writer.WriteLine(coordsScaler * binormalX + " " + coordsScaler * binormalY + " " + coordsScaler * binormalZ);
                    }
        
                    //write data for time and veclocity graphs
                    float timeDistX = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].TimeDistancePoints[i].x;
                    float timeDistY = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].TimeDistancePoints[i].y;
                    writer.WriteLine(timeDistX + " " + timeDistY);
        
                    float timeVelX = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].TimeVelocityPoints[i].x;
                    float timeVelY = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].TimeVelocityPoints[i].y;
                    writer.WriteLine(timeVelX + " " + timeVelY);
        
                }
                //yield return null;
            }
        
            yield return null;
        
            var asyncOp = SceneManager.LoadSceneAsync("CockpitScene");
        
            while (!asyncOp.isDone)
            {
                yield return null;
            }
        }

        #endregion

        private void Exit()
        {
            StartCoroutine(WriteCoordsData());
        }

        private void Cancel()
        {
            ExitConfirmationPanel.SetActive(false);
            DisplayViewParent.SetActive(true);
        }

    }
}
