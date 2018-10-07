using BooTube.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooTube.BLL.Repos
{
    public class PostRepo
    {
        BooTubeEntities db = DBTool.GetInstance();

        public List<Post> GetPosts()
        {
            return db.Posts.ToList();
        }
        public List<Post> GetPosts(bool isDeleted)
        {
            return db.Posts.Where(p => p.IsDeleted == isDeleted).ToList();
        }
        public Post GetPost(int postID)
        {
            Post post;
            try
            {
                if (postID > 0)
                {
                    post = db.Posts.Find(postID);
                    return post;
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
        public Post GetPost(int postID, bool isDeleted)
        {
            return db.Posts.Where(p => p.PostID == postID && p.IsDeleted == isDeleted).SingleOrDefault();
        }
        public bool DeletePost(int postID, out string islemSonucu)
        {

            bool sonuc = false;

            try
            {
                Post postToDelete = GetPost(postID);
                if (postToDelete != null)
                {
                    postToDelete.IsDeleted = true;
                    db.SaveChanges();

                    islemSonucu = "Gönderi başarıyla silindi";
                    sonuc = true;
                }
                else
                {
                    islemSonucu = "Böyle bir gönderi bulunamadı.";
                }

            }
            catch (Exception ex)
            {
                islemSonucu = ex.Message;
                sonuc = false;
            }

            return sonuc;
        }

        /// <summary>
        /// Gets most viewed videos in a range of days.
        /// </summary>
        /// <param name="dayRange"></param>
        /// <returns></returns>
        public List<Post> GetTrendPosts(double dayRange)
        {
            DateTime compare = DateTime.Now.AddDays(-dayRange);
            List<Post> feed = GetPosts(false).Where(p => p.CreateTime.CompareTo(compare) > 0).OrderByDescending(p => p.PostAddons.Where(pa => pa.AddonType == 5).Count()).ToList();

            if (feed.Count == 0) feed = GetTrendPosts(dayRange + 1);

            return feed;
        }

        public bool InsertPost(Post newPost, out string islemSonucu)
        {
            try
            {
                if (newPost != null)
                {
                    newPost.CreateTime = DateTime.Now;
                    newPost.IsDeleted = false;
                    newPost.IsPublished = false;

                    if (newPost.PostThumbnailPath == null)
                        newPost.PostThumbnailPath = "def.png";

                    if (!(string.IsNullOrEmpty(newPost.PostTitle)) && newPost.ChannelID > 0)
                    {
                        db.Posts.Add(newPost);
                        bool sonuc = db.SaveChanges() > 0;

                        if (sonuc)
                            islemSonucu = "Kayıt işlemi başarıyla tamamlandı";
                        else
                            islemSonucu = "Bir sorunla karşılaşıldı";

                        return sonuc;

                    }
                    else
                    {
                        islemSonucu = "Video yolu, başlığı veya kanalı boş bırakılamaz.";
                        return false;
                    }
                }
                else
                {
                    islemSonucu = "Gönderi boş olamaz.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                islemSonucu = ex.Message;
                return false;
            }

        }
        public void AddTagsToPost(string tags, Post post)
        {

            if (!string.IsNullOrEmpty(tags) && tags.Contains(',') && post != null)
            {
                foreach (string tagName in tags.Split(','))
                {
                    Tag newTag;
                    string tagText = GenFunx.CleanStringForUse(tagName.ToLower());
                    try
                    {
                        newTag = db.Tags.Where(t => t.TagText == tagText).First();
                    }
                    catch (Exception)
                    {
                        newTag = new Tag() { TagText = tagName };
                        db.Tags.Add(newTag);
                        db.SaveChanges();
                    }
                    PostTag newPostTag = new PostTag() { PostID = post.PostID, TagID = newTag.TagID };
                    db.PostTags.Add(newPostTag);
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all the posts posted by channels that a person follows
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<Post> GetFollowedPosts(int userID)
        {
            ChannelRepo cr = new ChannelRepo();
            List<Channel> followedChannels = cr.GetFollowedChannels();
            List<Post> feed = new List<Post>();
            foreach (Channel ch in followedChannels)
            {
                feed.AddRange(db.Posts.Where(p => p.ChannelID == ch.ChannelID && p.IsDeleted == false).OrderByDescending(p => p.CreateTime).ToList());
            }
            return feed;
        }
        /// <summary>
        /// Gets all the posts posted by channels that a person follows in a specific day range
        /// </summary>
        /// <param name="userID">user id</param>
        /// <param name="dayRange">maximum oldness of post</param>
        /// <returns></returns>
        public List<Post> GetFollowedPosts(int userID, double dayRange)
        {
            ChannelRepo cr = new ChannelRepo();
            List<Channel> followedChannels = cr.GetFollowedChannels(userID);
            List<Post> feed = new List<Post>();
            if (followedChannels != null)
            {
                DateTime compare = DateTime.Now.AddDays(-dayRange);
                foreach (Channel ch in followedChannels)
                {
                    feed.AddRange(db.Posts.Where(p => p.ChannelID == ch.ChannelID && p.IsDeleted == false && p.CreateTime.CompareTo(compare) > 0).OrderByDescending(p => p.CreateTime).ToList());
                }
            }
            if (feed.Count < 1) feed = GetFollowedPosts(userID, dayRange + 1);
            return feed;
        }

        public List<List<Post>> GetSuggestedPostsForTags(List<Tag> tags)
        {
            if (tags != null)
            {
                try
                {
                    List<List<Post>> listOfLists = new List<List<Post>>();
                    tags.ForEach(tag => listOfLists.Add(GetPostsWithTag(tag)));
                    List<Post> mostRelateds = new List<Post>();

                    foreach (List<Post> list in listOfLists)
                    {
                        if (listOfLists.Last() != list)
                        {
                            mostRelateds.AddRange(list.Intersect(listOfLists[listOfLists.IndexOf(list) + 1]));
                        }
                    }

                    List<Post> otherRelateds = new List<Post>();
                    mostRelateds = mostRelateds.Distinct().ToList();
                    listOfLists.ForEach(list => otherRelateds = otherRelateds.Union(list).ToList());
                    otherRelateds = otherRelateds.Distinct().ToList();
                    mostRelateds.ForEach(p => otherRelateds.Remove(p));

                    listOfLists.Clear();
                    listOfLists.Add(mostRelateds);
                    listOfLists.Add(otherRelateds);
                    return listOfLists;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public List<Tag> GetTagsOfPost(int postID)
        {
            List<Tag> tagsOfThePost = new List<Tag>();
            foreach (PostTag postTag in db.Posts.Find(postID).PostTags)
            {
                tagsOfThePost.Add(postTag.Tag);
            }
            return tagsOfThePost;
        }

        public List<Post> GetPostsWithTag(Tag tag)
        {
            if (tag != null)
            {
                List<PostTag> postTags = db.PostTags.Where(p => p.TagID == tag.TagID).ToList();
                List<Post> posts = new List<Post>();
                foreach (PostTag pt in postTags)
                {
                    posts.Add(pt.Post);
                }
                return posts;
            }
            else
            {
                return null;
            }
        }
        public List<Post> GetPostsWithTitle(string postTitle)
        {
            return db.Posts.Where(p => p.PostTitle.Contains(postTitle) && p.IsDeleted == false).ToList();
        }
        public List<Post> GetPostsWithDesc(string description)
        {
            return db.Posts.Where(p => p.PostTitle.Contains(description) && p.IsDeleted == false).ToList();
        }


    }
}
