using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Plugin.Misc.Hamkaran.Domain;

namespace Nop.Plugin.Misc.Hamkaran.Services
{
    public partial class HamkaranTransferService : IHamkaranTransferService
    {

        private readonly IRepository<HamkaranProductRecord> _repoHamkaranTransfer;
        public HamkaranTransferService(IRepository<HamkaranProductRecord> repoHamkaranTransfer)
        {
            this._repoHamkaranTransfer = repoHamkaranTransfer;
        }

        public void InsertHamkaranProductRecord(HamkaranProductRecord hamkaranProductRecord)
        {
            _repoHamkaranTransfer.Insert(hamkaranProductRecord);
        }


        public void UpdateHamkaranProductRecord(HamkaranProductRecord hamkaranProductRecord , string code)
        {
            var uItem = _repoHamkaranTransfer.Table.Where(x => x.Code == code).FirstOrDefault();
            if (uItem != null)
            {
                hamkaranProductRecord.Id = uItem.Id;
                _repoHamkaranTransfer.Update(hamkaranProductRecord);
            }
        }

        public void DeleteHamkaranProductRecord(HamkaranProductRecord hamkaranProductRecord)
        {
            _repoHamkaranTransfer.Delete(hamkaranProductRecord);
        }


        public void SetActiveHamkaranProductRecord(int id , bool type)
        {
            var t = _repoHamkaranTransfer.GetById(id);
            t.Active = type;
            _repoHamkaranTransfer.Update(t);
        }


        public List<HamkaranProductRecord> GetAll()
        {
            return  _repoHamkaranTransfer.Table.ToList();
        }

    }
}
