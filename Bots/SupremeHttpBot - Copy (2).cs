using HtmlAgilityPack;
using NAudio.Wave;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Speech.Recognition;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Dashboard1.ProfileSection;
using Cookie = OpenQA.Selenium.Cookie;

namespace Dashboard1.Bots
{
    class SupremeHttpBot
    {

        public static List<string> GetCategoryList()
        {
            var time = DateTime.Now.ToShortDateString();
            //Creating Client connection	
            RestClient restClient = new RestClient("https://www.supremenewyork.com/mobile_stock.json?_=${" + time + "}");

            //Creating request to get data from server
            RestRequest restRequest = new RestRequest(Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);
            dynamic dynJson = JsonConvert.DeserializeObject(restResponse.Content);
            List<string> names = new List<string>();
            //string response = "";
            foreach (var item in dynJson)
            {
                if (item.Name == "products_and_categories")
                {
                    var response = item.Value;
                    foreach (var r in item.Value)
                    {
                        names.Add(r.Name);
                    }
                }
            }

            return names;

            // Extracting output data from received response
            //string response = restResponse.Content["sds"];

        }
        static string products = "";
        public static string GetProductsInfo(string keyword, string proxy, string category)
        {
            products = keyword;
            var time = DateTime.Now.ToShortDateString();
            //Creating Client connection	
            RestClient restClient = new RestClient("https://www.supremenewyork.com/mobile_stock.json?_=${" + time + "}");
            try {
                //Creating request to get data from server
                RestRequest restRequest = new RestRequest(Method.GET);
                if (proxy != "" && proxy != "NA")
                {
                    var plen = proxy.Split(':').Length;
                    if (plen > 2)
                    {
                        WebProxy proxy1 = new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1], true);
                        proxy1.Credentials = new NetworkCredential(proxy.Split(':')[2], proxy.Split(':')[3]);
                        //proxy1.UseDefaultCredentials = true;
                        WebRequest.DefaultWebProxy = proxy1;
                        restClient.Proxy = proxy1; // new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1]);

                    }
                    else
                    {
                        restClient.Proxy = new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1]);
                    }
                }
                else
                {
                    restClient.Proxy = new WebProxy();
                }
                // Executing request to server and checking server response to the it
                IRestResponse restResponse = restClient.Execute(restRequest);
                dynamic dynJson = JsonConvert.DeserializeObject(restResponse.Content);
                List<string> names = new List<string>();
                //string response = "";
                foreach (var item in dynJson)
                {
                    if (item.Name == "products_and_categories")
                    {
                        var response = item.Value;
                        foreach (var r in item.Value)
                        {
                            if (r.Name == category || category == "All" || category == "")
                            {
                                foreach (var re in r.Value)
                                {
                                    var keys = keyword.Split(',');
                                    foreach(var k in keys)
                                    {
                                        if (re["name"].ToString().ToLower().Contains(k.ToLower()))
                                        {
                                            var name = re["name"].ToString();
                                            var id = re["id"].ToString();
                                            var image = re["image_url_hi"].ToString();
                                            var newItem = re["new_item"].ToString();
                                            //var category = re["categoey_name"].ToString();
                                            var result = image + "|" + id;
                                            return result;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }

                return "Not Found";
            }
            catch (Exception ex){
                return "Not Found";
            }
            // Extracting output data from received response
            //string response = restResponse.Content["sds"];

        }

        public static string GetProductInfo(int id, string Tsize, string color, string proxy)
        {
            try {
            //Creating Client connection	
            RestClient restClient = new RestClient("https://www.supremenewyork.com/shop/" + id + ".json");
            
            //Creating request to get data from server
            RestRequest restRequest = new RestRequest(Method.GET);

            if (proxy != "" && proxy != "NA")
            {
                var plen = proxy.Split(':').Length;
                if (plen > 2)
                {
                    WebProxy proxy1 = new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1], true);
                    proxy1.Credentials = new NetworkCredential(proxy.Split(':')[2], proxy.Split(':')[3]);
                    //proxy1.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy1;
                    restClient.Proxy = proxy1; // new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1]);

                }
                else
                {
                    restClient.Proxy = new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1]);
                }
            }
            else
            {
                restClient.Proxy = new WebProxy();
            }

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;
            dynamic dynJson = JsonConvert.DeserializeObject(response);
            foreach (var item in dynJson)
            {
                try
                {
                    foreach (var re in item.Value)
                    {
                        if (color == "" || color == "NA" || color == "Random")
                        {
                            var styleId = re["id"].ToString();
                            foreach (var size in re["sizes"])
                            {
                                try
                                {
                                    if(Tsize == "Random")
                                    {
                                        var FinalId = size["id"].ToString();
                                        return FinalId + "|" + styleId;
                                    }
                                    else
                                    {
                                        if (size["name"].ToString().ToLower() == Tsize.ToLower())
                                        {
                                            var FinalId = size["id"].ToString();
                                            return FinalId + "|" + styleId;
                                        }

                                    }
                                }
                                catch
                                {
                                    return "Size Not Found!";
                                }
                            }
                        }
                        else
                        {
                            if (re["name"].ToString().ToLower() == color.ToLower())
                            {
                                var styleId = re["id"].ToString();
                                var count = 0;
                                foreach (var size in re["sizes"])
                                {
                                    count++;
                                    if(re["sizes"].Count < count || Tsize == "Random")
                                    {
                                        var FinalId = size["id"].ToString();
                                        return FinalId + "|" + styleId;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (size["name"].ToString().ToLower() == Tsize.ToLower())
                                            {
                                                var FinalId = size["id"].ToString();
                                                return FinalId + "|" + styleId;
                                            }
                                        }
                                        catch
                                        {
                                            return "Size Not Found!";
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    return "Failed!";
                }
            }

                return "Not Found!";
            }
            catch(Exception ex)
            {

                return ex.Message;
            }
        }

        public static IList<RestResponseCookie> AddToCart(int ID, string size, string color, string profile, string proxy, string site)
        {
            //GetSlugStatus("16033218519c0978f6793f325355de0a");

            RestResponseCookie ck = new RestResponseCookie();
            var FinalId = GetProductInfo(ID, size, color,proxy);
            var s = "";
            var st = "";
            if(FinalId.Split('|').Length > 1) {
                s = FinalId.Split('|')[0];
                st = FinalId.Split('|')[1];
            }
            var url = "https://www.supremenewyork.com/shop/" + ID + "/add.json";
            //var csrf = "tdjOhyLywoot+7kYc2qywEga4cKVmTAgeOKZ1WL25TO5ZkfqRI8S+V4xLLu3QnyFiLta22JZk+MCw18drQNOOg==";
   
            RestClient restClient = new RestClient(url);
            

            RestRequest restRequest = new RestRequest(Method.POST);

            if (proxy != "" && proxy != "NA")
            {
                var plen = proxy.Split(':').Length;
                if (plen > 2)
                {
                    WebProxy proxy1 = new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1], true);
                    proxy1.Credentials = new NetworkCredential(proxy.Split(':')[2], proxy.Split(':')[3]);
                    //proxy1.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy1;
                    restClient.Proxy = proxy1; // new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1]);

                }
                else
                {
                    restClient.Proxy = new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1]);
                }
            }
            else
            {
                restClient.Proxy = new WebProxy();
            }
            //restRequest.AddParameter("utf-8", "%E2%9C%93");
            if(site.ToLower() == "supreme eu")
            {
                restRequest.AddParameter("size", s);
                restRequest.AddParameter("style", st);
            }
            else
            {
                restRequest.AddParameter("s", s);
                restRequest.AddParameter("st", st);

            }
            restRequest.AddParameter("qty", "1");
            //restRequest.AddParameter("X-CSRF-Token", csrf);
            //restRequest.AddParameter("h", "1");
            //restRequest.AddParameter("commit", "add to basket");

            restRequest.AddHeader("User-agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 13_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/80.0.3987.95 Mobile/15E148 Safari/604.1");
            restRequest.AddHeader("Accept-Language", "en-US,en;q=0.5");
            restRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
            restRequest.AddHeader("If-None-Match", "'*'");
            restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restRequest.AddHeader("Origin", "https://www.supremenewyork.com");
            restRequest.AddHeader("DNT", "1");
            restRequest.AddHeader("Connection", "keep-alive");
            restRequest.AddHeader("Referer", "https://www.supremenewyork.com/checkout/");
            restRequest.AddHeader("Pragma", "no-cache");
            restRequest.AddHeader("Cache-Control", "no-cache");
            restRequest.AddHeader("TE", "Trailers");
            //Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);
            if (FinalId == "Add to cart Failed!" || FinalId == "Color Not Found!" || FinalId == "Size Not Found!" || FinalId == "Not Found!")
            {
                ck.Expires = DateTime.Now.AddDays(1);
                ck.Name = "Result";
                ck.Value = FinalId;
                restResponse.Cookies.Add(ck);
                
                return restResponse.Cookies;
            }
            if (FinalId == "Failed!")
            {
                ck.Expires = DateTime.Now.AddDays(1);
                ck.Name = "Result";
                ck.Value = FinalId;
                restResponse.Cookies.Add(ck);
                return restResponse.Cookies;
            }
            string response = restResponse.Content;
            try { 
            dynamic dynJson = JsonConvert.DeserializeObject(response);
            foreach (var item in dynJson)
            {
                if(site.ToLower() == "supreme eu")
                {
                    try
                    {
                            if (item["size_id"] != null || item[0] != "")
                            {
                                var sizes = item["size_id"].ToString();
                                ck.Expires = DateTime.Now.AddDays(1);
                                ck.Name = "Result";
                                ck.Value = "Added to cart";
                                restResponse.Cookies.Add(ck);

                                RestResponseCookie ck1 = new RestResponseCookie();
                                ck1.Expires = DateTime.Now.AddDays(1);
                                ck1.Name = "Size";
                                ck1.Value = sizes;
                                restResponse.Cookies.Add(ck1);

                                //var result =  ProceedToCheckOut(restResponse.Cookies, profile);
                                return restResponse.Cookies;
                            }
                            else
                            {

                            }
                    }
                    catch
                    {

                    }

                }
                else
                {
                    if (item.Name == "success")
                    {
                        if (item.Value == "True")
                        {
                            ck.Expires = DateTime.Now.AddDays(1);
                            ck.Name = "Result";
                            ck.Value = "Added to cart";
                            restResponse.Cookies.Add(ck);

                            RestResponseCookie ck1 = new RestResponseCookie();
                            ck1.Expires = DateTime.Now.AddDays(1);
                            ck1.Name = "Size";
                            ck1.Value = s;
                            restResponse.Cookies.Add(ck1);

                            //var result =  ProceedToCheckOut(restResponse.Cookies, profile);
                            return restResponse.Cookies;
                        }
                        else
                        {
                            ck.Expires = DateTime.Now.AddDays(1);
                            ck.Name = "Result";
                            ck.Value = "Sold Out!";
                            restResponse.Cookies.Add(ck);
                            return restResponse.Cookies;
                        }
                    }

                }

            }
            ck.Expires = DateTime.Now.AddDays(1);
            ck.Name = "Result";
            ck.Value = "Add to cart Failed!";
            restResponse.Cookies.Add(ck);
            return restResponse.Cookies;
            }
            catch(Exception ex)
            {
                var result = ExtractText(response);
                ck.Expires = DateTime.Now.AddDays(1);
                ck.Name = "Result";
                ck.Value = result;
                restResponse.Cookies.Add(ck);
                return restResponse.Cookies;

            }

        }

        public static string ProceedToCheckOut(IList<RestResponseCookie> cookies, string profile, string delay, string proxy, string site="")
        {
            try { 
            //GetSlugStatus();
            var url = "https://www.supremenewyork.com/checkout.json";
            RestClient restClient = new RestClient(url);
            RestRequest restRequest = new RestRequest(Method.POST);
           
            if(proxy != "" && proxy != "NA")
            {
                var plen = proxy.Split(':').Length;
                if(plen > 2) {
                    WebProxy proxy1 = new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1], true);
                    proxy1.Credentials = new NetworkCredential(proxy.Split(':')[2], proxy.Split(':')[3]);
                    //proxy1.UseDefaultCredentials = true;
                    WebRequest.DefaultWebProxy = proxy1;
                    restClient.Proxy = proxy1; // new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1]);

                }
                else {
                    restClient.Proxy = new WebProxy(proxy.Split(':')[0] + ":" + proxy.Split(':')[1]);
                }
            }
            else
            {
                restClient.Proxy = new WebProxy();
            }
            string variant = "";
            foreach (var cook in cookies)
            {
                if (cook.Name == "Result" || cook.Name == "Profile" || cook.Name == "Delay" || cook.Name == "Proxy") {
                }
                else if(cook.Name == "Size")
                {
                    variant = cook.Value;
                }
                else
                {
                    restRequest.AddCookie(cook.Name, cook.Value);
                }
            }

            // GetSiteKey(cookies);

            var sitekey = "6LeWwRkUAAAAAOBsau7KpuC9AV-6J8mhw4AjC3Xz";
            var apiKey = "0204a048b7b2f9bc22f6a6926b254082";

                List<CaptchaData> ss = getCaptchaList();
                var gresponse = "";
                if (ss.Count > 0)
                {
                    gresponse = ss[0].gresponse;
                    deletecaptcha(gresponse);
                }
                else
                {
                    gresponse = Bypasscaptcha(sitekey, apiKey);
                    if (gresponse == "NA")
                    {
                        //System.Threading.Thread.Sleep(3000);
                        do
                        {
                            gresponse = Bypasscaptcha(sitekey, apiKey);
                        } while (gresponse == "NA");
                    }

                }
                if (site.ToLower() == "supreme eu")
                {
                    restRequest = FillSupremeEU(restRequest, gresponse, variant, profile);
                }
                else
                {
                    restRequest = FillSupremeUS(restRequest, gresponse, variant, profile);
                }

            // Executing request to server and checking server response to the it
            System.Threading.Thread.Sleep(Convert.ToInt32(delay));
            IRestResponse restResponse = restClient.Execute(restRequest);
            string response = restResponse.Content;

            dynamic dynJson = JsonConvert.DeserializeObject(response);
            var result = "";
            foreach (var item in dynJson)
            {
                if (item.Name == "status")
                {
                    if(item.Value == "queued")
                    {
                        result = "queued.waiting..";
                    }
                    else {
                    result = item.Value;
                    }
                        //if (result == "failed")
                        //{
                        //    return result;
                        //}
                    //return result;
                    //if (item.Value == "failed")
                    //{
                    //    //return "Checked Out";
                    //}
                    //else
                    //{
                    //    return "Checked Out.";
                    //}
                }
                if(item.Name == "cart")
                {
                    foreach(var v in item.Value)
                    {
                        if(v["in_stock"] == true)
                        {
                        }
                        else
                        {
                            result = "Sold Out!";
                        }
                    }
                }

                if(item.Name == "errors")
                {
                    foreach (var v in item.Value)
                    {
                        if(v.Name == "order") {
                            if(v.Value == "")
                            {

                            }
                            else {
                                result = v.Value;
                            }
                        }
                        if(v.Name == "credit_card")
                        {
                            if (v.Value == "")
                            {

                            }
                            else
                                {
                                    try { 
                                result=  ExtractText(v.Value);
                                    }
                                    catch
                                    {
                                        result = v.Value.ToString();

                                    }
                            }
                        }
                    }

                }

                if(item.Name == "slug")
                {
                    result = "queued:" + item.Value.ToString();
                        saveSlugs(item.Value.ToString(), products);
                    //result = GetSlugStatus(item.Value.ToString());
                }

                try
                {
                    if(item.Name == "page")
                    {
                        result = ExtractText(item.Value.ToString());
                        result = result.Replace("EDIT / VIEW CART SHIPPING / PAYMENT CONFIRMATION", "");
                    }
                }
                catch(Exception ex)
                {

                }
            }

            return result;
            }
            catch(Exception ex)
            {
                return "failed";
            }
        }


        public static void deletecaptcha(string gresponse)
        {
            var filepath = "Data/ProfileData/captchas.csv";
            if (File.Exists(filepath))
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    //foreach (string test in File.ReadLines(filepath))
                    //{
                    String line;
                    var count = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts[0].Trim() == gresponse)
                        {
                            sr.Dispose();
                            string text = File.ReadAllText(filepath);
                            text = text.Replace(line, "").Remove(count, 1);
                            File.WriteAllText(filepath, text);
                            break;
                        }
                        count++;
                    }
                    //}
                }
            }
            else
            {

            }
        }

        public class CaptchaData
        {
            public string gresponse { get; set; }
            public string site { get; set; }
            public string datetime { get; set; }

        }

        public static List<CaptchaData> getCaptchaList()
        {
            var filepath = "Data/ProfileData/captchas.csv";
            List<CaptchaData> ls = new List<CaptchaData>();
            if (File.Exists(filepath))
            {
                using (var reader = new StreamReader(filepath))
                {
                    List<string> listA = new List<string>();
                    List<string> listB = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        CaptchaData obj = new CaptchaData();
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (line == "")
                        {
                        }
                        else
                        {
                            if(DateTime.Now < Convert.ToDateTime(values[2]).AddSeconds(80)) {
                                obj.gresponse = values[0];
                                obj.site = values[1];
                                obj.datetime = values[2];
                                ls.Add(obj);
                            }
                        }
                    }
                    reader.Dispose();
                }
            }
            return ls;

        }

        public static void saveSlugs(string slug, string product)
        {
            var filepath = "Data/ProfileData/slugs.csv";
            if (!System.IO.Directory.Exists("Data/ProfileData"))
                System.IO.Directory.CreateDirectory("Data/ProfileData");//.Dispose();
            if (!System.IO.File.Exists(filepath))
                System.IO.File.Create(filepath).Close();//.Dispose();
            bool result = false;

            using (StreamReader sr = new StreamReader(filepath))
            {                
                    sr.Dispose();
                    var csv = new StringBuilder();

                    //in your loop
                    var newLine = string.Format("{0},{1},{2}",
                        slug, product, DateTime.Now.ToString()
                        );
                    csv.AppendLine(newLine);
                    //after your loop
                    File.AppendAllText(filepath, csv.ToString());
                    result = true;
               
            }
        }


        public static RestRequest FillSupremeUS(RestRequest restRequest, string gresponse, string variant, string profile)
        {
            List<ProfileData> ss = getprofileList(profile);
            //restRequest.AddParameter("store_credit_id", "");
            //restRequest.AddParameter("authenticity_token", '1');
            //restRequest.AddParameter("current_time", DateTime.Now.ToShortTimeString());
            //restRequest.AddParameter("order[bn]", ss[0].firstname + " " + ss[0].lastname);
            restRequest.AddParameter("order[billing_name]", ss[0].firstname + " " + ss[0].lastname);//"anon mous"
            restRequest.AddParameter("cerear", "RMCEAR");
            restRequest.AddParameter("order[email]", ss[0].email); //"anon@mailinator.com"
            restRequest.AddParameter("order[tel]", ss[0].phone); //"999-999-9999"
            restRequest.AddParameter("order[billing_address]", ss[0].address1); //"123 Seurat lane"
            restRequest.AddParameter("order[billing_address_2]", ss[0].apt); // ""
            restRequest.AddParameter("order[billing_country]", ss[0].Country); //"USA"
            restRequest.AddParameter("order[billing_zip]", ss[0].zipcode); //"90210"
            restRequest.AddParameter("order[billing_city]", ss[0].city); //"Beverly Hills"
            restRequest.AddParameter("order[billing_state]", ss[0].state); //"CA"
            restRequest.AddParameter("same_as_billing_address", "1");
            restRequest.AddParameter("store_credit_id", "");
            restRequest.AddParameter("store_address", "1");
            restRequest.AddParameter("cookie-sub", "%7B%22" + variant + "%22%3A1%7D");
            restRequest.AddParameter("credit_card[type]", "credit card");
            restRequest.AddParameter("riearmxa", ss[0].cardnumber); //"9999 9999 9999 9999"
            restRequest.AddParameter("credit_card[month]", ss[0].expiry.Split('/')[0]);
            restRequest.AddParameter("credit_card[year]", ss[0].expiry.Split('/')[1]);
            restRequest.AddParameter("credit_card[meknk]", ss[0].cvv); //"123"
            restRequest.AddParameter("credit_card[vval]", ss[0].cvv);
            restRequest.AddParameter("order[terms]", "0");
            restRequest.AddParameter("order[terms]", "1");
            restRequest.AddParameter("g-recaptcha-response", gresponse);
            restRequest.AddHeader("User-agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 13_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/80.0.3987.95 Mobile/15E148 Safari/604.1");
            restRequest.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            restRequest.AddHeader("Accept-Language", "en-US,en;q=0.5");
            restRequest.AddHeader("DNT", "1");
            restRequest.AddHeader("Connection", "keep-alive");
            restRequest.AddHeader("Upgrade-Insecure-Requests", "1");
            restRequest.AddHeader("Pragma", "no-cache");
            restRequest.AddHeader("Cache-Control", "no-cache");
            restRequest.AddHeader("TE", "Trailers");
            restRequest.AddHeader("If-None-Match", "'*'");
            //restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restRequest.AddHeader("Origin", "https://www.supremenewyork.com");
            restRequest.AddHeader("Referer", "https://www.supremenewyork.com/mobile/");

            return restRequest;
        }

        public static RestRequest FillSupremeEU(RestRequest restRequest, string gresponse, string variant, string profile)
        {
            List<ProfileData> ss = getprofileList(profile);
            //restRequest.AddParameter("store_credit_id", "");
            //restRequest.AddParameter("authenticity_token", '1');
            //restRequest.AddParameter("current_time", DateTime.Now.ToShortTimeString());
            //restRequest.AddParameter("order[bn]", ss[0].firstname + " " + ss[0].lastname);
            restRequest.AddParameter("order[billing_name]", ss[0].firstname + " " + ss[0].lastname);//"anon mous"
            restRequest.AddParameter("order[email]", ss[0].email); //"anon@mailinator.com"
            restRequest.AddParameter("order[tel]", ss[0].phone); //"999-999-9999"
            restRequest.AddParameter("order[billing_address]", ss[0].address1); //"123 Seurat lane"
            restRequest.AddParameter("order[billing_address_2]", ss[0].apt); // ""
            restRequest.AddParameter("order[billing_address_3]", "");
            restRequest.AddParameter("order[billing_city]", ss[0].city); //"Beverly Hills"
            restRequest.AddParameter("order[billing_zip]", ss[0].zipcode); //"90210"
            restRequest.AddParameter("order[billing_country]", ss[0].Billingregion); //"USA"
            restRequest.AddParameter("cardinal_order_no", ""); //5052ece4-0583-4b04-a83c-989525384d60
            restRequest.AddParameter("same_as_billing_address", "1");
            restRequest.AddParameter("store_credit_id", "");
            restRequest.AddParameter("store_address", "1");
            restRequest.AddParameter("credit_card[type]", "visa");
            restRequest.AddParameter("credit_card[cnb]", ss[0].cardnumber); //"9999 9999 9999 9999"
            restRequest.AddParameter("credit_card[month]", ss[0].expiry.Split('/')[0]);
            restRequest.AddParameter("credit_card[year]", ss[0].expiry.Split('/')[1]);
            restRequest.AddParameter("credit_card[ovv]", ss[0].cvv); //"123"
            restRequest.AddParameter("order[terms]", "0");
            restRequest.AddParameter("order[terms]", "1");
            restRequest.AddParameter("g-recaptcha-response", gresponse);
            restRequest.AddParameter("hpcvv", "");
            restRequest.AddParameter("cardinal_id", "1_0401dbeb-f793-440a-b67f-ec11864b550d");


            restRequest.AddHeader("user-agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 13_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/80.0.3987.95 Mobile/15E148 Safari/604.1");
            restRequest.AddHeader("accept", "*/*");
            restRequest.AddHeader("accept-language", "en-IN,en-GB;q=0.9,en-US;q=0.8,en;q=0.7,la;q=0.6,hi;q=0.5");
           
            restRequest.AddHeader("If-None-Match", "'*'");
            //restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restRequest.AddHeader("Origin", "https://www.supremenewyork.com");
            restRequest.AddHeader("Referer", "https://www.supremenewyork.com/checkout/");

            return restRequest;
        }


        public static string Bypasscaptcha(string sitekey, string apiKey)
        {
            var captchaAddress = "https://www.supremenewyork.com/checkout";
            string urlAddress = "https://2captcha.com/in.php?key=" + apiKey + "&method=userrecaptcha&googlekey=" + sitekey + "&pageurl=" + captchaAddress + "";
            RestClient restClient = new RestClient(urlAddress);
            RestRequest restRequest = new RestRequest(Method.GET);
            IRestResponse response = restClient.Execute(restRequest);
            System.Threading.Thread.Sleep(15000);
            var result = "";
            if (response.Content.Split('|').Length > 1)
            {
                var id = response.Content.Split('|')[1];
                result = getCaptchaResponse(id, apiKey);
            }
            else
            {
                result = "NA";
            }
            return result;
        }

        public static string getCaptchaResponse(string id, string apiKey)
        {
            RestClient restClient = new RestClient("https://2captcha.com/res.php?key=" + apiKey + "&action=get&id=" + id + "");
            RestRequest restRequest1 = new RestRequest(Method.GET);
            IRestResponse response1 = restClient.Execute(restRequest1);
            var gresponse = "";
            if (response1.Content.Split('|').Length > 1) {
                gresponse = response1.Content.Split('|')[1];
            }
            else
            {
                //System.Threading.Thread.Sleep(1000);
                gresponse = getCaptchaResponse(id, apiKey);

            }
            return gresponse;
        }

        public static void GetSiteKey(IList<RestResponseCookie> cookies)
        {
            string urlAddress = "https://www.supremenewyork.com/checkout";
            RestClient restClient = new RestClient(urlAddress);
            RestRequest restRequest = new RestRequest(Method.GET);
            foreach (var cook in cookies)
            {
                if (cook.Name == "Result" || cook.Name == "Profile" || cook.Name == "Delay" || cook.Name == "Proxy")
                {
                }
                else if (cook.Name == "Size")
                {
                }
                else
                {
                    restRequest.AddCookie(cook.Name, cook.Value);
                }
            }
            restRequest.AddHeader("User-agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 13_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/80.0.3987.95 Mobile/15E148 Safari/604.1");
            restRequest.AddHeader("Accept", "text/plain");
            restRequest.AddHeader("Content-Type", "text/plain");
            restRequest.AddHeader("Origin", "https://www.supremenewyork.com");
            restRequest.AddHeader("Connection", "keep-alive");
            restRequest.AddHeader("Referer", "https://www.supremenewyork.com/");
            restRequest.AddHeader("Pragma", "no-cache");
            restRequest.AddHeader("Cache-Control", "no-cache");
            //Executing request to server and checking server response to the it
            IRestResponse response = restClient.Execute(restRequest);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                var parsed = Regex.Match(content, "<[^>]+data-sitekey=\"([^\"]+)\"[^>]*>");
                var sitekey = parsed.Value;
                //Stream receiveStream = response.GetResponseStream();
                //StreamReader readStream = null;

                //if (String.IsNullOrWhiteSpace(response.CharacterSet))
                //    readStream = new StreamReader(receiveStream);
                //else
                //    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                //string data = readStream.ReadToEnd();

                //response.Close();
                //readStream.Close();
            }
        }
        public static string GetSlugStatus(string slugId = "16033218519c0978f6793f325355de0a")
        {
            var time = DateTime.Now.ToShortDateString();
            //Creating Client connection	
            RestClient restClient = new RestClient("https://www.supremenewyork.com/checkout/" + slugId + "/status.json");

            //Creating request to get data from server
            RestRequest restRequest = new RestRequest(Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);
            dynamic dynJson = JsonConvert.DeserializeObject(restResponse.Content);
            var result = "";
            foreach (var item in dynJson)
            {
                if (item.Name == "status")
                {
                    if (item.Value == "queued")
                    {
                        result = "queued.waiting..";
                        System.Threading.Thread.Sleep(10000);
                        GetSlugStatus(item.Value.ToString());
                    }
                    else
                    {
                        result = item.Value;
                    }
                }
                if (item.Name == "cart")
                {
                    foreach (var v in item.Value)
                    {
                        if (v["in_stock"] == true)
                        {
                        }
                        else
                        {
                            result = "Sold Out!";
                        }
                    }
                }

                if (item.Name == "errors")
                {
                    foreach (var v in item.Value)
                    {
                        if (v.Name == "order")
                        {
                            if (v.Value == "")
                            {

                            }
                            else
                            {
                                result = v.Value;
                            }
                        }
                        if (v.Name == "credit_card")
                        {
                            if (v.Value == "")
                            {

                            }
                            else
                            {
                                result = ExtractText(v.Value);
                            }
                        }
                    }

                }

                if (item.Name == "slug")
                {
                    result = GetSlugStatus(item.Value.ToString());
                }

                try
                {
                    if (item.Name == "page")
                    {
                        result = ExtractText(item.Value.ToString());
                        result = result.Replace("EDIT / VIEW CART SHIPPING / PAYMENT CONFIRMATION", "");
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return result;

            // Extracting output data from received response
            //string response = restResponse.Content["sds"];

        }

        public static string ExtractText(string html)
        {
            try {
            if (html == null)
            {
                throw new ArgumentNullException("html");
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var chunks = new List<string>();

            foreach (var item in doc.DocumentNode.DescendantNodesAndSelf())
            {
                if (item.NodeType == HtmlNodeType.Text)
                {
                    if (item.InnerText.Trim() != "")
                    {
                        chunks.Add(item.InnerText.Trim());
                    }
                }
            }

                return String.Join(" ", chunks);
            }
            catch
            {
                return "failed";
            }
        }


        public static string initiateDriver(IList<RestResponseCookie> cookies, string pr, string delay, string captcha, string proxy)
        {
            try { 
            List<Cookie> lst = new List<Cookie>();
            foreach (var cook in cookies)
            {
                Cookie ss = new Cookie(cook.Name, cook.Value, "www.supremenewyork.com", "/", DateTime.Now.AddDays(1));
                lst.Add(ss);
            }

            IWebDriver driver;

            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            var useragents = new List<string> { "Mozilla/5.0 (Windows NT 4.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2049.0 Safari/537.36", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.517 Safari/537.36", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_2) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1309.0 Safari/537.17" };
            int index = new Random().Next(useragents.Count);
            var name = useragents[index];
            useragents.RemoveAt(index);

            ChromeOptions option = new ChromeOptions();


            //option.AddArgument("--ignore-certificate-errors");
            option.AddArguments("--window-size=600,800");
            //option.AddArguments("--user-data-dir=C:/Users/intel/AppData/Local/Google/Chrome/User Data");

            option.AddArguments("--user-agent='" + name + "'");
            //option.AddArguments("--headless");
            //option.PageLoadStrategy = PageLoadStrategy.Normal;
            
            driver = new ChromeDriver(service, option);
            var _cookies = driver.Manage().Cookies.AllCookies;
            driver.Manage().Cookies.DeleteAllCookies();

            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2000));

            driver.Navigate().GoToUrl("https://www.supremenewyork.com/");
            foreach (Cookie cookie in lst)
            {
                driver.Manage().Cookies.AddCookie(cookie);
            }
            //option.AddArguments("--headless");
            //option.PageLoadStrategy = PageLoadStrategy.Normal;

            driver.Navigate().GoToUrl("https://www.supremenewyork.com/checkout");
            var result = "";
            try
            {
                result = fillDelivery(driver, pr, captcha, delay);
            }catch(Exception ex)
            {
                result = ex.Message;
                driver.Quit();
            }
            return result;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }


        public static string  fillDelivery(IWebDriver driver, string pr, string captcha, string delay)
        {
            //Add Delivery Info
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3000));

            //ProfileSection s = new ProfileSection();
            List<ProfileData> ss = getprofileList(pr);

            //System.Threading.Thread.Sleep(3000);
            try
            {
                wait.Until(ExpectedConditions.ElementExists(By.Id("order_billing_name")));
                driver.FindElement(By.Id("order_billing_name")).SendKeys(ss[0].firstname + " " + ss[0].lastname);
            }
            catch { }
            driver.FindElement(By.Id("order_email")).SendKeys(ss[0].email);

            driver.FindElement(By.Id("order_tel")).SendKeys(ss[0].phone);
            driver.FindElement(By.Id("order_billing_zip")).SendKeys(ss[0].zipcode);

            driver.FindElement(By.Id("order_billing_state")).SendKeys(ss[0].state);

            driver.FindElement(By.Id("order_billing_city")).Clear();
            driver.FindElement(By.Id("order_billing_city")).SendKeys(ss[0].city);

            try
            {
                driver.FindElement(By.Id("bo")).SendKeys(ss[0].address1);
            }
            catch { }
            try
            {
                driver.FindElement(By.Id("order_billing_address")).SendKeys(ss[0].address1);
            }
            catch { }
            try
            {
                driver.FindElement(By.Id("oba3")).SendKeys(ss[0].apt);
            }
            catch { }
            var month = ss[0].expiry.Split('/')[0].ToString().Trim();
            var year = ss[0].expiry.Split('/')[1].ToString().Trim();

            SelectElement oSelect = new SelectElement(driver.FindElement(By.Id("credit_card_month")));

            oSelect.SelectByText(month);

            //driver.FindElement(By.Id("credit_card_month")).SendKeys(month);

            driver.FindElement(By.Id("credit_card_year")).SendKeys(year);

            enterCreditCardNumber(ss[0].cardnumber, driver);
            try
            {
                driver.FindElement(By.Id("orcer")).SendKeys(ss[0].cvv);
            }
            catch { }
            try
            {
                driver.FindElement(By.Id("vval")).SendKeys(ss[0].cvv);
            }
            catch { }

            try
            {
                driver.FindElement(By.XPath("//*[@id='cart-cc']/fieldset/p/label/div/ins")).Click();
            }
            catch { }
            try
            {
                driver.FindElement(By.Id("order_terms")).Click();
            }
            catch { }
            try
            {
                driver.FindElement(By.Name("commit")).Click();
            }
            catch { }
            try
            {
                driver.FindElement(By.Name("checkout")).Click();
            }
            catch { }

            if(captcha.ToLower() == "true")
            {
                //wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//iframe[starts-with(@name, 'a-') and starts-with(@src, 'https://www.google.com/recaptcha')]")));
                //var s2 = driver.FindElement(By.XPath("//iframe[starts-with(@name, 'a-') and starts-with(@src, 'https://www.google.com/recaptcha')]")).Text;

                try
                {
                    driver.SwitchTo().Frame(driver.FindElement(By.XPath("/html/body/div[3]/div[2]/iframe")));
                }
                catch { }

                try
                {
                    driver.SwitchTo().Frame(driver.FindElement(By.XPath("/html/body/div[2]/div[2]/iframe")));
                }
                catch { }

                try
                {
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("recaptcha-audio-button")));
                    driver.FindElement(By.Id("recaptcha-audio-button")).Click();
                    var text1 = getCaptchaText();
                    System.Threading.Thread.Sleep(Convert.ToInt32(delay));
                    driver.FindElement(By.Id("audio-response")).SendKeys(text1);
                    driver.FindElement(By.Id("recaptcha-verify-button")).Click();

                    var error = driver.FindElement(By.ClassName("rc-audiochallenge-error-message")).Text;
                    int count = 0;
                    do
                    {
                        count++;
                        var text = getCaptchaText(count.ToString());
                        System.Threading.Thread.Sleep(Convert.ToInt32(delay));
                        driver.FindElement(By.Id("audio-response")).SendKeys(text);
                        driver.FindElement(By.Id("recaptcha-verify-button")).Click();
                        error = driver.FindElement(By.ClassName("rc-audiochallenge-error-message")).Text;
                    } while (error != "");
                    var result = driver.FindElement(By.XPath("//*[@id='confirmation']/p[1]")).Text;
                    driver.Quit();
                    return result;
                }
                catch (Exception ex)
                {
                    //StartCheckout(global_sz, global_pr, global_url, global_proxy);
                    var result = driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[2]/div")).Text;
                    driver.Quit();
                    return result;

                }
            }
            else
            {
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='confirmation']/p[1]")));
                //driver.FindElement(By.Name("commit")).Click();
                var result = driver.FindElement(By.XPath("//*[@id='confirmation']/p[1]")).Text;
                driver.Quit();
                return result;
            }


        }

        public static void enterCreditCardNumber(string cardNumber, IWebDriver driver)
        {
            for (int i = 0; i < cardNumber.Length; i++)
            {
                char c = cardNumber[i];
                String s = new StringBuilder().Append(c).ToString();
                try
                {
                    driver.FindElement(By.Id("rnsnckrn")).SendKeys(s);
                }
                catch { }
                try
                {
                    driver.FindElement(By.Id("cnb")).SendKeys(s);
                }
                catch { }
            }
        }

        public static string getCaptchaText(string co = "", IWebDriver driver = null)
        {
            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20000));
            try
            {
                // wait.Until(ExpectedConditions.ElementExists(By.Id("audio-source")));
                var asource = driver.FindElement(By.Id("audio-source")).GetAttribute("src");

                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(asource, @"Data/captcha" + co + ".mp3");
                }

                return getPassCode("Data/captcha" + co + ".mp3");
            }
            catch(Exception ex)
            {
                driver.Quit();
                return ex.Message;
            }
        }

        public static string getPassCode(string path)
        {
            ConvertMp3ToWav(path, path.Replace(".mp3", ".wav"));
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
            Grammar gr = new DictationGrammar();
            sre.LoadGrammar(gr);

            sre.SetInputToWaveFile(path.Replace(".mp3", ".wav"));
            sre.BabbleTimeout = new TimeSpan(Int32.MaxValue);
            sre.InitialSilenceTimeout = new TimeSpan(Int32.MaxValue);
            sre.EndSilenceTimeout = new TimeSpan(100000000);
            sre.EndSilenceTimeoutAmbiguous = new TimeSpan(100000000);

            StringBuilder sb = new StringBuilder();
            while (true)
            {
                try
                {
                    var recText = sre.Recognize();
                    if (recText == null)
                    {
                        break;
                    }

                    sb.Append(recText.Text);
                }
                catch (Exception ex)
                {
                    //handle exception      
                    //...

                    break;
                }
            }
            return sb.ToString();
        }

        private static void ConvertMp3ToWav(string _inPath_, string _outPath_)
        {
            using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
            {
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(_outPath_, pcm);
                }
            }
        }

    }
}
