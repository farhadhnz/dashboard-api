using System.Collections.Generic;
using System.Data;
using dashboard_api.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace dashboard_api.Services
{

    public class CovidItemService
    {
        private readonly CovidContext _context;

        public CovidItemService(CovidContext context)
        {
            _context = context;
        }

        private async Task<bool> CheckIfDatabaseHasData()
        {
            return await _context.CovidItems.AnyAsync();
        }

        private async Task<bool> CheckIfCovidCountryHasData()
        {
            return await _context.CovidCountry.AnyAsync();
        }

        public async Task<TimeSpan> AddBulkDataAsync(DataTable dataTable)
        {
            if (await CheckIfCovidCountryHasData())
            {
                return TimeSpan.MinValue;
            }

            List<CovidCountry> countryItems = new();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (countryItems.Any(x => x.Name == dataTable.Rows[i]["location"].ToString()))
                    continue;

                var covidItem = new CovidCountry();
                covidItem.Id = i + 1;
                covidItem.GDP =  GetFloatForNull(dataTable, i, "gdp_per_capita");
                covidItem.HumanDevelopmentIndex = GetFloatForNull(dataTable, i, "human_development_index");
                covidItem.LifeExpectancy = GetFloatForNull(dataTable, i, "life_expectancy");
                covidItem.Name = dataTable.Rows[i]["location"].ToString();
                covidItem.Population = GetIntForNullAndFloat(dataTable, i, "population");
                covidItem.PopulationDensity = GetFloatForNull(dataTable, i, "population_density");
                countryItems.Add(covidItem);
            }
            await _context.BulkInsertAsync(countryItems);

            if (await CheckIfDatabaseHasData())
            {
                return TimeSpan.MinValue;
            }

            List<CovidItem> items = new();
            DateTime start = DateTime.Now;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var covidItem = new CovidItem();
                covidItem.Id = i + 1;
                covidItem.Continent = dataTable.Rows[i]["continent"].ToString();
                covidItem.Date = Convert.ToDateTime(dataTable.Rows[i]["date"]);
                covidItem.IsoCode = dataTable.Rows[i]["iso_code"].ToString();
                covidItem.Location = dataTable.Rows[i]["location"].ToString();
                covidItem.NewCases = GetIntForNullAndFloat(dataTable, i, "new_cases");
                covidItem.NewDeaths = GetIntForNullAndFloat(dataTable, i, "new_deaths");
                covidItem.TotalCases = GetIntForNullAndFloat(dataTable, i, "total_cases");
                covidItem.TotalDeaths = GetIntForNullAndFloat(dataTable, i, "total_deaths");
                covidItem.TotalCasesPerMilion = GetFloatForNull(dataTable, i, "total_cases_per_million");
                covidItem.TotalDeathsPerMilion = GetFloatForNull(dataTable, i, "total_deaths_per_million");
                covidItem.NewCasesPerMilion = GetFloatForNull(dataTable, i, "new_cases_per_million");
                covidItem.NewDeathsPerMilion = GetFloatForNull(dataTable, i, "new_deaths_per_million");
                covidItem.StringencyIndex = GetFloatForNull(dataTable, i, "stringency_index");
                items.Add(covidItem);
            }
            await _context.BulkInsertAsync(items);
            return DateTime.Now - start;
        }

        private int GetIntForNullAndFloat(DataTable dt, int i, string title)
        {
            var data = dt.Rows[i][title];

            if (data.Equals(DBNull.Value))
                return -1;

            try
            {
                var intOutput = (Convert.ToInt32(data));
                return intOutput;
            }
            catch
            {
                try
                {
                    double doubleOutput = (Convert.ToDouble(data));
                    return (int)doubleOutput;
                }
                catch
                {
                    return -1;
                }
            }

        }

        private double GetFloatForNull(DataTable dt, int i, string title)
        {
            var data = dt.Rows[i][title];

            if (data.Equals(DBNull.Value))
                return -1;

            try
            {
                var doubleOutput = (Convert.ToDouble(data));
                return doubleOutput;
            }
            catch
            {
                return -1;
            }

        }

    }
}