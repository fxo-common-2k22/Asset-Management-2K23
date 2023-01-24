using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.ViewModel
{
    public class Announcement
    {
        public int ContentTypeID { get; set; }
        public int PostId { get; set; }
        public long? Priority { get; set; }
        public string TypeSlug { get; set; }
        public string ImageSource { get; set; }
        public string VideoSource { get; set; }
        public string PostSlug { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string PostTitle { get; set; }
        public string UrduTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string PageHeaderPhoto { get; set; }
        public string VideoKey { get; set; }
        public string Itinerary { get; set; }
        public string Service { get; set; }
        public string TermsConditions { get; set; }
        public string Thumbnail { get; set; }
        public bool IsPublished { get; set; }
        public bool? ShowOnHomePage { get; set; }
        public bool? ShowInSlider { get; set; }
        public bool? ShowInMainMenu { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime Date { get; set; }
        public int? IconId { get; set; }
        public int? ParentID { get; set; }
        public int? WebsiteId { get; set; }
        public bool? Default { get; set; }
        //public List<Category> Categories { get; set; }
        //public List<Photos> Photos { get; set; }
        //public List<Events> Events { get; set; }
        public string OtherTitle { get; set; }
        public string ImageUrl { get; set; }
        public string CategorySlug { get; set; }
        public string ContentUrl { get; set; }
        public string ThumbnailUrl { get; internal set; }
        public string ParentSlug { get; internal set; }
        public bool? IsCategory { get; internal set; }
        public string OtherShortDescription { get; set; }
    }
}