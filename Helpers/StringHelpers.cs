using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FAPP.Helpers
{
    public static class StringHelper
    {
        public static string GetBarCode(this string barcode)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (var bitMap = new System.Drawing.Bitmap(barcode.Length * 40, 80))
                {
                    using (var graphics = System.Drawing.Graphics.FromImage(bitMap))
                    {
                        var oFont = new System.Drawing.Font("IDAutomationHC39M", 16);
                        var point = new System.Drawing.PointF(2f, 2f);
                        var whiteBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                        var blackBrush = new System.Drawing.SolidBrush(System.Drawing.Color.DarkBlue);
                        graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                    }

                    bitMap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                    return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        //public static string GetNewFamilyRef()
        //{   
        //    using (var db = new OneDbContext())
        //    {
        //        var check = db.Families.Any();
        //        if (check)
        //        {
        //            var key = db.Families.Max(s => s.FamilyRef);
        //            int num = Convert.ToInt32(Regex.Match(key, @"\d+").Value) + 1;
        //            return "FML-" + string.Format("{0:D5}", num);
        //        }

        //        return "FML-00001";

        //    }
        //}

        

        //public static async Task<string> GetNewPaperNo()
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var key = await db.QuestionPapers.MaxAsync(s => s.PaperNo) ?? "QP-00000";
        //        int num = Convert.ToInt32(System.Text.RegularExpressions.Regex.Match(key, @"\d+").Value) + 1;
        //        return "QP-" + string.Format("{0:D5}", num);
        //    }
        //}

       

        //public static string GenerateProfileId(Branch branch, int year, Info company)
        //{
        //    string profileId = "";
        //    using (var dc = new OneDbContext())
        //    {
        //        string prefix = branch.RegPrefix;
        //        string format = company.ProfileFormat;

        //        object maxProfileNo = dc.Students
        //            .Where(st => st.RegistrationDate.Value.Year == year)
        //            .OrderByDescending(st => st.ProfileId.Substring(st.ProfileId.Length - format.Length))
        //            .Select(st => st.ProfileId.Substring(st.ProfileId.Length - format.Length))
        //            .FirstOrDefault() ?? "0";

        //        int profileNo = Convert.ToInt32(maxProfileNo);
        //        do
        //        {
        //            profileNo++;
        //            profileId = year.ToString() + prefix + profileNo.ToString(format);

        //        } while (dc.Students.Any(p => p.ProfileId == profileId));
        //    }
        //    return profileId;
        //}

        //public static string GenerateRegistrationNumber(Branch branch, int year, Info company)
        //{
        //    string registrationNo = "";
        //    using (var dc = new OneDbContext())
        //    {
        //        string prefix = branch.RegPrefix;
        //        string format = company.ProfileFormat;
        //        //Pakistan Overseas Higher Secondary School
        //        if (company.CompanyName.Contains("Pakistan Overseas Higher Secondary School"))
        //        {
        //            prefix = prefix + branch.BranchCode.ToString("0"); // To be fixed Later. It Was  prefix = prefix + branch.BranchCode.ToString("00"); we change it to ToString("0");
        //        }
        //        else
        //        {
        //            prefix = prefix + branch.BranchCode.ToString("00"); // To be fixed Later. It Was  prefix = prefix + branch.BranchCode.ToString("00");
        //        }

        //        int regnumber = Convert.ToInt32(dc.StudentAdmissions
        //            .Where(st => st.AdmissionDate.Year == year && st.BranchId == branch.BranchId)
        //            .OrderByDescending(st => st.RegistrationNo.Substring(st.RegistrationNo.Length - format.Length))
        //            .Select(st => st.RegistrationNo.Substring(st.RegistrationNo.Length - format.Length))
        //            .FirstOrDefault() ?? "0");

        //        do
        //        {
        //            regnumber++;
        //            registrationNo = prefix + year.ToString().Substring(2) + regnumber.ToString(format);
        //        } while (dc.StudentAdmissions.Any(p => p.RegistrationNo == registrationNo) || dc.Students.Any(p => p.RegistrationNumber == registrationNo));
        //    }
        //    return registrationNo;
        //}
        //public static string GenerateGrNo(short BranchId)
        //{
        //    using (var dc = new OneDbContext())
        //    {
        //        var maxStr = dc.Students
        //              .OrderByDescending(p => p.GRNo.Length)
        //              .ThenByDescending(p => p.GRNo)
        //              .FirstOrDefault(p => p.BranchId == BranchId);

        //        var grNoStr = "";
        //        if (maxStr != null)
        //            grNoStr = maxStr.GRNo;

        //        var grNo = 0;
        //        if (int.TryParse(grNoStr, out grNo))
        //        {
        //            grNo++;
        //            grNoStr = grNo.ToString();
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrWhiteSpace(grNoStr))
        //            {
        //                var index = grNoStr.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
        //                if (index > -1)
        //                {
        //                    if (int.TryParse(grNoStr.Substring(index), out grNo))
        //                    {
        //                        grNo++;
        //                        grNoStr = grNoStr.Substring(0, index) + grNo.ToString();
        //                    }
        //                }
        //            }
        //        }
        //        return grNoStr;
        //    }
        //}

        //public static async Task<string> GenerateHouseCode(string prefix)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        if (prefix.Length > 1)
        //        {
        //            var code = await db.SchoolHouses.Select(s => s.HouseCode).MaxAsync();
        //            if (string.IsNullOrEmpty(code))
        //            {
        //                return prefix + "-01".ToUpper();
        //            }
        //            if (Regex.Match(code, @"\d+") != null && Regex.Match(code, @"\d+").Value != "")
        //            {
        //                var maxNumber = Convert.ToInt32(Regex.Match(code, @"\d+")?.Value ?? 0.ToString()) + 1;
        //                prefix = prefix + "-" + string.Format("{0:D1}", maxNumber);
        //            }
        //            else
        //            {
        //                prefix = prefix + "-" + string.Format("{0:D1}", 1);
        //            }

        //            return prefix.ToUpper();
        //        }
        //        return string.Empty;
        //    }
        //}


        private static string ones(string Number)
        {
            int _Number = Convert.ToInt32(Number);
            string name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        private static string tens(string Number)
        {
            int _Number = Convert.ToInt32(Number);
            string name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static string ConvertWholeNumber(string Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX    
                bool isDone = false;//test if already translated    
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))    
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric    
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping    
                    string place = "";//digit grouping name:hundres,thousand,etc...    
                    switch (numDigits)
                    {
                        case 1://ones' range    

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range    
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range    
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range    
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range    
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range    
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...    
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)    
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros    
                        //if (beginsZero) word = " and " + word.Trim();    
                    }
                    //ignore digit grouping names    
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }
        private static string ConvertToWords(string numb)
        {
            string val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            string endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents    
                        endStr = "Paisa " + endStr;//Cents    
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = string.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }
        private static string ConvertDecimals(string number)
        {
            string cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }

       public static string ConvertNumberToString(string number)
        {
            string isNegative = "";
            try
            {
                number = Convert.ToDouble(number).ToString();

                if (number.Contains("-"))
                {
                    isNegative = "Minus ";
                    number = number.Substring(1, number.Length - 1);
                }
                if (number == "0")
                {
                   return("The number in currency fomat is \nZero Only");
                }
                else
                {
                    return(isNegative + ConvertToWords(number));
                }
            }
            catch (Exception ex)
            {
                return(ex.Message);
            }
        }
    }

}