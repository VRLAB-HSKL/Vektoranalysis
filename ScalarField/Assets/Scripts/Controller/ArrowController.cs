using UnityEngine;

namespace Controller
{
    /// <summary>
    /// Controller class for the Arrow Prefab.
    /// Can be used for other arrow prefabs. Make sure the mesh is initially pointing in the direction
    /// of the forward vector (0.0, 0.0, 1.0), because Transform.LookAt() is used to point the arrow
    /// </summary>
    public class ArrowController : MonoBehaviour
    {
        /// <summary>
        /// Points the arrow in the specified direction.
        /// </summary>
        /// <param name="target"></param>
        public void PointTowards(Vector3 target)
        {   
            transform.LookAt(target);        
        }
    }
}
