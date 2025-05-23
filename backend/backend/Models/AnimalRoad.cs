namespace backend.Models
{
    public class AnimalRoad
    {
        public string id_road { get; set; }
        public string id_animal { get; set; }
        public string position_a { get; set; }
        public string position_b { get; set; }
        public string escape_radius { get; set; }
        public string duration { get; set; }
    }
}
