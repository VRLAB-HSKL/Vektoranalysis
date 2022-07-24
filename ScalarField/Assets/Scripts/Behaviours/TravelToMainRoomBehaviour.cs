using UnityEngine.SceneManagement;
using VR.Scripts.Behaviours.Button;

namespace Behaviours
{
    public class TravelToMainRoomBehaviour : AbstractButtonBehaviour
    {
        protected override void HandleButtonEvent()
        {
            SceneManager.LoadScene(0);
        }
    }
}