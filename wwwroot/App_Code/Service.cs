using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using HtmlAgilityPack;
using System.Xml.XPath;
using MySql.Web;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using LemmaSharp;
using System.Net;


// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
	public void UpdateDBForPlayer(Player player)
	{
        string html = player.html;
        string playerID = player.playerID;

        //Stats page parsing setup
        HtmlAgilityPack.HtmlWeb web = new HtmlWeb();
        HtmlAgilityPack.HtmlDocument doc = web.Load(html);

        //MySql connection set up
        MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        conn_string.Server = "sethia.cosfzwdmywyo.us-west-2.rds.amazonaws.com";
        conn_string.UserID = "cheetobuv10";
        conn_string.Password = "cheeto10";
        conn_string.Database = "NBAbase";
        MySqlConnection l_DBConn = new MySqlConnection();
        l_DBConn.ConnectionString = conn_string.ToString();

        //Store stat values in here
        string[] values;
        string results = "";

        //Get per game stats from HTML
        foreach (HtmlNode row in doc.DocumentNode.SelectNodes("//*[@id=\"per_game.2015\" and @class=\"full_table\"]"))
        {
            //Console.WriteLine(row.InnerText);
            results = row.InnerText;
        }

        //Clean them up
        values = results.Trim().Split('\n');
        for (int j = 0; j < values.Length; j++)
        {
            values[j] = values[j].Trim();
            Console.WriteLine("Column {0} = {1}", j, values[j]);
        }

        Console.WriteLine("{0} columns", values.Length);

        //Add them to database
        MySqlCommand command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "replace into PerGameStats SET playerID='" + playerID + "', Season='" + values[0].Replace("&nbsp;", "") + "', G='" + values[5] + "', MP='" + values[7] + "', FG='" + values[8] + "', FGA='" + values[9] + "', FGPCT='" + values[10] + "', 3P='" + values[11] + "', 3PA='" + values[12] + "', 3PPCT='" + values[13] + "', 2P='" + values[14] + "', 2PA='" + values[15] + "', 2PPCT='" + values[16] + "', FT='" + values[18] + "', FTA='" + values[19] + "', FTPCT='" + values[20] + "', ORB='" + values[21] + "', DRB='" + values[22] + "', TRB='" + values[23] + "', AST='" + values[24] + "', STL='" + values[25] + "', BLK='" + values[26] + "', TOV='" + values[27] + "', PF='" + values[28] + "', PTS='" + values[29] + "'";
        //Open connection to DB
        l_DBConn.Open();
        int i = command.ExecuteNonQuery();

        //Confirmation
        Console.WriteLine("{0} rows updated.", i);

        //Get per 36 stats from HTML
        foreach (HtmlNode row in doc.DocumentNode.SelectNodes("//*[@id=\"per_minute.2015\" and @class=\"full_table\"]"))
        {
            //Console.WriteLine(row.InnerText);
            results = row.InnerText;
        }

        //Clean them up
        values = results.Trim().Split('\n');
        for (int j = 0; j < values.Length; j++)
        {
            values[j] = values[j].Trim();
            Console.WriteLine("Column {0} = {1}", j, values[j]);
        }

        Console.WriteLine("{0} columns", values.Length);

        //Add them to database
        command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "replace into Per36Stats SET playerID='" + playerID + "', Season='" + values[0].Replace("&nbsp;", "") + "', G='" + values[5] + "', MP='36', FG='" + values[8] + "', FGA='" + values[9] + "', FGPCT='" + values[10] + "', 3P='" + values[11] + "', 3PA='" + values[12] + "', 3PPCT='" + values[13] + "', 2P='" + values[14] + "', 2PA='" + values[15] + "', 2PPCT='" + values[16] + "', FT='" + values[17] + "', FTA='" + values[18] + "', FTPCT='" + values[19] + "', ORB='" + values[20] + "', DRB='" + values[21] + "', TRB='" + values[22] + "', AST='" + values[23] + "', STL='" + values[24] + "', BLK='" + values[25] + "', TOV='" + values[26] + "', PF='" + values[27] + "', PTS='" + values[28] + "'";
        i = command.ExecuteNonQuery();

        //Confirmation
        Console.WriteLine("{0} rows updated.", i);

        //Get career stats
        foreach (HtmlNode row in doc.DocumentNode.SelectNodes("//*[@id=\"per_game\"]/tfoot/tr[1]"))
        {
            //Console.WriteLine(row.InnerText);
            results = row.InnerText;
        }

        //Clean them up
        values = results.Trim().Split('\n');
        for (int j = 0; j < values.Length; j++)
        {
            values[j] = values[j].Trim();
            Console.WriteLine("Column {0} = {1}", j, values[j]);
        }

        Console.WriteLine("{0} columns", values.Length);

        //Add them to database
        command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "replace into CareerStats SET playerID='" + playerID + "', G='" + values[5] + "', MP='" + values[7] + "', FG='" + values[8] + "', FGA='" + values[9] + "', FGPCT='" + values[10] + "', 3P='" + values[11] + "', 3PA='" + values[12] + "', 3PPCT='" + values[13] + "', 2P='" + values[14] + "', 2PA='" + values[15] + "', 2PPCT='" + values[16] + "', FT='" + values[18] + "', FTA='" + values[19] + "', FTPCT='" + values[20] + "', ORB='" + values[21] + "', DRB='" + values[22] + "', TRB='" + values[23] + "', AST='" + values[24] + "', STL='" + values[25] + "', BLK='" + values[26] + "', TOV='" + values[27] + "', PF='" + values[28] + "', PTS='" + values[29] + "'";
        i = command.ExecuteNonQuery();

        //Confirmation
        Console.WriteLine("{0} rows updated.", i);

        //Get per game advanced stats
        foreach (HtmlNode row in doc.DocumentNode.SelectNodes("//*[@id=\"advanced.2015\" and @class=\"full_table\"]"))
        {
            //Console.WriteLine(row.InnerText);
            results = row.InnerText;
        }

        //Clean them up
        values = results.Trim().Split('\n');
        for (int j = 0; j < values.Length; j++)
        {
            values[j] = values[j].Trim();
            Console.WriteLine("Column {0} = {1}", j, values[j]);
        }

        Console.WriteLine("{0} columns", values.Length);

        //Add them to database
        command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "replace into AdvancedStats SET playerID='" + playerID + "', Season='" + values[0].Replace("&nbsp;", "") + "', PER='" + values[7] + "', TSPCT='" + values[8] + "', 3PAr='" + values[9] + "', FTr='" + values[10] + "', ORBPCT='" + values[11] + "', DRBPCT='" + values[12] + "', TRBPCT='" + values[13] + "', ASTPCT='" + values[14] + "', STLPCT='" + values[15] + "', BLKPCT='" + values[16] + "', TOVPCT='" + values[17] + "', USGPCT='" + values[18] + "', OWS='" + values[20] + "', DWS='" + values[21] + "', WS='" + values[22] + "', WS48='" + values[23] + "', OBPM='" + values[25] + "', DBPM='" + values[26] + "'";
        i = command.ExecuteNonQuery();

        //Confirmation
        Console.WriteLine("{0} rows updated.", i);

        //Get career advanced stats
        foreach (HtmlNode row in doc.DocumentNode.SelectNodes("//*[@id=\"advanced\"]/tfoot/tr[1]"))
        {
            //Console.WriteLine(row.InnerText);
            results = row.InnerText;
        }

        //Clean them up
        values = results.Trim().Split('\n');
        for (int j = 0; j < values.Length; j++)
        {
            values[j] = values[j].Trim();
            Console.WriteLine("Column {0} = {1}", j, values[j]);
        }

        Console.WriteLine("{0} columns", values.Length);

        //Add them to database
        command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "replace into CareerAdvancedStats SET playerID='" + playerID + "', PER='" + values[7] + "', TSPCT='" + values[8] + "', 3PAr='" + values[9] + "', FTr='" + values[10] + "', ORBPCT='" + values[11] + "', DRBPCT='" + values[12] + "', TRBPCT='" + values[13] + "', ASTPCT='" + values[14] + "', STLPCT='" + values[15] + "', BLKPCT='" + values[16] + "', TOVPCT='" + values[17] + "', USGPCT='" + values[18] + "', OWS='" + values[20] + "', DWS='" + values[21] + "', WS='" + values[22] + "', WS48='" + values[23] + "', OBPM='" + values[25] + "', DBPM='" + values[26] + "'";
        i = command.ExecuteNonQuery();

        //Confirmation
        Console.WriteLine("{0} rows updated.", i);
        Console.ReadLine();

        //Set update day in database to today
        command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "update Player set Updated='" + System.DateTime.Now.Day +"' where playerID='" + playerID + "'";
	i = command.ExecuteNonQuery();

        //Update player articles if it is the right day to do so (any day divisible by 3, or roughly every third day)
        /*if(System.DateTime.Now.Day % 3 == 0)
        {
            updateArticlesForPlayer(player.name, player.playerID);
        }*/
		//For now, just always update
		//updateArticlesForPlayer(player.name, player.playerID);

        //Close connection to DB
        l_DBConn.Close();
	}

    public String StatsFileGenerator(string playerID)
    {

        MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        conn_string.Server = "sethia.cosfzwdmywyo.us-west-2.rds.amazonaws.com";
        conn_string.UserID = "cheetobuv10";
        conn_string.Password = "cheeto10";
        conn_string.Database = "NBAbase";
        MySqlConnection l_DBConn = new MySqlConnection();
        l_DBConn.ConnectionString = conn_string.ToString();

        l_DBConn.Open();

        string jsonString = "{\"" + playerID + "\": {\n";

        MySqlCommand command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        //command.CommandText = "SELECT * FROM Player P, playsFor T WHERE P.playerID='" + playerID + "' AND T.playerID='" + playerID + "'";
        command.CommandText = "SELECT * FROM Player NATURAL JOIN playsFor WHERE playerID='" + playerID + "'";
        MySqlDataReader reader = command.ExecuteReader();
        reader.Read();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            //Console.WriteLine("\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",");
            jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",\n";
        }
        reader.Close();

        jsonString += "\"PerGame\": {\n";

        command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "SELECT * FROM PerGameStats WHERE playerID='" + playerID + "' AND Season='2014-15'";
        reader = command.ExecuteReader();
        reader.Read();
        for (int i = 0; i < reader.FieldCount; i++)
        {
			if(reader.GetName(i).Equals("playerID")){}
			else{
				if (i != (reader.FieldCount - 1))
				{
					//Console.WriteLine("\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",");
					jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",\n";
				}
				else
				{
					jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\"\n";
				}
			}
        }
        reader.Close();

        jsonString += "},\n\"Per36\":{\n";

        command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "SELECT * FROM Per36Stats WHERE playerID='" + playerID + "' AND Season='2014-15'";
        reader = command.ExecuteReader();
        reader.Read();
        for (int i = 0; i < reader.FieldCount; i++)
        {
			if(reader.GetName(i).Equals("playerID")){}
			else{
				if (i != (reader.FieldCount - 1))
				{
					//Console.WriteLine("\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",");
					jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",\n";
				}
				else
				{
					jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\"\n";
				}
			}
        }
        reader.Close();

        jsonString += "},\n\"AdvancedStats\":{\n";

        command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "SELECT * FROM AdvancedStats WHERE playerID='" + playerID + "' AND Season='2014-15'";
        reader = command.ExecuteReader();
        reader.Read();
        for (int i = 0; i < reader.FieldCount; i++)
        {
			if(reader.GetName(i).Equals("playerID")){}
			else{
				if (i != (reader.FieldCount - 1))
				{
					//Console.WriteLine("\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",");
					jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",\n";
				}
				else
				{
					jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\"\n";
				}
			}
        }
        reader.Close();

        jsonString += "},\n\"CareerStats\":{\n";

        command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "SELECT * FROM CareerStats WHERE playerID='" + playerID + "'";
        reader = command.ExecuteReader();
        reader.Read();
        for (int i = 0; i < reader.FieldCount; i++)
        {
			if(reader.GetName(i).Equals("playerID")){}
				else{
				if (i != (reader.FieldCount - 1))
				{
					//Console.WriteLine("\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",");
					jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",\n";
				}
				else
				{
					jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\"\n";
				}
			}
        }
        reader.Close();

        jsonString += "},\n\"CareerAdvancedStats\":{\n";

        command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "SELECT * FROM CareerAdvancedStats WHERE playerID='" + playerID + "'";
        reader = command.ExecuteReader();
        reader.Read();
        for (int i = 0; i < reader.FieldCount; i++)
        {
			if(reader.GetName(i).Equals("playerID")){}
				else{
				if (i != (reader.FieldCount - 1))
				{
					//Console.WriteLine("\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",");
					jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",\n";
				}
				else
				{
					jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\"\n";
				}
			}
        }
        reader.Close();

        jsonString += "}\n}\n}";

        l_DBConn.Close();

        //Console.WriteLine(jsonString);
        //Console.ReadLine();

        return jsonString;
        //return "hello";
    }

    public int UpdateDBMethod(string updateDate, string playerID)
    {
        int updateDay = Int32.Parse(updateDate);

        //Get current day
        int currentDay = System.DateTime.Now.Day;

        //Connect to SQL Database
        MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        conn_string.Server = "sethia.cosfzwdmywyo.us-west-2.rds.amazonaws.com";
        conn_string.UserID = "cheetobuv10";
        conn_string.Password = "cheeto10";
        conn_string.Database = "NBAbase";
        MySqlConnection l_DBConn = new MySqlConnection();
        l_DBConn.ConnectionString = conn_string.ToString();

        l_DBConn.Open();

        MySqlCommand command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "SELECT Updated FROM Player WHERE playerID='" + playerID + "'";
        MySqlDataReader reader = command.ExecuteReader();
        reader.Read();

        int dbUpdateDay = Int32.Parse(reader.GetString(0));

        //How the app should be updated
        int updateOption = 0;

        //if (updateDay == dbUpdateDay) { }
        if (currentDay == dbUpdateDay)
        {
            //Update by pulling stats from the database
            updateOption = 1;
        }
        else
        {
            //Get stats from online, update db, and then grab stats
            updateOption = 2;
        }

        l_DBConn.Close();

        return updateOption;
    }

    public string testService()
    {
        return "Yo yo sup?";
    }

    public string GetPlayerList()
    {
        //Connect to SQL Database
        MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        conn_string.Server = "sethia.cosfzwdmywyo.us-west-2.rds.amazonaws.com";
        conn_string.UserID = "cheetobuv10";
        conn_string.Password = "cheeto10";
        conn_string.Database = "NBAbase";
        MySqlConnection l_DBConn = new MySqlConnection();
        l_DBConn.ConnectionString = conn_string.ToString();

        l_DBConn.Open();

        string jsonString = "{\n";

        MySqlCommand command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        //command.CommandText = "SELECT * FROM Player P, playsFor T WHERE P.playerID='" + playerID + "' AND T.playerID='" + playerID + "'";
        command.CommandText = "SELECT playerID, PlayerName, WebLink FROM Player";
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (i == 0)
                {
                    jsonString += "\"" + reader.GetString(i) + "\":{\n";
                }
                else if (i != reader.FieldCount - 1)
                {
                    jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\",\n";
                }
                else
                {
                    jsonString += "\"" + reader.GetName(i) + "\": \"" + reader.GetString(i) + "\"\n";
                }
            }
            jsonString += "},";
        }

        jsonString = jsonString.Substring(0, jsonString.Length - 1);
        jsonString += "\n}";

        return jsonString;
    }

    public string GetArticles(string playerID)
    {
        //Connect to SQL Database
        MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
        conn_string.Server = "sethia.cosfzwdmywyo.us-west-2.rds.amazonaws.com";
        conn_string.UserID = "cheetobuv10";
        conn_string.Password = "cheeto10";
        conn_string.Database = "NBAbase";
        MySqlConnection l_DBConn = new MySqlConnection();
        l_DBConn.ConnectionString = conn_string.ToString();

        l_DBConn.Open();

        MySqlCommand command = l_DBConn.CreateCommand();
        command.Connection = l_DBConn;
        command.CommandText = "SELECT title, url FROM Articles WHERE playerID='" + playerID + "'";
        MySqlDataReader reader = command.ExecuteReader();
        string jsonString = "{";
        int i = 1;
        while(reader.Read())
        {
            jsonString += "\"" + i + "\":{";
            for (int j = 0; j < reader.FieldCount; j++ )
            {
                if (j != (reader.FieldCount - 1))
                {
                    jsonString += "\"" + reader.GetName(j) + "\": \"" + reader.GetString(j) + "\",\n";
                }
                else
                {
                    jsonString += "\"" + reader.GetName(j) + "\": \"" + reader.GetString(j) + "\"},";
                }
            }
            
            i++;
        }
        jsonString = jsonString.Substring(0, jsonString.Length - 1);
        jsonString += "}";
        return jsonString;
    }

    static double StandardDeviation(double[] valueList)
    {
        double M = 0.0;
        double S = 0.0;
        int k = 1;
        foreach (double value in valueList)
        {
            double tmpM = M;
            M += (value - tmpM) / k;
            S += (value - tmpM) * (value - M);
            k++;
        }
        return Math.Sqrt(S / (k - 1));
    }

    static string getArticleBody(string url)
    {
        try
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);
            Console.WriteLine(url);
            string text = "";
            string stemmed = "";
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//p/text()"))
            {
                text += Regex.Replace(node.InnerText, @"[^\w\s]", " ").ToLower();
            }

            text = Services.StopwordTool.RemoveStopwords(text);

            //Get just the list of words
            string[] toStemSplit = text.Split(
                new char[] { ' ', ',', '.', ')', '(' }, StringSplitOptions.RemoveEmptyEntries);

            //Load the Lemmatizer for English
            ILemmatizer lmtz = new LemmatizerPrebuiltCompact(LemmaSharp.LanguagePrebuilt.English);

            foreach (string word in toStemSplit)
            {
                //Put the word in lower case;
                string wordLower = word.ToLower();
                //Lemmatize the word to get the stem
                string lemma = lmtz.Lemmatize(wordLower);
                //Add it to the output
                stemmed += lemma + " ";
            }

            //Console.WriteLine("The stemmed article\n\n\n" + stemmed);
            return stemmed;
        }
        catch
        {
            return "ERROR";
        }

    }

    static void updateArticlesForPlayer(string name, string id)
    {
            string topics = name;
            string playerID = id;
            int numOfArticles = 15;
            string[] topicsArray = topics.Split('_');
            string results = "";
            List<string> resultTitles = new List<string>();
            List<string> articleBodies = new List<string>();
            List<string> urls = new List<string>();
            try
            {
                string AccountKey = "W9xfglp9hpPxpxgoSsWqXzu7k6f+bqUA9Z+S7QHwwsg";
                //For each user topic
                foreach (string query in topicsArray)
                {
                    // Root URL for bing search API
                    string rootUrl = "https://api.datamarket.azure.com/Bing/Search";
                    //Create a Bing Container per API specifications
                    var bingContainer = new Bing.BingSearchContainer(new Uri(rootUrl));
                    // The market location to use.
                    string location = "en-us";
                    // Add credentials
                    bingContainer.Credentials = new NetworkCredential(AccountKey, AccountKey);
                    // Create the search, getting top 5 results.
                    var newsQuery =
                    bingContainer.News(query, null, location, null, null, null, null, null, null);
                    newsQuery = newsQuery.AddQueryOption("$top", numOfArticles);
                    // Execute search.
                    var newsResults = newsQuery.Execute();
                    //Console.WriteLine("Number of articles: " + newsResults.Count());
                    //Add results to list.
                    foreach (var result in newsResults)
                    {
                        string article = getArticleBody(result.Url);
                        if (!article.Equals("ERROR"))
                        {
                            articleBodies.Add(article);
                            resultTitles.Add(result.Title);
                            urls.Add(result.Url);
                        }
                        else
                        {
                            numOfArticles--;
                        }
                    }
                }
            }
            //Error running the query.
            catch
            {
                results += "Error Processing Request. Please try again";
            }

            Console.WriteLine("Num of articles: {0}", numOfArticles);
            Console.WriteLine("Num of article bodies: {0}", articleBodies.Count);
            

            //Gets the unique words from each of the titles and stores them in a list
            //Also get number of words in each title
            List<string> words = new List<string>();
            int[] numOfWords = new int[numOfArticles];
            for(int i = 0; i < numOfArticles; i++)
            {
                string[] sArray = articleBodies[i].Split(' ');
                numOfWords[i] = sArray.Length;
                foreach(string s2 in sArray)
                {
                    if(!words.Contains(s2))
                    {
                        words.Add(s2);
                        //Console.WriteLine(s2);
                    }
                }
            }

            //Stores the frequency of the occurences of each word
            int[,] frequencyArray = new int[numOfArticles, words.Count];
            Array.Clear(frequencyArray, 0, frequencyArray.Length);

            for (int i = 0; i < numOfArticles; i++ )
            {
                string[] sArray = articleBodies[i].Split(' ');
                foreach (string s2 in sArray)
                {
                    //Location of the word in the array of words
                    int j = words.IndexOf(s2);
                    frequencyArray[i,j]++;
                }
            }

            //Normalized Frequency Array
            double[,] nFrequencyArray = new double[numOfArticles, words.Count];
            for (int i = 0; i < numOfArticles; i++)
            {
                for(int j = 0; j < words.Count; j++)
                {
                    nFrequencyArray[i, j] = ((double) frequencyArray[i, j]) / ((double) numOfWords[i]);
                }
            }

            //Inverse Document Frequency
            double[] idf = new double[words.Count];
            for(int i = 0; i < words.Count; i++)
            {
                //How many documents the current word appears in
                int appearances = 0;
                for(int j = 0; j < numOfArticles; j++)
                {
                    if(frequencyArray[j,i] > 0)
                    {
                        appearances++;
                    }
                }

                idf[i] = (double)(1 + Math.Log((double)numOfArticles/(double)appearances));
            }

            //TF-IDF
            double[,] tfIDF = new double[numOfArticles, words.Count];
            for (int i = 0; i < numOfArticles; i++)
            {
                for(int j = 0; j < words.Count; j++)
                {
                    tfIDF[i, j] = idf[j] * nFrequencyArray[i, j];
                    //Console.WriteLine(tfIDF[i, j]);
                }
            }

            //The location in the title list of each chosen article. The first one is always 0.
            int[] selectedArticles = { -1, -1, -1, -1 };

            //Calculate cosine similarity between all articles, to see which ones are the least similar
            //int leastSimilarArticle = 0;
            //double smallestAngle = 1;
            double[,] cosineArray = new double[numOfArticles,numOfArticles];
            for (int k = 0; k < numOfArticles; k++)
            {
                for (int i = 0; i < numOfArticles; i++)
                {
                    double dotProduct = 0;
                    double length1 = 0;
                    double length2 = 0;

                    for (int j = 0; j < words.Count; j++)
                    {
                        dotProduct += (tfIDF[k, j] * tfIDF[i, j]);
                        length1 += Math.Pow(tfIDF[k, j], 2);
                        length2 += Math.Pow(tfIDF[i, j], 2);
                    }

                    double cosine = dotProduct / (Math.Pow(length1, .5) * Math.Pow(length2, .5));
                    cosineArray[k, i] = Math.Round(cosine, 6);

                }
            }

            //Print out cosine array
            System.IO.StreamWriter file = new System.IO.StreamWriter("C:\\Users\\Sumbhav\\Documents\\Thesis\\cosine.txt");
            for (int i = 0; i < numOfArticles; i++ )
            {
                for(int j = 0; j < numOfArticles; j++)
                {
                    file.Write(cosineArray[j, i].ToString("0.000000") + "\t\t\t");
                }

                file.WriteLine();
            }

            file.WriteLine("\n");

            
            //Get the average similarity of each article to the other articles
            double[] averageSimilarity = new double[numOfArticles];
            Array.Clear(averageSimilarity, 0, averageSimilarity.Length);

            for (int i = 0; i < numOfArticles; i++)
            {
                for(int j = 0; j < numOfArticles; j++)
                {
                    averageSimilarity[i] += cosineArray[j,i];
                }

                averageSimilarity[i] /= numOfArticles;
                file.WriteLine(averageSimilarity[i]);
            }

            //Find the mean and standard deviation of the average similarities
            double mean = averageSimilarity.Average();
            double stDev = StandardDeviation(averageSimilarity);

            //Current selected article
            int selectedArticle = -1;

            //Find and select the articles with a similarity score closest to -2, -1, 0, and 1 standard deviation(s) from the mean
            for (int i = -2; i <= 1; i++ )
            {
                double compareValue = mean + i * stDev;
                //Distance between the compare value and the similarity score of the article being examined
                double distance = 1.0;
                for(int j = 0; j < numOfArticles; j++)
                {
                    if(Math.Abs(averageSimilarity[j] - compareValue) < distance)
                    {
                        distance = Math.Abs(averageSimilarity[j] - compareValue);
                        selectedArticle = j;
                    }
                }

                //Add selected article to final selection of articles
                selectedArticles[i + 2] = selectedArticle;
            }

            file.WriteLine();

            foreach(int i in selectedArticles)
            {
                file.WriteLine(resultTitles[i]);
                file.WriteLine(urls[i] + "\n");
            }
                

            file.WriteLine("\n");

            foreach(string s in resultTitles)
            {
                file.WriteLine(s);
            }
            file.Close();

            //MySql connection set up
            MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
            conn_string.Server = "sethia.cosfzwdmywyo.us-west-2.rds.amazonaws.com";
            conn_string.UserID = "cheetobuv10";
            conn_string.Password = "cheeto10";
            conn_string.Database = "NBAbase";
            MySqlConnection l_DBConn = new MySqlConnection();
            l_DBConn.ConnectionString = conn_string.ToString();

            //Open connection to DB (already open based on where it's being called)
            //l_DBConn.Open();

            //Add articles to database
            MySqlCommand command;
            for (int i = 0; i < selectedArticles.Length; i++)
            {
                command = l_DBConn.CreateCommand();
                command.Connection = l_DBConn;
                command.CommandText = "replace into Articles SET playerID='" + playerID + "', articleNumber=" + (i+1) + ", url='" + urls[selectedArticles[i]] + "', title='" + resultTitles[selectedArticles[i]] + "'";
                command.ExecuteNonQuery();
            }

            //Close connection to DB
            l_DBConn.Close();
            Console.WriteLine("Database updated with articles");

            Console.ReadLine();
    }

}
