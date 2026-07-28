using BreadApp_Struct.AuthModel;
using BreadApp_Struct.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadApp_DL
{
    public class StatisticsData
    {
        public static StatisticsModel.AdminStatisticsDTO? GetAdminStatistics()
        {
            //StatisticsModel.AdminStatisticsDTO adminStats = new StatisticsModel.AdminStatisticsDTO();
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetAdminStatistics", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;



                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return MapToAdminStatistics(reader);
                    }
                   
                }
            }
            return null;
        }
        public static StatisticsModel.LogsStatisticsDTO? GetLogStatistics()
        {
            using (SqlConnection conn = new SqlConnection(clsConnectionSetting.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetLogStatistics", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return MapToLogsStatistics(reader);
                    }

                }
            }
            return null;
        }

        //-----------------------------/////////////MAPPER/////////////------------------------------
        private static StatisticsModel.AdminStatisticsDTO MapToAdminStatistics(SqlDataReader reader)
        {
            return new StatisticsModel.AdminStatisticsDTO
            {
                TotalReservation = reader.GetInt32(reader.GetOrdinal("TotalReservation")),
                RedeemReservation = reader.GetInt32(reader.GetOrdinal("RedeemReservation")),
                PendingReservation = reader.GetInt32(reader.GetOrdinal("PendingReservation")),
                DailyServedUser = reader.GetInt32(reader.GetOrdinal("DailyServedUser")),
                ActiveDistributionPoints = reader.GetInt32(reader.GetOrdinal("ActiveDistributionPoints")),
                TotalUsers = reader.GetInt32(reader.GetOrdinal("TotalUsers"))
            };
        }
        private static StatisticsModel.LogsStatisticsDTO MapToLogsStatistics(SqlDataReader reader)
        {
            return new StatisticsModel.LogsStatisticsDTO
            {
                TotalApiRequests = reader.GetInt32(reader.GetOrdinal("TotalApiRequests")),
                TotalLoginToday = reader.GetInt32(reader.GetOrdinal("TotalLoginToday")),
                FailedLoginToday = reader.GetInt32(reader.GetOrdinal("FailedLoginToday")),
                TotalAuditActionToday = reader.GetInt32(reader.GetOrdinal("TotalAuditActionToday")),
                SuspiciousActivities = reader.GetInt32(reader.GetOrdinal("SuspiciousActivities"))
            };
        }
    }
}

