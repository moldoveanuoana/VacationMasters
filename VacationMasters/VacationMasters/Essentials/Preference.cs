namespace VacationMasters.Essentials
{
    public class Preference
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public Preference(string name,string category)
        {
            Name = name;
            Category = category;
        }
        public Preference()
        {

        }
    }
}
