using BooTube.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooTube.BLL.Repos
{
    public class VideoRepo
    {
        BooTubeEntities db = DBTool.GetInstance();
        public Video GetVideo(int videoID)
        {
            Video video;
            try
            {
                if (videoID > 0)
                {
                    video = db.Videos.Find(videoID);
                    return video;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Video> GetVideosOfPost(int postID)
        {
            List<Video> videos = null;
            try
            {
                videos = db.Videos.Where(v => v.PostID == postID).ToList();
                return videos;
            }
            catch (Exception)
            {
                return videos;
            }
        }

        public bool InsertVideo(Video video, out string islemSonucu)
        {
            try
            {
                db.Videos.Add(video);
                bool sonuc = db.SaveChanges() > 0;

                if (sonuc)
                    islemSonucu = "Başarılı";
                else
                    islemSonucu = "Eklenemedi";

                return sonuc;
            }
            catch (Exception ex)
            {
                islemSonucu = ex.Message;
                return false;
            }
        }
    }
}
