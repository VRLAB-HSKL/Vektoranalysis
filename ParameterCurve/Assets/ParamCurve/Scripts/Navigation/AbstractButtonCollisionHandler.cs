using System;
using HTC.UnityPlugin.ColliderEvent;
using UnityEngine;

namespace Navigation
{
    public abstract class AbstractButtonCollisionHandler : MonoBehaviour,
        IColliderEventHoverEnterHandler,
        IColliderEventHoverExitHandler
    {

        #region Public members
        
        /// <summary>
        /// Start is called before the first frame update 
        /// </summary>
        public GameObject target;

        /// <summary>
        /// Material applied when the button is pressed
        /// </summary>
        public Material pressedMat;
        
        #endregion Public members
        
        #region Protected members
        
        /// <summary>
        /// System action that is activated on collision
        /// </summary>
        protected Action OnHitFunc;
        
        #endregion Protected members
        
        #region Private members
        
        /// <summary>
        /// Material applied initially when the button is not pressed
        /// </summary>
        private Material _initMat;

        #endregion Private members

        #region Public functions

        /// <summary>
        /// Called when collider enters button
        /// </summary>
        /// <param name="eventData">event data</param>
        public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
        {
            OnHitFunc();
        }

        /// <summary>
        /// Called when collider exits button
        /// </summary>
        /// <param name="eventData">event data</param>
        public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
        {
            var msr = target.GetComponent<MeshRenderer>();
            if (msr != null)
            {
                msr.material = _initMat;
            }
        }
        
        #endregion Public functions

        #region Private functions

        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update, after
        /// <see>
        ///     <cref>Awake</cref>
        /// </see>
        /// </summary>
        private void Start()
        {
            var msr = target.GetComponent<MeshRenderer>();
            if (msr != null)
            {
                _initMat = msr.material;
            }
        }
        
        #endregion Private functions
    }
}
