using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServer.DB
{
    [Table("Account")]
    public class AccountDB
    {
        public int AccountDBId { get; set; }
        public string AccountId { get; set; }
        public string Password { get; set; }
    }
}
