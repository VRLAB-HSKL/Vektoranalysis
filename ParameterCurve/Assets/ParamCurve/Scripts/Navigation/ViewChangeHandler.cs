using Model;
using ParamCurve.Scripts.Controller;
using UnityEngine;

namespace ParamCurve.Scripts.Navigation
{
    /// <summary>
    /// Change view based on set <see cref="viewIndex"/> on collision
    /// </summary>
    public class ViewChangeHandler : AbstractButtonCollisionHandler
    {
        #region Public members
        
        /// <summary>
        /// Associated view index this handler should change to
        /// </summary>
        public int viewIndex;

        #endregion Public members
        
        #region Public functions
        
        /// <summary>
        /// Assign local function to inherited handler
        /// </summary>
        public ViewChangeHandler()
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
                GlobalDataModel.WorldCurveViewController.SwitchView(viewIndex);
                GlobalDataModel.TableCurveViewController?.SwitchView(viewIndex);
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
