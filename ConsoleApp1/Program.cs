using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static int[] no;
        static string[] stype;
        static string[] sname;
        static string[] addr1;
        static string[] addr2;
        static string[] lat;
        static string[] longt;
        static string[] iscctv;
        static int[] cctvcount;
        static DataSet ds = new DataSet();
        static string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\artfj\\Desktop\\통합 문서1.accdb;MODE=ReadWrite;";
        
        private static void count_info1()
        {
            string sql = "Select * FROM Sheet1";
            OleDbConnection conn = new OleDbConnection(connStr);
            OleDbCommand cmd = new OleDbCommand(sql, conn);

            cmd.CommandText = sql;
            int count = 0;
            conn.Open();
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                count++;
            }
            stype = new string[count];
            sname = new string[count];
            addr1 = new string[count];
            addr2 = new string[count];
            lat = new string[count];
            longt = new string[count];
            iscctv = new string[count];
            cctvcount = new int[count];

            Console.WriteLine("완료 {0}", count);
            reader.Close();
            conn.Close();
        }
        private static void search_info()
        {
            string sql = "Select * FROM Sheet1";
            OleDbConnection conn = new OleDbConnection(connStr);
            OleDbCommand cmd = new OleDbCommand(sql, conn);

            cmd.CommandText = sql;
            int i = 0;

            conn.Open();
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                stype[i] = reader["시설종류"].ToString();
                sname[i] = reader["대상시설명"].ToString();
                addr1[i] = reader["소재지도로"].ToString();
                addr2[i] = reader["소재지지번"].ToString();
                lat[i] = reader["위도"].ToString();
                longt[i] = reader["경도"].ToString();
                iscctv[i] = reader["CCTV설치유무"].ToString();
                cctvcount[1] = int.Parse(reader["CCTV설치대수"].ToString());
                i++;
            }
            reader.Close();
            conn.Close();
            Console.WriteLine("완료 {0}", i);
        }
        static void Main(string[] args)
        {
            OleDbConnection conn = new OleDbConnection(connStr);
            string sql1 = "SELECT * FROM Sheet1";
            OleDbDataAdapter adp = new OleDbDataAdapter(sql1, conn);
            adp.Fill(ds);
            count_info1();
            search_info();

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "DB주소";
                builder.UserID = "유저이름";
                builder.Password = "비밀번호";
                builder.InitialCatalog = "DB이름";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data ADD:");
                    Console.WriteLine("=========================================\n");

                    connection.Open();
                    SqlCommand command = null;
                    for (int i = 0; i<stype.Length; i++)
                    {
                        string sql = "INSERT INTO SECURITY VALUES ('" + stype[i] + "','" + sname[i] + "','" + addr1[i] + "','" + addr2[i] + "','"+lat+"','" + longt + "','" + iscctv + "',"+cctvcount+");";
                        command = new SqlCommand(sql, connection);
                        command.ExecuteNonQuery();
                    }
                    
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append("SELECT ADDR FROM BELL");
                    
                    String sql2 = Convert.ToString(sb1.ToString());
                    
                    using (SqlCommand command2 = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}", reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.ReadLine();
        }
    }
}

