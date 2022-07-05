using Nop.Core.Domain.Tasks;
using Nop.Services.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.Hamkaran.Services
{
    public class HamkaranSchedule : ITask
    {
        public class ImportProduct
        {
            public string Code { get; set; }
            public string OldCode { get; set; }
            public string Name { get; set; }
            public string EnName { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public string Unit { get; set; }
            public string Height { get; set; }
            public string Weight { get; set; }
            public string Length { get; set; }
            public string Width { get; set; }
            public string Nature { get; set; }
            public string NameFeature { get; set; }
            public string ValueFeature { get; set; }

        }

        private readonly HamkaranTransferService _hamkaranTransferService;
        public HamkaranSchedule(HamkaranTransferService hamkaranTransferService )
        {
            this._hamkaranTransferService = hamkaranTransferService;

        }

        private List<ImportProduct> GetDataFromDatabase()
        {
            string queryString = "select * from FakeProduct";
            SqlConnection sqlConnection = new SqlConnection();
            var sqlCommand = sqlConnection.CreateCommand();

            sqlCommand.CommandType = CommandType.Text;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = new SqlCommand(queryString, sqlConnection);

            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            List<ImportProduct> importProducts = new List<ImportProduct>();

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                importProducts.Add(new ImportProduct
                {
                    Code = dataTable.Rows[i]["کد کالا"].ToString(),
                    Description = dataTable.Rows[i]["توضیحات"].ToString(),
                    EnName = dataTable.Rows[i]["نام انگلیسی"].ToString(),
                    Height = dataTable.Rows[i]["ارتفاع"].ToString(),
                    Length = dataTable.Rows[i]["طول"].ToString(),
                    Name = dataTable.Rows[i]["نام کالا"].ToString(),
                    NameFeature = dataTable.Rows[i]["نام ویژگی"].ToString(),
                    Nature = dataTable.Rows[i]["ماهیت"].ToString(),
                    OldCode = dataTable.Rows[i]["کد قدیم"].ToString(),
                    Type = dataTable.Rows[i]["نوع کالا"].ToString(),
                    Unit = dataTable.Rows[i]["واحد سنجش"].ToString(),
                    ValueFeature = dataTable.Rows[i]["مقدار ویژگی"].ToString(),
                    Weight = dataTable.Rows[i]["وزن"].ToString(),
                    Width = dataTable.Rows[i]["عرض"].ToString(),
                }
                );
            }

            return importProducts;
        }

        public void Execute()
        {
            try
            {
                var rows = GetDataFromDatabase();
                if (rows.Count > 0)
                {
                    var hamkaranTransferRows = _hamkaranTransferService.GetAll();

                    foreach (var item in rows)
                    {
                        if (hamkaranTransferRows.Where(x => x.Code == item.Code).Any())
                        {
                            _hamkaranTransferService.InsertHamkaranProductRecord(
                                new Domain.HamkaranProductRecord
                                {
                                    Active = true,
                                    Code = item.Code,
                                    LastUpdateTime = DateTime.Now,
                                    OldCode = item.OldCode,
                                    ProductEnName = item.EnName,
                                    ProductName = item.Name
                                });
                        }
                        else
                        {
                            _hamkaranTransferService.UpdateHamkaranProductRecord(
                                new Domain.HamkaranProductRecord
                                {
                                    Active = true,
                                    Code = item.Code,
                                    LastUpdateTime = DateTime.Now,
                                    OldCode = item.OldCode,
                                    ProductEnName = item.EnName,
                                    ProductName = item.Name
                                });
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

        }

    }
}
