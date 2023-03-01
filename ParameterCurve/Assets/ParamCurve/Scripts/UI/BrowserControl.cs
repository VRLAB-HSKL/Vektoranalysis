//using SimpleWebBrowser;

using Model;
using UnityEngine;

namespace ParamCurve.Scripts.UI
{
    /// <summary>
    /// Control class for the in-game browser
    ///
    /// ToDo: Look for android compliant in-game browser and refactor this class 
    /// </summary>
    public class BrowserControl : MonoBehaviour
    {
        //public WebBrowser Browser;

        void Start()
        {
            // if(Browser is null)
            // {
            //     if(!TryGetComponent(out Browser))
            //     {
            //         Debug.LogError("BrowserControl - WebBrowser Component not found");
            //     }                        
            // }
            //
            // Browser.gameObject.SetActive(GlobalData.initFile.ApplicationSettings.BrowserSettings.Activated);
            // OpenURL(GlobalData.initFile.ApplicationSettings.BrowserSettings.Url);
        }
    
        public void OpenURL(string url)
        {
            if (GlobalDataModel.InitFile.ApplicationSettings.BrowserSettings.Activated)
            {
                //Browser.OpenCommentFile(url);    
            }
        }

    }
}
