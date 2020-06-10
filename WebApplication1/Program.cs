using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Threading;
using Microsoft.Extensions.Logging;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;

namespace WebApplication1
{
    public class Program
    {
        public static string[] addr1 = new string[10];
        public static string[] sname;
        public static string[] stype;
        public static string[] lat;
        public static string[] longt;
        public static string[] iscctv;
        public static int[] cctvcount;


        
        public static void Main(string[] args)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "DB서버";
                builder.UserID = "유저이름";
                builder.Password = "비밀번호";
                builder.InitialCatalog = "DB이름";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    int x = 0;
                    connection.Open();                    

                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append("SELECT schooltype,schoolname,addr1,lat,long,iscctv,cctvcount FROM SECURITY");

                    String sql2 = Convert.ToString(sb1.ToString());

                    using (SqlCommand command2 = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                x++;
                            }
                        }
                    }
                    sname = new string[x];
                    stype = new string[x];
                    addr1= new string[x];
                    lat = new string[x];
                    longt = new string[x];
                    iscctv = new string[x];
                    cctvcount = new int[x];
                    int i = 0;
                    using (SqlCommand command2 = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //Console.WriteLine("{0}", reader.GetString(0));
                                stype[i] = reader.GetString(0);
                                sname[i] = reader.GetString(1);
                                addr1[i] = reader.GetString(2);
                                lat[i] = reader.GetString(3);
                                longt[i] = reader.GetString(4);
                                iscctv[i] = reader.GetString(5);
                                cctvcount[i] = reader.GetInt32(6);
                                i++;
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
