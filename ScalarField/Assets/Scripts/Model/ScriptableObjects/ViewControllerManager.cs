using Controller;
using UnityEngine;

namespace Model.ScriptableObjects
{
    /// <summary>
    /// Data container for all MVC-based view controllers in the application
    /// </summary>
    [CreateAssetMenu(fileName = "ViewController", menuName = "ScriptableObjects/ViewControllerManager", order = 2)]
    public class ViewControllerManager : ScriptableObject
    {
        public FieldViewController FieldViewController { get; set; }

    }
}