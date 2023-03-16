using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace IMDLAB.EasyKeyPlayer
{
    public class ScreenManager : MonoBehaviour
    {
        private async void Awake()
        {
            while(ConfigManager.Instance is null)
            {
                await Task.Yield();
            }

            SetResolution();
        }

        private void SetResolution()
        {
            var config = ConfigManager.Instance;
            var size = config.GetScreenSize();
            var fullScreen = config.GetFullScreen();
            Screen.SetResolution(size.x, size.y, fullScreen);
        }
    }
}