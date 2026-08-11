using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadApp_Struct.Models
{
    public  class UserModel
    { 
        public class UserByBreadPointDTO
        {

      
            public UserByBreadPointDTO(int UserID, string NationalNumber,
                string FullName, DateTime CreatedAt, bool IsScanned, DateTime? ScannedAt)
            {
                this.UserID = UserID;
                this.NationalNumber = NationalNumber;
                this.FullName = FullName;
                this.CreatedAt = CreatedAt;
                this.IsScanned = IsScanned;
                this.ScannedAt = ScannedAt;
            }
            public int UserID { get; set; }
            public string NationalNumber { get; set; }
            public string FullName { get; set; }
            public DateTime CreatedAt { get; set; }
            public bool IsScanned { get; set; }
            public DateTime? ScannedAt { get; set; }

        }


    }
}
