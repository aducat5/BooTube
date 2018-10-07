using BooTube.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BooTube.BLL.Repos
{
    public class ChannelRepo
    {

        BooTubeEntities db = DBTool.GetInstance();

        public Channel GetChannelByID(int id) => db.Channels.Find(id);

        public Channel GetChannelByID(int id, bool isDeleted)
        {
            Channel ch = db.Channels.Find(id);

            if (ch != null && ch.IsDeleted == isDeleted)
                return ch;
            else
                return null;
        }

        public List<Channel> GetChannelsByID(int id)
        {
            if (id < 1)
            {
                return null;
            }
            else
            {
                return db.Channels.Where(c => c.User.UserID == id).ToList();
            }

        }

        /// <summary>
        /// Returns channels as desired to be deleted or not
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public List<Channel> GetChannelsByID(int id, bool isDeleted)
        {
            if (id < 1)
                return null;
            else
                return db.Channels.Where(c => c.User.UserID == id && c.IsDeleted == isDeleted).ToList();

        }

        /// <summary>
        /// Returns channels as desired to be deleted and frozen or not
        /// </summary>
        /// <param name="id">id of channel</param>
        /// <param name="isDeleted">is deleted</param>
        /// <param name="isFrozen">is frozen</param>
        /// <returns></returns>
        public List<Channel> GetChannelsByID(int id, bool isDeleted, bool isFrozen)
        {
            if (id < 1)
                return null;
            else
                return db.Channels.Where(c => c.User.UserID == id && c.IsDeleted == isDeleted && c.IsFrozen == isFrozen).ToList();

        }

        public bool InsertChannel(Channel newChannel, out string islemSonucu)
        {
            try
            {
                if (newChannel != null)
                {
                    newChannel.CreateTime = DateTime.Now;
                    newChannel.IsDeleted = false;
                    newChannel.IsFrozen = false;

                    if (newChannel.ChannelProfilePicture == null)
                        newChannel.ChannelProfilePicture = "def.png";

                    if (newChannel.ChannelCoverPicture == null)
                        newChannel.ChannelCoverPicture = "def.png";

                    if (!string.IsNullOrEmpty(newChannel.ChannelName) && newChannel.ChannelOwnerID > 0)
                    {

                        db.Channels.Add(newChannel);
                        bool sonuc = db.SaveChanges() > 0;
                        if (sonuc)
                        {
                            islemSonucu = "Kayıt işlemi başarıyla tamamlanmıştır.";
                            return sonuc;
                        }
                        else
                        {
                            islemSonucu = "Bir hata oluştu. Lütfen tüm alanları kontrol edin.";
                            return sonuc;
                        }
                    }
                    else
                    {
                        islemSonucu = "Kanal adı veya sahibi boş bırakılamaz.";
                        return false;
                    }
                }
                else
                {
                    islemSonucu = "Nesne boş olarak gönderilemez.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                islemSonucu = ex.Message;
                return false;
            }
        }

        public bool DeleteChannel(int id, out string islemSonucu)
        {
            bool sonuc = false;

            try
            {
                Channel channelToDelete = GetChannelByID(id);
                if (channelToDelete != null)
                {
                    channelToDelete.IsDeleted = true;
                    db.SaveChanges();

                    islemSonucu = "Kanal başarıyla silindi";
                    sonuc = true;
                }
                else
                {
                    islemSonucu = "Böyle bir kanal bulunamadı.";
                }

            }
            catch (Exception ex)
            {
                islemSonucu = ex.Message;
                sonuc = false;
            }

            return sonuc;
        }

        public bool FreezeChannel(int id, out string islemSonucu)
        {
            bool sonuc = false;

            try
            {
                Channel channelToFreeze = GetChannelByID(id);
                if (channelToFreeze != null)
                {
                    channelToFreeze.IsFrozen = true;
                    db.SaveChanges();

                    islemSonucu = "Kanal başarıyla askıya alındı.";
                    sonuc = true;
                }
                else
                {
                    islemSonucu = "Böyle bir kanal bulunamadı.";
                }

            }
            catch (Exception ex)
            {
                islemSonucu = ex.Message;
                sonuc = false;
            }

            return sonuc;
        }

        public bool IsUserOwnerOfChannel(int userID, int channelID)
        {
            Channel chToCheck = GetChannelByID(channelID);

            if (chToCheck.ChannelOwnerID == userID)
                return true;
            else
                return false;
        }

        public bool EditChannel(Channel ch, out string islemSonucu)
        {
            try
            {

                if (ch != null)
                {
                    ch.UpdateTime = DateTime.Now;

                    ch.IsDeleted = false;

                    if (ch.ChannelProfilePicture == null)
                        ch.ChannelProfilePicture = "def.png";

                    if (ch.ChannelCoverPicture == null)
                        ch.ChannelCoverPicture = "def.png";

                    if (!string.IsNullOrEmpty(ch.ChannelName) && ch.ChannelID > 0 && ch.ChannelOwnerID > 0)
                    {

                        db.Entry(db.Channels.Find(ch.ChannelID)).CurrentValues.SetValues(ch);
                        db.SaveChanges();
                        islemSonucu = "Güncelleme başarılı";
                        return true;


                    }
                    else
                    {
                        islemSonucu = "Kanal adı veya sahibi ve ID boş bırakılamaz.";
                        return false;
                    }
                }
                else
                {
                    islemSonucu = "Kanal nesnesi boş bırakılamaz.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                islemSonucu = ex.Message;
                return false;
            }
        }

        public void FollowChannel(int channelID, int userID)
        {

            if (channelID > 0 && userID > 0)
            {
                if (db.Follows.Where(f => f.FollowedID == channelID && f.FollowerID == userID).Count() < 1)
                {
                    Follow follow = new Follow()
                    {
                        CreateTime = DateTime.Now,
                        FollowerID = userID,
                        FollowedID = channelID,
                        IsDeleted = false
                    };

                    db.Follows.Add(follow);
                    db.SaveChanges();
                }
                else if (db.Follows.Where(f => f.FollowedID == channelID && f.FollowerID == userID && f.IsDeleted == true).Count() > 0)
                {
                    Follow follow = db.Follows.Where(f => f.FollowedID == channelID && f.FollowerID == userID && f.IsDeleted == true).Single();
                    follow.IsDeleted = false;

                    db.Entry(db.Follows.Find(follow.FollowID)).CurrentValues.SetValues(follow);
                    db.SaveChanges();

                }
            }
        }

        public void UnfollowChannel(int channelID, int userID)
        {

            if (channelID > 0 && userID > 0)
            {
                if (db.Follows.Where(f => f.FollowedID == channelID && f.FollowerID == userID && f.IsDeleted == false).Count() > 0)
                {
                    Follow follow = db.Follows.Where(f => f.FollowedID == channelID && f.FollowerID == userID && f.IsDeleted == false).Single();
                    follow.IsDeleted = true;

                    db.Entry(db.Follows.Find(follow.FollowID)).CurrentValues.SetValues(follow);
                    db.SaveChanges();
                }
            }
        }

        public bool IsUserFollowing(int channelID, int userID)
        {
            if (db.Follows.Where(f => f.FollowedID == channelID && f.FollowerID == userID && f.IsDeleted == false).Count() > 0)
                return true;
            else
                return false;
        }

        public List<Channel> GetFollowedChannels(int userID = 0)
        {
            if (userID > 0)
            {
                List<Channel> channels = new List<Channel>();
                List<Follow> follows = db.Follows.Where(f => f.FollowerID == userID && f.IsDeleted == false).ToList();

                follows.ForEach(f => channels.Add(f.Channel));

                return channels;
            }
            else
                return null;
        }

    }
}
