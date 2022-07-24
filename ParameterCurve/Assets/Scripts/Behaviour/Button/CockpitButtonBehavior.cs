using Controller;
using Model;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;
using HTC.UnityPlugin.Vive;

namespace Behaviour.Button
{
    /// <summary>
    /// Button Behaviour used to switch to the previous curve in the current dataset
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
        /// Path to text file
        /// </summary>
        private string path = "Assets/Resources/linecoords.txt";


        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update
        /// </summary>
        protected new void Start()
        {
            base.Start();
            gameObject.SetActive(GlobalDataModel.InitFile.ApplicationSettings.TableSettings.ShowNavButtons);
            YesButton.onClick.AddListener(exit);
            CancelButton.onClick.AddListener(cancel);
        }

        #region Clicked
        protected override void HandleButtonEvent()
        {
            ExitConfirmationPanel.SetActive(true);
            DisplayViewParent.SetActive(false);
        }

        IEnumerator WriteCoordsData()
        {
            bool is3D = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].Is3DCurve;
            using (StreamWriter writer = new StreamWriter(path))
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

                writer.WriteLine(line.positionCount);

                //write points of current linerenderer to text file to be read by new line renderer
                for (int i = 0; i < line.positionCount; i++)
                {
                    float x = line.GetPosition(i).x;
                    //subtract 1 to offset parent TableCurve height
                    float y = line.GetPosition(i).y - 1;
                    //add 2.5 to offset table position 
                    float z = line.GetPosition(i).z + 2.5f;

                    //writer.WriteLine(x + " " + y + " " + z);
                    //table display is already flat, no need to rotate
                    //scale up size to match cockpit
                    writer.WriteLine(35 * x + " " + 35 * y + " " + 35 * z);

                    FresnetSerretApparatus fsr = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].FresnetApparatuses[i];
                    float tangentX = fsr.Tangent.normalized.x;
                    float tangentY = fsr.Tangent.normalized.y;
                    float tangentZ = fsr.Tangent.normalized.z;
                    float normalX = fsr.Normal.normalized.x;
                    float normalY = fsr.Normal.normalized.y;
                    float normalZ = fsr.Normal.normalized.z;
                    float binormalX = fsr.Binormal.normalized.x;
                    float binormalY = fsr.Binormal.normalized.y;
                    float binormalZ = fsr.Binormal.normalized.z;

                    float coordsScaler = 2.5f;

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

        private void exit()
        {
            StartCoroutine(WriteCoordsData());
        }

        private void cancel()
        {
            ExitConfirmationPanel.SetActive(false);
            DisplayViewParent.SetActive(true);
        }

        private void OnApplicationQuit()
        {
            //clear curve data when application stops running
            File.WriteAllText(path, "");
        }
    }
}
