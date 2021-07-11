using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class AccountController : ControllerBase
    {
        private IUserDao userDao;
        private IAccountDao dao;
        public AccountController(IAccountDao _dao, IUserDao _userDao)
        {
            dao = _dao;
            userDao = _userDao;
        }

        [HttpGet("balance")]
        public decimal GetBalance()                             // APi to get the balance of the logged in user
        {
            string username = User.Identity.Name;
            int accountId = dao.GetUserAccountId(username);
            decimal balance = dao.GetBalanceSql(accountId);
            return balance;
        }

        [HttpGet("transfer")]
        public List<int> GetUsersTable()                        // APi to Get a list of all the users 
        {
            List<int> listOfUserIds = new List<int>();

            listOfUserIds = dao.GetUsersForTransfer();

            return listOfUserIds;
        }

        [HttpPost("transfer")]
        public Transfer MakeATranser(Transfer transfer)             // APi to create a transaction record 
        {
            string userName = User.Identity.Name;
            int accountId = dao.GetUserAccountId(userName);
            Transfer transferMoney = new Transfer();
            transferMoney.AccountFrom = accountId;
            transferMoney.AccountTo = transfer.AccountTo;
            transferMoney.Amount = transfer.Amount;
            transferMoney = dao.CreateTransfer(transferMoney);
            return transferMoney;
        }

       
        [HttpPut("transfer")]
        public void ExecuteTransfer(Transfer transfer)              // APi to Add money to one account and reduce money in another
        {
            
            string userName = User.Identity.Name;
            int accountId = dao.GetUserAccountId(userName);
            Transfer transferMoney = new Transfer();
            transferMoney.AccountFrom = accountId;
            transferMoney.AccountTo = transfer.AccountTo;
            transferMoney.Amount = transfer.Amount;
            dao.ReduceAccount(transferMoney);
            dao.IncreaseAccount(transferMoney);


        }
        [HttpGet("transfers")]
        public List<Transfer> FetchAllTransfers()           // APi to get all transfer for a logged in user
        {
            string username = User.Identity.Name;
            int accountId = dao.GetUserAccountId(username);
            List<Transfer> returnedTransfers = new List<Transfer>();
            returnedTransfers = dao.GetAllTransfers(accountId);
            return returnedTransfers;

        }
        [HttpGet("{transferId}/transfer")]          // APi to get a specific transaction
        public Transfer FetchTransfer(int transferId)
        {

            Transfer getTransfer = new Transfer();
            getTransfer = dao.GetTransfer(transferId);
            return getTransfer;
        }
    }
}
