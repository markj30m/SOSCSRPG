using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Models;
using Engine.Factories;
using Engine.EventArgs;
using System.ComponentModel;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;



        private Location _currentLocation;
        private Monster _currentMonster;
        private Trader _currentTrader;

        public World CurrentWorld { get; set; }
        public Player CurrentPlayer { get; set; } 
        public Location CurrentLocation
        { get
            {
                return _currentLocation;
            }
          set
            {
                _currentLocation = value;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToSouth));

                CompleteQuestAtLocation();
                GivePlayerQuestsAtLocation();
                GetMonsterAtLocation();

                CurrentTrader = CurrentLocation.TraderHere;
            }
        }

        public Monster CurrentMonster
        {
            get 
            { 
                return _currentMonster; 
            }
            set
            {
                _currentMonster = value;

                OnPropertyChanged(nameof(CurrentMonster));
                OnPropertyChanged(nameof(HasMonster));

                if(CurrentMonster != null)
                {
                    RaiseMessage("");
                    RaiseMessage($"You See a {CurrentMonster.Name} here!");
                }
            }

        }
        
        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set 
            {
                _currentTrader = value;

                OnPropertyChanged(nameof(CurrentTrader));
                OnPropertyChanged(nameof(HasTrader));

            }
        }

        public Weapon CurrentWeapon { get; set; }
        // checks map around the players if there is something there in the certian location 
        public bool HasLocationToNorth => 
               CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate +1) !=null;            
        public bool HasLocationToWest =>
               CurrentWorld.LocationAt(CurrentLocation.XCoordinate -1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToEast =>
                CurrentWorld.LocationAt(CurrentLocation.XCoordinate +1, CurrentLocation.YCoordinate) != null;                
        public bool HasLocationToSouth =>
                CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate -1) != null;
                 
        public bool HasMonster => CurrentMonster != null;

        public bool HasTrader => CurrentTrader != null;

        public GameSession()
        {
            // Information of player 

            CurrentPlayer = new Player 
            {
                Name = "Mark", 
                CharacterClass = "Fighter",
                HitPoints = 10, 
                Gold = 1000000, 
                ExperiencePoints = 0, 
                Level = 1
            };

            if(!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }

            // The world the player is on 
            CurrentWorld = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, 0);

        }
      
        public void MoveNorth()
        {
            if(HasLocationToNorth)
            {
            CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);

            }
        }
        public void MoveWest()
        {
            if (HasLocationToWest) 
            {
            CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate -1, CurrentLocation.YCoordinate);          
            }
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
            { 
            CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate +1, CurrentLocation.YCoordinate);           
            }
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
            CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate , CurrentLocation.YCoordinate -1);      
            }
        }

        public void CompleteQuestAtLocation()
        {
            foreach(Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete =
                    CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID
                                                             && !q.IsCompleted);
                if(questToComplete != null)
                {
                    if(CurrentPlayer.HasAllTheseItems(quest.ItemToComplete))
                    {
                        // removes quest item from player inventory
                        foreach(ItemQuantity itemQuantity in quest.ItemToComplete)
                        {
                            for(int i = 0; i < itemQuantity.Quantity; i++)
                            {
                                CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
                            }
                        }

                        RaiseMessage("");
                        RaiseMessage($"You Completed the '{quest.Name}' quest");

                        // code to give the player the quest rewards
                        CurrentPlayer.ExperiencePoints += quest.RewardsExperiencePoints;
                        RaiseMessage($"You received {quest.RewardsExperiencePoints} experience points");

                        CurrentPlayer.Gold += quest.RewardGold;
                        RaiseMessage($"You received {quest.RewardGold} gold");

                        foreach(ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewarditem = ItemFactory.CreateGameItem(itemQuantity.ItemID);

                            CurrentPlayer.AddItemToInventory(rewarditem);
                            RaiseMessage($"You received a {rewarditem.Name}");
                        }

                        // The quest is completed
                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }

        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));

                    RaiseMessage("");
                    RaiseMessage($"You receive the '{quest.Name}' quest");
                    RaiseMessage("Return with:");
                    foreach(ItemQuantity itemQuantity in quest.ItemToComplete)
                    {
                        RaiseMessage($" {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name }");
                    }

                    RaiseMessage("And you will receive");
                    RaiseMessage($" {quest.RewardsExperiencePoints} experience points");
                    RaiseMessage($" {quest.RewardGold} gold");
                    foreach(ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        RaiseMessage($" {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name} item");
                    }


                }
            }
        }

        public void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        public void AttackCurrentMonster()
        {
            if(CurrentWeapon == null)
            {
                RaiseMessage("You must select a weaponm to attack.");
            }

            // damage delt to monster
            int damageToMonster = RandomNumberGenerator.NumberBetween(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage);

            if(damageToMonster == 0)
            {
                RaiseMessage($"You missed the {CurrentMonster.Name}.");
            }
            else
            {
                CurrentMonster.HitPoints -= damageToMonster;
                RaiseMessage($"You Hit the {CurrentMonster.Name} For {damageToMonster} points.");
            }

            // if the monster was killed 
            if(CurrentMonster.HitPoints <= 0)
            {
                RaiseMessage($"");
                RaiseMessage($"You defeated the {CurrentMonster.Name}!.");

                CurrentPlayer.ExperiencePoints += CurrentMonster.RewardExperiencePoints;
                RaiseMessage($"You received {CurrentMonster.RewardExperiencePoints} experience points for killing {CurrentMonster.Name}.");

                CurrentPlayer.Gold += CurrentMonster.RewardGold;
                RaiseMessage($"You received {CurrentMonster.RewardGold} Gold for killing {CurrentMonster.Name}.");

                foreach(ItemQuantity itemQuantity in CurrentMonster.Inventory)
                {
                    GameItem item = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                    CurrentPlayer.AddItemToInventory(item);
                    RaiseMessage($"You received {itemQuantity.Quantity} {item.Name}.");
                }

                // spawn another monster
                GetMonsterAtLocation();
            }
            else
            {
                // if monster is alive the monster will attack back
                int damageToPlayer = RandomNumberGenerator.NumberBetween(CurrentMonster.MinimumDamage, CurrentMonster.MaximumDamage);

                if (damageToPlayer == 0)
                {
                    RaiseMessage($"{CurrentMonster.Name} missed it's attack.");
                }
                else
                {
                    CurrentPlayer.HitPoints -= damageToPlayer;
                    RaiseMessage($"The {CurrentMonster.Name} hit you for {damageToPlayer} points.");
                }
                // if player is killed
                if(CurrentPlayer.HitPoints <= 0)
                {
                    RaiseMessage($"");
                    RaiseMessage($"You Was Killed By {CurrentMonster.Name}!.");

                    // player respawns in there home
                    CurrentLocation = CurrentWorld.LocationAt(0, -1); // the players home
                    CurrentPlayer.HitPoints = CurrentPlayer.Level * 10; // will heal the player to full
                }
            }
        }


        private void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }
    }
}
