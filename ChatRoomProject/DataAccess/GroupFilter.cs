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
    }
}
