using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace IMDLAB.EasyKeyPlayer
{
    public class MediaManager : MonoBehaviour
    {
        internal static MediaManager Instance { get; private set; }

        public VideoPlayer Player;
        public RawImage Container;

        private const string PATH = "EasyKeyPlayer_Media";
        private const string PICTURE_PATTERN = @"\d+\.(jpg|jpeg|png)";
        private const string MOVIE_PATTERN = @"\d+\.(mp4)";
        private const string PICTURE_ID_PATTERN = @"^{0}\.(jpg|jpeg|png)";
        private const string MOVIE_ID_PATTERN = @"^{0}\.(mp4)";
        [SerializeField]
        private string[] Pictures;
        [SerializeField]
        private string[] Movies;

        private void Awake()
        {
            Instance = this;
            ReadFiles();
        }

        private void Start()
        {
            PlayDefaultMedia();
        }

        private bool CheckFolderIntegrity()
        {
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
            }

            return true;
        }

        private void ReadFiles()
        {
            if (CheckFolderIntegrity())
            {
                Pictures = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), PATH))
                                 .Where(file => Regex.Match(file, PICTURE_PATTERN).Success)
                                 .Select(file => file.Replace(@"\", "/"))
                                 .ToArray();
                Movies = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), PATH))
                                 .Where(file => Regex.Match(file, MOVIE_PATTERN).Success)
                                 .Select(file => file.Replace(@"\", "/"))
                                 .ToArray();
            }
        }

        internal void PlayMedia(int id)
        {
            var media = GetMediaPath(id);
            if (media is null)
            {
                Debug.LogError("Media Not Found!");
                return;
            }


            if (media.Value.isMovie)
            {
                Container.gameObject.SetActive(false);
                if (Player.isPlaying)
                {
                    Player.Stop();
                }
                Player.url = media.Value.path;
                Player.isLooping = true;
                Player.Play();
            }
            else
            {
                Container.gameObject.SetActive(true);
                var raw = File.ReadAllBytes(media.Value.path);
                var texture2d = new Texture2D(1, 1);
                texture2d.filterMode = FilterMode.Point;
                texture2d.LoadImage(raw);
                Container.texture = texture2d;
                Player.Stop();
                Player.url = string.Empty;
                Player.isLooping = true;
            }
        }

        internal void PlayDefaultMedia()
        {
            PlayMedia(0);
        }

        internal (bool isMovie, string path)? GetMediaPath(int id)
        {
            var movie = GetMovieNamePattern(id);
            if (!string.IsNullOrEmpty(movie))
            {
                return (true, movie);
            }

            var picture = GetPictureNamePattern(id);
            if (!string.IsNullOrEmpty(picture))
            {
                return (false, picture);
            }

            return null;
        }

        internal string GetPictureNamePattern(int id)
        {
            return Pictures.Where(file => Regex.Match(file.Split(@"/").Last(), string.Format(PICTURE_ID_PATTERN, id)).Success)
                        .FirstOrDefault();
        }

        internal string GetMovieNamePattern(int id)
        {
            return Movies.Where(file => Regex.Match(file.Split(@"/").Last(), string.Format(MOVIE_ID_PATTERN, id)).Success)
                        .FirstOrDefault();
        }
    }
}
