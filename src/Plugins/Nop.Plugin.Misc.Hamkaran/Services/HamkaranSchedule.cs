using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Tasks;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
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
        private readonly ISettingService _settingService;
        private readonly IHamkaranTransferService _hamkaranTransferService;
        private readonly HamkaranSettings hamkaranSettings;
        private readonly ILogger _logger;
        private readonly IProductService _productService;
        private readonly ILocalizationService _localizationService;
        private readonly ISpecificationAttributeService specificationAttributeService;

        public HamkaranSchedule(IHamkaranTransferService hamkaranTransferService , ISettingService settingService 
            , ILogger logger 
            , IProductService productService 
            ,ILocalizationService  localizationService
            , ISpecificationAttributeService specificationAttributeService)
        {
            this._hamkaranTransferService = hamkaranTransferService;
            this._settingService = settingService;
            hamkaranSettings = _settingService.LoadSetting<HamkaranSettings>();
            this._logger = logger;
            this._productService = productService;
            this.specificationAttributeService = specificationAttributeService;

        }

        private bool CreateProduct(ImportProduct importProduct)
        {

            var sProduct = _productService.GetProductBySku(importProduct.Code);
            if (sProduct == null)
                sProduct = new Product();

            
            sProduct.Name = importProduct.Name;
            sProduct.Sku = importProduct.Code;
            decimal weight = 0;
            if (decimal.TryParse(importProduct.Weight, out weight))
                sProduct.Weight = weight;

            decimal Width = 0;
            if (decimal.TryParse(importProduct.Width, out Width))
                sProduct.Width = Width;

            decimal Length = 0;
            if (decimal.TryParse(importProduct.Length, out Length))
                sProduct.Length = Length;

            decimal Height = 0;
            if (decimal.TryParse(importProduct.Height, out Height))
                sProduct.Height = Height;


            
            sProduct.ShortDescription = importProduct.Description;
            sProduct.ProductType = ProductType.SimpleProduct;
            sProduct.ProductTypeId = (int)ProductType.SimpleProduct;
            sProduct.IsShipEnabled = true;


            var productSpecificationAttributes = specificationAttributeService.GetProductSpecificationAttributes();

           // _productAttributeService.Get

            return true;
        }

        private List<ImportProduct> GetDataFromDatabase()
        {
            try
            {
                string queryString = "select * from vwPartCommrcial";
                SqlConnection sqlConnection = new SqlConnection(hamkaranSettings.ConnectionString);
                sqlConnection.Open();
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
            catch(Exception e)
            {
                _logger.Error(e.Message, e);
                return null;
            }

        }

        public void Execute()
        {
            try
            {
                if (!hamkaranSettings.Enable || string.IsNullOrEmpty(hamkaranSettings.ConnectionString))
                    return;

                var rows = GetDataFromDatabase();
                if (rows != null && rows.Count > 0)
                {
                    var hamkaranTransferRows = _hamkaranTransferService.GetAll();

                    foreach (var item in rows)
                    {
                        if (hamkaranTransferRows.Where(x => x.Code == item.Code).Any())
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
                                }, item.Code);

                        }
                        else
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
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

        }

    }
}
