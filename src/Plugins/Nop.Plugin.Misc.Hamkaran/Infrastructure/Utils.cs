using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.Hamkaran.Infrastructure
{
    public class Utils
    {

        public static bool TestConnection(string cstr)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(cstr);
                sqlConnection.Open();
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                    return true;
            }
            catch(Exception e)
            {

            }
            return false;

        }
    }
}
