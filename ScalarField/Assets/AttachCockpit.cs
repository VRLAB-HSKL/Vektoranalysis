using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachCockpit : MonoBehaviour
{
    /// <summary>
    /// Source object whose transform values are mapped to the cockpit
    /// </summary>
    public Transform TargetTf;

    
    
    // Update is called once per frame
    void Start()
    {
        // Update cockpit position
        //transform.position = TargetTf.position + CockpitOffset;
        
        // Rotate cockpit
        
        // var rotation = transform.rotation;
        // transform.rotation = TargetTf.rotation;
        // transform.localEulerAngles = new Vector3(0f, TargetTf.localEulerAngles.y, 0f);
    }
}
