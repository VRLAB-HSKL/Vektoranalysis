
using System;
using Controller;

namespace Behaviours.Button
{
    public class PreviousDatasetButtonBehaviour : AbstractButtonBehaviour
    {
        public WorldStateController world;


        private void Start()
        {
            gameObject.SetActive(GlobalData.initFile.ApplicationSettings.TableSettings.ShowNavButtons);
        }

        public override void HandleButtonEvent()
        {
            world.SwitchToPreviousDataset();
        }
    }
}
