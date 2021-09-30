
using System.Collections.Generic;
using TMPro;
using Views;

public abstract class AbstractCompoundView : IView
{
    protected List<IView> views = new List<IView>();

    protected AbstractCompoundView()
    {
        
    }

    public void UpdateView()
    {
        foreach (IView v in views)
        {
            v.UpdateView();
        }
    }

}