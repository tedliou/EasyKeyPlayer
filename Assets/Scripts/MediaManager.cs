using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Video;
using UnityEngine.UI;

namespace IMDLAB.EasyKeyPlayer
{
    public class MediaManager : MonoBehaviour
    {
        internal static MediaManager Instance { get; private set; }

        public VideoPlayer Player;
        public Canvas UI;
        public RawImage Container;

        private const string PATH = "EasyKeyPlayer_Media";
        private const string PICTURE_PATTERN = @"\d+\.(jpg|jpeg|png)";
        private const string MOVIE_PATTERN = @"\d+\.(mp4|avi)";
        private const string PICTURE_ID_PATTERN = @"^{0}\.(jpg|jpeg|png)";
        private const string MOVIE_ID_PATTERN = @"^{0}\.(mp4|avi)";
        [SerializeField]
        private string[] Pictures;
        [SerializeField]
        private string[] Movies;

        private void Awake()
        {
            Instance = this;
            ReadFiles();
            Player.loopPointReached += Player_loopPointReached;
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

        internal void PlayMedia(int id, bool loop = false)
        {
            var media = GetMediaPath(id);
            if (media is null)
            {
                Debug.LogError("Media Not Found!");
                return;
            }


            if (media.Value.isMovie)
            {
                UI.enabled = false;
                Player.url = media.Value.path;
                Player.isLooping = loop;
                Player.Play();
            }
            else
            {
                UI.enabled = true;
                var raw = File.ReadAllBytes(media.Value.path);
                var texture2d = new Texture2D(1, 1);
                texture2d.LoadImage(raw);
                Container.texture = texture2d;
                Player.Stop();
                Player.url = string.Empty;
                Player.isLooping = false;
            }

            Debug.Log(media);
        }

        internal void PlayDefaultMedia()
        {
            PlayMedia(0, true);
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

        private void Player_loopPointReached(VideoPlayer videoPlayer)
        {
            if (!videoPlayer.isLooping)
            {
                PlayDefaultMedia();
            }
        }
    }
}
