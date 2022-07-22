using Model.Enums;

namespace Model
{
    public class CriticalPointData
    {
        public int PointIndex { get; set; } = -1;
        public CriticalPointType Type { get; set; } = CriticalPointType.CRITICAL_POINT;
    }
}