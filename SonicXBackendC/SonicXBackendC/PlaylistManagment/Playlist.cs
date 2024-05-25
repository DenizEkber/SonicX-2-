using System;
using System.Collections.Generic;
using SonicXBackendC.MusicManagment;
using SonicXBackendC.UserManagement;

namespace SonicXBackendC.PlaylistManagment
{
    public class Playlist
    {
        private string name;
        private PlaylistDAO playlistDAO;

        public Playlist(string name)
        {
            this.name = name;
            try
            {
                this.playlistDAO = new PlaylistDAO();
            }
            catch (Exception e)
            {
                throw new Exception("Veritabanı bağlantısı oluşturulurken hata oluştu: " + e.Message);
            }
        }

        public string Name
        {
            get { return name; }
        }

        public void AddMusicFileToPlaylist(MusicFile musicFile, UserService userService)
        {
            try
            {
                playlistDAO.AddMusicFileToPlaylist(this.name, musicFile, userService);
            }
            catch (Exception e)
            {
                throw new Exception("Müzik dosyası eklenirken hata oluştu: " + e.Message);
            }
        }

        public void RemoveMusicFileFromPlaylist(string musicTitle, UserService userService)
        {
            try
            {
                playlistDAO.RemoveMusicFileFromPlaylist(this.name, musicTitle, userService);
            }
            catch (Exception e)
            {
                throw new Exception("Müzik dosyası silinirken hata oluştu: " + e.Message);
            }
        }

        public List<MusicFile> GetAllMusicFiles(UserService userService)
        {
            try
            {
                return playlistDAO.GetMusicFilesInPlaylist(this.name, userService);
            }
            catch (Exception e)
            {
                throw new Exception("Müzik dosyaları getirilirken hata oluştu: " + e.Message);
            }
        }

        public void Close()
        {
            playlistDAO.Close();
        }
    }
}
