using System;

namespace VacationMasters.Essentials
{
    class Package
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Included { get; set; }
        public string Transport { get; set; }
        public double Price { get; set; }
        public double SearchIndexRate { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[] Picture { get; set; }
        

        public Package(string name,string type,string included,string transport,double price,double searchIndexRate,
                               DateTime beginDate,DateTime endDate,byte[] picture)
        {
            Name = name;
            Type = type;
            Included = included;
            Transport = transport;
            Price = price;
            BeginDate = beginDate;
            EndDate = endDate;
            Picture = picture;

        }

        public Package()
        {
            
        }
    }
}
