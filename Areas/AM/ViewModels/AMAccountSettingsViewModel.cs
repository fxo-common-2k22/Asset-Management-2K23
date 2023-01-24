using System.Collections.Generic;

namespace FAPP.Areas.AM.ViewModels
{
    public class AMAccountSettingsViewModel
    {
        public Model.AMNature Nature { get; set; }
        public IEnumerable<ValueAndText> AccountsDD { get; set; }
        public int? NatureId { get; set; }
        public string AccountAutokey { get; set; }
        public string AccountName { get; set; }
        public int AccountSettingsId { get; set; }
        public string FixedParentId { get; set; }
        public string FixedAutokey { get; set; }
        public string ConsumeableParentId { get; set; }
        public string ConsumeableAutokey { get; set; }
        public bool IndividualItemAccount { get; set; }
        public IEnumerable<AccountSettingsRecord> AccountSettingsRecords { get; set; }
    }
    public class AccountSettingsRecord
    {
        public string ParentAccount { get; set; }
        public string Account { get; set; }
    }
    public class ValueAndText
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}