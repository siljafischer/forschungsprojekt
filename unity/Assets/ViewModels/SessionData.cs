using Assets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.ViewModels
{
    public static class SessionData
    {
        public static User CurrentUser { get; set; }

        // logout
        public static void Clear()
        {
            CurrentUser = null;
        }
    }
}
