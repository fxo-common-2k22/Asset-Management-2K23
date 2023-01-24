using FAPP.Model;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace FAPP.Areas.AM.ViewModels.PrimaryData
{
    public class ChartOfAccounts
    {
        private readonly OneDbContext context = null;
        public ChartOfAccounts(OneDbContext db)
        {
            context = db;
        }

        public List<Account> GetAllDefaults()
        {

            return new List<Account>(){
                 new Account
            {
                autokey = "00000000-0000-0000-0000-000000303832",
                TITLE = "ROOT",
                ACCOUNT_ID = 0,
                BranchId = null,
                ParentAccountId = null,
                ParentId = null,
                LINEAGE = "/",
                DEPTH = 0,
                AccountCode = "0",
                CONTROLACCOUNT = true,
                ISTRANSACTION = false,
                CurrencyId = null,
                BYDEFAULT = true,
                DESCN = "",
                DBSTATUS = true,
                ModifiedBy = null,


                OldAccountCode = null,
                IsLocked = true,
                GroupNo = 1,
                ControlAccountRef = 1,
                StatementSetupId = null,
                AccountGroupId = null,
            },
new Account {
autokey ="00000000-0000-0000-0000-000000303033",
TITLE ="Bank",
ACCOUNT_ID = 101004,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000363832",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101004",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 2,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000303133",
TITLE ="Utilities",
ACCOUNT_ID = 402002,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000303233",
TITLE ="Loan Receivable",
ACCOUNT_ID = 1010010002,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000373932",
ParentId = 101001,
LINEAGE = "/0/1/101/101001/",
DEPTH = 4,
AccountCode = "1010010002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},

new Account {
autokey ="00000000-0000-0000-0000-000000303932",
TITLE ="Administrative Expenses",
ACCOUNT_ID = 401,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000343832",
ParentId = 4,
LINEAGE = "/0/4/",
DEPTH = 2,
AccountCode = "401",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000313033",
TITLE ="Land and Building",
ACCOUNT_ID = 102001,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000373832",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000313133",
TITLE ="Repair & Maintenance Expenses",
ACCOUNT_ID = 402003,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402003",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000313832",
TITLE ="Assets",
ACCOUNT_ID = 1,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000303832",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "1",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000313932",
TITLE ="Operating Expenses",
ACCOUNT_ID = 402,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000343832",
ParentId = 4,
LINEAGE = "/0/4/",
DEPTH = 2,
AccountCode = "402",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000323033",
TITLE ="Property, Plant & Equipment",
ACCOUNT_ID = 102002,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000373832",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000323133",
TITLE ="Promotional Expenses",
ACCOUNT_ID = 402004,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402004",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000323233",
TITLE ="Supplier Payable",
ACCOUNT_ID = 201004,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "201004",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 5,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000323832",
TITLE ="Liabilities",
ACCOUNT_ID = 2,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000303832",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "2",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000323932",
TITLE ="Other Expenses",
ACCOUNT_ID = 403,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000343832",
ParentId = 4,
LINEAGE = "/0/4/",
DEPTH = 2,
AccountCode = "403",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000333033",
TITLE ="Furniture & Fixture",
ACCOUNT_ID = 102003,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000373832",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102003",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000333133",
TITLE ="Printing and Stationary",
ACCOUNT_ID = 402005,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402005",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000333832",
TITLE ="Capital",
ACCOUNT_ID = 3,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000303832",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "3",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000333932",
TITLE ="Sales",
ACCOUNT_ID = 501,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000353832",
ParentId = 5,
LINEAGE = "/0/5/",
DEPTH = 2,
AccountCode = "501",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000343033",
TITLE ="Other",
ACCOUNT_ID = 102004,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000373832",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102004",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000343233",
TITLE ="Salary Payable",
ACCOUNT_ID = 201006,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "201006",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000343832",
TITLE ="Expenses",
ACCOUNT_ID = 4,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000303832",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "4",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000353033",
TITLE ="Payable",
ACCOUNT_ID = 201001,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201001",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000353233",
TITLE ="Tax Liability",
ACCOUNT_ID = 2010020001,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "2010020001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "This identifies taxes that the company should have paid by now but hasn't because of differences between accounting rules and tax law.",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000353832",
TITLE ="Income",
ACCOUNT_ID = 5,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000303832",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "5",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000363033",
TITLE ="Other Current Liabilities",
ACCOUNT_ID = 201002,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000363034",
TITLE ="Employee Payable",
ACCOUNT_ID = 201003,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201003",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "Employee",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 4,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000363133",
TITLE ="Travelling Expenses",
ACCOUNT_ID = 402008,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402008",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000363233",
TITLE ="Electricity",
ACCOUNT_ID = 4020020001,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000303133",
ParentId = 402002,
LINEAGE = "/0/4/402/402002/",
DEPTH = 4,
AccountCode = "4020020001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000363832",
TITLE ="Current Assets",
ACCOUNT_ID = 101,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000313832",
ParentId = 1,
LINEAGE = "/0/1/",
DEPTH = 2,
AccountCode = "101",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000363932",
TITLE ="Other Income",
ACCOUNT_ID = 504,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000353832",
ParentId = 5,
LINEAGE = "/0/5/",
DEPTH = 2,
AccountCode = "504",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000373033",
TITLE ="Salary Expense",
ACCOUNT_ID = 401001,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000303932",
ParentId = 401,
LINEAGE = "/0/4/401/",
DEPTH = 3,
AccountCode = "401001",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000373133",
TITLE ="Tax Expense",
ACCOUNT_ID = 403001,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000323932",
ParentId = 403,
LINEAGE = "/0/4/403/",
DEPTH = 3,
AccountCode = "403001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000373832",
TITLE ="Fixed Assets",
ACCOUNT_ID = 102,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000313832",
ParentId = 1,
LINEAGE = "/0/1/",
DEPTH = 2,
AccountCode = "102",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000373932",
TITLE ="Receivable",
ACCOUNT_ID = 101001,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000363832",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101001",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000383033",
TITLE ="Other Admin Expenses",
ACCOUNT_ID = 401002,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000303932",
ParentId = 401,
LINEAGE = "/0/4/401/",
DEPTH = 3,
AccountCode = "401002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000383832",
TITLE ="Current Liabilities",
ACCOUNT_ID = 201,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000323832",
ParentId = 2,
LINEAGE = "/0/2/",
DEPTH = 2,
AccountCode = "201",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000383932",
TITLE ="Advances & Prepayments",
ACCOUNT_ID = 101002,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000363832",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101002",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000393033",
TITLE ="Purchases",
ACCOUNT_ID = 402001,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000393233",
TITLE ="Tax Payable",
ACCOUNT_ID = 201007,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "201007",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "This merely identifies taxes that the company has incurred but hasn't had to pay because tax time hasn't rolled around yet. ",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000393832",
TITLE ="Long Term Liabilities",
ACCOUNT_ID = 202,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000323832",
ParentId = 2,
LINEAGE = "/0/2/",
DEPTH = 2,
AccountCode = "202",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000393932",
TITLE ="Cash",
ACCOUNT_ID = 101003,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000363832",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101003",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "abc",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="00000000-0000-0000-0000-000000393933",
TITLE ="Petty Cash",
ACCOUNT_ID = 1010030001,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000393932",
ParentId = 101003,
LINEAGE = "/0/1/101/101003/",
DEPTH = 4,
AccountCode = "1010030001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="3B6221E1-5D26-4CC2-BD82-5DFB751CEF19",
TITLE ="Client Receivable",
ACCOUNT_ID = 101005,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000363832",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101005",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = false,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 3,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="4B4A2CC6-196A-4D7C-B4F3-06CC65E3223F",
TITLE ="Agent Payable",
ACCOUNT_ID = 201005,
BranchId = null,
ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201005",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = false,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
}
            };
        }

        //Account to add in Finance.DefaultAccount Table
        public List<DefaultAccount> GetAllDefaultsAccounts()
        {
            return new List<DefaultAccount>(){
                 new DefaultAccount
            {
                autokey = "00000000-0000-0000-0000-000000303832",
                TITLE = "ROOT",
                ACCOUNT_ID = 0,

                ParentAccountId = null,
                ParentId = null,
                LINEAGE = "/",
                DEPTH = 0,
                AccountCode = "0",




                CONTROLACCOUNT = true,
                ISTRANSACTION = false,
                CurrencyId = null,
                ExchangeRate = 0.0000M,
                BYDEFAULT = true,
                DESCN = "",
                DBSTATUS = true,
                ModifiedBy = null,


                OldAccountCode = null,
                IsLocked = true,
                GroupNo = 1,
                ControlAccountRef = 1,
                StatementSetupId = null,
                AccountGroupId = null,
            },
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000303033",
TITLE ="Bank",
ACCOUNT_ID = 101004,

ParentAccountId = "00000000-0000-0000-0000-000000363832",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101004",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 2,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000303133",
TITLE ="Utilities",
ACCOUNT_ID = 402002,

ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000303233",
TITLE ="Loan Receivable",
ACCOUNT_ID = 1010010002,

ParentAccountId = "00000000-0000-0000-0000-000000373932",
ParentId = 101001,
LINEAGE = "/0/1/101/101001/",
DEPTH = 4,
AccountCode = "1010010002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},

new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000303932",
TITLE ="Administrative Expenses",
ACCOUNT_ID = 401,

ParentAccountId = "00000000-0000-0000-0000-000000343832",
ParentId = 4,
LINEAGE = "/0/4/",
DEPTH = 2,
AccountCode = "401",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000313033",
TITLE ="Land and Building",
ACCOUNT_ID = 102001,

ParentAccountId = "00000000-0000-0000-0000-000000373832",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000313133",
TITLE ="Repair & Maintenance Expenses",
ACCOUNT_ID = 402003,

ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402003",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000313832",
TITLE ="Assets",
ACCOUNT_ID = 1,

ParentAccountId = "00000000-0000-0000-0000-000000303832",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "1",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000313932",
TITLE ="Operating Expenses",
ACCOUNT_ID = 402,

ParentAccountId = "00000000-0000-0000-0000-000000343832",
ParentId = 4,
LINEAGE = "/0/4/",
DEPTH = 2,
AccountCode = "402",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000323033",
TITLE ="Property, Plant & Equipment",
ACCOUNT_ID = 102002,

ParentAccountId = "00000000-0000-0000-0000-000000373832",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000323133",
TITLE ="Promotional Expenses",
ACCOUNT_ID = 402004,

ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402004",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000323233",
TITLE ="Supplier Payable",
ACCOUNT_ID = 201004,

ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "201004",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 5,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000323832",
TITLE ="Liabilities",
ACCOUNT_ID = 2,

ParentAccountId = "00000000-0000-0000-0000-000000303832",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "2",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000323932",
TITLE ="Other Expenses",
ACCOUNT_ID = 403,

ParentAccountId = "00000000-0000-0000-0000-000000343832",
ParentId = 4,
LINEAGE = "/0/4/",
DEPTH = 2,
AccountCode = "403",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000333033",
TITLE ="Furniture & Fixture",
ACCOUNT_ID = 102003,

ParentAccountId = "00000000-0000-0000-0000-000000373832",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102003",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000333133",
TITLE ="Printing and Stationary",
ACCOUNT_ID = 402005,

ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402005",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000333832",
TITLE ="Capital",
ACCOUNT_ID = 3,

ParentAccountId = "00000000-0000-0000-0000-000000303832",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "3",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000333932",
TITLE ="Sales",
ACCOUNT_ID = 501,

ParentAccountId = "00000000-0000-0000-0000-000000353832",
ParentId = 5,
LINEAGE = "/0/5/",
DEPTH = 2,
AccountCode = "501",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000343033",
TITLE ="Other",
ACCOUNT_ID = 102004,

ParentAccountId = "00000000-0000-0000-0000-000000373832",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102004",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000343233",
TITLE ="Salary Payable",
ACCOUNT_ID = 201006,

ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "201006",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000343832",
TITLE ="Expenses",
ACCOUNT_ID = 4,

ParentAccountId = "00000000-0000-0000-0000-000000303832",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "4",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000353033",
TITLE ="Payable",
ACCOUNT_ID = 201001,

ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201001",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000353233",
TITLE ="Tax Liability",
ACCOUNT_ID = 2010020001,

ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "2010020001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "This identifies taxes that the company should have paid by now but hasn't because of differences between accounting rules and tax law.",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000353832",
TITLE ="Income",
ACCOUNT_ID = 5,

ParentAccountId = "00000000-0000-0000-0000-000000303832",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "5",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000363033",
TITLE ="Other Current Liabilities",
ACCOUNT_ID = 201002,

ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000363034",
TITLE ="Employee Payable",
ACCOUNT_ID = 201003,

ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201003",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "Employee",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 4,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000363133",
TITLE ="Travelling Expenses",
ACCOUNT_ID = 402008,

ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402008",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000363233",
TITLE ="Electricity",
ACCOUNT_ID = 4020020001,

ParentAccountId = "00000000-0000-0000-0000-000000303133",
ParentId = 402002,
LINEAGE = "/0/4/402/402002/",
DEPTH = 4,
AccountCode = "4020020001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000363832",
TITLE ="Current Assets",
ACCOUNT_ID = 101,

ParentAccountId = "00000000-0000-0000-0000-000000313832",
ParentId = 1,
LINEAGE = "/0/1/",
DEPTH = 2,
AccountCode = "101",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000363932",
TITLE ="Other Income",
ACCOUNT_ID = 504,

ParentAccountId = "00000000-0000-0000-0000-000000353832",
ParentId = 5,
LINEAGE = "/0/5/",
DEPTH = 2,
AccountCode = "504",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000373033",
TITLE ="Salary Expense",
ACCOUNT_ID = 401001,

ParentAccountId = "00000000-0000-0000-0000-000000303932",
ParentId = 401,
LINEAGE = "/0/4/401/",
DEPTH = 3,
AccountCode = "401001",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000373133",
TITLE ="Tax Expense",
ACCOUNT_ID = 403001,

ParentAccountId = "00000000-0000-0000-0000-000000323932",
ParentId = 403,
LINEAGE = "/0/4/403/",
DEPTH = 3,
AccountCode = "403001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000373832",
TITLE ="Fixed Assets",
ACCOUNT_ID = 102,

ParentAccountId = "00000000-0000-0000-0000-000000313832",
ParentId = 1,
LINEAGE = "/0/1/",
DEPTH = 2,
AccountCode = "102",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000373932",
TITLE ="Receivable",
ACCOUNT_ID = 101001,

ParentAccountId = "00000000-0000-0000-0000-000000363832",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101001",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000383033",
TITLE ="Other Admin Expenses",
ACCOUNT_ID = 401002,

ParentAccountId = "00000000-0000-0000-0000-000000303932",
ParentId = 401,
LINEAGE = "/0/4/401/",
DEPTH = 3,
AccountCode = "401002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000383832",
TITLE ="Current Liabilities",
ACCOUNT_ID = 201,

ParentAccountId = "00000000-0000-0000-0000-000000323832",
ParentId = 2,
LINEAGE = "/0/2/",
DEPTH = 2,
AccountCode = "201",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000383932",
TITLE ="Advances & Prepayments",
ACCOUNT_ID = 101002,

ParentAccountId = "00000000-0000-0000-0000-000000363832",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101002",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000393033",
TITLE ="Purchases",
ACCOUNT_ID = 402001,

ParentAccountId = "00000000-0000-0000-0000-000000313932",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 2,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000393233",
TITLE ="Tax Payable",
ACCOUNT_ID = 201007,

ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "201007",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "This merely identifies taxes that the company has incurred but hasn't had to pay because tax time hasn't rolled around yet. ",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000393832",
TITLE ="Long Term Liabilities",
ACCOUNT_ID = 202,

ParentAccountId = "00000000-0000-0000-0000-000000323832",
ParentId = 2,
LINEAGE = "/0/2/",
DEPTH = 2,
AccountCode = "202",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000393932",
TITLE ="Cash",
ACCOUNT_ID = 101003,

ParentAccountId = "00000000-0000-0000-0000-000000363832",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101003",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "abc",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="00000000-0000-0000-0000-000000393933",
TITLE ="Petty Cash",
ACCOUNT_ID = 1010030001,

ParentAccountId = "00000000-0000-0000-0000-000000393932",
ParentId = 101003,
LINEAGE = "/0/1/101/101003/",
DEPTH = 4,
AccountCode = "1010030001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= true ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="3B6221E1-5D26-4CC2-BD82-5DFB751CEF19",
TITLE ="Client Receivable",
ACCOUNT_ID = 101005,

ParentAccountId = "00000000-0000-0000-0000-000000363832",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101005",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = false,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 3,
StatementSetupId = null,
AccountGroupId = null,
},
new DefaultAccount {
autokey ="4B4A2CC6-196A-4D7C-B4F3-06CC65E3223F",
TITLE ="Agent Payable",
ACCOUNT_ID = 201005,

ParentAccountId = "00000000-0000-0000-0000-000000383832",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201005",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = false,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
}
            };
        }

        public List<Account> GetAllWithBranch()
        {
            return new List<Account>()
            {
                new Account {
autokey ="291EC6F9-6A43-403D-97D0-C77F38D0A2E0",
TITLE ="ROOT",
ACCOUNT_ID = 0,
BranchId = null,
ParentAccountId = null,
ParentId = null,
LINEAGE = "/",
DEPTH = 0,
AccountCode = "0",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="45B19674-4200-4C1D-9A79-9D289F0A5857",
TITLE ="Assets",
ACCOUNT_ID = 1,
BranchId = null,
ParentAccountId = "291EC6F9-6A43-403D-97D0-C77F38D0A2E0",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "1",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="77C02388-75AC-42BD-B62E-B0EB9CCAFD0F",
TITLE ="Liabilities",
ACCOUNT_ID = 2,
BranchId = null,
ParentAccountId = "291EC6F9-6A43-403D-97D0-C77F38D0A2E0",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "2",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="75632042-8AC1-441A-8594-07A9268C860D",
TITLE ="Capital",
ACCOUNT_ID = 3,
BranchId = null,
ParentAccountId = "291EC6F9-6A43-403D-97D0-C77F38D0A2E0",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "3",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="BD5558E4-7520-4AC6-8595-86920C7B6EBD",
TITLE ="Expenses",
ACCOUNT_ID = 4,
BranchId = null,
ParentAccountId = "291EC6F9-6A43-403D-97D0-C77F38D0A2E0",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "4",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="343788FA-0254-4F61-9755-A4F541C25E04",
TITLE ="Income",
ACCOUNT_ID = 5,
BranchId = null,
ParentAccountId = "291EC6F9-6A43-403D-97D0-C77F38D0A2E0",
ParentId = 0,
LINEAGE = "/0/",
DEPTH = 1,
AccountCode = "5",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="18A79E38-A8D7-4363-AF7B-E6D547E70BC0",
TITLE ="Current Assets",
ACCOUNT_ID = 101,
BranchId = null,
ParentAccountId = "45B19674-4200-4C1D-9A79-9D289F0A5857",
ParentId = 1,
LINEAGE = "/0/1/",
DEPTH = 2,
AccountCode = "101",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="81CEBDA5-A5D7-4C6E-84EE-5E41579DEB22",
TITLE ="Fixed Assets",
ACCOUNT_ID = 102,
BranchId = null,
ParentAccountId = "45B19674-4200-4C1D-9A79-9D289F0A5857",
ParentId = 1,
LINEAGE = "/0/1/",
DEPTH = 2,
AccountCode = "102",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="7AB3F6EA-F2FA-42DE-9BC8-AEC0AB0B996C",
TITLE ="Current Liabilities",
ACCOUNT_ID = 201,
BranchId = null,
ParentAccountId = "77C02388-75AC-42BD-B62E-B0EB9CCAFD0F",
ParentId = 2,
LINEAGE = "/0/2/",
DEPTH = 2,
AccountCode = "201",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="9B3AFA0B-CA50-402A-B0D2-5BB33BB48FF7",
TITLE ="Long Term Liabilities",
ACCOUNT_ID = 202,
BranchId = null,
ParentAccountId = "77C02388-75AC-42BD-B62E-B0EB9CCAFD0F",
ParentId = 2,
LINEAGE = "/0/2/",
DEPTH = 2,
AccountCode = "202",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="04B06DA6-78B8-468A-B1EF-5434BD9E557D",
TITLE ="Administrative Expenses",
ACCOUNT_ID = 401,
BranchId = null,
ParentAccountId = "BD5558E4-7520-4AC6-8595-86920C7B6EBD",
ParentId = 4,
LINEAGE = "/0/4/",
DEPTH = 2,
AccountCode = "401",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="B294F531-06F3-43F5-86EC-754847203984",
TITLE ="Operating Expenses",
ACCOUNT_ID = 402,
BranchId = null,
ParentAccountId = "BD5558E4-7520-4AC6-8595-86920C7B6EBD",
ParentId = 4,
LINEAGE = "/0/4/",
DEPTH = 2,
AccountCode = "402",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="0E03C70F-5E6B-4F32-B02B-047B6A6929AC",
TITLE ="Other Expenses",
ACCOUNT_ID = 403,
BranchId = null,
ParentAccountId = "BD5558E4-7520-4AC6-8595-86920C7B6EBD",
ParentId = 4,
LINEAGE = "/0/4/",
DEPTH = 2,
AccountCode = "403",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="58DAE7B1-ABBB-42AC-8245-15734E403FA1",
TITLE ="Sales",
ACCOUNT_ID = 501,
BranchId = null,
ParentAccountId = "343788FA-0254-4F61-9755-A4F541C25E04",
ParentId = 5,
LINEAGE = "/0/5/",
DEPTH = 2,
AccountCode = "501",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="A4B16098-C719-49C4-9C6F-B39E68331526",
TITLE ="Other Income",
ACCOUNT_ID = 504,
BranchId = null,
ParentAccountId = "343788FA-0254-4F61-9755-A4F541C25E04",
ParentId = 5,
LINEAGE = "/0/5/",
DEPTH = 2,
AccountCode = "504",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="FA7D71AD-0540-4311-B134-F399578AC999",
TITLE ="Receivable",
ACCOUNT_ID = 101001,
BranchId = null,
ParentAccountId = "18A79E38-A8D7-4363-AF7B-E6D547E70BC0",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101001",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="51F0EBDF-1782-4133-BC35-9A7E872EF662",
TITLE ="Advances & Prepayments",
ACCOUNT_ID = 101002,
BranchId = null,
ParentAccountId = "18A79E38-A8D7-4363-AF7B-E6D547E70BC0",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101002",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="D95B4F78-4540-4A5C-91C0-6A486D9002B6",
TITLE ="Cash",
ACCOUNT_ID = 101003,
BranchId = null,
ParentAccountId = "18A79E38-A8D7-4363-AF7B-E6D547E70BC0",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101003",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="0FCA0EFD-64C0-4A03-900D-517781602829",
TITLE ="Bank",
ACCOUNT_ID = 101004,
BranchId = null,
ParentAccountId = "18A79E38-A8D7-4363-AF7B-E6D547E70BC0",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101004",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="EFD5AE65-63AF-43BE-9388-C6A113E7483F",
TITLE ="Client Receivable",
ACCOUNT_ID = 101005,
BranchId = null,
ParentAccountId = "18A79E38-A8D7-4363-AF7B-E6D547E70BC0",
ParentId = 101,
LINEAGE = "/0/1/101/",
DEPTH = 3,
AccountCode = "101005",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = false,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="7112DB59-9FFD-41CB-8254-1D551915569F",
TITLE ="Land and Building",
ACCOUNT_ID = 102001,
BranchId = null,
ParentAccountId = "81CEBDA5-A5D7-4C6E-84EE-5E41579DEB22",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="53F52056-26CE-418A-AF08-E5BD5AFE48A9",
TITLE ="Property, Plant & Equipment",
ACCOUNT_ID = 102002,
BranchId = null,
ParentAccountId = "81CEBDA5-A5D7-4C6E-84EE-5E41579DEB22",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="0FA0652C-13A6-4CEC-92A2-03953D188AF3",
TITLE ="Furniture & Fixture",
ACCOUNT_ID = 102003,
BranchId = null,
ParentAccountId = "81CEBDA5-A5D7-4C6E-84EE-5E41579DEB22",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102003",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="4D65EED5-D5CE-4667-846B-DB65553754B6",
TITLE ="Other",
ACCOUNT_ID = 102004,
BranchId = null,
ParentAccountId = "81CEBDA5-A5D7-4C6E-84EE-5E41579DEB22",
ParentId = 102,
LINEAGE = "/0/1/102/",
DEPTH = 3,
AccountCode = "102004",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="1EDDB217-A935-4B88-8DB1-F92D0F2B1ACC",
TITLE ="Payable",
ACCOUNT_ID = 201001,
BranchId = null,
ParentAccountId = "7AB3F6EA-F2FA-42DE-9BC8-AEC0AB0B996C",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201001",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="DC8300DC-B6E8-4DCA-9E16-3B4A800905E2",
TITLE ="Other Current Liabilities",
ACCOUNT_ID = 201002,
BranchId = null,
ParentAccountId = "7AB3F6EA-F2FA-42DE-9BC8-AEC0AB0B996C",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="15DB09F1-3837-42EB-9CE1-36B3414D4F0A",
TITLE ="Employee Payable",
ACCOUNT_ID = 201003,
BranchId = null,
ParentAccountId = "7AB3F6EA-F2FA-42DE-9BC8-AEC0AB0B996C",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201003",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="1E93D7BC-3DEC-4183-8750-EF114B7045D6",
TITLE ="Supplier Payable",
ACCOUNT_ID = 201004,
BranchId = null,
ParentAccountId = "7AB3F6EA-F2FA-42DE-9BC8-AEC0AB0B996C",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "201004",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="0877EA8C-0183-4ADE-BDC1-13FB727C15C4",
TITLE ="Agent Payable",
ACCOUNT_ID = 201005,
BranchId = null,
ParentAccountId = "7AB3F6EA-F2FA-42DE-9BC8-AEC0AB0B996C",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 3,
AccountCode = "201005",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = false,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="043D5DD5-F608-4166-8970-76638A8D52B1",
TITLE ="Salary Payable",
ACCOUNT_ID = 201006,
BranchId = null,
ParentAccountId = "7AB3F6EA-F2FA-42DE-9BC8-AEC0AB0B996C",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "201006",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="2397E474-496E-4367-A949-2CF10437183B",
TITLE ="Tax Payable",
ACCOUNT_ID = 201007,
BranchId = null,
ParentAccountId = "7AB3F6EA-F2FA-42DE-9BC8-AEC0AB0B996C",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "201007",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="7B093A37-AA19-4480-9121-24B618FB6B1C",
TITLE ="Salary Expense",
ACCOUNT_ID = 401001,
BranchId = null,
ParentAccountId = "04B06DA6-78B8-468A-B1EF-5434BD9E557D",
ParentId = 401,
LINEAGE = "/0/4/401/",
DEPTH = 3,
AccountCode = "401001",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="EAE498D9-C151-4556-B7BC-1F7DA649363B",
TITLE ="Other Admin Expenses",
ACCOUNT_ID = 401002,
BranchId = null,
ParentAccountId = "04B06DA6-78B8-468A-B1EF-5434BD9E557D",
ParentId = 401,
LINEAGE = "/0/4/401/",
DEPTH = 3,
AccountCode = "401002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="64E4C616-0617-4198-ADF1-985C63454182",
TITLE ="Purchases",
ACCOUNT_ID = 402001,
BranchId = null,
ParentAccountId = "B294F531-06F3-43F5-86EC-754847203984",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="54129907-1B83-458C-83CB-5475A05B58C9",
TITLE ="Utilities",
ACCOUNT_ID = 402002,
BranchId = null,
ParentAccountId = "B294F531-06F3-43F5-86EC-754847203984",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402002",




CONTROLACCOUNT = true,
ISTRANSACTION = false,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="1B34789E-B832-4489-926F-5A267EE407A9",
TITLE ="Repair & Maintenance Expenses",
ACCOUNT_ID = 402003,
BranchId = null,
ParentAccountId = "B294F531-06F3-43F5-86EC-754847203984",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402003",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="2D314D1D-F950-4D00-8F7A-D416146D6711",
TITLE ="Promotional Expenses",
ACCOUNT_ID = 402004,
BranchId = null,
ParentAccountId = "B294F531-06F3-43F5-86EC-754847203984",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402004",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="86DC9E6D-DA01-4698-8045-E722120F573C",
TITLE ="Printing and Stationary",
ACCOUNT_ID = 402005,
BranchId = null,
ParentAccountId = "B294F531-06F3-43F5-86EC-754847203984",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402005",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="8C9E2FF1-BE4B-4892-A4DE-2CE6226E161E",
TITLE ="Travelling Expenses",
ACCOUNT_ID = 402008,
BranchId = null,
ParentAccountId = "B294F531-06F3-43F5-86EC-754847203984",
ParentId = 402,
LINEAGE = "/0/4/402/",
DEPTH = 3,
AccountCode = "402008",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="4EBDC427-3C9B-46A8-8D96-6FD7D9BE57B6",
TITLE ="Tax Expense",
ACCOUNT_ID = 403001,
BranchId = null,
ParentAccountId = "0E03C70F-5E6B-4F32-B02B-047B6A6929AC",
ParentId = 403,
LINEAGE = "/0/4/403/",
DEPTH = 3,
AccountCode = "403001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="9095B212-5081-4361-8B3A-B246990B60B4",
TITLE ="Loan Receivable",
ACCOUNT_ID = 1010010002,
BranchId = null,
ParentAccountId = "FA7D71AD-0540-4311-B134-F399578AC999",
ParentId = 101001,
LINEAGE = "/0/1/101/101001/",
DEPTH = 4,
AccountCode = "1010010002",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="4FB83640-2000-4D68-9AE4-64359CB8C7D1",
TITLE ="Petty Cash",
ACCOUNT_ID = 1010030001,
BranchId = null,
ParentAccountId = "D95B4F78-4540-4A5C-91C0-6A486D9002B6",
ParentId = 101003,
LINEAGE = "/0/1/101/101003/",
DEPTH = 4,
AccountCode = "1010030001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="162C660A-E744-44CF-9018-57B84A6F8743",
TITLE ="Tax Liability",
ACCOUNT_ID = 2010020001,
BranchId = null,
ParentAccountId = "7AB3F6EA-F2FA-42DE-9BC8-AEC0AB0B996C",
ParentId = 201,
LINEAGE = "/0/2/201/",
DEPTH = 4,
AccountCode = "2010020001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
},
new Account {
autokey ="675CD4B8-3C30-49D6-9129-35814F55AA86",
TITLE ="Electricity",
ACCOUNT_ID = 4020020001,
BranchId = null,
ParentAccountId = "54129907-1B83-458C-83CB-5475A05B58C9",
ParentId = 402002,
LINEAGE = "/0/4/402/402002/",
DEPTH = 4,
AccountCode = "4020020001",




CONTROLACCOUNT = false,
ISTRANSACTION = true,
CurrencyId = null ,

BYDEFAULT = true,
DESCN = "",
DBSTATUS = true,
ModifiedBy = null ,


OldAccountCode= 0 ,
IsLocked= false ,
GroupNo = 1,
ControlAccountRef  = 1,
StatementSetupId = null,
AccountGroupId = null,
}
            };
        }

        public void AddAllDefault()
        {
            //for (int i = 0; i < GetAllDefaults().Max(s=> s.DEPTH); i++)
            //{
            //    context.Accounts.AddOrUpdate(s => s.autokey, GetAllDefaults().Where(s=> s.DEPTH ==i).ToArray());
            //    context.SaveChanges();
            //}
            context.Accounts.AddOrUpdate(s => s.autokey, GetAllDefaults().ToArray());
        }

        public void AddAllDefaultWithBranch(short branchId)
        {
            var accounts = GetAllWithBranch();
            accounts.ForEach(x => x.BranchId = branchId);
            context.Accounts.AddOrUpdate(s => s.autokey, accounts.ToArray());
        }

        //add accounts in Finance.DefaultAccount Table
        public void AddAllDefaultAccunts()
        {
            //for (int i = 0; i < GetAllDefaults().Max(s=> s.DEPTH); i++)
            //{
            //    context.Accounts.AddOrUpdate(s => s.autokey, GetAllDefaults().Where(s=> s.DEPTH ==i).ToArray());
            //    context.SaveChanges();
            //}
            context.DefaultAccounts.AddOrUpdate(s => s.autokey, GetAllDefaultsAccounts().ToArray());
        }

        public async System.Threading.Tasks.Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }
    }
}