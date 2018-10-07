using BooTube.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooTube.BLL.Repos
{
    public class AddonRepo
    {
        BooTubeEntities db = DBTool.GetInstance();

        public List<PostAddon> GetPostAddons(int postID = 0)
        {
            return db.PostAddons.Where(pa => pa.PostID == postID).ToList();
        }
        public List<PostAddon> GetPostAddons(int postID = 0, int addonTypeID = 0)
        {
            if (postID > 0 && addonTypeID > 0)
            {
                return db.PostAddons.Where(pa => pa.PostID == postID && pa.AddonType == addonTypeID).ToList();
            }
            else
            {
                return null;
            }
            
        }

        public void AddClick(int postID = 0)
        {
            if (postID > 0)
            {
                PostAddon click = new PostAddon()
                {
                    AddonType = 5,
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    PostID = postID,
                };

                db.PostAddons.Add(click);
                db.SaveChanges();
            }
        }
        public void AddClick(int postID, int userID)
        {
            if (postID > 0)
            {
                PostAddon click = new PostAddon()
                {
                    AddonType = 5,
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    UserID = userID,
                    PostID = postID,
                };

                db.PostAddons.Add(click);
                db.SaveChanges();
            }
        }
       
        /// <summary>
        /// Adds new like entry and returns like count. Returns zero if there is already an entry
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int AddLike(int postID, int userID)
        {
            if (postID > 0 && userID > 0)
            {
                if (db.PostAddons.Where(pa => pa.PostID == postID && pa.UserID == userID && pa.AddonType == 2).Count() == 0)
                {
                    PostAddon pad = new PostAddon()
                    {
                        AddonType = 2,
                        UserID = userID,
                        CreateTime = DateTime.Now,
                        PostID = postID,
                        IsDeleted = false,
                    };
                    db.PostAddons.Add(pad);
                    bool sonuc = db.SaveChanges() > 0;
                    return GetPostAddons(postID).Where(pa => pa.AddonType == 2 && pa.IsDeleted == false).Count();
                }
                else return 0;
            }
            else return 0;
        }

        /// <summary>
        /// Sets like state with given state and returns like count. Returns zero if there is no entry to set
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userID"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public int SetAddonState(int postID, int userID, int addonType, bool state)
        {
            if (postID > 0 && userID > 0)
            {
                if (db.PostAddons.Where(pa => pa.PostID == postID && pa.UserID == userID).Count() != 0)
                {
                    PostAddon pad = db.PostAddons.Where(pa => pa.PostID == postID && pa.UserID == userID && pa.AddonType == addonType).FirstOrDefault();
                    pad.IsDeleted = state;
                    db.Entry(db.PostAddons.Find(pad.AddonID)).CurrentValues.SetValues(pad);
                    db.SaveChanges();
                }
                return GetPostAddons(postID).Where(pa => pa.AddonType == addonType && pa.IsDeleted == false).Count();
            }
            else return 0;
        }

        /// <summary>
        /// Adds new dislike entry and returns like count. Returns zero if there is already an entry
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int AddDislike(int postID, int userID)
        {
            if (postID > 0 && userID > 0)
            {
                if (db.PostAddons.Where(pa => pa.PostID == postID && pa.UserID == userID && pa.AddonType == 4).Count() == 0)
                {
                    PostAddon pad = new PostAddon()
                    {
                        AddonType = 4,
                        UserID = userID,
                        CreateTime = DateTime.Now,
                        PostID = postID,
                        IsDeleted = false,
                    };
                    db.PostAddons.Add(pad);
                    db.SaveChanges();
                }
                return GetPostAddons(postID).Where(pa => pa.AddonType == 2 && pa.IsDeleted == false).Count();
            }
            else return 0;
        }

        /// <summary>
        /// Returns true if there is entry about given parameters.
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userID"></param>
        /// <param name="addonType"></param>
        /// <returns></returns>
        public bool IsAddonThere(int postID, int userID, int addonType)
        {
            if (db.PostAddons.Where(p => p.PostID == postID && p.UserID == userID && p.AddonType == addonType).Count() > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Returns true if there is entry about given parameters.
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userID"></param>
        /// <param name="addonType"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public bool IsAddonThere(int postID, int userID, int addonType, bool isDeleted)
        {
            if (db.PostAddons.Where(p => p.PostID == postID && p.UserID == userID && p.AddonType == addonType && p.IsDeleted == isDeleted).Count() > 0)
                return true;
            else
                return false;
        }


    }
}
