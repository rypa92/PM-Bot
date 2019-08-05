using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace MastersBot.Core.Commands
{
    public class infoCommands
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "PokemonMastersBot";
        static readonly string SpreadsheetId = "1ymE_SKZ1znH5YAoF5LL7J8JTcPvdiUuufXVdYLLy7Qk";
        static SheetsService service;
        static TextInfo properText = new CultureInfo("en-US", false).TextInfo;
        static int rowNumber = 0;
        public List<string> getTrainerInfo(string input)
        {
            List<string> results = new List<string>();

            GoogleCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            getRow("Trainer", input);

            var range = $"Trainer!C{rowNumber}:E{rowNumber}";
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = request.Execute();
            var values = response.Values;
            if(values != null && values.Count > 0)
            {
                foreach(var col in values)
                {
                    results.Add(col[0].ToString());
                    results.Add(col[1].ToString());
                    results.Add(col[2].ToString());
                }
            }

            return results;
        }

        public List<string> getPokemonInfo(string input)
        {
            List<string> results = new List<string>();

            GoogleCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            getRow("Pokemon", input);

            var range = $"Pokemon!C{rowNumber}:P{rowNumber}";
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var col in values)
                {
                    results.Add(col[0].ToString()); //Name
                    results.Add(col[1].ToString()); //Type
                    results.Add(col[2].ToString()); //Weakness
                    results.Add(col[3].ToString()); //HP
                    results.Add(col[4].ToString()); //ATK
                    results.Add(col[5].ToString()); //DEF
                    results.Add(col[6].ToString()); //SATK
                    results.Add(col[7].ToString()); //SDEF
                    results.Add(col[8].ToString()); //SPD
                  //results.Add(col[9].ToString()); //Empty
                  //results.Add(col[10].ToString()); //Bulk
                    results.Add(col[11].ToString()); //Rank
                  //results.Add(col[12].ToString()); //Stat
                    results.Add(col[13].ToString()); //Rank
                }
            }

            return results;
        }

        public List<string> getPairInfo(string input)
        {
            List<string> results = new List<string>();

            GoogleCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            getRow("SyncPair", input);

            var range = $"SyncPair!B{rowNumber}:AF{rowNumber}";
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var col in values)
                {
                    results.Add(col[0].ToString()); //ID
                    results.Add(col[1].ToString()); //Trainer
                    results.Add(col[2].ToString()); //Rarity
                    results.Add(col[3].ToString()); //Pokemon
                    results.Add(col[4].ToString()); //Role
                    results.Add(col[14].ToString()); //Move 1
                    results.Add(col[16].ToString()); //Move 2
                    results.Add(col[18].ToString()); //Move 3
                    results.Add(col[20].ToString()); //Move 4
                    results.Add(col[23].ToString()); //Sync Move
                    results.Add(col[26].ToString()); //Passive 1
                    if(col.Count >= 29)
                    {
                        results.Add(col[28].ToString()); //Passive 2
                    }
                    if(col.Count >= 31)
                    {
                        results.Add(col[30].ToString()); //Passive 3
                    }
                }
            }

            return results;
        }

        public void getRow(string sheetName, string insertValue)
        {
            GoogleCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var updateRange = $"{sheetName}LookUp!A1";
            var updateValueRange = new ValueRange();

            var objectList = new List<object>() { insertValue };
            updateValueRange.Values = new List<IList<object>> { objectList };

            var updateRequest = service.Spreadsheets.Values.Update(updateValueRange, SpreadsheetId, updateRange);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = updateRequest.Execute();

            var range = $"{sheetName}LookUp!B:B";
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    rowNumber = int.Parse(row[0].ToString());
                }
            }
        }
    }
}
