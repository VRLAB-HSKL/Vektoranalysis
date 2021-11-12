//using SimpleWebBrowser;
using UnityEngine;

namespace UI
{
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
            if (GlobalData.initFile.ApplicationSettings.BrowserSettings.Activated)
            {
                //Browser.OpenCommentFile(url);    
            }
        }

    }
}
