﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            // Declare the items need to complete the quest, and its reward items
            List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> rewardItems = new List<ItemQuantity>();

            itemsToComplete.Add(new ItemQuantity(9001, 5));
            rewardItems.Add(new ItemQuantity(1002, 1));

            // Create the quest
            _quests.Add(new Quest(1,
                                  "Clear the herb garden",
                                  "Defeat the snakes in the Herbalist's garden",
                                  itemsToComplete,
                                  25, 10,
                                  rewardItems));

            _quests.Add(new Quest(2,
                "Clear the farmer's field",
                "Defeat the rats that are in the farmer's field",
                new List<ItemQuantity> { new ItemQuantity(9003, 5) },
                25, 10,
                new List<ItemQuantity> { new ItemQuantity(1001, 1) }));
        }

        internal static Quest GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.ID == id);
        }
    }
}
