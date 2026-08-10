using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadApp_Struct.Models
{
    public class StatisticsModel
    {
        public class AdminStatisticsDTO
        {
            public int? TotalReservation { get; set; }
            public int? RedeemReservation { get; set; }
            public int? PendingReservation { get; set; }
            public int? DailyServedUser { get; set; }
            public int? ActiveDistributionPoints { get; set; }
            public int? TotalUsers { get; set; }
        }
        public class BreadPointStatisticsDTO
        {
            public int? TotalOrders { get; set; }
            public int? Waiting { get; set; }
            public int? Deliviered { get; set; }
            public int? Cancelled { get; set; }
            public int? Expired { get; set; }
            public decimal? TotalMoneyRecived { get; set; }
        }
        public class LogsStatisticsDTO
        {
            public int? TotalLoginToday { get; set; }
            public int? FailedLoginToday { get; set; }
            public int? TotalApiRequests { get; set; }
            public int? TotalAuditActionToday { get; set; }
            public int? SuspiciousActivities { get; set; }
           
        }
    }
}
