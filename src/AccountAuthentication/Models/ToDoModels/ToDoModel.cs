using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAuthentication.Models.ToDoModels
{
    public class ToDoModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
        public string Details { get; set; }
        public bool Done { get; set; }

        public ToDoModel()
        {

        }


    }
}
