using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotNetcoreApiSwagger.Business
{
    public class BusinessManagement : IBusinessManagement
    {
        public BusinessManagement()
        {
            //Get AppSetting from appsettings.json
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            lineNotifyUrl = configuration.GetSection("LineUrl").Value;
            token = configuration.GetSection("Token").Value;
        }

        private string token { get; set; }

        private string lineNotifyUrl { get; set; }

        public string CalculateNumberSeries()
        {
            int[] numberSeries = new int[7];
            numberSeries = RegonitionPattern(ref numberSeries);
            return string.Format("X, 5, 9, 15, 23, Y, Z Result : X = {0}, Y = {1}, Z = {2}"
                ,numberSeries[0], numberSeries[5], numberSeries[6]);
        }

        private int[] RegonitionPattern(ref int[] numberSeries)
        {
            for (int i = 0; i <= numberSeries.Length - 1; i++)
            {
                if (i == 0)
                {
                    numberSeries[0] = 3;
                }
                else
                {
                    int n = i + 1;
                    numberSeries[i] = numberSeries[i - 1] + 2 * (n - 1);
                }
            }
            return numberSeries;
        }

        public bool LineNotifyMessage(string message)
        {
            bool IsSuccess = false;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(lineNotifyUrl);
                var postData = string.Format("message={0}", message);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    IsSuccess = true;
                }
                return IsSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return IsSuccess;
            }
        }
    }
}
