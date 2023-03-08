using UnityEngine;

namespace Model.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Travel", menuName = "ScriptableObjects/TravelManager", order = 3)]
    public class TravelManager : ScriptableObject
    {
        public int estimatedIndex = 0;
        public Vector3 ClosestPointOnMesh = Vector3.zero;
    }
}