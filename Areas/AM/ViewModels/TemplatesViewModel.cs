using FAPP.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FAPP.Areas.AM.ViewModels
{
    public class TemplatesViewModel
    {
        public Templates1 Template { get; set; }
        public Branch Branch { get; set; }
        public View View { get; set; }
        public List<View> ViewList { get; set; }
        public List<Templates1> TemplateList { get; set; }
        public List<SelectListItem> ViewFieldsDD { get; set; }
        public List<SelectListItem> TemplatesDD { get; set; }
        public IEnumerable<SelectListItem> GroupsDD { get; set; }
        public IEnumerable<SelectListItem> StagesDD { get; set; }
        public IEnumerable<SelectListItem> ClassesDD { get; set; }
        public List<SelectListItem> ViewsDD { get; set; }
        public ArrayList Fields { get; set; }
        public List<string> SelectedFields { get; set; }
        public List<string> SelectedFilters { get; set; }
        public ReportBuilderViewModel ReportBuilderViewModel { get; set; }
        public CustomReportVM CustomReport { get; set; }
        [DisplayName("Print Header")]
        [UIHint("YesNo")]
        public bool IsPrintHeader { get; set; }
        [DisplayName("Print Footer")]
        [UIHint("YesNo")]
        public bool IsPrintFooter { get; set; }

    }
    public class ReportBuilderViewModel
    {
        public int ReportTemplateId { get; set; }
        public Guid? ClassId { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? StageId { get; set; }
        public string Title { get; set; }
    }
    public class CustomReportVM
    {
        public ArrayList Fields { get; set; }
        public List<List<String>> Values { get; set; }
    }
}