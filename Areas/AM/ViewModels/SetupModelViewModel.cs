using FAPP.Areas.AM.BLL;
using FAPP.Model;
using PagedList;
using System;
using System.Collections.Generic;

namespace FAPP.Areas.AM.ViewModels
{
    public partial class SetupModelViewModel
    {
        public short? DefaultBranchId { get; set; }
        public bool All { get; set; }
        public bool Client { get; set; }
        public bool Supplier { get; set; }
        public bool Agent { get; set; }
        public string ProfileType { get; set; }
        public int? ClientTypeId { get; set; }
        public int? CategoryId { get; set; }
        public int? WareHouseId { get; set; }
        public string Search { get; set; }
        public WeekDay WeekDays { get; set; }
        public List<WeekDay> WeekDaysList { get; set; }
        public Year Years { get; set; }
        public List<Year> YearsList { get; set; }
        public UserGroup UserGroups { get; set; }
        public List<UserGroup> UserGroupsList { get; set; }
        public User Users { get; set; }
        public List<User> UsersList { get; set; }
        public UserBranch UserBranchs { get; set; }
        public List<UserBranch> UserBranchsList { get; set; }
        public List<Branches> BranchesList { get; set; }
        public int GroupId { get; set; }
        public int FormId { get; set; }
        public List<v_mnl_FormRights_Result> v_FormRights { get; set; }
        //public Specialization Specialization { get; set; }
        //public List<Specialization> SpecializationList { get; set; }
        //public Service Services { get; set; }
        //public List<Service> ServicesList { get; set; }
        public Country Country { get; set; }
        public List<Country> CountriesList { get; set; }
        public State State { get; set; }
        public List<State> StatesList { get; set; }
        public City City { get; set; }
        public List<City> CitiesList { get; set; }
        public District District { get; set; }
        public List<District> DistrictsList { get; set; }
        public Area Area { get; set; }
        public List<Area> AreasList { get; set; }
        //public ServicePricing ServicePricing { get; set; }
        //public List<ServicePricing> ServicePricingList { get; set; }
        public int? SpecializationId { get; set; }
        public int? ServiceTypeId { get; set; }
        //public ServiceType ServiceType { get; set; }
        //public List<ServiceType> ServiceTypeList { get; set; }
        public string autoKey { get; set; }
        public Nullable<long> ParentId { get; set; }
        public Account Accounts { get; set; }
        public List<Account> AccountList { get; set; }
        public long? AccountId { get; set; }
        public List<AMProceduresModel.p_mnl_Account__Search_Result> p__Account__Search_List { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public List<FiscalYear> FiscalYearList { get; set; }
        public TaxSlab TaxSlab { get; set; }
        public List<TaxSlab> TaxSlabList { get; set; }
        public CostGroup CostGroup { get; set; }
        public List<CostGroup> CostGroupList { get; set; }
        public int? FiscalYearId { get; set; }
        public IPagedList<AMProceduresModel.v_mnl_FillYearlyBalances_Result> v__FillYearlyBalancesList { get; set; }
        public List<AMProceduresModel.v_mnl_FillYearlyBalances_Result> FillYearlyBalancesList { get; set; }
        public IPagedList<AMProceduresModel.v_mnl_BankAccounts_Result> v__BankAccountsList { get; set; }
        public BankAccount BankAccount { get; set; }
        public ChequeBook ChequeBook { get; set; }
        public List<ChequeBook> ChequeBookList { get; set; }
        public CashAccount CashAccount { get; set; }
        public List<CashAccount> CashAccountList { get; set; }
        public CashAccountModel CashAccountModel { get; set; }
        public List<CashAccountModel> CashAccountModelList { get; set; }
        public List<AMProceduresModel.p_mnl__AccountsBalances_Result> p_mnl__AccountsBalances_Result { get; set; }
        //public SMBrand Brand { get; set; }
        //public List<SMBrand> BrandList { get; set; }
        public AMCategory Category { get; set; }
        public List<AMCategory> CategoryList { get; set; }
        //public SMProductGroup ProductGroup { get; set; }
        //public List<SMProductGroup> ProductGroupList { get; set; }
       
        public AMItem Product { get; set; }
        public List<AMItem> ProductList { get; set; }
        public IPagedList<AMItem> ProductPagedList { get; set; }
        public IPagedList<AMProceduresModel.p_mnl__OpeningStock_Result> p_mnl__OpeningStockPagedList { get; set; }
        public List<AMProceduresModel.p_mnl__OpeningStock_Result> p_mnl__OpeningStockList { get; set; }
        public IPagedList<AMProceduresModel.v_mnl_Clients_Result> v_mnl_ClientsList { get; set; }
        public Client Clients { get; set; }
        //public AccountGroup AccountGroups { get; set; }
        //public List<AccountGroup> AccountGroupList { get; set; }
        public List<Account> AccountListNew { get; set; }
        public List<AMProceduresModel.v_mnl_VoucherTransactions> VoucherTransactions { get; set; }
        //public SMSaleCounter SMSaleCounters { get; set; }
        //public List<SMSaleCounter> SMSaleCounterList { get; set; }

    }

    public partial class SetupModelViewModel
    {
        public IPagedList<AMWarehouse> AMWarehousePagedList { get; set; }
        public AMWarehouse AMWarehouse { get; set; }
        public List<AMWarehouse> AMWarehouseList { get; set; }
    }

   
  

    public class CashAccountModel
    {
        public string CashAccountId { get; set; }
        public string NewCashAccountId { get; set; }
    }






   
}