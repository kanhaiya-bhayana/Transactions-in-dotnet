using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace TransacDemoTest
{
    class Program
    {
        private static void GetAccountsData()
        {
            //Store the connection string in a variable
            string ConnectionString = @"Data Source=OCTOCAT\SQLEXPRESS;Initial Catalog=BankDB;Integrated Security=True";

            //Create the connection object
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from Accounts", connection);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Console.WriteLine(sdr["AccountNumber"] + ",  " + sdr["CustomerName"] + ",  " + sdr["Balance"]);
                }
            }
        }

        

        private static void MoneyTransfer()
        {
            string ConnectionString = @"Data Source=OCTOCAT\SQLEXPRESS;Initial Catalog=BankDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                
                using (SqlCommand cmd = new SqlCommand("InsertRecords", connection))
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@accNo", "Account9");
                    cmd.Parameters.AddWithValue("@custName", "Adam");
                    cmd.Parameters.AddWithValue("@balance", 10000);
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        cmd.Transaction = transaction;
                            using (SqlDataReader reader = cmd.ExecuteReader()) 
                            {    
                                if (reader.Read())
                                {
                                    Console.WriteLine("AccountNumber: "+reader.GetString(0) + 
                                        " CustomerName: "+reader.GetString(1) + " Balance: "+reader.GetInt32(2));
                                }
                            }

                    
                        
                            transaction.Commit();
                            Console.WriteLine("Transaction Committed");
                    }
                    catch (Exception EX)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction Rollback");
                    }
                }
                
            }
        }

        public static void Main(string[] args)
        {
            string connectionString = "Data Source=OCTOCAT\\SQLEXPRESS;Initial Catalog=BankDB;Integrated Security=True";
            try
            {
               /* Console.WriteLine("Before Transaction");
                GetAccountsData();*/

                //Doing the Transaction
                MoneyTransfer();

                //Verifying the Data After Transaction
                /*Console.WriteLine("After Transaction");

                GetAccountsData();*/
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred: {ex.Message}");
            }
            Console.ReadKey();


        }
    }
}