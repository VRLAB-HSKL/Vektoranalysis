using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Controller
{
    public class FieldViewController : AbstractFieldViewController
    {
        public FieldViewController(GameObject mesh, GameObject boundingBox) : base()
        {
            Debug.Log("FieldViewController Constructor");
            
            Views = new List<AbstractFieldView>()
            {
               new SimpleView(mesh, boundingBox) 
            };
            
            SwitchView(0);
            
            CurrentView.UpdateView();
        }
    }
}