using System;

namespace SonicXBackendC.MusicManagment
{
    public class MusicFile
    {
        private string title;
        private string artist;
        private string album;
        private int duration;
        private string path;

        public MusicFile(string title, string artist, string album, int duration, string path)
        {
            this.title = title;
            this.artist = artist;
            this.album = album;
            this.duration = duration;
            this.path = path;
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Artist
        {
            get { return artist; }
            set { artist = value; }
        }

        public string Album
        {
            get { return album; }
            set { album = value; }
        }

        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public override string ToString()
        {
            return $"MusicFile{{title='{title}', artist='{artist}', album='{album}', duration={duration}, path='{path}'}}";
        }
    }
}
