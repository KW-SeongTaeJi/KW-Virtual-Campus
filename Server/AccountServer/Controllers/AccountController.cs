using AccountServer.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Linq;
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

            AccountDB account = _appDb.Accounts
                .AsNoTracking()
                .Where(a => a.AccountId == req.AccountId)
                .FirstOrDefault();

            if (account == null)
            {
                AccountDB newAccount = new AccountDB()
                {
                    AccountId = req.AccountId,
                    Password = req.Password,
                    Name = req.Name
                };
                _appDb.Accounts.Add(newAccount);
                res.CreateAccountOk = _appDb.SaveChangesEx();
            }
            else
            {
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

            AccountDB account = _appDb.Accounts
                .AsNoTracking()
                .Where(a => a.AccountId == req.AccountId && a.Password == a.Password)
                .FirstOrDefault();

            if (account == null)
            {
                res.LoginOk = false;
            }
            else
            {
                // Regis에 token 저장
                string newToken = new Random().Next(Int32.MinValue, Int32.MaxValue).ToString();
                string tokenKey = account.AccountDBId.ToString();
                byte[] tokenValue = Encoding.UTF8.GetBytes(newToken);
                var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10));
                _redisCache.Set(tokenKey, tokenValue, options);

                res.LoginOk = true;
                res.AccountId = account.AccountId;
                res.Token = newToken;
                res.Name = account.Name;
                // TODO : 채널 정보 수정
                res.Channel = new ChannelInfo()
                {
                    IpAddress = "temp",
                    Port = 8000
                };
            }

            return res;
        }
    }
}
