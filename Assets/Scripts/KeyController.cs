using UnityEngine;
using UnityEngine.SceneManagement;

namespace IMDLAB.EasyKeyPlayer
{
    public class KeyController : MonoBehaviour
    {
        private KeyCode[] Keys = new KeyCode[] {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8
        };

        private void Update()
        {
            for (var i = 0; i < Keys.Length; i++)
            {
                if (Input.GetKeyDown(Keys[i]))
                {
                    if (MediaManager.Instance)
                    {
                        var id = i + 1;
                        MediaManager.Instance.PlayMedia(id);
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
