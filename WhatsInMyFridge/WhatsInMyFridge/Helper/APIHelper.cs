using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using ZXing.OneD;

namespace WhatsInMyFridge.Helper
{
    public static class APIHelper
    {

        private static readonly HttpClient client = new HttpClient();

        public static async Task<bool> addRecipeRequest(RecipeModel model)
        {
            try
            {
                RecipeXML newRecipe = new RecipeXML()
                {
                    recipe = model,
                };

                string convertedRecipe = JsonConvert.SerializeObject(newRecipe);

                var response = await client.PostAsync("https://whatsinmyfridge123.herokuapp.com/requests", new StringContent(convertedRecipe, Encoding.UTF8, "application/json"));

                var responseString = await response.Content.ReadAsStringAsync();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }


        public static async Task<List<RecipeModel>> getRecipesFromAPI(ObservableCollection<Food> ingredients)
        {

            if(ingredients.Count < 1)
            {
                return null;
            }

            List<RecipeModel> retVal = new List<RecipeModel>();
            string foodNames = string.Empty;
            //Zutatenliste erstellen
            ingredients.ForEach(y => { foodNames += y.name + ","; });
            foodNames = foodNames.TrimEnd(',');

            string url = $"https://whatsinmyfridge123.herokuapp.com/searchRecipes/{foodNames}";
            
            //TODO: remove
            //string url = $"https://whatsinmyfridge123.herokuapp.com/searchRecipes/Eier,Milch,Pflaumen";
            
            string json;

            WebRequest request = WebRequest.Create(url);
            using (HttpWebResponse resp = (HttpWebResponse)await request.GetResponseAsync().ConfigureAwait(false))
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    json = sr.ReadToEnd();
                }
            }

            try
            {
                List<RecipeModel> avaiableRecipes = JsonConvert.DeserializeObject<List<RecipeModel>>(json);

                if (avaiableRecipes.Count > 0 )
                {
                    return avaiableRecipes;
                }
            }
            catch(Exception)
            {
                
            }

            return null;
        }

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

                if(obj["status_verbose"].ToString() == "product not found")
                {
                    return null;
                }

                JToken products = obj["product"];

                JToken ingredients = products["ingredients_text"];
                JToken nutri_grade = products["nutriscore_grade"];
                JToken front_image = products["image_front_url"];
                JToken brand = products["brands"];
                JToken name = products["product_name"];
                JToken quantity = products["quantity"];

                Food food = new Food()
                {
                    addingType = AddingType.Scanned,
                    name = name != null ? name.ToString() : "",
                    brand = brand != null ? brand.ToString() : "",
                    BarCode = Code
                };

                if (quantity != null)
                {
                    string[] split = quantity.ToString().Replace(",", ".").Split(' ');

                    if (double.TryParse(split[0], out double parsed))
                    {
                        food.unit = split[1];
                        food.amount = parsed;
                    }
                }
                else
                {
                    food.unit = "x";
                }

                //YES sadly this is the only way to not throw any exception...
                if (front_image != null)
                {
                    food.imageUrl = front_image.ToString();
                }

                if(nutri_grade != null)
                {
                    food.nutrition_img_url = $"https://static.openfoodfacts.org/images/misc/nutriscore-{nutri_grade.ToString()}.png";
                }

                return food;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
