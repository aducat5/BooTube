using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooTube.DAL
{
    public class DBTool
    {
        private DBTool() {}


        private static BooTubeEntities _instance;

        public static BooTubeEntities GetInstance()
        {
            if (_instance == null)
                _instance = new BooTubeEntities();

            return _instance;
        }
    }
}
