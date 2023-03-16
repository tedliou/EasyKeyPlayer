using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace IMDLAB.EasyKeyPlayer
{
    public class ScreenManager : MonoBehaviour
    {
        public CanvasScaler UIScaler;

        private async void Awake()
        {
            while(ConfigManager.Instance is null)
            {
                await Task.Yield();
            }

            SetResolution();
            Application.targetFrameRate = 60;
        }

        private void SetResolution()
        {
            var config = ConfigManager.Instance;
            var size = config.GetScreenSize();
            var fullScreen = config.GetFullScreen();
            Screen.SetResolution(size.x, size.y, fullScreen);
            UIScaler.referenceResolution = size;
        }
    }
}