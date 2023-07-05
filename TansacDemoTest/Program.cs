using System.Data.SqlClient;

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
            //Store the connection string in a variable
            string ConnectionString = @"Data Source=OCTOCAT\SQLEXPRESS;Initial Catalog=BankDB;Integrated Security=True";

            //Creating the connection object
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //Open the connection
                //The connection needs to be open before we begin a transaction
                connection.Open();

                // Create the transaction object by calling the BeginTransaction method on connection object
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Associate the first update command with the transaction
                    SqlCommand cmd = new SqlCommand("UPDATE Accounts SET Balance = Balance - 500 WHERE AccountNumber = 'Account1'", connection, transaction);
                    //Execute the First Update Command
                    cmd.ExecuteNonQuery();

                    // Associate the second update command with the transaction
                    cmd = new SqlCommand("UPDATE MyAccounts SET Balance = Balance + 500 WHERE AccountNumber = 'Account2'", connection, transaction);
                    //Execute the Second Update Command
                    cmd.ExecuteNonQuery();

                    // If everythinhg goes well then commit the transaction
                    transaction.Commit();
                    Console.WriteLine("Transaction Committed");
                }
                catch (Exception EX)
                {
                    // If anything goes wrong, then Rollback the transaction
                    transaction.Rollback();
                    Console.WriteLine("Transaction Rollback");
                }
            }
        }

        public static void Main(string[] args)
        {
            string connectionString = "Data Source=OCTOCAT\\SQLEXPRESS;Initial Catalog=BankDB;Integrated Security=True";
            try
            {
                Console.WriteLine("Before Transaction");
                GetAccountsData();

                //Doing the Transaction
                MoneyTransfer();

                //Verifying the Data After Transaction
                Console.WriteLine("After Transaction");

                GetAccountsData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Occurred: {ex.Message}");
            }
            Console.ReadKey();


        }
    }
}