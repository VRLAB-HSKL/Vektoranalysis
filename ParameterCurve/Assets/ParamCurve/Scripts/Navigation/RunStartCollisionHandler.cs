using Model;
using ParamCurve.Scripts.Controller;
using UnityEngine;

namespace ParamCurve.Scripts.Navigation
{
    /// <summary>
    /// Starts run on object collision
    /// </summary>
    public class RunStartCollisionHandler : AbstractButtonCollisionHandler
    {
        #region Public functions
        
        /// <summary>
        /// Assign local function to inherited handler
        /// </summary>
        public RunStartCollisionHandler()
        {
            OnHitFunc = HandleCollision;
        }
        
        #endregion Public functions

        #region Private functions
        
        /// <summary>
        /// Handles collision with target object
        /// </summary>
        private void HandleCollision()
        {
            // Logical operation
            var world = target.GetComponent<WorldStateController>();
            if (world != null)
            {
                if (!GlobalDataModel.IsRunning)
                {
                    //world.StartRun();
                }

            }

            // Visual update
            var msr = target.GetComponent<MeshRenderer>();
            if (msr != null)
            {
                msr.material = pressedMat;
            }
        }
        
        #endregion Private functions
    }
}
