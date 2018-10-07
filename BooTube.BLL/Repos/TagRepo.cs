using BooTube.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooTube.BLL.Repos
{
    public class TagRepo
    {
        BooTubeEntities db = DBTool.GetInstance();

        public List<Tag> GetTagsWithText(string tagText)
        {
            tagText = GenFunx.CleanStringForUse(tagText.ToLower());
            return db.Tags.Where(t => t.TagText.Contains(tagText)).ToList();
        }
    }
}
