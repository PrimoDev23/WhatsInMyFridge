﻿using Newtonsoft.Json.Linq;
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
                string url = $"https://world.openfoodfacts.org/api/v0/product/{Code}.json";

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

                JToken products = obj["product"];

                JToken ingredients = products["ingredients_text"];
                JToken nutri_grade = products["nutriscore_grade"];
                JToken front_image = products["image_front_url"];
                JToken brand = products["brands"];
                JToken name = products["product_name"];

                string igm = front_image.ToString();
                Uri uri = new Uri(front_image.ToString());

                try
                {
                    ImageSource source = ImageSource.FromUri(uri);
                }catch(Exception ex)
                {

                }

                return new Food()
                {
                    main_img_url = front_image.ToString(),
                    ingredients_string = ingredients.ToString(),
                    Name = name.ToString(),
                    brand = brand.ToString(),
                    nutrition_img_url = $"https://static.openfoodfacts.org/images/misc/nutriscore-{nutri_grade.ToString()}.png"
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
