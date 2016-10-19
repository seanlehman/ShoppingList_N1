using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingListApp.Models
{
    public enum Priority
    {
        [Display(Name = "It can wait")]
        ItCanWait=1,
        [Display(Name = "Need it soon")]
        NeedItSoon=2,
        [Display(Name = "Grab it now")]
        GrabItNow=3
    }
}