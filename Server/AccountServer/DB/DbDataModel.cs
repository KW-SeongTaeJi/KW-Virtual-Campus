using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServer.DB
{
    [Table("UserAccount")]
    public class UserAccountDb
    {
        public int UserAccountDbId { get; set; }
        public string AccountId { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
