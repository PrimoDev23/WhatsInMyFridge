using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using WhatsInMyFridge.Models;

namespace WhatsInMyFridge.Helper
{
    public static class SaveHandler
    {
        public static void saveFood(ObservableCollection<Food> items)
        {
            using (FileStream fs = new FileStream(VarContainer.food_path, FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Food>));
                xml.Serialize(fs, new ObservableCollection<Food>(items));
            }
        }
        
        public static ObservableCollection<Food> readFood()
        {
            if (!File.Exists(VarContainer.food_path))
            {
                return new ObservableCollection<Food>();
            }

            using (FileStream fs = new FileStream(VarContainer.food_path, FileMode.Open))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Food>));
                return (ObservableCollection<Food>)xml.Deserialize(fs);
            }
        }
    }
}
