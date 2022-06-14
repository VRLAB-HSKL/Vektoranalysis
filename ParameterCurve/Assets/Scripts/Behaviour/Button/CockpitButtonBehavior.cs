using Controller;
using Model;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

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
        /// Line renderer for current curve on display
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
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(line.positionCount);
                
                //write points of current linerenderer to text file to be read by new line renderer
                for (int i = 0; i < line.positionCount; i++)
                {
                    float x = line.GetPosition(i).x;
                    //subtract 2 to offset parent DisplayView height
                    float y = line.GetPosition(i).y - 2;
                    float z = line.GetPosition(i).z;

                    //writer.WriteLine(x + " " + y + " " + z);
                    //rotate to be flat rather than vertical
                    //scale up size to match cockpit
                    writer.WriteLine(5*x + " " + -5*z + " " + 5*y);
                }
            }
            //SceneManager.LoadScene("/Assets/Scenes/CockpitScene");
        }
        #endregion
    }
}
