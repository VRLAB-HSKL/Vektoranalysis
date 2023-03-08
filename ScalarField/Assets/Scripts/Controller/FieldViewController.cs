using System.Collections.Generic;
using Model.ScriptableObjects;
using UnityEngine;
using Views;

namespace Controller
{
    /// <summary>
    /// View controller for scalar field visualizations 
    /// </summary>
    public class FieldViewController : AbstractFieldViewController
    {
        public FieldViewController(ScalarFieldManager data, GameObject mesh, GameObject boundingBox)
        {
            Views = new List<AbstractFieldView>
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