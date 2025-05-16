// classes

namespace backend.Models
{
    // user class
    public class User : BusinessObject
    {
        // attributes
        public string name { get; set; }
        public string username { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
    }
}
