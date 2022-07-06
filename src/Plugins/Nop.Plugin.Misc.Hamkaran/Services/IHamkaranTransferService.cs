using Nop.Core;
using Nop.Plugin.Misc.Hamkaran.Domain;
using System.Collections.Generic;

namespace Nop.Plugin.Misc.Hamkaran.Services
{
    public partial interface IHamkaranTransferService
    {
        void InsertHamkaranProductRecord(HamkaranProductRecord hamkaranProductRecord);

        void UpdateHamkaranProductRecord(HamkaranProductRecord hamkaranProductRecord, string code);

        void DeleteHamkaranProductRecord(HamkaranProductRecord hamkaranProductRecord);

        void SetActiveHamkaranProductRecord(int id, bool type);

        List<HamkaranProductRecord> GetAll();

    }
}
