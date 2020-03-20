 //You will need the following usings, all ontainable from nugget. 
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading;
using Google.Apis.Util.Store;
using Microsoft.EntityFrameworkCore.Internal;



       public async Task getCoronaInfoGoogle()
        {
            //Most of this code was provided by google, they have speicific OAuth2 requirements for accessing their data. 
            //I'm not sure if reverifcation is always needed but this works. 
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {

                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    (new string[] { SheetsService.Scope.SpreadsheetsReadonly }),
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }


            var sheetservice = new SheetsService(new BaseClientService.Initializer() { HttpClientInitializer = credential, ApplicationName = "<YOURBOTNAME>"});
            string ssID = "1qQ2IovoB76jHiGCVx8VwKBRW4Wa_5rugyVK8bT8WArc";
            string range = "Sheet1!A2:J"; //This skips the first gray line, additionally, it only gives you 10 rows (might expand later as data grows)
            SpreadsheetsResource.ValuesResource.GetRequest request = sheetservice.Spreadsheets.Values.Get(ssID, range);
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            //Basically pulling the values into a List<List>
            //Console.WriteLine(values.Count.ToString()); Debug line to make sure you got the data

            //Data parsing, kind of sloppy but it's a WIP. 
            int index = 0;
            string lastupdate = "";
            List<string> countries = new List<string>();
            string countries2 = "";
                foreach (var row in values)
            {

                if (index == 0)
                {
                    lastupdate = (string)row[0];
                    index++;
                }
                if (row.Count == 10 && row[0] != "")
                {

                    if ((string)row[0] == "Country/Region")
                    {
                        continue;
                    }


                    if (index >= 11)
                    {


                        if ((string)row[0] == "Totals")
                        {

                            countries.Add(Environment.NewLine + " **" + row[0] + " **|** Total Confirmed: " + row[2] + " **|** Active Cases: " + row[1] + " **|** Deaths: " + row[5] + Environment.NewLine +  " **|** Recovery %: " + row[6] + " **|** Death %: " + row[7] + Environment.NewLine + " ------------------------------- **");
                            break;
                        }
                        //row[0] = Country/Region | 1 = Active | 2 = Confirmed || 3 = Serious | 4 = Recovered | 5 = Deaths | 6 = Recov % | 7 = Death rate |
                    }
                    else
                    {
                        countries.Add("**#" + (index).ToString() + ".** Country: " + row[0] + " **|** Active Cases: " + row[1] + " **|** Deaths: " + row[5] + " **|** Death Rate: " + row[7] + Environment.NewLine);
                        index++;
                    }

                    
                }

            }
            
            
            countries2 = String.Concat(countries);
            countries2 += Environment.NewLine + "Source: https://docs.google.com/spreadsheets/d/1qQ2IovoB76jHiGCVx8VwKBRW4Wa_5rugyVK8bT8WArc/edit#gid=176307533";

            await Context.Channel.SendMessageAsync(countries2);
            
            //I used an embed here but for the purposes of making this portable, I decided not to include it and put a generic send message. The example picture will show what embedded output would look like. 
        }
