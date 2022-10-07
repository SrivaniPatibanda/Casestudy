using System;
using System.Collections.Generic;

#nullable disable

namespace CustomerApp.Models
{
    public partial class TblLogin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
