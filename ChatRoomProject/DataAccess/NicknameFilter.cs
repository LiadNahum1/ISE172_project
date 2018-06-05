using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomProject.DataAccess
{
    class NicknameFilter:IQueryAction
    {
        private string nickname;
        public NicknameFilter(string nickname)
        {
            this.nickname = nickname;
        }
    }
}
