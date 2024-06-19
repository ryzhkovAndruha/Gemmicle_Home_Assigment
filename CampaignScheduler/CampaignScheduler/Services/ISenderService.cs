using CampaignScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignScheduler.Services
{
    internal interface ISenderService
    {
        void SendCampaign(Campaign campaign, Customer customer);
    }
}
