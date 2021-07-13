using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Quest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ItemQuantity> ItemToComplete { get; set; }

        public int RewardsExperiencePoints { get; set; }
        public int RewardGold { get; set; }
        public List<ItemQuantity> RewardItems { get; set; }

        public Quest(int id, string name, string description, List<ItemQuantity> itemsToComplete
            ,int rewardExperiencePoints, int rewardgold, List<ItemQuantity> rewardItems)
        {
            ID = id;
            Name = name;
            Description = description;
            ItemToComplete = itemsToComplete;
            RewardsExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardgold;
            RewardItems = rewardItems;

        }
    }
}
