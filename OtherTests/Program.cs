using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace OtherTests
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            //TestApi();
            //log.Info("Hello logging world!");
            //Console.WriteLine("Hit enter");
            //log.Error("Hello I am an Exception");
            //Console.ReadLine();
            ParseAu480();
             Console.ReadLine();
        }

        private static async void TestApi()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.GetAsync("http://olivecliq.com/ricapi/site/TestAtt/bid/2/tid/429");

            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }

        }
        static void ParseAu480()
        {
            var text = System.IO.File.ReadAllText(@"E:\WriteMe.txt");
            var splitter1 = new char[1] { Convert.ToChar(3) };
            var splitter2 = new string[] { " " };
            var arrayafter1stseperator = text.Split(splitter1, StringSplitOptions.RemoveEmptyEntries);
            string labid = "";
            List<mi_tresult> result = new List<mi_tresult>();
            foreach (string str1 in arrayafter1stseperator)
            {
                try
                {
                    if (str1.Contains("DB") || str1.Contains("DE") || string.IsNullOrEmpty(str1))//skip start and end strings
                        continue;

                    var arrayafter2ndseperator = str1.Substring(0, 40).Split(splitter2, StringSplitOptions.RemoveEmptyEntries);
                    if (arrayafter2ndseperator.Length > 3)
                    {
                        labid = arrayafter2ndseperator[3];

                        string testsandresults = str1.Substring(40);
                        var splitter3 = new string[] { "r", "nr" };
                        var arrayafter3rdseperator = testsandresults.Split(splitter3, StringSplitOptions.None);
                        foreach (string testresultall in arrayafter3rdseperator)
                        {
                            string testresultsingle = testresultall.Replace("E", "").Trim();
                            if (testresultsingle.Length > 1)
                            {
                                string machinetestcode = testresultsingle.Substring(0, 3).Trim();
                                string resultsingle = testresultsingle.Substring(3).Trim();

                                result.Add(new mi_tresult
                                {
                                    BookingID = labid,
                                    AttributeID = machinetestcode,
                                    ClientID = "007",
                                    EnteredBy = "1",
                                    EnteredOn = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"),
                                    MachineName = "1",
                                    Result = resultsingle

                                });
                            }
                        }

                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(str1);
                }
            }


            //foreach(byte a in text)
            //{
            //    Console.Write(a.ToString());
            //    Console.Write('\t');
            //    Console.Write(Convert.ToChar(a).ToString());
            //    Console.Write('\n');
            //    Console.ReadLine();
            //}
            if (result.Count > 0)
            {
                var jsonSerialiser = new JavaScriptSerializer();
              //  var json = jsonSerialiser.Serialize(result);
               // System.IO.File.AppendAllText(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "InterfacingJSON.json"), json);
            }
            Console.WriteLine("Done");
        }
    }
}
