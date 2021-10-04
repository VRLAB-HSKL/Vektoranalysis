using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PointData
{
    public string t { get; set; }
    public string x { get; set; }
    public string y { get; set; }
    
    public string z { get; set; }
    
    public List<string> tan { get; set; } = new List<string>();
    public List<string> norm { get; set; } = new List<string>();
    public List<string> binorm { get; set; } = new List<string>();
}