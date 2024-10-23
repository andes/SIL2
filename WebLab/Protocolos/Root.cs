using System.Collections.Generic;

public class Root
    {
        public string resourceType { get; set; }
        public List<Identifier> identifier { get; set; }
        public bool active { get; set; }
        public List<Name> name { get; set; }
        public List<Telecom> telecom { get; set; }
        public string gender { get; set; }
        public string birthDate { get; set; }
        public List<Address> address { get; set; }
    }
