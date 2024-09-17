using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetChatDetails
{
    public class GetChatDetailsQueryResult
    {
        public JObject Result { get; set; }
        public bool IsBloked { get; set; }
    }
}
