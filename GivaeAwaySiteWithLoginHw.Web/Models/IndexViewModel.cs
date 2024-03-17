using GiveAwayWithLoginHw.Data;

namespace GiveAwaySiteWithLoginHw.Web.Models
{
    public class IndexViewModel
    {
        public int CurrentUserId { get; set; }
        public List<Ad> Ads { get; set; }
    }
}
