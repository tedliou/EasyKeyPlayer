using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IMDLAB.EasyKeyPlayer
{
    public class KeyController : MonoBehaviour
    {
        private List<KeyCode[]> Keys = new List<KeyCode[]>
        {
            new[] { KeyCode.Alpha1, KeyCode.Keypad1 },
            new[] { KeyCode.Alpha2, KeyCode.Keypad2  },
            new[] { KeyCode.Alpha3, KeyCode.Keypad3  },
            new[] { KeyCode.Alpha4, KeyCode.Keypad4  },
            new[] { KeyCode.Alpha5, KeyCode.Keypad5  },
            new[] { KeyCode.Alpha6, KeyCode.Keypad6  },
            new[] { KeyCode.Alpha7, KeyCode.Keypad7  },
            new[] { KeyCode.Alpha8, KeyCode.Keypad8  },
            new[] { KeyCode.Alpha9, KeyCode.Keypad9  }
        };

        private void Update()
        {
            for (var i = 0; i < Keys.Count; i++)
            {
                for (var j = 0; j < Keys[i].Length; j++)
                {
                    if (Input.GetKeyDown(Keys[i][j]))
                    {
                        if (MediaManager.Instance)
                        {
                            var id = i + 1;
                            MediaManager.Instance.PlayMedia(id);
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
