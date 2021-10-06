using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

[Serializable]
public class PointData
{
    [JsonProperty("t")]
    public float T { get; set; }
    
    [JsonProperty("pVec")]
    public List<float> pVec { get; set; }
    
    [JsonProperty("velVec")]
    public float[] velVec { get; set; }
    
    [JsonProperty("accVec")]
    public float[] accVec { get; set; }
    
    [JsonProperty("arcT")]
    public float arcT { get; set; }
    
    [JsonProperty("arcPVec")]
    public float[] arcPVec { get; set; }
    
    [JsonProperty("arcVelVec")]
    public float[] arcVelVec { get; set; }
    
    [JsonProperty("arcAccVec")]
    public float[] arcAccVec { get; set; }
}