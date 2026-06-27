using BreadApp_DL;

namespace BreadApp_API.Servieces
{
    public class clsConnectionString
    {

       
        public clsConnectionString(IConfiguration configuration)
        {
            clsConnectionSetting.ConnectionString(configuration.GetConnectionString("DefaultConnection") ?? "");
        }
        
    }
}
