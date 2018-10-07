using BooTube.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooTube.BLL.Repos
{
    public class CommentRepo
    {
        BooTubeEntities db = DBTool.GetInstance();
        
        //public List<Comment> GetComments()
        //{
        //    return db.Comments.ToList();
        //}

        //public List<Comment> GetComments(User user)
        //{
        //    if (user != null)
        //    {
        //        return db.Comments.Where(c => c.OwnerID == user.UserID).ToList();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public List<Comment> GetComments(User user, bool isDeleted)
        //{
        //    if (user != null)
        //    {
        //        return db.Comments.Where(c => c.OwnerID == user.UserID && c.IsDeleted == isDeleted).ToList();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public List<Comment> GetComments(Post post)
        //{
        //    if (post != null)
        //    {
        //        return db.Comments.Where(c => c.PostID == post.PostID).ToList();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public List<Comment> GetComments(Post post, bool isDeleted)
        //{
        //    if (post != null)
        //    {
        //        return db.Comments.Where(c => c.PostID == post.PostID && c.IsDeleted == isDeleted).ToList();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public List<Comment> GetComments(User user, Post post)
        //{
        //    if (user != null && post != null)
        //    {
        //        return db.Comments.Where(c => c.OwnerID == user.UserID && c.PostID == post.PostID).ToList();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public List<Comment> GetComments(User user, Post post, bool isDeleted)
        //{
        //    if (user != null && post != null)
        //    {
        //        return db.Comments.Where(c => c.OwnerID == user.UserID && c.PostID == post.PostID && c.IsDeleted == isDeleted).ToList();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public bool InsertComment(Comment newComment, out string islemSonucu)
        //{
        //    try
        //    {
        //        if (newComment != null)
        //        {
        //            if (newComment.OwnerID > 0 && newComment.PostID > 0)
        //            {
        //                if (!string.IsNullOrEmpty(newComment.CommentText))
        //                {
        //                    if (newComment.CreateTime == null) newComment.CreateTime = DateTime.Now;
        //                    newComment.IsDeleted = false;

        //                    db.Comments.Add(newComment);
        //                    bool sonuc = db.SaveChanges() > 0;
        //                    if (sonuc)
        //                    {
        //                        islemSonucu = "Yorum başarıyla eklendi";
        //                        return true;
        //                    }
        //                    else
        //                    {
        //                        islemSonucu = "Kayıt sırasında bir hata oldu";
        //                        return false;
        //                    }
        //                }
        //                else
        //                {
        //                    islemSonucu = "Metinsiz yorum yapılamaz.";
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                islemSonucu = "Yorum sahipsiz olamaz.";
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            islemSonucu = "Yorum nesenesi boş olamaz.";
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        islemSonucu = ex.Message;
        //        return false;
        //    }
        //}


    }
}
