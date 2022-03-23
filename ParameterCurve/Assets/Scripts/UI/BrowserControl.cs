//using SimpleWebBrowser;

using Model;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Control class for the ingame browser
    ///
    /// ToDo: Look for android compliant ingame browser and refactor this class 
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
