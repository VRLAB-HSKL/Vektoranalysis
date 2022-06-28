using System.Collections;
using System.Collections.Generic;
using UnityEditor.Scripting.Python;
using UnityEngine;

public class PythonTestRunner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PythonRunner.EnsureInitialized();
        PythonUtility.CalculatePoints();    
    }
}
