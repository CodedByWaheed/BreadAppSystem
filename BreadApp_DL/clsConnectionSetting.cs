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

        public static string ConnectionString => _connection?? "";

        
    }
}
