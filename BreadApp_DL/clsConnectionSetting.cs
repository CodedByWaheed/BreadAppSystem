using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BreadApp_DL
{
    public class clsConnectionSetting
    {

       
            private static string? _connection;

            
            public static void SetConnection(string connection)
            {
                _connection = connection;
            }

            public static string ConnectionString =>
                _connection ?? LocalConnString;

           // public static string LocalConnString =
               //    "Server=localhost;Database=BreadApp;User Id=sa;Password=123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";
        

        //private static string? Connection;

        //public static void ConnectionString(string connection)
        //{
        //    Connection = connection;
        //}

        //public static string? ConnectionString(bool local = false)
        //{
        //    return local switch
        //    {
        //        true =>LocalConnString ,
        //        false => Connection,
        //    };
        //}
        public static string LocalConnString = "Server=localhost;Database=BreadApp;User Id=sa;Password=123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";
        //public static string ConnectionString = "Server=tcp:breadapp-api-server.database.windows.net,1433;Initial Catalog=BreadAppDB;Persist Security Info=False;User ID=BreadAppAdmin;Password=BreadApp@2026;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //public static string ConnectionString = 
    }
}
