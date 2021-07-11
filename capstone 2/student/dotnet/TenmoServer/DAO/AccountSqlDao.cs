using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TenmoServer.DAO
{
    public class AccountSqlDao : IAccountDao
    {
        private readonly string connectionString;



        public AccountSqlDao(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        public decimal GetBalanceSql(int accountId)         //SQL query to get the account balance of the logged in user
        {
            
            Account account = new Account();
            decimal balance = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select user_id, account_id, balance From accounts where account_id = @account_id", conn);
                cmd.Parameters.AddWithValue("@account_id", accountId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    account = CreateAccountFromReader(reader);
                }
                return balance = account.Balance;
            }

        }

        public List<int> GetUsersForTransfer()              // SQL query to get a list of users that can transfer money 
        {
            List<int> userAccounts = new List<int>();
            int currentId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("select account_id from users join accounts on users.user_id = accounts.user_id", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        currentId = Convert.ToInt32(reader["account_id"]);
                        userAccounts.Add(currentId);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return userAccounts;

        }

        public Transfer CreateTransfer(Transfer transfer)               // SQL query to create a transfer if the requested amount is over the available balance
        {
            if (GetBalanceSql(transfer.AccountFrom) >= transfer.Amount)
            {

                try
                {
                    int newTransferId;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("insert into dbo.transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount)" +
                            " Output inserted.transfer_id" +
                            " Values(2, 2, @account_from, @account_to, @amount)", conn);
                        cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                        cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                        cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                        newTransferId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    return GetTransfer(newTransferId);
                }
                catch (Exception ex)
                {
                    throw new Exception("An unexpected error occurred");
                }
            }
            else
            {
                throw new Exception("Unable to process transfer due to insufficient funds.");
            }
        }

        public Transfer GetTransfer(int transferId)             // SQL query to get a specific transfer record
        {
            Transfer transfer = new Transfer();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select transfer_id, transfer_type_id, transfer_status_id, account_from," +
                " account_to, amount from transfers where transfer_id = @transfer_id", conn);
                cmd.Parameters.AddWithValue("@transfer_id", transferId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    transfer = CreateTransferFromReader(reader);
                }
            }

            return transfer;
        }

        private Transfer CreateTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            transfer.AccountFrom = Convert.ToInt32(reader["account_from"]);
            transfer.AccountTo = Convert.ToInt32(reader["account_to"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);

            return transfer;
        }

        private Account CreateAccountFromReader(SqlDataReader reader)
        {
            Account account = new Account();
            account.AccountId = Convert.ToInt32(reader["account_id"]);
            account.UserId = Convert.ToInt32(reader["user_id"]);
            account.Balance = Convert.ToDecimal(reader["balance"]);

            return account;
        }
        public void ReduceAccount(Transfer transfer)                // SQL query to take money from one acount and reduce the available balance
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("update dbo.accounts set balance = (balance - @amount) where account_id = @account_id", conn);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                cmd.Parameters.AddWithValue("@account_id", transfer.AccountFrom);
                cmd.ExecuteNonQuery();
            }

        }
        public void IncreaseAccount(Transfer transfer)          // SQL query to add money to one account and increase balance in that account
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("update dbo.accounts set balance = (balance + @amount) where account_id = @account_id", conn);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                cmd.Parameters.AddWithValue("@account_id", transfer.AccountTo);
                cmd.ExecuteNonQuery();


            }
        }
        public List<Transfer> GetAllTransfers(int accountId)            // SQL query to get a list of all the transfers for a logged in user
        { 
            
            List<Transfer> fetchedTransfers = new List<Transfer>();
            Transfer transfer = new Transfer();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("select transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount " +
                        "from transfers Left join accounts on accounts.account_id = transfers.account_from " +
                        "where account_from = @account_from or account_to = @account_to", conn);
                    cmd.Parameters.AddWithValue("@account_from", accountId);
                    cmd.Parameters.AddWithValue("@account_to", accountId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        transfer = CreateTransferFromReader(reader);
                        fetchedTransfers.Add(transfer);
                       
                    }
                    return fetchedTransfers;                                           
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No transfers could be found for account id given");
            }
        }
        public int GetUserAccountId(string username)            // SQL query to get a users account ID
        {
            int accountId = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select account_id from accounts join users on accounts.user_id = users.user_id where username = @username", conn);
                cmd.Parameters.AddWithValue("@username", username);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    accountId = Convert.ToInt32(reader["account_id"]);
                }
            }
            return accountId;
        }
    }
}
