using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankApi.Models
{
    public class CreateAccountViewModel
    {
        public string Owner { get; set; }

        public decimal InitialAmount { get; set; }
    }
}