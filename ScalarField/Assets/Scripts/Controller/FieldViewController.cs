using System.Collections.Generic;
using UnityEngine;
using Views;

namespace Controller
{
    public class FieldViewController : AbstractFieldViewController
    {
        public FieldViewController(ScalarFieldManager data, GameObject mesh, GameObject boundingBox) : base()
        {
            Views = new List<AbstractFieldView>()
            {
               new SimpleView(data, mesh, boundingBox) 
            };
            
            SwitchView(0);
            
            CurrentView.UpdateView();
        }

        public void UpdateViews()
        {
            foreach (var view in Views)
            {
                view.UpdateView();
            }
        }
    }
}