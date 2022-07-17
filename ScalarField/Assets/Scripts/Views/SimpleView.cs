using UnityEngine;

namespace Views
{
    public class SimpleView : AbstractFieldView
    {
        public SimpleView(GameObject mesh, GameObject boundingBox) : base(mesh, boundingBox) {}

        public override void UpdateView()
        {
            //Debug.Log("SimpleView - UpdateView()");
            base.UpdateView();
        }
    }
}