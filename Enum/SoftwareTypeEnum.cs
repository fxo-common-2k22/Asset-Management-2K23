using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FAPP.Enum
{
    public enum SoftwareTypeEnum
    {
        School = 1,
        College = 2,
        [Description("Other_Business")]
        OtherBusiness = 3,
    }
}