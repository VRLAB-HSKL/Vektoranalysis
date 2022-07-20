using Controller;
using UnityEngine;

namespace Model.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    [CreateAssetMenu(fileName = "ViewController", menuName = "ScriptableObjects/ViewControllerManager", order = 2)]
    public class ViewControllerManager : ScriptableObject
    {
        public FieldViewController FieldViewController { get; set; }

    }
}