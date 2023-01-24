using FAPP.Model;
using System;
using System.Text.RegularExpressions;

namespace FAPP.Service
{
    public class StringFormate
    {
        public static string CreateSalaryBatchCode(string code, DateTime salaryMonth)
        {
            var bcode = DAL.SessionHelper.BranchCode.ToString();
            if (string.IsNullOrEmpty(bcode))
                bcode = "1";
            else
            {
                if (bcode.Length > 2)
                    bcode = bcode.Substring(bcode.Length - 2);
            }
            if (string.IsNullOrEmpty(code))
            {
                //generate new code
                code = "SB-" + bcode + salaryMonth.ToString("yy") + salaryMonth.ToString("MM") + String.Format("{0:D4}", 1);

            }
            else
            {
                Regex re = new Regex(@"\d+");
                Match result = re.Match(code);
                int numaricPart = Convert.ToInt32(result.Value);
                code = "SB-" + bcode + salaryMonth.ToString("yy") + salaryMonth.ToString("MM") + String.Format("{0:D4}", numaricPart + 1);
            }
            return code;
        }

        public static string CreatePurchaseCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                //generate new code
                code = "PO-" + String.Format("{0:D5}", 1);

            }
            else
            {
                Regex re = new Regex(@"\d+");
                Match result = re.Match(code);
                int numaricPart = Convert.ToInt32(result.Value);
                code = "PO-" + String.Format("{0:D5}", numaricPart + 1);
            }
            return code;
        }

        public static string GenerateItemShortName(string name, int categoryId)
        {
            string newName = "";
            if (name.Contains(" "))
            {
                string[] temp = name.Split(' ');
                newName = GetFirstLettersOfStrings(temp);
            }
            else
            {
                newName = GenerateShortName(name);
            }
            using (var db = new OneDbContext())
            {
                var category = db.AMCategories.Find(categoryId);
                newName = category.ShortName + "-" + newName;
            }

            return newName;
        }

        public static string GetFirstLettersOfStrings(string[] list)
        {
            string newName = "";
            foreach (var item in list)
            {
                newName = newName + item[0];
            }
            return newName.ToUpper();
        }

        public static string GenerateShortName(string name)
        {
            string newName = name[0].ToString().ToUpper() + name[name.Length - 1].ToString().ToUpper();
            return newName;
        }

    }
}