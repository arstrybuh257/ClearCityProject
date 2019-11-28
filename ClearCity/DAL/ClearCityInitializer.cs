using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ClearCity.Models;

namespace ClearCity.DAL
{
    public class ClearCityInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ClearCityContext>
    {
        protected override void Seed(ClearCityContext context)
        {
            var districts = new List<District>
            {
                new District {DistrictName = "Днепровский", Area = "34 кв.км", Population = "120 тыс.чел", Manager="Васильев Евгений Петрович"},
                new District {DistrictName = "Корабельный", Area = "27 кв.км", Population = "80 тыс.чел", Manager="Иванов Геннадий Петрович"},
                new District {DistrictName = "Суворовский", Area = "23 кв.км", Population = "95 тыс.чел", Manager="Жуков Петр Степанович"},
            };

            districts.ForEach(s => context.Districts.Add(s));
            context.SaveChanges();

            var microdistricts = new List<Microdistrict>
            {
                new Microdistrict{MicrodistrictName = "ХБК", DistrictId = 1},
                new Microdistrict{MicrodistrictName = "Стеклотара", DistrictId = 1},
                new Microdistrict{MicrodistrictName = "Восточный", DistrictId = 1},
                new Microdistrict{MicrodistrictName = "Таврический", DistrictId = 3},
                new Microdistrict{MicrodistrictName = "Шуменский", DistrictId = 2},
                new Microdistrict{MicrodistrictName = "Остров", DistrictId = 2},
            };

            microdistricts.ForEach(s => context.Microdistricts.Add(s));
            context.SaveChanges();

            var houses = new List<House>
            {
                new House{Adress = "Перекопская 181а", AmountOfCans = 8, MicrodistrictId = 1},
                new House{Adress = "Перекопская 183а", AmountOfCans = 12, MicrodistrictId = 1},
                new House{Adress = "Перекопская 1821а", AmountOfCans = 12, MicrodistrictId = 2},
                new House{Adress = "Перекопская 13а", AmountOfCans = 32, MicrodistrictId = 3},
                new House{Adress = "Перекопская 131а", AmountOfCans = 45, MicrodistrictId = 3}
            };

            houses.ForEach(s => context.Houses.Add(s));
            context.SaveChanges();

            var teams = new List<Team>
            {
                new Team{TeamName = "Без Бригады"},
                new Team{TeamName = "Дн-р1"},
                new Team{TeamName = "Дн-р2"},
                new Team{TeamName = "Cув-р1"},
                new Team{TeamName = "Кор-р1"},
            };

            teams.ForEach(s => context.Teams.Add(s));
            context.SaveChanges();

            var cars = new List<Car>
            {
                new Car{Model = "Volvo", Number = "ВТ 1766 АЕ",
                    DateOfRelease = new DateTime(2003, 12, 14),
                    DateOfLastInspection = new DateTime(2017,11,12), TeamId = 1 },
                new Car{Model = "Scania", Number = "AA 1736 АЕ",
                    DateOfRelease = new DateTime(2003, 12, 14),
                    DateOfLastInspection = new DateTime(2017,11,12), TeamId = 2},
                new Car{Model = "Volvo", Number = "AA 1234 АЕ",
                    DateOfRelease = new DateTime(2003, 12, 14),
                    DateOfLastInspection = new DateTime(2017,11,12), TeamId = 3 },
            };

            cars.ForEach(s => context.Cars.Add(s));
            context.SaveChanges();

        }
    }
}