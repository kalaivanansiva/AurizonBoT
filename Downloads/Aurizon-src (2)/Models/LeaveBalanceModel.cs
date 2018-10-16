using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Bot.Sample.LuisBot
{
	
	public class LeaveBalance
    {
        [JsonProperty("leave_type")]
        public string LeaveType { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty("leaves_remaining")]
        public double LeavesRemaining { get; set; }

    }

    public class LeaveBalanceModel{
        [JsonProperty("leaveBalance")]
        public List<LeaveBalance> LeaveBalance { get; set; }
    }
	
}