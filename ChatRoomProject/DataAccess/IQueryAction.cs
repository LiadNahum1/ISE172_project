﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomProject.DataAccess
{
    interface IQueryAction
    {
        String execute(String query); // build sql statement 
    }
}
