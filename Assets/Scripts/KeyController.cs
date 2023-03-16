using UnityEngine;
using UnityEngine.SceneManagement;

namespace IMDLAB.EasyKeyPlayer
{
    public class KeyController : MonoBehaviour
    {
        private KeyCode[] Keys = new KeyCode[] {
            KeyCode.Alpha0,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9
        };

        private void Update()
        {
            for (var i = 0; i < Keys.Length; i++)
            {
                if (Input.GetKeyDown(Keys[i]))
                {
                    if (MediaManager.Instance)
                    {
                        if (i == 0)
                        {
                            MediaManager.Instance.PlayDefaultMedia();
                        }
                        else
                        {
                            MediaManager.Instance.PlayMedia(i);
                        }
                        return;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
