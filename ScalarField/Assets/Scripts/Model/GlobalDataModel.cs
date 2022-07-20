using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Model.InitFile;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Model
{
    /// <summary>
    /// Static global model class containing constants and data that has to be easily accessible to the whole application
    /// </summary>
    public class GlobalDataModel
    {
        public enum OptimizationAlgorithm
        {
            STEEPEST_DESCENT = 0,
            NELDER_MEAD = 1,
            NEWTON = 2,
            NEWTON_DISCRETE = 3,
            NEWTON_TRUSTED = 4,
            BFGS = 5
        }
        
        public static int EstimatedIndex = 0;

        public Vector3 ClosestPointOnMesh = Vector3.zero;

        
        
        
    
        


        
        
        // /// <summary>
        // /// Initializes static global data class
        // /// </summary>
        // public void InitializeData()
        // {
        //     //IronPythonTest.Foo();
        //     ParseInitFile();
        // }

        
        
        
        
        
    }
}
