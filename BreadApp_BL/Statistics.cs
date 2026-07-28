using BreadApp_DL;
using BreadApp_Struct.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadApp_BL
{
    public class Statistics
    {
        public static StatisticsModel.AdminStatisticsDTO? GetAdminStatistics()
        {
            return StatisticsData.GetAdminStatistics();
        }
        public static StatisticsModel.LogsStatisticsDTO? GetLogsStatistics()
        {
            return StatisticsData.GetLogStatistics();
        }   
    }
}
