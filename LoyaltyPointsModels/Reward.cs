using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPointsModels
{
    public class Reward
    {
        public int RewardId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PointsCost { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}