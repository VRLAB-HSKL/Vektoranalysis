
using System;
using Controller;

namespace Behaviours.Button
{
    public class PreviousDatasetButtonBehaviour : AbstractButtonBehaviour
    {
        public WorldStateController world;


        private new void Start()
        {
            base.Start();
            gameObject.SetActive(GlobalData.initFile.ApplicationSettings.TableSettings.ShowNavButtons);
        }

        protected override void HandleButtonEvent()
        {
            world.SwitchToPreviousDataset();
        }
    }
}
