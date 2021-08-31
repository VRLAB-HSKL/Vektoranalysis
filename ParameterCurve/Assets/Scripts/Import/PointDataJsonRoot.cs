using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PointDataJsonRoot
{
    public string name { get; set; }

    public List<PointData> pointData { get; set; } = new List<PointData>();
}
