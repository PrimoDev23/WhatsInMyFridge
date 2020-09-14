using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Models;
using Xamarin.Forms;

namespace WhatsInMyFridge.Helper
{
    public static class APIHelper
    {
        public static async Task<Food> getFoodFromCode(string Code)
        {
            try
            {
                string url = "https://world.openfoodfacts.org/api/v0/product/BARCODE.json".Replace("BARCODE", Code);

                string json;

                WebRequest request = WebRequest.Create(url);
                using (HttpWebResponse resp = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false))
                {
                    using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                    {
                        json = sr.ReadToEnd();
                    }
                }

                JObject obj = JObject.Parse(json);

                JToken ingredients = obj["ingredients_text_debug"];
                JToken nutri_grade = obj["nutriscore_grade"];
                JToken front_image = obj["image_front_url"];
                JToken brand = obj["brands"];
                JToken name = obj["product_name"];

                return new Food()
                {
                    main_img = ImageSource.FromUri(new Uri(front_image.ToString())),
                    ingredients_string = ingredients.ToString(),
                    Name = name.ToString(),
                    brand = brand.ToString(),
                    nutrition_img_source = ImageSource.FromUri(new Uri(nutri_grade.ToString()))
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
