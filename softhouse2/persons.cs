

namespace XMLconverter

{
    public class person : address {
        public person(bool isNew){
            isNewPerson = isNew;
        }

        public bool isNewPerson {get;set;} = false;
        public string firstName {get;set;} = "";
        public string lastName {get;set;} = "";
        public List<family> Members = new List<family>();
    }

    public class family : address {
        public family(bool isNew){
            isNewMember = isNew;
        }
        
        public bool isNewMember {get;set;} = false;
        public string name {get;set;} = "";
        public string born {get;set;} = "";
    }

    public class address : phone {
        public string street {get;set;} = "";
        public string city {get;set;} = "";
        public string zip {get;set;} = "";
    }

    public class phone {
        public string mobile {get;set;} = "";
        public string landLine {get;set;} = "";
    }
}