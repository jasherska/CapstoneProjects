using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class ApiAccount
    {
        public int AccountId { get; set; }
        public int UserId { get; }
        public decimal Balance { get; set; }
    }
}
