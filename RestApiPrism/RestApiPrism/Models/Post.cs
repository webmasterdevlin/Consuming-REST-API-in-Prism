using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace RestApiPrism.Models
{
    public class Post : BindableBase
    {

        public int Id { get; set; }
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }   
        public string Body { get; set; }
    }
}
