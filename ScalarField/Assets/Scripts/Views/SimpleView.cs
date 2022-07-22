using Model.ScriptableObjects;
using UnityEngine;

namespace Views
{
    public class SimpleView : AbstractFieldView
    {
        public SimpleView(ScalarFieldManager data, GameObject mesh, GameObject boundingBox) 
            : base(data, mesh, boundingBox) {}
    }
}