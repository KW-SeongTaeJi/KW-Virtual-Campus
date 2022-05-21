using AccountServer.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AccountServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        AppDbContext _appDb;
        IDistributedCache _redisCache;


        public AccountController(AppDbContext appDb, IDistributedCache redisCache)
        {
            _appDb = appDb;
            _redisCache = redisCache;
        }

        // "api/account/createAccount" 
        [HttpPost]
        [Route("createAccount")]
        public CreateAccountPacketRes CreateAccount([FromBody]CreateAccountPacketReq req)
        {
            CreateAccountPacketRes res = new CreateAccountPacketRes();

            UserAccountDb account = _appDb.Accounts
                .AsNoTracking()
                .Where(a => a.AccountId == req.AccountId || a.Name == req.Name)
                .FirstOrDefault();

            if (account == null)
            {
                UserAccountDb newAccount = new UserAccountDb()
                {
                    AccountId = req.AccountId,
                    Password = req.Password,
                    Name = req.Name
                };
                _appDb.Accounts.Add(newAccount);
                res.CreateAccountOk = _appDb.SaveChangesEx();
                res.ErrorCode = 0;
            }
            else
            {
                if (account.Name == req.Name)
                    res.ErrorCode = 1;
                else if (account.AccountId == req.AccountId)
                    res.ErrorCode = 2;

                res.CreateAccountOk = false;
            }

            return res;
        }

        // "api/account/login" 
        [HttpPost]
        [Route("login")]
        public LoginPakcetRes Login([FromBody]CreateAccountPacketReq req)
        {
            LoginPakcetRes res = new LoginPakcetRes();

            UserAccountDb account = _appDb.Accounts
                .AsNoTracking()
                .Where(a => a.AccountId == req.AccountId)
                .FirstOrDefault();

            /* Login Fail */
            // ErrorCode 1 : not registered ID
            if (account == null)
            {
                res.LoginOk = false;
                res.ErrorCode = 1;
                return res;
            }
            // ErrorCode 2 : incorrect PW
            if (account.Password != req.Password)
            {
                res.LoginOk = false;
                res.ErrorCode = 2;
                return res;
            }
            // ErrorCode 3 : same user access 
            string tokenKey = account.AccountId;
            byte[] validCheck = _redisCache.Get($"{tokenKey}Where");
            if (validCheck != null && Encoding.Default.GetString(validCheck).Equals("end") == false)
            {
                res.LoginOk = false;
                res.ErrorCode = 3;
                return res;
            }

            /* Login Success */
            // Save token to Redis
            string newToken = new Random().Next(Int32.MinValue, Int32.MaxValue).ToString();
            byte[] tokenValue = Encoding.UTF8.GetBytes(newToken);
            //var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));
            _redisCache.Set(tokenKey, tokenValue);
            _redisCache.Set($"{tokenKey}Where", Encoding.UTF8.GetBytes("end"));

            res.LoginOk = true;
            res.ErrorCode = 0;
            res.AccountId = account.AccountId;
            res.Token = newToken;
            res.Name = account.Name;
            // TODO : 채널 주소 정보 수정
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            res.Channel = new ChannelInfo()
            {
                IpAddress = ipAddr.ToString(),
                Port = 4000
            };

            return res;
        }
    }
}
