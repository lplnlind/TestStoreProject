using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; private set; }

        public Email(string value)
        {
            if (!IsValid(value))
                throw new ArgumentException("Invalid email address.");

            Value = value;
        }

        private static bool IsValid(string email)
        {
            return email.Contains("@");
        }
    }
}
