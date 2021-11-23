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

        public async Task<TimeSpan> AddBulkDataAsync(DataTable dataTable)
        {
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

    }
}