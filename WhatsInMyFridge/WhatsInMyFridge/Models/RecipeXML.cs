using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsInMyFridge.Models
{
    public class RecipeXML
    {
        public string id { get; set; }

        public TimeSpan createdTimestamp { get; set; }

        public TimeSpan updatedTimestamp { get; set; }

        public RecipeModel recipe { get; set; }

        public string status { get; set; }

        public string reason { get; set; }
    }
}
