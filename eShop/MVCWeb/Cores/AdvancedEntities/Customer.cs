using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCWeb.Cores.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerName = string.Empty;
            PhoneNo = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            Region = string.Empty;
            Area = string.Empty;
            Note = string.Empty;
        }

        [NotMapped]
        public string SuggestName
        {
            get
            {
                return CustomerName +
                        (!string.IsNullOrEmpty(PhoneNo) ? " - " + PhoneNo : "") +
                        (!string.IsNullOrEmpty(Email) ? " - " + Email : "");
            }
        }

        [NotMapped]
        public string SuggestNameFull
        {
            get
            {
                return CustomerName +
                       (!string.IsNullOrEmpty(PhoneNo) ? " - " + PhoneNo : "") +
                       (!string.IsNullOrEmpty(Email) ? " - " + Email : "") +
                       (!string.IsNullOrEmpty(Region) ? " - " + Region : "") +
                       (!string.IsNullOrEmpty(Area) ? " - " + Area : "");
            }
        }

        [NotMapped]
        public string FullAddress
        {
            get
            {
                return Address +
                       (!string.IsNullOrEmpty(Region) ? " - " + Region : "") +
                       (!string.IsNullOrEmpty(Area) ? " - " + Area : "");
            }
        }
    }
}