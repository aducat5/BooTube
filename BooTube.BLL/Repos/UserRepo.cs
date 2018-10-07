using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooTube.DAL;

namespace BooTube.BLL.Repos
{
    public class UserRepo
    {

        BooTubeEntities db = DBTool.GetInstance();
        public bool InsertUser(User item, out string islemSonucu)
        {

            try
            {
                if (item != null)
                {
                    item.CreateTime = DateTime.Now;
                    item.IsDeleted = false;

                    if (item.ProfilePicture == null)
                        item.ProfilePicture = "def.png";

                    if (item.FullName == null)
                        item.FullName = item.UserName;

                    if (!string.IsNullOrEmpty(item.Mail) && !string.IsNullOrEmpty(item.Password))
                    {
                        db.Users.Add(item);
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
                        islemSonucu = "Mail ve Password bilgisi boş geçilemez.";
                        return false;
                    }
                }
                else
                {
                    islemSonucu = "Kaydetmek istediğiniz nesne null olarak gönderilmiştir.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                islemSonucu = ex.Message;
                return false;
            }
        }

        public User IsThereThisUser(string uName, string pword)
        {
            if (string.IsNullOrEmpty(uName) || string.IsNullOrEmpty(pword))
            {
                return null;
            }
            else if (pword.Length < 6)
            {
                return null;
            }
            else
            {
                if (db.Users.Any(u => (u.UserName == uName && u.Password == pword) || (u.Mail == uName && u.Password == pword)))
                {
                    User current = db.Users.Where(u => (u.UserName == uName && u.Password == pword) || (u.Mail == uName && u.Password == pword)).FirstOrDefault();
                    return current;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<User> GetAll()
        {
            return db.Users.ToList();
        }

        public User GetUserByID(int id)
        {
            if (db.Users.Any(d => d.UserID == id))
            {
                return db.Users.Find(id);
            }
            else
            {
                return null;
            }
        }

        public int UserCount()
        {
            return db.Users.Count(u => u.FullName.Contains("a"));
        }
    }
}
