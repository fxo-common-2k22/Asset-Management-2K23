namespace FAPP.Model
{
    using FAPP.AM.Models;
    //using DAL;
    using FAPP.Areas.Clinic.Models;
    using FAPP.Areas.Contact.Models;
    using FAPP.Areas.DairyFarm.Models;
   // using FAPP.Areas.FrontDesk.Models;
    using FAPP.Clinic.Models;
    using FAPP.Data;
    using FAPP.Finance.Models;
    using FAPP.FrontDesk.Models;
    using FAPP.HRPayroll.Models;
    using FAPP.INV;
    using FAPP.INV.Models;
    using FAPP.Res.Models;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class OneDbContext : DbContext
    {
        public static readonly int pagesize = 30;

        public OneDbContext() : base("name=OneDbContext")
        {
            Database.SetInitializer<OneDbContext>(null);
        }

        //public OneDbContext(string connString)
        //{
        //    Database.Connection.ConnectionString = connString;
        //}
        //System --------------------
        //public virtual DbSet<Application> Applications { get; set; }
        //public virtual DbSet<ApplicationModule> ApplicationModules { get; set; }
        public virtual DbSet<AppForm> AppForms { get; set; }
        public virtual DbSet<FormAction> FormActions { get; set; }
        public virtual DbSet<FormActionGroupRight> FormActionGroupRights { get; set; }
        //public virtual DbSet<FormGroupRight> FormGroupRights { get; set; }
        //public virtual DbSet<AppFormLabel> AppFormLabels { get; set; }
        //public virtual DbSet<SystemModule> SystemModules { get; set; }
        public virtual DbSet<UserGroupApplication> UserGroupApplications { get; set; }
        //public virtual DbSet<UserGroupApplicationModule> UserGroupApplicationModules { get; set; }

        //INV //Inventory // Procurement ----------
        //public virtual DbSet<CategoriesModule> CategoriesModules { get; set; }
        //public virtual DbSet<ProductModule> ProductModules { get; set; }
        //public virtual DbSet<InvoiceGRN> InvoiceGRNs { get; set; }
        //public virtual DbSet<InvoiceDeliveryChallan> InvoiceDeliveryChallans { get; set; }
        //public virtual DbSet<InvPriceChangeLog> InvPriceChangeLog { get; set; }
        //public virtual DbSet<InvOpeningStock> InvOpeningStocks { get; set; }
        //public virtual DbSet<InvOpeningStockMaster> InvOpeningStockMaster { get; set; }
        //public virtual DbSet<InvStockTransfer> InvStockTransfers { get; set; }
        //public virtual DbSet<InvStockTransferDetail> InvStockTransferDetails { get; set; }
        //public virtual DbSet<InvBrand> InvBrands { get; set; }
        public virtual DbSet<InvCategory> InvCategories { get; set; }
        //public virtual DbSet<GRN> GRN { get; set; }
        //public virtual DbSet<GRNProduct> GRNProducts { get; set; }
        //public virtual DbSet<InvProductGroup> InvProductGroups { get; set; }
        //public virtual DbSet<InvProductModel> InvProductModels { get; set; }
        //public virtual DbSet<InvProductPhoto> InvProductPhotos { get; set; }
        public virtual DbSet<InvProduct> InvProducts { get; set; }
        public virtual DbSet<InvPurchaseInvoiceProduct> InvPurchaseInvoiceProducts { get; set; }
        public virtual DbSet<InvPurchaseInvoice> InvPurchaseInvoices { get; set; }
        public virtual DbSet<InvPurchaseOrderProduct> InvPurchaseOrderProducts { get; set; }
        public virtual DbSet<InvPurchaseOrder> InvPurchaseOrders { get; set; }
        //public virtual DbSet<InvPurchaseReturnProduct> InvPurchaseReturnProducts { get; set; }
        //public virtual DbSet<InvPurchaseReturn> InvPurchaseReturns { get; set; }
        //public virtual DbSet<InvUnit> InvUnits { get; set; }
        //public virtual DbSet<InvWarehouseProduct> InvWarehouseProducts { get; set; }
        public virtual DbSet<InvWarehouse> InvWarehouses { get; set; }
        //public virtual DbSet<IssuanceDetail> InvIssuanceDetails { get; set; }
        //public virtual DbSet<Issuance> InvIssuance { get; set; }
        //public virtual DbSet<InvReturnIssuance> InvReturnIssuances { get; set; }
        //public virtual DbSet<InvReturnIssuanceDetail> InvReturnIssuanceDetails { get; set; }
        //public virtual DbSet<InvAccountSetting> InvAccountSettings { get; set; }
        public virtual DbSet<InvRequestStatus> InvRequestStatuses { get; set; }
        //public virtual DbSet<InvRequestTypes> InvRequestTypes { get; set; }
        public virtual DbSet<InvConditionType> InvConditionTypes { get; set; }
        public virtual DbSet<InvRequest> InvRequests { get; set; }
        public virtual DbSet<InvRequestDetail> InvRequestDetails { get; set; }
        //public virtual DbSet<InvShippingCompany> InvShippingCompanies { get; set; }
        //public virtual DbSet<InvShippingCompanyType> InvShippingCompanyTypes { get; set; }
        //public virtual DbSet<InvShippingMethod> InvShippingMethods { get; set; }
        //public virtual DbSet<InvShippingRatePolicy> InvShippingRatePolicy { get; set; }
        //public virtual DbSet<InvShippingType> InvShippingTypes { get; set; }
        //public virtual DbSet<InvUploadedProduct> InvUploadedProducts { get; set; }
        //public virtual DbSet<InvCategoryPhoto> InvCategoryPhotos { get; set; }
        //public virtual DbSet<InvSetting> InvSettings { get; set; }
        //public virtual DbSet<TaxItem> TaxItems { get; set; }
        //public virtual DbSet<TaxGroupItem> TaxGroupItems { get; set; }
        //public virtual DbSet<InvDiscountItem> InvDiscountItem { get; set; }
        //public virtual DbSet<InvDiscountGroupItem> InvDiscountGroupItems { get; set; }


        //PM Project Management--------
        //public virtual DbSet<PM.ProjectTaskUpdateHistory> ProjectTaskUpdateHistory { get; set; }
        //public virtual DbSet<PM.ProjectFiles> ProjectFiles { get; set; }
        //public virtual DbSet<PM.Projects> Projects { get; set; }
        //public virtual DbSet<PM.ProjectStatuses> ProjectStatuses { get; set; }
        //public virtual DbSet<PM.ProjectTaskDiscussion> ProjectTaskDiscussion { get; set; }
        //public virtual DbSet<PM.ProjectTaskFiles> ProjectTaskFiles { get; set; }
        //public virtual DbSet<PM.ProjectTasks> ProjectTasks { get; set; }
        //public virtual DbSet<PM.ProjectTaskUsers> ProjectTaskUsers { get; set; }
        //public virtual DbSet<PM.TaskStatuses> TaskStatuses { get; set; }
        //public virtual DbSet<PM.TaskTypes> TaskTypes { get; set; }
        //public virtual DbSet<PM.ProjectTypes> ProjectTypes { get; set; }
        //public virtual DbSet<PM.ProjectUsers> ProjectUsers { get; set; }
        //public virtual DbSet<PM.TeamUser> TeamUsers { get; set; }
        //public virtual DbSet<PM.Team> Teams { get; set; }
        //public virtual DbSet<PM.UserRole> UserRoles { get; set; }
        //public virtual DbSet<PM.Action> Actions { get; set; }
        //public virtual DbSet<PM.UserRoleAction> UserRoleActions { get; set; }

        //ImportExport---------------
        //public virtual DbSet<Models.ImportExport.ImportExportFeeReceipt> ImportExportFeeReceipts { get; set; }
        //public virtual DbSet<Models.ImportExport.ImportExportFeeVoucher> ImportExportFeeVoucher { get; set; }
        //public virtual DbSet<Models.ImportExport.ImportExportFinanceVoucher> ImportExportFinanceVoucher { get; set; }
        //public virtual DbSet<Models.ImportExport.ImportExportResult> ImportExportResult { get; set; }
        //public virtual DbSet<Models.ImportExport.ImportExportStudent> ImportExportStudent { get; set; }
        //public virtual DbSet<Models.ImportExport.ImportFeeStudentCredit> ImportFeeStudentCredit { get; set; }
        //public virtual DbSet<Models.ImportExport.ImportFeeStudentDiscount> ImportFeeStudentDiscounts { get; set; }
        //public virtual DbSet<Models.ImportExport.ImportAdmissionVouchers> ImportAdmissionVouchers { get; set; }
        //public virtual DbSet<Models.ImportExport.ImportApplicant> ImportApplicants { get; set; }
        //public virtual DbSet<Models.ImportExport.ImportExportEmployee> ImportExportEmployees { get; set; }

        public virtual DbSet<ApplicationPortal> ApplicationPortals { get; set; }

        //clc--------------
        //public virtual DbSet<AppointmentRecordType> AppointmentRecordTypes { get; set; }
        //public virtual DbSet<AppointmentRecord> AppointmentRecords { get; set; }
        //public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        //public virtual DbSet<ServicePricing> ServicePricings { get; set; }
        //public virtual DbSet<Service> Services { get; set; }
        //public virtual DbSet<ClinicDepartment> ClinicDepartments { get; set; }
        //public virtual DbSet<Appointment> Appointments { get; set; }
        //public virtual DbSet<Doctor> Doctors { get; set; }
        //public virtual DbSet<Patient> Patients { get; set; }
        //public virtual DbSet<AppointmentEntryType> AppointmentEntryType { get; set; }
        //public virtual DbSet<AppointmentStatus> AppointmentStatus { get; set; }
        //public virtual DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }
        //public virtual DbSet<Specialization> Specializations { get; set; }
        //public virtual DbSet<AppointmentSupplies> AppointmentSupplies { get; set; }
        //public virtual DbSet<ClinicSetting> ClinicSettings { get; set; }
        //public virtual DbSet<ClcInvoice> ClcInvoices { get; set; }
        //public virtual DbSet<ClcInvoiceParticular> ClcInvoiceParticulars { get; set; }
        //public virtual DbSet<ClcInvoicePayment> ClcInvoicePayments { get; set; }
        //public virtual DbSet<ClcInvoicePaymentMain> ClcInvoicePaymentMain { get; set; }


        //Client-----------------
        public virtual DbSet<Client> Clients { get; set; }
        //public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        //public virtual DbSet<NextOfKin> NextOfKins { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        //public virtual DbSet<ContactCategory> ContactCategories { get; set; }
        //public virtual DbSet<DiscountGroup> DiscountGroups { get; set; }
        public virtual DbSet<FAPP.Areas.Contact.Models.SupplierInvoicePayment> SupplierInvoicePayments { get; set; }
        public virtual DbSet<FAPP.Areas.Contact.Models.SupplierPayment> SupplierPayments { get; set; }
        //public virtual DbSet<DeviceInfo> DeviceInfoes { get; set; }
        //public virtual DbSet<ClientDocumentSetting> ClientDocumentSettings { get; set; }




        //Company---------------------
        public virtual DbSet<Branch> Branches { get; set; }
        //public virtual DbSet<BusinessGroup> BusinessGroups { get; set; }
        //public virtual DbSet<ApiIntegrationSetting> ApiIntegrationSettings { get; set; }
        public virtual DbSet<Info> Info { get; set; }
        public virtual DbSet<WeekDay> WeekDays { get; set; }
        //public virtual DbSet<Year> Years { get; set; }
        //public virtual DbSet<AuthorisedIP> AuthorisedIPs { get; set; }
        //public virtual DbSet<Franchise> Franchises { get; set; }
        //public virtual DbSet<ShippingPersonnel> ShippingPersonnels { get; set; }
        //public virtual DbSet<ShippingVehicle> ShippingVehicles { get; set; }
        //public virtual DbSet<ShippingPersonnelVehicle> ShippingPersonnelVehicles { get; set; }
        //public virtual DbSet<BranchFranchise> BranchFranchises { get; set; }
        //public virtual DbSet<CompanySystemLanguage> CompanySystemLanguage { get; set; }
        //public virtual DbSet<CompanyRBSetting> CompanyRBSettings { get; set; }


        //Data--------------
        //public virtual DbSet<Language> Languages { get; set; }
        //public virtual DbSet<CountryConfiguration> CountryConfigurations { get; set; }
        //public virtual DbSet<CountrySpecification> CountrySpecifications { get; set; }
        //public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        //public virtual DbSet<Zone> Zones { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<HouseType> HouseTypes { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<PaymentMode> PaymentModes { get; set; }
        public virtual DbSet<Profession> Professions { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        //public virtual DbSet<Relation> Relations { get; set; }
        public virtual DbSet<Religion> Religions { get; set; }
        public virtual DbSet<State> States { get; set; }
        //public virtual DbSet<CountryMetaField> CountryMetaFields { get; set; }
        //public virtual DbSet<CountryMetaFieldValue> CountryMetaFieldValues { get; set; }
        //public virtual DbSet<DataTransportType> DataTransportTypes { get; set; }


        //Finance------------------
        //public virtual DbSet<FinanceVoucherIdBackup> FinanceVoucherIdBackups { get; set; }
        //public virtual DbSet<AllowanceConfiguration> AllowanceConfiguration { get; set; }
        //public virtual DbSet<AllowanceAndDeductionType> AllowanceAndDeductionTypes { get; set; }
        //public virtual DbSet<AllowanceDeductionGroup> AllowanceDeductionGroups { get; set; }
        //public virtual DbSet<FinanceDocument> FinanceDocuments { get; set; }
        //public virtual DbSet<AccountGroup> AccountGroups { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountSetting> AccountSettings { get; set; }
        //public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        //public virtual DbSet<BudgetDetail> BudgetDetails { get; set; }
        //public virtual DbSet<Budget> Budgets { get; set; }
        public virtual DbSet<CashAccount> CashAccounts { get; set; }
        //public virtual DbSet<ChequeBook> ChequeBooks { get; set; }
        public virtual DbSet<CostGroup> CostGroups { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<CurrencyValue> CurrencyValues { get; set; }
        //public virtual DbSet<ExpectedMonthlyExpens> ExpectedMonthlyExpenses { get; set; }
        public virtual DbSet<FiscalYear> FiscalYears { get; set; }
        //public virtual DbSet<MonthlyBalance> MonthlyBalances { get; set; }
        //public virtual DbSet<MonthlyExpens> MonthlyExpenses { get; set; }
        public virtual DbSet<StatementSetup> StatementSetups { get; set; }
        public virtual DbSet<TaxSlab> TaxSlabs { get; set; }
        public virtual DbSet<VoucherDetail> VoucherDetails { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        //public virtual DbSet<VoucherCategory> VoucherCategories { get; set; }
        public virtual DbSet<VoucherType> VoucherTypes { get; set; }
        //public virtual DbSet<YearlyBalance> YearlyBalances { get; set; }
        //public virtual DbSet<VoucherImportLog> VoucherImportLogs { get; set; }
        //public virtual DbSet<DefaultBudget> DefaultBudgets { get; set; }
        //public virtual DbSet<DefaultBudgetDetail> DefaultBudgetDetails { get; set; }
        //public virtual DbSet<ConfigurationValue> ConfigurationValues { get; set; }
        //public virtual DbSet<PaymentModeAccount> PaymentModeAccounts { get; set; }

        //Helpers-----------------
        //public virtual DbSet<Calendar> Calendars { get; set; }
        //public virtual DbSet<Number> Numbers { get; set; }

        //Membership
        //public virtual DbSet<PortalLoginHistory> PortalLoginHistory { get; set; }
        public virtual DbSet<Form> Forms { get; set; }
        //public virtual DbSet<FormRight> FormRights { get; set; }
        //public virtual DbSet<GroupRight> GroupRights { get; set; }
        //public virtual DbSet<FormLabel> FormLabels { get; set; }
        //public virtual DbSet<OpeningBalance> OpeningBalances { get; set; }
        //public virtual DbSet<Areas.Finance.Models.BillItems> BillItems { get; set; }
        //public virtual DbSet<Areas.Finance.Models.Bills> Bills { get; set; }
        //public virtual DbSet<Areas.Finance.Models.FinanceSupplierInvoicePayment> FinanceSupplierInvoicePayments { get; set; }
        //public virtual DbSet<Areas.Finance.Models.FinanceSupplierPayment> FinanceSupplierPayments { get; set; }
        //public virtual DbSet<ClientService> ClientServices { get; set; }
        //public virtual DbSet<Areas.Finance.Models.RecurringBill> RecurringBills { get; set; }
        //public virtual DbSet<ClientServiceProduct> ClientServiceProducts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserBranch> UserBranches { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<DefaultAccount> DefaultAccounts { get; set; }
        //public virtual DbSet<Salesman> Salesmans { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<ActionHistoryLog> ActionHistoryLogs { get; set; }
        public virtual DbSet<UrlSetting> UrlSettings { get; set; }

        //POS Schema---------
        //public virtual DbSet<PosPriceChangeLog> PosPriceChangeLogs { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSBrand> PosBrands { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSCategory> PosCategories { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSClientInvoicePayment> PosClientInvoicePayments { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSClientPayment> PosClientPayments { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSClientRefundInvoice> PosClientRefundInvoices { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSClientRefund> PosClientRefunds { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSDamageProduct> PosDamageProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSDamage> PosDamages { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSOpeningStock> PosOpeningStocks { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSOpeningStockMaster> POSOpeningStockMaster { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSProductGroup> PosProductGroups { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSProductModel> PosProductModels { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSProductPhoto> PosProductPhotos { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSProduct> PosProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSPurchaseInvoiceProduct> PosPurchaseInvoiceProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSPurchaseInvoice> PosPurchaseInvoices { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSPurchaseOrderProduct> PosPurchaseOrderProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSPurchaseOrder> PosPurchaseOrders { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSPurchaseReturnProduct> PosPurchaseReturnProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSPurchaseReturn> PosPurchaseReturns { get; set; }
        public virtual DbSet<Areas.POS.Models.POSSaleInvoiceProduct> PosSaleInvoiceProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSSaleInvoice> PosSaleInvoices { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSSaleReturnProduct> PosSaleReturnProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSSaleReturn> PosSaleReturns { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSSetting> PosSettings { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSSupplierInvoicePayment> PosSupplierInvoicePayments { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSSupplierPayment> PosSupplierPayments { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSSupplierRefundInvoice> PosSupplierRefundInvoices { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSSupplierRefund> PosSupplierRefunds { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSUnit> PosUnits { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSWarehouseProduct> PosWarehouseProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSWarehouse> PosWarehouses { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSSaleCounter> PosSaleCounters { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSSaleCounterSession> POSSaleCounterSessions { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSPackage> POSPackages { get; set; }
        //public virtual DbSet<Areas.POS.Models.PosProduction> PosProduction { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSProductionUnit> PosProductionUnits { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSMaterialIssuance> PosMaterialIssuances { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSMaterialIssuanceProduct> POSMaterialIssuanceProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSDeliveryChallan> PosDeliveryChallans { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSDeliveryChallanDetail> PosDeliveryChallanDetails { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSWorkOrderDeliveryChallanHistory> PosWorkOrderDeliveryChallanHistory { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSWorkOrder> PosWorkOrders { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSWorkOrderDetail> PosWorkOrderDetails { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSWorkOrderSaleInvoiceHistory> PosWorkOrderSaleInvoiceHistory { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSProductionVoucher> PosProductionVouchers { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSProductionVoucherProduct> PosProductionVoucherProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSFinishGoodVoucher> PosFinishGoodVouchers { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSFinishGoodVoucherProduct> PosFinishGoodVoucherProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSProductionVoucherFinishedGoodVoucherHistory> PosProductionVoucherFinishedGoodVoucherHistory { get; set; }
        //public virtual DbSet<POSInvoiceNote> POSInvoiceNotes { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSVehicleMakers> POSVehicleMakers { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSVehicleModels> POSVehicleModels { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSVehicleYears> POSVehicleYears { get; set; }
        public virtual DbSet<POSGeneralSetting> POSGeneralSettings { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSVehicles> POSVehicles { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSInvoiceVehicles> POSInvoiceVehicles { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSStockTaking> POSStockTakings { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSShipper> POSShippers { get; set; }
        //public virtual DbSet<Areas.POS.Models.Port> Ports { get; set; }
        //public virtual DbSet<Areas.POS.Models.POSContainer> Containers { get; set; }
        //public virtual DbSet<Areas.POS.Models.ContainerProduct> ContainerProducts { get; set; }
        //public virtual DbSet<Areas.POS.Models.ContainerClearingBill> ClearingBills { get; set; }
        //public virtual DbSet<Areas.POS.Models.ClearingBillItems> ClearingBillItems { get; set; }
        //public virtual DbSet<Areas.POS.Models.FreightBill> FreightBills { get; set; }
        //public virtual DbSet<Areas.POS.Models.FreightBillItem> FreightBillItems { get; set; }
        //public virtual DbSet<Areas.POS.Models.ClearingBillContainer> ClearingBillContainers { get; set; }
        //public virtual DbSet<Areas.POS.Models.FreigthBillContainer> FreigthBillContainers { get; set; }
        //public virtual DbSet<Areas.POS.Models.SupplierClearingPayment> SupplierClearingPayments { get; set; }
        //public virtual DbSet<Areas.POS.Models.SupplierClearingBillPayment> SupplierClearingBillPayments { get; set; }
        //public virtual DbSet<Areas.POS.Models.SupplierFreightPayment> SupplierFreightPayments { get; set; }
        //public virtual DbSet<Areas.POS.Models.SupplierFreightBillPayment> SupplierFreightBillPayments { get; set; }
        //public virtual DbSet<Areas.POS.Models.ClientAddress> ClientAdresses { get; set; }
        //public virtual DbSet<Areas.POS.Models.DeliveryStatus> DeliveryStatuses { get; set; }
        //public virtual DbSet<Areas.POS.Models.Status> Statuses { get; set; }
        //public virtual DbSet<Areas.POS.Models.SaleInvoiceDeliveryHistory> SaleInvoiceDeliveryHistories { get; set; }
        //public virtual DbSet<Areas.POS.Models.PosDiscount> PosDiscounts { get; set; }
        //public virtual DbSet<Areas.POS.Models.PosDiscountsProduct> PosDiscountsProducts { get; set; }

        //Res Schema--------------
        //public virtual DbSet<ReturnItem> ReturnItems { get; set; }
        //public virtual DbSet<ReturnItemDetail> ReturnItemDetails { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResKitchens> ResKitchens { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResSections> ResSections { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResTables> ResTables { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResLocations> ResLocations { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResSubLocations> ResSubLocations { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResWaiters> ResWaiters { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResItemOrderStatuses> ResItemOrderStatuses { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResMenuItems> ResMenuItems { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResOrders> ResOrders { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResOrderDetails> ResOrderDetails { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResSaleCounter> ResSaleCounters { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResSaleCounterSession> ResSaleCounterSessions { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResOrderStatuses> ResOrderStatuses { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResClientInvoicePayment> ResClientInvoicePayments { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResClientPayment> ResClientPayments { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResSupplierInvoicePayment> ResSupplierInvoicePayments { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResSupplierPayment> ResSupplierPayments { get; set; }
        //public virtual DbSet<Areas.Contact.Models.ClientSupplierRefundInvoice> ClientSupplierRefundInvoices { get; set; }
        //public virtual DbSet<Areas.Contact.Models.ClientSupplierRefund> ClientSupplierRefunds { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResSettings> ResSettings { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResRecipe> ResRecipe { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResPackage> ResPackages { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResIssueItem> ResIssueItems { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResIssueItemDetail> ResIssueItemDetails { get; set; }
        //public virtual DbSet<ResAccountSetting> ResAccountSettings { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResOrdersTable> ResOrdersTables { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResOrderType> ResOrderTypes { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResComment> ResComments { get; set; }
        //public virtual DbSet<Areas.Res.Models.ResDiscountCoupon> ResDiscountCoupons { get; set; }
        //public virtual DbSet<MealCourse> ResMealCourses { get; set; }
        //public virtual DbSet<MealCuisine> ResMealCuisines { get; set; }
        //public virtual DbSet<LocalPOSUserRight> LocalPOSUserRights { get; set; }

        //FrontDesk----------
        //public virtual DbSet<ReservationChart> ReservationChart { get; set; }
        //public virtual DbSet<ReservationGuest> ReservationGuests { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.Building> Buildings { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.CategoryFacility> CategoryFacilities { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.Floor> Floors { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.Hotel> Hotels { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.InvoiceParticular> InvoiceParticulars { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.InvoicePayment> InvoicePayments { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.Refund> Refunds { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.Invoice> Invoices { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.ReservationDetail> ReservationDetails { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.Reservation> Reservations { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.ReservationService> ReservationServices { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.ReservationServiceDetail> ReservationServiceDetails { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.RoomCategory> RoomCategories { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.RoomFacility> RoomFacilities { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.RoomPrice> RoomPrices { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.Room> Rooms { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.RoomStatus> RoomStatuses { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskService> FrontDeskServices { get; set; }
        public virtual DbSet<FrontDeskSetting> FrontDeskSettings { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.InvoicePaymentMain> InvoicePaymentMains { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.NightPosting> NightPostings { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.GuestType> GuestTypes { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskBrand> FrontDeskBrands { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskCategory> FrontDeskCategories { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskProductGroup> FrontDeskProductGroups { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskProductModel> FrontDeskProductModels { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskProductPhoto> FrontDeskProductPhotos { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskProduct> FrontDeskProducts { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskPurchaseInvoiceProduct> FrontDeskPurchaseInvoiceProducts { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskPurchaseInvoice> FrontDeskPurchaseInvoices { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskPurchaseOrderProduct> FrontDeskPurchaseOrderProducts { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskPurchaseOrder> FrontDeskPurchaseOrders { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskPurchaseReturnProduct> FrontDeskPurchaseReturnProducts { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskPurchaseReturn> FrontDeskPurchaseReturns { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskUnit> FrontDeskUnits { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskWarehouseProduct> FrontDeskWarehouseProducts { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskWarehouse> FrontDeskWarehouses { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskSaleCounter> FrontDeskSaleCounters { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskSaleCounterSession> FrontDeskSaleCounterSessions { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskSupplierInvoicePayment> FrontDeskSupplierInvoicePayments { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskSupplierPayment> FrontDeskSupplierPayments { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskSupplierRefundInvoice> FrontDeskSupplierRefundInvoices { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskSupplierRefund> FrontDeskSupplierRefunds { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskOpeningStock> FrontDeskOpeningStocks { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskRecipe> FrontDeskRecipe { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskPackage> FrontDeskPackages { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskIssueItem> FrontDeskIssueItems { get; set; }
        //public virtual DbSet<Areas.FrontDesk.Models.FrontDeskIssueItemDetail> FrontDeskIssueItemDetails { get; set; }
        //public virtual DbSet<ReservationDocument> ReservationDocuments { get; set; }

        //CRM----------
        //public virtual DbSet<Areas.CRM.Models.CRMCategory> CRMCategory { get; set; }
        //public virtual DbSet<Areas.CRM.Models.CRMStage> CRMStage { get; set; }
        //public virtual DbSet<Areas.CRM.Models.CRMSource> CRMSource { get; set; }
        //public virtual DbSet<Areas.CRM.Models.CRMProduct> CRMProduct { get; set; }
        public virtual DbSet<Areas.CRM.Models.CRMAttachment> CRMAttachment { get; set; }
        //public virtual DbSet<Areas.CRM.Models.CRMOpportunity> CRMOpportunity { get; set; }

        //SM-------
        //public virtual DbSet<SMBrand> SMBrands { get; set; }
        //public virtual DbSet<SMCategory> SMCategories { get; set; }
        //public virtual DbSet<SMClientInvoicePayment> SMClientInvoicePayments { get; set; }
        //public virtual DbSet<SMClientPayment> SMClientPayments { get; set; }
        //public virtual DbSet<SMClientRefundInvoice> SMClientRefundInvoices { get; set; }
        //public virtual DbSet<SMClientRefund> SMClientRefunds { get; set; }
        //public virtual DbSet<SMDamageProduct> SMDamageProducts { get; set; }
        //public virtual DbSet<SMDamage> SMDamages { get; set; }
        //public virtual DbSet<SMGuarantor> SMGuarantors { get; set; }
        //public virtual DbSet<SMInstallmentPayment> SMInstallmentPayments { get; set; }
        //public virtual DbSet<SMInstallmentSaleProduct> SMInstallmentSaleProducts { get; set; }
        //public virtual DbSet<SMInstallmentSale> SMInstallmentSales { get; set; }
        //public virtual DbSet<SMOpeningStock> SMOpeningStocks { get; set; }
        //public virtual DbSet<SMPaymentSchedule> SMPaymentSchedules { get; set; }
        //public virtual DbSet<SMProductGroup> SMProductGroups { get; set; }
        //public virtual DbSet<SMProductModel> SMProductModels { get; set; }
        //public virtual DbSet<SMProductPhoto> SMProductPhotos { get; set; }
        //public virtual DbSet<SMProduct> SMProducts { get; set; }
        //public virtual DbSet<SMPurchaseInvoiceProduct> SMPurchaseInvoiceProducts { get; set; }
        //public virtual DbSet<SMPurchaseInvoice> SMPurchaseInvoices { get; set; }
        //public virtual DbSet<SMPurchaseOrderProduct> SMPurchaseOrderProducts { get; set; }
        //public virtual DbSet<SMPurchaseOrder> SMPurchaseOrders { get; set; }
        //public virtual DbSet<SMPurchaseReturnProduct> SMPurchaseReturnProducts { get; set; }
        //public virtual DbSet<SMPurchaseReturn> SMPurchaseReturns { get; set; }
        //public virtual DbSet<SMSaleInvoiceProduct> SMSaleInvoiceProducts { get; set; }
        //public virtual DbSet<SMSaleInvoice> SMSaleInvoices { get; set; }
        //public virtual DbSet<SMSaleReturnProduct> SMSaleReturnProducts { get; set; }
        //public virtual DbSet<SMSaleReturn> SMSaleReturns { get; set; }
        //public virtual DbSet<SMSetting> SMSettings { get; set; }
        //public virtual DbSet<SMSupplierInvoicePayment> SMSupplierInvoicePayments { get; set; }
        //public virtual DbSet<SMSupplierPayment> SMSupplierPayments { get; set; }
        //public virtual DbSet<SMSupplierRefundInvoice> SMSupplierRefundInvoices { get; set; }
        //public virtual DbSet<SMSupplierRefund> SMSupplierRefunds { get; set; }
        //public virtual DbSet<SMUnit> SMUnits { get; set; }
        //public virtual DbSet<SMWarehouseProduct> SMWarehouseProducts { get; set; }
        //public virtual DbSet<SMWarehouse> SMWarehouses { get; set; }
        //public virtual DbSet<EmployeeDeductionParameter> EmployeeDeductionParameter { get; set; }
        //public virtual DbSet<EmployeeWeeklyHoliday> EmployeeWeeklyHolidays { get; set; }
        //public virtual DbSet<DeductionParameter> DeductionParameters { get; set; }
        //public virtual DbSet<EmployeeLateDeductionParameter> EmployeeLateDeductionParameters { get; set; }

        //HR and Payroll------
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        //public virtual DbSet<PayScale> PayScales { get; set; }
        //public virtual DbSet<PayScaleAllowance> PayScaleAllowances { get; set; }
        //public virtual DbSet<HRDoc> HRDocs { get; set; }
        public virtual DbSet<HRShift> HRShifts { get; set; }
        //public virtual DbSet<HRSetting> HRSettings { get; set; }
        //public virtual DbSet<HolidayCalendar> HolidayCalendar { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        //public virtual DbSet<EmployeeBranch> EmployeeBranches { get; set; }
        public virtual DbSet<EmployeePlacement> EmployeePlacements { get; set; }
        //public virtual DbSet<EmploymentType> EmploymentTypes { get; set; }
        public virtual DbSet<PlacementType> PlacementTypes { get; set; }
        //public virtual DbSet<EmployeeContact> EmployeeContacts { get; set; }
        //public virtual DbSet<EmployeeCertificate> EmployeeCertificates { get; set; }
        public virtual DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public virtual DbSet<EmployeeEducation> EmployeeEducations { get; set; }
        public virtual DbSet<EmployeeGuarantor> EmployeeGuarantors { get; set; }
        //public virtual DbSet<EmployeeHostel> EmployeeHostels { get; set; }
        //public virtual DbSet<EducationalLevel> EducationalLevels { get; set; }
        public virtual DbSet<EmployeeWorkExperience> EmployeeWorkExperiences { get; set; }
        //public virtual DbSet<HRSection> HRSections { get; set; }
        public virtual DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public virtual DbSet<LeaveType> LeaveTypes { get; set; }
        //public virtual DbSet<Leave> Leaves { get; set; }
        //public virtual DbSet<LateDeductionParameter> LateDeductionParameters { get; set; }
        //public virtual DbSet<EmployeeAttendanceDetail> EmployeeAttendanceDetails { get; set; }
        //public virtual DbSet<HRAttendanceRule> HRAttendanceRules { get; set; }
        //public virtual DbSet<HREmployeeAttendanceRule> HREmployeeAttendanceRules { get; set; }
        //public virtual DbSet<AdvancePayment> AdvancePayments { get; set; }
        public virtual DbSet<AllowancesAndDeduction> AllowancesAndDeductions { get; set; }
        //public virtual DbSet<EmployeeAllowancesAndDeduction> EmployeeAllowancesAndDeductions { get; set; }
        //public virtual DbSet<MonthlyEmployeeAllowanceDeduction> MonthlyEmployeeAllowanceDeductions { get; set; }
        //public virtual DbSet<EmployeeSalaryIncrement> EmployeeSalaryIncrements { get; set; }
        //public virtual DbSet<Letter> Letters { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        //public virtual DbSet<SalaryAllowance> SalaryAllowances { get; set; }
        public virtual DbSet<SalaryDetail> SalaryDetails { get; set; }
        //public virtual DbSet<WeeklyHoliday> WeeklyHolidays { get; set; }
        //public virtual DbSet<DepartmentsLeaveQuota> DepartmentsLeaveQuotas { get; set; }
        //public virtual DbSet<LeaveApplication> LeaveApplications { get; set; }
        //public virtual DbSet<LeaveApplicationsHistory> LeaveApplicationsHistory { get; set; }
        //public virtual DbSet<Item> Items { get; set; }
        //public virtual DbSet<ItemCategory> ItemCategories { get; set; }
        //public virtual DbSet<ItemUnit> ItemUnits { get; set; }
        //public virtual DbSet<ItemBillDetail> ItemBillDetails { get; set; }
        //public virtual DbSet<ItemBillInvoice> ItemBillInvoices { get; set; }
        //public virtual DbSet<VendorPayment> VendorPayments { get; set; }
        //public virtual DbSet<VendorBillPayment> VendorBillPayments { get; set; }
        //public virtual DbSet<PaymentClient> Client_Payments { get; set; }
        //public virtual DbSet<ClientBillPayment> ClientBillPayments { get; set; }
        //public virtual DbSet<ItemNature> ItemNatures { get; set; }
        //public virtual DbSet<SMSaleCounterSession> SaleCounterSessions { get; set; }
        //public virtual DbSet<SMSaleCounter> SaleCounters { get; set; }
        //public virtual DbSet<Vacancy> HRVacancies { get; set; }
        //public virtual DbSet<ApplicantInfo> HRApplicantsInfo { get; set; }
        //public virtual DbSet<ApplicantDocument> HRApplicantDocuments { get; set; }
        //public virtual DbSet<ApplicantExperience> HRApplicantExperiences { get; set; }
        //public virtual DbSet<HRApplicantEducation> HRApplicantEducations { get; set; }
        //public virtual DbSet<VacancyApplication> HRVacancyApplications { get; set; }
        //public virtual DbSet<HREvaluationField> HREvaluationFields { get; set; }
        //public virtual DbSet<InterviewBoard> InterviewBoards { get; set; }
        //public virtual DbSet<InterviewBoardMember> InterviewBoardMembers { get; set; }
        //public virtual DbSet<HREvaluationGrade> HREvaluationGrades { get; set; }
        //public virtual DbSet<HREvaluationType> HREvaluationTypes { get; set; }
        //public virtual DbSet<VacancyApplicationEvaluation> VacancyApplicationEvaluations { get; set; }
        //public virtual DbSet<HREmployeeCategory> HREmployeeCategory { get; set; }
        //public virtual DbSet<HREmployeeSubCategory> HREmployeeSubCategory { get; set; }
        //public virtual DbSet<HRDocumentSetting> HRDocumentSettings { get; set; }

        //System------
        //public virtual DbSet<SystemSetting> SystemSettings { get; set; }

        //AM------
        public virtual DbSet<TransferHistory> TransferHistory { get; set; }
        public virtual DbSet<DepreciationMain> Depreciations { get; set; }
        public virtual DbSet<DepreciationDetail> DepreciationDetails { get; set; }
        public virtual DbSet<ItemRegister> ItemRegister { get; set; }
        public virtual DbSet<AMItem> AMItems { get; set; }
        public virtual DbSet<AMCategory> AMCategories { get; set; }
        public virtual DbSet<AMNature> AMNatures { get; set; }
        public virtual DbSet<AMUnit> AMUnits { get; set; }
        public virtual DbSet<AMPurchaseReturnProduct> AMPurchaseReturnProducts { get; set; }
        public virtual DbSet<AMPurchaseReturn> AMPurchaseReturns { get; set; }
        public virtual DbSet<AMPurchaseOrderProduct> AMPurchaseOrderProducts { get; set; }
        public virtual DbSet<AMPurchaseInvoiceProduct> AMPurchaseInvoiceProducts { get; set; }
        //public virtual DbSet<AMPurchaseInvoice> AMPurchaseInvoices { get; set; }
        //public virtual DbSet<AMPurchaseOrder> AMPurchaseOrders { get; set; }
        public virtual DbSet<AMWarehouse> AMWarehouses { get; set; }
        public virtual DbSet<AMWarehouseProduct> AMWarehouseProducts { get; set; }
        //public virtual DbSet<AMSupplierInvoicePayment> AMSupplierInvoicePayments { get; set; }
        //public virtual DbSet<AMSupplierPayment> AMSupplierPayments { get; set; }
        public virtual DbSet<AMSupplierRefund> AMSupplierRefunds { get; set; }
        public virtual DbSet<AMSupplierRefundInvoice> AMSupplierRefundInvoices { get; set; }
        public virtual DbSet<DepriciationType> DepriciationTypes { get; set; }
        //public virtual DbSet<AMRequest> AMRequests { get; set; }
        public virtual DbSet<AMRequestDetail> AMRequestDetail { get; set; }
        public virtual DbSet<AMRequestStatus> AMRequestStatus { get; set; }
        public virtual DbSet<AMPurchaseInvoiceProductDetail> AMPurchaseInvoiceProductDetails { get; set; }
        public virtual DbSet<AMIssuedItem> AMIssuedItems { get; set; }
        public virtual DbSet<AMIssuedItemDetail> AMIssuedItemDetails { get; set; }
        public virtual DbSet<AMOpeningStock> AMOpeningStocks { get; set; }
        public virtual DbSet<AMConditionType> AMConditionTypes { get; set; }
        public virtual DbSet<AMReturnIssue> AMReturnIssue { get; set; }
        public virtual DbSet<AMReturnIssueDetail> AMReturnIssueDetails { get; set; }
        //public virtual DbSet<AMRequestTypes> AMRequestTypes { get; set; }
        public virtual DbSet<AMDepreciationTypes> AMDepreciationTypes { get; set; }


        //Media---------
        public virtual DbSet<SmsQueue> SmsQueues { get; set; }
        //public virtual DbSet<EmailQueue> EmailQueues { get; set; }
        public virtual DbSet<TemplateType> TemplateTypes { get; set; }
        public virtual DbSet<Template> Templates { get; set; }
        //public virtual DbSet<NotificationSetting> NotificationSettings { get; set; }
        //public virtual DbSet<NotificationAdminUser> NotificationAdminUsers { get; set; }
        //public virtual DbSet<Notification> Notifications { get; set; }
        //public virtual DbSet<Conversation> Conversations { get; set; }
        //public virtual DbSet<ConversationDetail> ConversationDetails { get; set; }

        //dbo-------
        //public virtual DbSet<ACGroup> ACGroup { get; set; }
        //public virtual DbSet<acholiday> acholiday { get; set; }
        //public virtual DbSet<ACTimeZones> ACTimeZones { get; set; }
        //public virtual DbSet<ACUnlockComb> ACUnlockComb { get; set; }
        //public virtual DbSet<AlarmLog> AlarmLog { get; set; }
        //public virtual DbSet<AttParam> AttParam { get; set; }
        //public virtual DbSet<AuditedExc> AuditedExc { get; set; }
        //public virtual DbSet<AUTHDEVICE> AUTHDEVICE { get; set; }
        //public virtual DbSet<CHECKEXACT> CHECKEXACT { get; set; }
        public virtual DbSet<CHECKINOUT> CHECKINOUT { get; set; }
        //public virtual DbSet<DEPARTMENTS_dbo> DEPARTMENTS_dbo { get; set; }
        //public virtual DbSet<DeptUsedSchs> DeptUsedSchs { get; set; }
        //public virtual DbSet<FaceTemp> FaceTemp { get; set; }
        //public virtual DbSet<HOLIDAYS> HOLIDAYS { get; set; }
        //public virtual DbSet<LeaveClass> LeaveClass { get; set; }
        //public virtual DbSet<LeaveClass1> LeaveClass1 { get; set; }
        //public virtual DbSet<Machines> Machines { get; set; }
        //public virtual DbSet<NUM_RUN> NUM_RUN { get; set; }
        //public virtual DbSet<NUM_RUN_DEIL> NUM_RUN_DEIL { get; set; }
        //public virtual DbSet<OpLog> OpLog { get; set; }
        //public virtual DbSet<SchClass> SchClass { get; set; }
        //public virtual DbSet<SECURITYDETAILS> SECURITYDETAILS { get; set; }
        //public virtual DbSet<SHIFT> SHIFT { get; set; }
        //public virtual DbSet<SystemLog> SystemLog { get; set; }
        //public virtual DbSet<TBSMSALLOT> TBSMSALLOT { get; set; }
        //public virtual DbSet<TBSMSINFO> TBSMSINFO { get; set; }
        //public virtual DbSet<TEMPLATE_dbo> TEMPLATE_dbo { get; set; }
        //public virtual DbSet<USER_OF_RUN> USER_OF_RUN { get; set; }
        //public virtual DbSet<USER_SPEDAY> USER_SPEDAY { get; set; }
        //public virtual DbSet<USER_TEMP_SCH> USER_TEMP_SCH { get; set; }
        //public virtual DbSet<UserACMachines> UserACMachines { get; set; }
        //public virtual DbSet<UserACPrivilege> UserACPrivilege { get; set; }
        public virtual DbSet<USERINFO> USERINFOes { get; set; }
        //public virtual DbSet<UserProfile> UserProfile { get; set; }
        //public virtual DbSet<UserUpdates> UserUpdates { get; set; }
        //public virtual DbSet<UserUsedSClasses> UserUsedSClasses { get; set; }
        //public virtual DbSet<EmOpLog> EmOpLog { get; set; }
        //public virtual DbSet<Query> Query { get; set; }
        //public virtual DbSet<ServerLog> ServerLog { get; set; }
        //public virtual DbSet<UsersMachines> UsersMachines { get; set; }
        public virtual DbSet<ErrorLogs> ErrorLogs { get; set; }
        //public virtual DbSet<DebugLog> DebugLogs { get; set; }

        //Shop
        //public virtual DbSet<Areas.Shop.Models.ShopAccountSetting> ShopAccountSettings { get; set; }

        //PR
        //public virtual DbSet<Areas.HRPayroll.Models.EmployeeIncrementPlan> EmployeeIncrementPlans { get; set; }
        //public virtual DbSet<Areas.HRPayroll.Models.EmployeeSecurityPlan> EmployeeSecurityPlans { get; set; }
        //public virtual DbSet<Areas.HRPayroll.Models.EmployeeAdvancePaymentReturnPlan> EmployeeAdvancePaymentReturnPlan { get; set; }
        //public virtual DbSet<EmployeeSecurityRefund> EmployeeSecurityRefund { get; set; }
        
        //Hostel------
        //public virtual DbSet<HostelBuilding> HostelBuildings { get; set; }
        //public virtual DbSet<HostelFloor> HostelFloors { get; set; }
        //public virtual DbSet<HostelRoom> HostelRooms { get; set; }
        //public virtual DbSet<HostelRoomCategory> HostelRoomCategories { get; set; }
        //public virtual DbSet<HostelBed> HostelBeds { get; set; }

        //Event------
        //public virtual DbSet<Areas.Event.Models.EventType> EventTypes { get; set; }
        //public virtual DbSet<Areas.Event.Models.EventMeal> EventMeals { get; set; }
        //public virtual DbSet<Areas.Event.Models.EventLocation> EventLocations { get; set; }
        //public virtual DbSet<Areas.Event.Models.EventQuotation> EventQuotations { get; set; }
        //public virtual DbSet<Areas.Event.Models.EventQuotationDetail> EventQuotationDetails { get; set; }
        //public virtual DbSet<Areas.Event.Models.EventEvent> EventEvents { get; set; }
        //public virtual DbSet<Areas.Event.Models.EventEventDetail> EventEventDetails { get; set; }
        //public virtual DbSet<Areas.Event.Models.EventEventLocation> EventEventLocations { get; set; }
        //public virtual DbSet<Areas.Event.Models.QuotationLocation> QuotationLocations { get; set; }
        //public virtual DbSet<Areas.Event.Models.EventClientPayment> EventClientPayments { get; set; }
        //public virtual DbSet<Areas.Event.Models.EventClientInvoicePayment> EventClientInvoicePayments { get; set; }
        //public virtual DbSet<Areas.Event.Models.EventDetailSubItem> EventDetailSubItems { get; set; }
        //public virtual DbSet<Areas.Event.Models.QuotationDetailSubItem> QuotationDetailSubItems { get; set; }

        //RealEstate // 03.09.2020
        //public virtual DbSet<REAccountSetting> REAccountSettings { get; set; }
        //public virtual DbSet<Inventory> Inventory { get; set; }
        //public virtual DbSet<InventoryType> InventoryTypes { get; set; }
        //public virtual DbSet<RealEstateProject> RealEstateProjects { get; set; }
        //public virtual DbSet<RealEstateProjectType> RealEstateProjectTypes { get; set; }
        //public virtual DbSet<RealEstateContractNominee> RealEstateContractNominees { get; set; }
        //public virtual DbSet<Phase> Phases { get; set; }
        //public virtual DbSet<Block> Blocks { get; set; }
        //public virtual DbSet<Street> Streets { get; set; }
        //public virtual DbSet<PlotSize> PlotSizes { get; set; }
        //public virtual DbSet<ProjectTypeInventoryType> ProjectTypeInventoryTypes { get; set; }
        //public virtual DbSet<InventoryFeatureType> InventoryFeatureTypes { get; set; }
        //public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        //public virtual DbSet<InventoryFeature> InventoryFeatures { get; set; }
        //public virtual DbSet<Contract> Contracts { get; set; } 
        //public virtual DbSet<PaymentPlan> PaymentPlans { get; set; }
        //public virtual DbSet<PaymentPlanDetail> PaymentPlanDetails { get; set; }
        //public virtual DbSet<MembershipPaymentPlanDetail> MembershipPaymentPlanDetails { get; set; }
        //public virtual DbSet<MembershipPuchasePaymentPlanDetail> MembershipPuchasePaymentPlanDetails { get; set; }
        //public virtual DbSet<MembershipPayment> MembershipPayments { get; set; }
        //public virtual DbSet<MembershipInvoice> MembershipInvoices { get; set; }
        //public virtual DbSet<MembershipInvoiceDetail> MembershipInvoiceDetails { get; set; }
        //public virtual DbSet<MembershipHistory> MembershipHistory { get; set; }
        //public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        //public virtual DbSet<RealEstateLetter> RealEstateLetters { get; set; }
        //public virtual DbSet<RealEstatePayment> RealEstatePayments { get; set; }
        //public virtual DbSet<RealEstateInvoicePayment> RealEstateInvoicePayments { get; set; }
        //public virtual DbSet<RealEstateInvoice> RealEstateInvoices { get; set; }
        //public virtual DbSet<RealEstateInvoiceDetail> RealEstateInvoiceDetails { get; set; }
        //public virtual DbSet<RealEstatePurchaseInvoice> RealEstatePurchaseInvoices { get; set; }
        //public virtual DbSet<RealEstatePurchaseInvoiceDetail> RealEstatePurchaseInvoiceDetails { get; set; }
        //public virtual DbSet<RealEstatePurchasePayment> RealEstatePurchasePayments { get; set; }
        //public virtual DbSet<RealEstatePurchaseInvoicePayment> RealEstatePurchaseInvoicePayments { get; set; }
        //public virtual DbSet<MembershipStage> MembershipStages { get; set; }
        //public virtual DbSet<MembershipOpeningBalance> MembershipOpeningBalances { get; set; }
        //public virtual DbSet<RealEstateDocument> RealEstateDocuments { get; set; }
        //public virtual DbSet<ReaslEstateFloor> ReaslEstateFloors { get; set; }
        //public virtual DbSet<RealEstateContract> RealEstateContracts { get; set; }
        //public virtual DbSet<RealEstateContractType> RealEstateContractTypes { get; set; }
        //public virtual DbSet<RealEstateContractInventory> RealEstateContractInventory { get; set; }
        //public virtual DbSet<RealEstateContractOwner> RealEstateContractContacts { get; set; }
        //public virtual DbSet<RealEstateSetting> RealEstateSettings { get; set; }
        //public virtual DbSet<RealEstatePurchaseContractItem> RealEstatePurchaseContractItems { get; set; }
        //public virtual DbSet<PurchasePaymentPlan> PurchasePaymentPlans { get; set; }
        //public virtual DbSet<ProjectExtraFeatureTypePricing> ProjectExtraFeatureTypePricing { get; set; }
        //public virtual DbSet<RealEstateHeldInventory> RealEstateHeldInventory { get; set; }
        

        #region schoolproject
        public virtual DbSet<ImportWizard> ImportWizard { get; set; }
        //public virtual DbSet<UserFiscalYear> UserFiscalYears { get; set; }
        //public virtual DbSet<CustomReportActionLinks> CustomReportActionLinks { get; set; }
        //public virtual DbSet<TemplateFilter> TemplateFilters { get; set; }
        ////BillingManagementSystem
        //public virtual DbSet<ContactNumber> ContactNumbers { get; set; }
        //public virtual DbSet<GroupType> GroupTypes { get; set; }
        //public virtual DbSet<MainGroup> MainGroups { get; set; }
        //public virtual DbSet<PricingHead> PricingHeads { get; set; }
        //public virtual DbSet<Pricing> Pricings { get; set; }
        //public virtual DbSet<ProductsService> ProductsServices { get; set; }
        //public virtual DbSet<PromotionCode> PromotionCodes { get; set; }

        //Contact------------
        //public virtual DbSet<AccountSettings1> AccountSettings1 { get; set; }

        //CM----------
        //public virtual DbSet<ContentType> ContentTypes { get; set; }
        //public virtual DbSet<Location> Locations { get; set; }
        //public virtual DbSet<Order> Orders { get; set; }
        //public virtual DbSet<OrderStatuss> OrderStatusses { get; set; }
        //public virtual DbSet<PackageType> PackageTypes { get; set; }
        //public virtual DbSet<CMPricing> CMPricings { get; set; }
        //public virtual DbSet<Tracking> Trackings { get; set; }
        //public virtual DbSet<Vehicle> Vehicles { get; set; }
        //public virtual DbSet<VehicleType> VehicleTypes { get; set; }
        //public virtual DbSet<CMZone> CMZones { get; set; }
        //public virtual DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }
        //public virtual DbSet<DeletionLog> DeletionLogs { get; set; }
        //public virtual DbSet<EntryLog> EntryLogs { get; set; }
        //public virtual DbSet<Donation> Donations { get; set; }
        //public virtual DbSet<Donor> Donors { get; set; }
        //public virtual DbSet<DMProject> DMProjects { get; set; }
        //public virtual DbSet<C_Placements> C_Placements { get; set; }
        //public virtual DbSet<ContractorEmployee> ContractorEmployees { get; set; }
        //public virtual DbSet<Contractor> Contractors { get; set; }
        //public virtual DbSet<EmployeeComplaint> EmployeeComplaints { get; set; }
        //public virtual DbSet<EmployeeLateMin> EmployeeLateMins { get; set; }
        //public virtual DbSet<EmployeeLeaveQuota> EmployeeLeaveQuotas { get; set; }
        //public virtual DbSet<EmployeeLetter> EmployeeLetters { get; set; }
        //public virtual DbSet<TaskEmployee> TaskEmployees { get; set; }
        //public virtual DbSet<TaskItem> TaskItems { get; set; }
        //public virtual DbSet<Task> Tasks { get; set; }

        public virtual DbSet<Certificate> Certificates { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        //public virtual DbSet<Report> Reports { get; set; }
        //public virtual DbSet<Filter> Filters { get; set; }
        public virtual DbSet<Templates1> Templates11 { get; set; }
        public virtual DbSet<View> Views { get; set; }

        //SB
        //public virtual DbSet<Payments1> Payments1 { get; set; }

        //TM----------
        //public virtual DbSet<Areas1> Areas1 { get; set; }
        //public virtual DbSet<ClientReceipt> ClientReceipts { get; set; }
        //public virtual DbSet<ClientTrip> ClientTrips { get; set; }
        //public virtual DbSet<Driver> Drivers { get; set; }
        //public virtual DbSet<Fine> Fines { get; set; }
        //public virtual DbSet<Locations3> Locations3 { get; set; }
        //public virtual DbSet<SupplierTrip> SupplierTrips { get; set; }
        //public virtual DbSet<Trip_Management> Trip_Management { get; set; }
        //public virtual DbSet<TripItem> TripItems { get; set; }
        //public virtual DbSet<Trip> Trips { get; set; }
        //public virtual DbSet<VehicleExpenseItem> VehicleExpenseItems { get; set; }
        //public virtual DbSet<VehicleExpens> VehicleExpenses { get; set; }
        //public virtual DbSet<VehicleExpType> VehicleExpTypes { get; set; }
        //public virtual DbSet<Vehicles1> Vehicles1 { get; set; }
        //public virtual DbSet<VehicleTypes1> VehicleTypes1 { get; set; }
        //public virtual DbSet<VisaReceipt> VisaReceipts { get; set; }
        //public virtual DbSet<VisaService> VisaServices { get; set; }
        //public virtual DbSet<VisaServiceImage> VisaServiceImages { get; set; }
        //public virtual DbSet<VisaServiceInvoiceParticular> VisaServiceInvoiceParticulars { get; set; }
        //public virtual DbSet<VisaServiceInvoice> VisaServiceInvoices { get; set; }
        //public virtual DbSet<Zones1> Zones1 { get; set; }

        //Dairy Farm

        //public virtual DbSet<Animal> Animals { get; set; }
        //public virtual DbSet<AnimalType> AnimalTypes { get; set; }
        //public virtual DbSet<AnimalBreed> AnimalBreeds { get; set; }
        //public virtual DbSet<AnimalWeight> AnimalWeights { get; set; }
        //public virtual DbSet<AnimalMilkProduction> MilkProductions { get; set; }
        //public virtual DbSet<AnimalPurchase> AnimalPurchases { get; set; }
        //public virtual DbSet<AnimalPurchaseItem> AnimalPurchaseItems { get; set; }
        //public virtual DbSet<AnimalSale> AnimalSales { get; set; }
        //public virtual DbSet<AnimalSaleItem> AnimalSalesItems { get; set; }
        //public virtual DbSet<AnimalGrowthStage> AnimanGrowthStages { get; set; }
        //public virtual DbSet<Semen> Semens { get; set; }
        //public virtual DbSet<AnimalServicing> AnimalServicings { get; set; }
        //public virtual DbSet<SemenPurchase> SemenPurchases { get; set; }
        //public virtual DbSet<SemenPurchaseDetail> SemenPurchaseDetails { get; set; }
        //public virtual DbSet<AnimalPregnancyRecord> AnimalPregnancyRecords { get; set; }
        //public virtual DbSet<AnimalLactation> AnimalLactations { get; set; }

        //Company
        public virtual DbSet<CompanyBuilding> CompanyBuildings { get; set; }
        public virtual DbSet<CompanyFloor> CompanyFloors { get; set; }
        public virtual DbSet<CompanyRoom> CompanyRooms { get; set; }
        //public virtual DbSet<CompanyRegion> CompanyRegions { get; set; }
        //public virtual DbSet<Locations1> Locations1 { get; set; }
        public virtual DbSet<NewsEvent> NewsEvents { get; set; }
        //public virtual DbSet<NewsEventsBranch> NewsEventsBranches { get; set; }
        //public virtual DbSet<NewsEventUserGroup> NewsEventUserGroups { get; set; }
        //public virtual DbSet<Signature> Signatures { get; set; }
        //public virtual DbSet<ToDo> ToDos { get; set; }


        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Doctor>()
                    .Property(e => e.Phone)
                    .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .Property(e => e.NIC)
                .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Doctor>()
                .Property(e => e.Nationality)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.MRNo)
                .IsUnicode(false);


            modelBuilder.Entity<Patient>()
                .Property(e => e.BloodGroup)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.NIC)
                .IsUnicode(false);

            modelBuilder.Entity<Patient>()
                .Property(e => e.Email)
                .IsUnicode(false);


            modelBuilder.Entity<ServiceType>()
                .Property(e => e.ServiceTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<Specialization>()
                .Property(e => e.SpecializationName)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.MaritalStatus)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.FatherName)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.HusbandName)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.NIC)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.PhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.MobileNo)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.AccountId)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.WorkAddress)
                .IsFixedLength();

            modelBuilder.Entity<Client>()
                .Property(e => e.WorkPhone)
                .IsFixedLength();

            modelBuilder.Entity<Client>()
                .Property(e => e.Remarks)
                .IsFixedLength();

            modelBuilder.Entity<Client>()
                .Property(e => e.VehicleRegistrationNo)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.PassportNo)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.PermanentAddress)
                .IsUnicode(false);


            modelBuilder.Entity<Client>()
                .HasMany(e => e.NextOfKins)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Group>()
                .Property(e => e.ClientGroupName)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Clients)
                .WithOptional(e => e.Group)
                .HasForeignKey(e => e.GroupId);

            modelBuilder.Entity<Type>()
                .Property(e => e.ClientTypeName)
                .IsUnicode(false);


            modelBuilder.Entity<Area>()
                .Property(e => e.AreaName)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.CityName)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.CountryName)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.CallingCode)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.ISO3166Code)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.IdCardNoVE)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.MobileNoVE)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.States)
                .WithRequired(e => e.Country)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<District>()
                .Property(e => e.DistrictName)
                .IsUnicode(false);

            //modelBuilder.Entity<District>()
            //    .HasMany(e => e.Areas)
            //    .WithOptional(e => e.District)
            //    .HasForeignKey(e => e.DistrictId);

            //modelBuilder.Entity<District>()
            //    .HasMany(e => e.Areas1)
            //    .WithOptional(e => e.District1)
            //    .HasForeignKey(e => e.DistrictId);


            modelBuilder.Entity<HouseType>()
                .Property(e => e.HouseTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<Nationality>()
                .Property(e => e.Nationality1)
                .IsUnicode(false);

            modelBuilder.Entity<Nationality>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMode>()
                .Property(e => e.PaymentModeName)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMode>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<Profession>()
                .Property(e => e.ProfessionName)
                .IsUnicode(false);

            modelBuilder.Entity<Profession>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<Region>()
                .Property(e => e.RegionName)
                .IsUnicode(false);

            modelBuilder.Entity<Region>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<Relation>()
                .Property(e => e.RelationName)
                .IsUnicode(false);

            modelBuilder.Entity<Relation>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<Relation>()
                .HasMany(e => e.NextOfKins)
                .WithRequired(e => e.Relation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Religion>()
                .Property(e => e.ReligionName)
                .IsUnicode(false);

            modelBuilder.Entity<Religion>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<State>()
                .Property(e => e.StateName)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.autokey)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.ParentAccountId)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.LINEAGE)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.AccountCode)
                .IsUnicode(false);


            modelBuilder.Entity<Account>()
                .HasMany(e => e.Accounts1)
                .WithOptional(e => e.Account1)
                .HasForeignKey(e => e.ParentAccountId);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Accounts11)
                .WithOptional(e => e.Account2)
                .HasForeignKey(e => e.ParentAccountId);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.AccountSettings)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.SalaryExpenseAccountId);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.AccountSettings1)
                .WithOptional(e => e.Account1)
                .HasForeignKey(e => e.EmployeePayableAccountId);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.AccountSettings2)
                .WithOptional(e => e.Account2)
                .HasForeignKey(e => e.FeeIncomeAccountId);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.AccountSettings3)
                .WithOptional(e => e.Account3)
                .HasForeignKey(e => e.FeeLiabilityAccountId);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.AccountSettings4)
                .WithOptional(e => e.Account4)
                .HasForeignKey(e => e.ReceiveableFromStudentsAccountId);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.BankAccounts)
                .WithOptional(e => e.Account)
                .HasForeignKey(e => e.PDCReceivable);

            modelBuilder.Entity<Account>()
                .HasOptional(e => e.BankAccount)
                .WithRequired(e => e.Account1);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.BankAccounts1)
                .WithOptional(e => e.Account2)
                .HasForeignKey(e => e.PDCPayable);

            //modelBuilder.Entity<Account>()
            //    .HasOptional(e => e.CashAccount)
            //    .WithRequired(e => e.Account);


            modelBuilder.Entity<AccountSetting>()
                .Property(e => e.FeeIncomeAccountId)
                .IsUnicode(false);

            modelBuilder.Entity<AccountSetting>()
                .Property(e => e.FeeLiabilityAccountId)
                .IsUnicode(false);

            modelBuilder.Entity<AccountSetting>()
                .Property(e => e.ReceiveableFromStudentsAccountId)
                .IsUnicode(false);

            modelBuilder.Entity<AccountSetting>()
                .Property(e => e.SalaryExpenseAccountId)
                .IsUnicode(false);

            modelBuilder.Entity<AccountSetting>()
                .Property(e => e.EmployeePayableAccountId)
                .IsUnicode(false);

            modelBuilder.Entity<AccountSetting>()
                .Property(e => e.BadDebtExpenseAccountId)
                .IsUnicode(false);

            modelBuilder.Entity<AccountSetting>()
                .Property(e => e.ResSaleIncomeAccountId)
                .IsUnicode(false);


            modelBuilder.Entity<AccountType>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<BankAccount>()
                .Property(e => e.AccountId)
                .IsUnicode(false);

            modelBuilder.Entity<BankAccount>()
                .Property(e => e.PDCReceivable)
                .IsUnicode(false);

            modelBuilder.Entity<BankAccount>()
                .Property(e => e.PDCPayable)
                .IsUnicode(false);

            modelBuilder.Entity<BankAccount>()
                .HasMany(e => e.ChequeBooks)
                .WithRequired(e => e.BankAccount)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<CashAccount>()
                .Property(e => e.CashAccountId)
                .IsUnicode(false);

            modelBuilder.Entity<ChequeBook>()
                .Property(e => e.AccountId)
                .IsUnicode(false);

            modelBuilder.Entity<CostGroup>()
                .HasMany(e => e.CostGroups1)
                .WithOptional(e => e.CostGroup1)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Currency>()
                .HasMany(e => e.CurrencyValues)
                .WithRequired(e => e.Currency)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<CurrencyValue>()
                .Property(e => e.Value)
                .HasPrecision(9, 4);

            modelBuilder.Entity<FiscalYear>()
                .Property(e => e.FiscalYearName)
                .IsUnicode(false);

            modelBuilder.Entity<FiscalYear>()
                .HasMany(e => e.TaxSlabs)
                .WithRequired(e => e.FiscalYear)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FiscalYear>()
                .HasMany(e => e.Vouchers)
                .WithRequired(e => e.FiscalYear)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<TaxSlab>()
                .Property(e => e.TaxSlabName)
                .IsUnicode(false);

            modelBuilder.Entity<TaxSlab>()
                .Property(e => e.TaxPercentage)
                .HasPrecision(4, 2);

            modelBuilder.Entity<VoucherDetail>()
                .Property(e => e.AccountId)
                .IsUnicode(false);

            modelBuilder.Entity<VoucherDetail>()
                .Property(e => e.TransactionType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VoucherDetail>()
                .Property(e => e.ChequeNo)
                .IsUnicode(false);

            modelBuilder.Entity<VoucherDetail>()
                .Property(e => e.Narration)
                .IsUnicode(false);

            modelBuilder.Entity<Voucher>()
                .Property(e => e.VoucherType)
                .IsUnicode(false);

            modelBuilder.Entity<Voucher>()
                .Property(e => e.VoucherName)
                .IsUnicode(false);

            modelBuilder.Entity<Voucher>()
                .Property(e => e.Balance)
                .HasPrecision(19, 2);

            modelBuilder.Entity<Voucher>()
                .Property(e => e.VoucherStatus)
                .IsUnicode(false);

            modelBuilder.Entity<Voucher>()
                .Property(e => e.ExchangeRate)
                .HasPrecision(9, 4);

            modelBuilder.Entity<Voucher>()
                .Property(e => e.CBAccountId)
                .IsUnicode(false);


            modelBuilder.Entity<VoucherType>()
                .Property(e => e.VoucherTypeId)
                .IsUnicode(false);

            modelBuilder.Entity<VoucherType>()
                .Property(e => e.VoucherTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<VoucherType>()
                .Property(e => e.VoucherTypeNo)
                .IsUnicode(false);

            modelBuilder.Entity<VoucherType>()
                .HasMany(e => e.Vouchers)
                .WithRequired(e => e.VoucherTypeLink)
                .HasForeignKey(e => e.VoucherType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contract>()
               .Property(e => e.Discount)
               .HasPrecision(18, 10);
            
            modelBuilder.Entity<Contract>()
               .Property(e => e.DiscountAmount)
               .HasPrecision(18, 5);

            modelBuilder.Entity<Form>()
                .HasMany(e => e.UserGroups)
                .WithOptional(e => e.Form)
                .HasForeignKey(e => e.FormID);

            modelBuilder.Entity<Form>()
                .HasMany(e => e.FormRights)
                .WithRequired(e => e.Form)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FormRight>()
                .Property(e => e.FormRightName)
                .IsUnicode(false);

            modelBuilder.Entity<UserGroup>()
                .HasMany(e => e.GroupRights)
                .WithRequired(e => e.UserGroup)
                .HasForeignKey(e => e.GroupId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserGroup>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.UserGroup)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ACGroup>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ACGroup>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<acholiday>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<ACTimeZones>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ACTimeZones>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<ACUnlockComb>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ACUnlockComb>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<AlarmLog>()
                .Property(e => e.Operator)
                .IsUnicode(false);

            modelBuilder.Entity<AlarmLog>()
                .Property(e => e.EnrollNumber)
                .IsUnicode(false);

            modelBuilder.Entity<AlarmLog>()
                .Property(e => e.MachineAlias)
                .IsUnicode(false);

            modelBuilder.Entity<AttParam>()
                .Property(e => e.PARANAME)
                .IsUnicode(false);

            modelBuilder.Entity<AttParam>()
                .Property(e => e.PARATYPE)
                .IsUnicode(false);

            modelBuilder.Entity<AttParam>()
                .Property(e => e.PARAVALUE)
                .IsUnicode(false);

            modelBuilder.Entity<AuditedExc>()
                .Property(e => e.UName)
                .IsUnicode(false);

            modelBuilder.Entity<AUTHDEVICE>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKEXACT>()
                .Property(e => e.CHECKTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKEXACT>()
                .Property(e => e.YUYIN)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKEXACT>()
                .Property(e => e.MODIFYBY)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKINOUT>()
                .Property(e => e.CHECKTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKINOUT>()
                .Property(e => e.SENSORID)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKINOUT>()
                .Property(e => e.Memoinfo)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKINOUT>()
                .Property(e => e.sn)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKINOUT>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<DEPARTMENTS_dbo>()
                .Property(e => e.DEPTNAME)
                .IsUnicode(false);

            modelBuilder.Entity<DEPARTMENTS_dbo>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<FaceTemp>()
                .Property(e => e.USERNO)
                .IsUnicode(false);

            modelBuilder.Entity<FaceTemp>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<HOLIDAYS>()
                .Property(e => e.HOLIDAYNAME)
                .IsUnicode(false);

            modelBuilder.Entity<HOLIDAYS>()
                .Property(e => e.XINBIE)
                .IsUnicode(false);

            modelBuilder.Entity<HOLIDAYS>()
                .Property(e => e.MINZU)
                .IsUnicode(false);

            modelBuilder.Entity<LeaveClass>()
                .Property(e => e.LeaveName)
                .IsUnicode(false);

            modelBuilder.Entity<LeaveClass>()
                .Property(e => e.ReportSymbol)
                .IsUnicode(false);

            modelBuilder.Entity<LeaveClass>()
                .Property(e => e.Code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<LeaveClass1>()
                .Property(e => e.LeaveName)
                .IsUnicode(false);

            modelBuilder.Entity<LeaveClass1>()
                .Property(e => e.ReportSymbol)
                .IsUnicode(false);

            modelBuilder.Entity<LeaveClass1>()
                .Property(e => e.Calc)
                .IsUnicode(false);

            modelBuilder.Entity<LeaveClass1>()
                .Property(e => e.Code)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Machines>()
                .Property(e => e.MachineAlias)
                .IsUnicode(false);

            modelBuilder.Entity<Machines>()
                .Property(e => e.CommPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Machines>()
                .Property(e => e.FirmwareVersion)
                .IsUnicode(false);

            modelBuilder.Entity<Machines>()
                .Property(e => e.ProductType)
                .IsUnicode(false);

            modelBuilder.Entity<Machines>()
                .Property(e => e.sn)
                .IsUnicode(false);

            modelBuilder.Entity<Machines>()
                .Property(e => e.PhotoStamp)
                .IsUnicode(false);

            modelBuilder.Entity<Machines>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<NUM_RUN>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<OpLog>()
                .Property(e => e.OpLogId)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OpLog>()
                .Property(e => e.UserId)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OpLog>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<OpLog>()
                .Property(e => e.TableName)
                .IsUnicode(false);

            modelBuilder.Entity<SchClass>()
                .Property(e => e.schName)
                .IsUnicode(false);

            modelBuilder.Entity<SchClass>()
                .Property(e => e.SensorID)
                .IsUnicode(false);

            modelBuilder.Entity<SECURITYDETAILS>()
                .Property(e => e.REPORT)
                .IsUnicode(false);

            modelBuilder.Entity<SHIFT>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.Operator)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.MachineAlias)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.LogDescr)
                .IsUnicode(false);

            modelBuilder.Entity<TBSMSALLOT>()
                .Property(e => e.GENTM)
                .IsUnicode(false);

            modelBuilder.Entity<TBSMSINFO>()
                .Property(e => e.SMSID)
                .IsUnicode(false);

            modelBuilder.Entity<TBSMSINFO>()
                .Property(e => e.SMSCONTENT)
                .IsUnicode(false);

            modelBuilder.Entity<TBSMSINFO>()
                .Property(e => e.SMSSTARTTM)
                .IsUnicode(false);

            modelBuilder.Entity<TBSMSINFO>()
                .Property(e => e.GENTM)
                .IsUnicode(false);


            modelBuilder.Entity<USER_SPEDAY>()
                .Property(e => e.YUANYING)
                .IsUnicode(false);


            modelBuilder.Entity<UserUpdates>()
                .Property(e => e.BadgeNumber)
                .IsUnicode(false);

            modelBuilder.Entity<EmOpLog>()
                .Property(e => e.SensorId)
                .IsUnicode(false);

            modelBuilder.Entity<ServerLog>()
                .Property(e => e.EVENT)
                .IsUnicode(false);

            modelBuilder.Entity<ServerLog>()
                .Property(e => e.EnrollNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ServerLog>()
                .Property(e => e.SENSORID)
                .IsUnicode(false);

            modelBuilder.Entity<ServerLog>()
                .Property(e => e.OPERATOR)
                .IsUnicode(false);
        }
        //commented because of CurentState issue on 21 Aug By Me
        //public override int SaveChanges()
        //{
        //    try
        //    {
        //        var AddedEntities = ChangeTracker.Entries()
        //        .Where(p => p.State == EntityState.Added);

        //        if (AddedEntities != null)
        //        {
        //            foreach (var entry in AddedEntities)
        //            {
        //                if (entry.Entity != null)
        //                {
        //                    //try
        //                    //{
        //                    //    if (entry.Entity.GetType().GetProperty("BranchId") != null && !SessionHelper.CurrentUrl.Contains("Global"))
        //                    //    {
        //                    //        if (Convert.ToInt16(((dynamic)entry.Entity).BranchId) == 0 && SessionHelper.SaveBranchId != false)
        //                    //        {
        //                    //            ((dynamic)entry.Entity).BranchId = Convert.ToInt16(SessionHelper.BranchId);
        //                    //        }
        //                    //    }
        //                    //}
        //                    //catch { }
        //                }
        //            }
        //        }

        //        //Code for audit log

        //        foreach (var entity in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
        //        {
        //            try
        //            {
        //                foreach (Audit audit in new Helpers.AuditHelpers().GetAuditRecordsForChange(this, entity))
        //                {
        //                    Audits.Add(audit);
        //                }

        //                foreach (DeletionLog deletionLog in new Helpers.AuditHelpers().GetDeleteRecordsForChange(entity))
        //                {
        //                    DeletionLogs.Add(deletionLog);
        //                }
        //            }
        //            catch (Exception e)
        //            { }

        //        }
        //        return base.SaveChanges();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        var errorMessages = ex.EntityValidationErrors
        //                .SelectMany(x => x.ValidationErrors)
        //                .Select(x => x.ErrorMessage);

        //        var fullErrorMessage = string.Join("; ", errorMessages);
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
        //        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //    }
        //}

        //public override async Task<int> SaveChangesAsync()
        //{
        //    try
        //    {
        //        var AddedEntities = ChangeTracker.Entries()
        //        .Where(p => p.State == EntityState.Added);

        //        if (AddedEntities != null)
        //        {
        //            //foreach (var entry in AddedEntities)
        //            //{
        //            //if (entry.Entity != null)
        //            //{
        //            //    try
        //            //    {
        //            //        if (entry.Entity.GetType().GetProperty("BranchId") != null && !SessionHelper.CurrentUrl.Contains("Global"))
        //            //        {
        //            //            if (Convert.ToInt16(((dynamic)entry.Entity).BranchId) == 0)
        //            //            {
        //            //                ((dynamic)entry.Entity).BranchId = Convert.ToInt16(SessionHelper.BranchId);
        //            //            }
        //            //        }
        //            //    }
        //            //    catch { }
        //            //}
        //            //}
        //        }

        //        //Code for audit log
        //        foreach (var entity in this.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
        //        {
        //            foreach (Audit audit in new Helpers.AuditHelpers().GetAuditRecordsForChange(this, entity))
        //            {
        //                Audits.Add(audit);
        //            }

        //            foreach (DeletionLog deletionLog in new Helpers.AuditHelpers().GetDeleteRecordsForChange(entity))
        //            {
        //                DeletionLogs.Add(deletionLog);
        //            }
        //        }

        //        return await base.SaveChangesAsync();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        var errorMessages = ex.EntityValidationErrors
        //                .SelectMany(x => x.ValidationErrors)
        //                .Select(x => x.ErrorMessage);

        //        var fullErrorMessage = string.Join("; ", errorMessages);
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
        //        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //    }
        //}

    }
}
