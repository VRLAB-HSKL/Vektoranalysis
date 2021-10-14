
using System;

namespace Behaviours.Button
{
    public class NextDatasetButtonBehaviour : AbstractButtonBehaviour
    {
        public WorldStateController world;


        private void Start()
        {
            gameObject.SetActive(GlobalData.initFile.ApplicationSettings.TableSettings.ShowNavButtons);
        }

        public override void HandleButtonEvent()
        {
            world.SwitchToNextDataset();
        }
    }
}
