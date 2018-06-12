using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomProject.DataAccess
{
    class GroupFilter:IQueryAction
    {
        private int id;
        public GroupFilter(int id)
        {
            this.id = id;
        }

        public string execute(string query)
        {
            String concat = query + String.Format(" [Users].[Group_Id]={0}", id); // check in table
            return concat;
        }
    }
}
