using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachCockpit : MonoBehaviour
{
    /// <summary>
    /// Source object whose transform values are mapped to the cockpit
    /// </summary>
    public Transform TargetTf;

    /// <summary>
    /// Positional offset, initially used to place the cockpit below the player
    /// </summary>
    public Vector3 CockpitOffset = new Vector3(0f, -2f, 0f);
    
    // Update is called once per frame
    void Update()
    {
        // Update cockpit position
        transform.position = TargetTf.position + CockpitOffset;
        
        // Rotate cockpit
        
        // var rotation = transform.rotation;
        // transform.rotation = TargetTf.rotation;
        // transform.localEulerAngles = new Vector3(0f, TargetTf.localEulerAngles.y, 0f);
    }
}
