using System;

[Serializable]
public class CurveJSON
{
    public int id;
    public string name = string.Empty;
    public RGBColorJSON color;

    public string travelObj = string.Empty;
    public RGBColorJSON travelObjColor;

    public string arcTravelObj = string.Empty;
    public RGBColorJSON arcTravelObjColor;

    public CurveDataJSON data;
}
