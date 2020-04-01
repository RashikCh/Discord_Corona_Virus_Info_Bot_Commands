//Returns top-13 couintries based on total case count (NOT ACTIVE!)
public async Task getCoronaInfo3()
        {
            JArray jdata = JArray.Parse(new WebClient().DownloadString("https://corona.lmao.ninja/countries?sort=cases"));
            JObject jdataALL = JObject.Parse(new WebClient().DownloadString("https://corona.lmao.ninja/all"));
            double deathrate = Math.Round((double)jdataALL["deaths"] / (double)jdataALL["cases"] * 100.0, 3);
            double recoveryrate = Math.Round((double)jdataALL["recovered"] / (double)jdataALL["cases"] * 100.0, 3);
            string results = "**Global Confirmed Cases:** " + (string)jdataALL["cases"] + Environment.NewLine + "**Global Deaths: **" + (string)jdataALL["deaths"] + "" + Environment.NewLine + "** Global Recovered: **" + (string)jdataALL["recovered"] + Environment.NewLine + "** Global Death Rate: ~" + deathrate.ToString() + "%**" + Environment.NewLine + "**Global Recovery Rate: ~" + recoveryrate.ToString() + "% **" + Environment.NewLine + Environment.NewLine + "** ------------------------------- **" + Environment.NewLine + Environment.NewLine;
            int index = 1;
            foreach (var country in jdata)
            {
                string flagstring = country["countryInfo"]["iso2"].ToString().ToLower();
                if (flagstring == "no data" || flagstring.Length > 2 || flagstring.Length < 2)
                {
                    results += "**#" + index.ToString() + ". Country: " + country["country"] + " || ** Cases: " + country["cases"] + " ** | **  New Cases: " + country["todayCases"] + "** | ** Deaths: " + country["deaths"] + "** |** Deaths Today: " + country["todayDeaths"] + Environment.NewLine;
                }
                else
                {
                    results += "**#" + index.ToString() + ". :flag_" + country["countryInfo"]["iso2"].ToString().ToLower() + ": || Country: " + country["country"] + " | ** Cases: " + country["cases"]+ " ** | ** New Cases: " + country["todayCases"] + "** | ** Deaths: " + country["deaths"] + "** | ** Deaths Today: " + country["todayDeaths"]+ Environment.NewLine;
                }
                index++;
                if (index >= 14) break;
            }
            results +=  Environment.NewLine + "** ------------------------------- **" + Environment.NewLine + Environment.NewLine + "Source: https://github.com/NovelCOVID/API";
            return results;

        }
