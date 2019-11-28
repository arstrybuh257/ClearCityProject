using ClearCity.DAL;
using ClearCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClearCity.Automatization
{
    public  class AutoHelper
    {
        public class TeamAmount
        {
            public Team Team;
            public int Value;
        }
        private List<Team> teams;
        private List<House> houses;
        public AutoHelper(ClearCityContext db)
        {
            teams = db.Teams.Where(t=> t.TeamId!=1).ToList();
            houses = db.Houses.OrderByDescending(h => h.AmountOfCans).ToList();
        }

        private int Avg()
        {
            int totalAmount = houses.Sum(h => h.AmountOfCans);
            int teamsCount = teams.Count();

            return totalAmount / teamsCount;
        }


        public List<Plan> GetPlan(DateTime date)
        {
            int avg = Avg();
            List<TeamAmount> dict = new List<TeamAmount>();
            List<Plan> list = new List<Plan>();         
            foreach(var t in teams)
            {
                dict.Add(new TeamAmount { Team = t, Value = 0 });
            }

            while (houses.Count > 0)
            {
                dict = dict.OrderByDescending(t=> t.Value).ToList();
                for (int i = 0; i < dict.Count; i++)
                {
                    if (houses.Count == 0) return list;
                    if (dict[i].Value < 1.2 * avg)
                    {
                        dict[i].Value += houses[0].AmountOfCans;
                        list.Add(new Plan { Date = date, HouseId = houses[0].HouseId, TeamId = dict[i].Team.TeamId });
                        houses.RemoveAt(0);
                    }
                }

                dict = dict.OrderBy(t => t.Value).ToList();

                for (int i = 0; i < dict.Count; i++)
                {
                    if (houses.Count == 0) return list;
                    if (dict[i].Value < 1.2 * avg)
                    {
                        dict[i].Value += houses[0].AmountOfCans;
                        list.Add(new Plan { Date = date, HouseId = houses[0].HouseId, TeamId = dict[i].Team.TeamId });
                        houses.RemoveAt(0);
                    }
                }
            }
            


            return list;
        }

    }
}