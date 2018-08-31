using Lianyun.UST.Model.Repositories;
using Lianyun.UST.Model.Entities;
using System.Collections.Generic;
using System;
using System.Linq;
using Lianyun.UST.Infrastructure;
using Lianyun.UST.Model.Lianyun_UST_Entities;
using AutoMapper;
using Lianyun.UST.Infrastructure.Logging;

namespace Lianyun.UST.Repository
{
    public class AccountRepository : BaseRepository<DSP_Advertisers>,IAccountRepository
    {
        public AccountRepository(Lianyun_DSPContext Lianyun_DSPContext, ILogger logger)
            : base(Lianyun_DSPContext,logger)
        {

        }
         public Account GetAccountById(int accountId)
         {
             Account result = null;
             //using (DonsonLomarkDSPDataContext context = new DonsonLomarkDSPDataContext())
             //{
             //    var entity = (from account in context.tb_Account
             //                  where account.AccountId == accountId &&
             //                 account.IsDeleted == false
             //                  select account).FirstOrDefault();
             //    // result = entity.ToModel();
             //}
             DSP_Advertisers adv = base.Find(o => o.ID == accountId && o.IsDeleted == false);
             if (null != adv)
             {
                 result = new Account();
                 result.AccountId = (int)adv.ID;
                 result.Email = adv.LoginEmail;
                 result.CreateDate = adv.CreatedOn;
                 result.Password = adv.Pwd;
                 result.Username = adv.Company;
                 //result.EmailVerfied = adv.IsEffective;
                 result.Status = adv.Status;
                 result.AccountCode = adv.Code;

             }

             return result;
         }

         public Account GetAccountByEmail(string email)
         {
             Account result = null;  
             DSP_Advertisers adv = base.Find(o => o.LoginEmail == email && o.IsDeleted==false);
             if (null != adv)
             {
                 result = new Account();
                 result.AccountId = (int)adv.ID;
                 result.Email = adv.LoginEmail;
                 result.CreateDate = adv.CreatedOn;
                 result.Password = adv.Pwd;
                 result.Username = adv.Company;
                 //result.EmailVerfied = adv.IsEffective;
                 result.Status = adv.Status;
                 result.AccountCode = adv.Code;
                 result.IsAgent = false;
             }            
             return result;
         }

         public Account GetAccountByUsername(string username)
         {
             Account result = null;
             DSP_Advertisers adv = base.Find(o => o.Company == username && o.IsDeleted == false);
             if (null != adv)
             {
                 result = new Account();
                 result.AccountId = (int)adv.ID;
                 result.Email = adv.LoginEmail;
                 result.CreateDate = adv.CreatedOn;
                 result.Password = adv.Pwd;
                 result.Username = adv.Company;
                 //result.EmailVerfied = adv.IsEffective;
                 result.Status = adv.Status;
                 result.AccountCode = adv.Code;

             }
             return result;
         }

    }
}
