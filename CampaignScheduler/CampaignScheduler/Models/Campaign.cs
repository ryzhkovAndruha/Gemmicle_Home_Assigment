using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignScheduler.Models
{
    public class Campaign
    {
        public string Template { get; set; }
        public Func<Customer, bool> Condition { get; set; }
        public DateTime SendTime { get; set; }
        public int Priority { get; set; }
    }
}
