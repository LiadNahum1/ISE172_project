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

        public string execute(string query)
        {
            String concat = query +String.Format( " [nickname]='{0}'",nickname); //todo check name in table
            return concat;
        }
    }
}
