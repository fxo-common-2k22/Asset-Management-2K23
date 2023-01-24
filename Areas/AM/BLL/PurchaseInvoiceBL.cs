using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.Model;
using System;
using System.Data.Entity;
using System.Linq;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
namespace FAPP.Areas.AM.BLL
{
    public class PurchaseInvoiceBL
    {
        short branch_ID = Convert.ToInt16(SessionHelper.BranchId);

        public PurchaseInvoiceBL()
        {

        }

        /// <summary>
        /// Get Product Id from barcode
        /// </summary>
        /// <param name="db"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public int? GetProductIdByBarcode(OneDbContext db, string barcode)
        {
            return db.AMItems.Where(u => u.Barcode == barcode).Select(u => (int?)u.ItemId).FirstOrDefault();
        }

        public decimal? GetProductDetailById(OneDbContext db, int? id = 0)
        {
            return db.AMItems.Where(u => u.ItemId == id).Select(u => (decimal?)u.Price).FirstOrDefault();
        }

        public void PostToAccounts()
        {

        }

        public string PostAMPurchaseInvoice(OneDbContext db ,long purchaseInvoiceId)
        {
            VoucherAndReceiptModel vr = new VoucherAndReceiptModel();
            var pInvoice = db.InvPurchaseInvoices.Where(u => u.BranchId == branch_ID).Where(u => u.PurchaseInvoiceId == purchaseInvoiceId).FirstOrDefault();
            if (pInvoice != null)
            {
                if (pInvoice.IsPosted && !pInvoice.IsAccountPosted && !pInvoice.IsCancelled)
                {
                    vr.Voucher = new Voucher();
                    vr.Voucher.VoucherType = "PI";
                    vr.Voucher.TransactionDate = pInvoice.PurchaseInvoiceDate;
                    vr.Voucher.VoucherStatus = "Posted";
                    vr.Voucher.CurrencyId = pInvoice.CurrencyId;
                    vr.Voucher.ExchangeRate = pInvoice.ExchangeRate;
                    vr.Voucher.CBAccountId = db.Clients.Where(u => u.ClientId == pInvoice.SupplierId).Select(u => u.AccountId).FirstOrDefault();
                    if(vr.Voucher.CBAccountId==null)
                    {
                        return "Supplier CBAccountId not Found,Please Add And Try Again.";
                    }
                    vr.Voucher.Particulars = "Purchase Invoice # " + pInvoice.PurchaseInvoiceId;


                    var SupplierAccount = db.Clients.Where(u => u.ClientId == pInvoice.SupplierId).Select(u => u.AccountId).FirstOrDefault();


                    if (pInvoice.VoucherId == null)
                    {
                        vr.Voucher.VoucherId = ProceduresModel.Voucher_Insert(db, vr.Voucher);
                        pInvoice.VoucherId = vr.Voucher.VoucherId;


                    }
                    else
                    {
                        vr.Voucher.VoucherId = Convert.ToInt64(pInvoice.VoucherId);
                        ProceduresModel.Voucher_Update(db, "Posted", vr.Voucher);
                    }

                    pInvoice.IsAccountPosted = true;
                    pInvoice.AccountPostedOn = DateTime.Now;
                    pInvoice.AccountPostedBy = SessionHelper.UserID;
                    //db.Entry(pInvoice).State = EntityState.Modified;
                    db.SaveChanges();

                    if (vr.Voucher.VoucherId > 0)
                    {
                        //Delete VoucherDetails
                        ProceduresModel.p_DeleteVoucherDetailsExceptfirst(db, vr.Voucher.VoucherId);
                        var pDetails = db.AMPurchaseInvoiceProducts.Where(u => u.PurchaseInvoiceId == purchaseInvoiceId).ToList();
                        if (pDetails != null)
                        {
                            foreach (var details in pDetails)
                            {
                                var vDetails = new VoucherDetail();
                                vDetails.VoucherId = vr.Voucher.VoucherId;
                                vDetails.AccountId = details.Item.InventoryAccountId;
                                vDetails.Debit = Convert.ToDecimal(details.NetTotal);
                                vDetails.Narration = vr.Voucher.Particulars;

                                if (details.VoucherDetailId > 0 && db.VoucherDetails.Where(u => u.VoucherDetailId == details.VoucherDetailId).Any())
                                {
                                    vDetails.VoucherDetailId = Convert.ToInt64(details.VoucherDetailId);
                                    ProceduresModel.VoucherDetail_Update(db, vDetails);
                                }
                                else
                                {
                                    var VoucherDetailId = ProceduresModel.VoucherDetail_Insert(db, vr.Voucher.VoucherId, vr.Voucher.VoucherType, vDetails);
                                    if (VoucherDetailId > 0)
                                    {
                                        details.VoucherDetailId = VoucherDetailId;
                                        db.Entry(details).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                            //VoucherDetail Trigger
                            ProceduresModel.t_VoucherDetail(db, vr.Voucher.VoucherId);
                        }

                        #region DiscountEntry
                        if (pInvoice.Discount > 0 && !string.IsNullOrEmpty(SessionHelper.DiscountAccountId_POS))
                        {
                            //string PosPurhcaseDiscoutAcc = "";
                            //var acc = db.AccountSettings.Where(m => m.BranchId == branch_ID).FirstOrDefault();
                            //if (acc != null)
                            //{
                            //    PosPurhcaseDiscoutAcc = acc.PurchaseDiscountAccountId;
                            //}
                            //else {

                            //}

                            //Discount Credit Voucher Entry
                            var vDetails = new VoucherDetail();
                            vDetails.VoucherId = (long)pInvoice.VoucherId;
                            //vDetails.AccountId = clientAccount;
                            //vDetails.AccountId = SessionHelper.DiscountAccountId_POS;
                            vDetails.AccountId = SessionHelper.PurchaseDiscountAccountId_POS;


                            vDetails.Credit = pInvoice.Discount;
                            //vDetails.Narration = _Voucher.Particulars + ", Discount debit entry";
                            vDetails.Narration = vr.Voucher.Particulars + ", Discount credit entry";

                            //var VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, _Voucher.VoucherId, "SI", vDetails);
                            var VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, vr.Voucher.VoucherId, "PI", vDetails);

                            if (VoucherDetailId > 0)
                            {
                                //pInvoice.DiscountDebitVoucherDetailId = VoucherDetailId;
                                db.Entry(pInvoice).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                            //Discount Debit Voucher Entry
                            vDetails = new VoucherDetail();
                            vDetails.VoucherId = vr.Voucher.VoucherId;
                            vDetails.AccountId = SupplierAccount;
                            //vDetails.AccountId = SessionHelper.DiscountAccountId_POS;
                            vDetails.Debit = pInvoice.Discount;
                            //vDetails.Narration = _Voucher.Particulars + ", Discount credit entry";
                            vDetails.Narration = vr.Voucher.Particulars + ", Discount debit entry";



                            VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, vr.Voucher.VoucherId, "PI", vDetails);
                            if (VoucherDetailId > 0)
                            {
                                //pInvoice.DiscountCreditVoucherDetailId = VoucherDetailId;
                                db.Entry(pInvoice).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                           
                        }
                        #endregion

                    }
                }
            }
            return null;
        }




    }
}