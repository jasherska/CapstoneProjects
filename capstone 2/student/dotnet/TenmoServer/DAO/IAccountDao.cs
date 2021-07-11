using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        public decimal GetBalanceSql(int userId);
        public Transfer CreateTransfer(Transfer transfer);
        public List<int> GetUsersForTransfer();
        public void ReduceAccount(Transfer transfer);
        public void IncreaseAccount(Transfer transfer);
        public List<Transfer> GetAllTransfers(int accountId);
        public Transfer GetTransfer(int transferId);
        public int GetUserAccountId(string username);
    }
}
