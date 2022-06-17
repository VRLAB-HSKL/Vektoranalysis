using Controller;
using Model;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;

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

        }

        /// <summary>
        /// changes appearance of button when pressed
        /// </summary>

        #region Clicked
        protected override void HandleButtonEvent()
        {
            StartCoroutine(WriteCoordsData());
            SceneManager.LoadSceneAsync("CockpitScene");
        }

        IEnumerator WriteCoordsData()
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
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
                    writer.WriteLine(20 * x + " " + 20 * y + " " + 20 * z);

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

                    if (!GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex].Is3DCurve)
                    {
                        //Debug.Log("not 3D");
                        writer.WriteLine(coordsScaler * tangentX + " " + coordsScaler * tangentZ + " " + coordsScaler * tangentY);
                        writer.WriteLine(coordsScaler * normalX + " " + coordsScaler * normalZ + " " + coordsScaler * normalY);
                        writer.WriteLine(coordsScaler * binormalX + " " + coordsScaler * binormalZ + " " + coordsScaler * binormalY);
                    } else
                    {
                        //Debug.Log("3D");
                        writer.WriteLine(coordsScaler * tangentX + " " + coordsScaler * tangentY + " " + coordsScaler * tangentZ);
                        writer.WriteLine(coordsScaler * normalX + " " + coordsScaler * normalY + " " + coordsScaler * normalZ);
                        writer.WriteLine(coordsScaler * binormalX + " " + coordsScaler * binormalY + " " + coordsScaler * binormalZ);
                    }
                }
            }

            yield return null;
        }
        #endregion
    }
}
