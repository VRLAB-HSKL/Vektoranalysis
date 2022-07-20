using UnityEngine;

namespace Views
{
    public class SimpleView : AbstractFieldView
    {
        public SimpleView(ScalarFieldManager data, GameObject mesh, GameObject boundingBox) 
            : base(data, mesh, boundingBox) {}

        public override void UpdateView()
        {
            //Debug.Log("SimpleView - UpdateView()");
            base.UpdateView();
        }
    }
}