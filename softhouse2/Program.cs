using System;
using System.IO;
using System.Xml;

namespace XMLconverter 
{
    internal class Program {
        static void Main (string[] args)
        {
            //ta emot fil och läs dess rader
            string fileName = args[0];
            string[] lines = readFile(fileName);

            //spara personer i lista
            List<person> listPersons = saveObjects(lines);
            //Skriv ut personerna i XML filen
            writeXMLFile(listPersons);
        }

        static string[] readFile(string fileName) {

            string file = fileName;
            string[] data = File.ReadAllLines(file);

            return data;
        }

        static List<person> saveObjects(string[] rows){
            
            //Lista me alla person objekt
            List<person> personList = new List<person>();

            //Aktiv nyckel, P eller F
            bool personKey = true;
            //Objekt för att spara data
            person p = new person(true);
            family f = new family(true);

            //gå igenom raderna
            foreach(string line in rows){

                //Splitta raden på | för att få kulumnvärden
                string[] columns = line.Split("|");

                if(columns.Length >= 1){
                    //första kolumnnyckel
                    string key = columns.Length >= 1 ? columns[0] : "";

                    switch(key) {
                        case "P":
                            //ny person
                            if(p.isNewPerson == false){

                                //lägg till familjemedlem
                                if(f.isNewMember == false){
                                    p.Members.Add(f);
                                    f = new family(true);
                                }

                                //lägg till i listan
                                personList.Add(p);
                                p = new person(true);
                            }
                            //hämta person
                            p = getPerson(p, columns, columns.Length -1);
                            //Person nyckel P aktiv
                            personKey = true;

                        break;

                        case "T":
                            if(personKey){
                                p = getPhone(p, columns, columns.Length -1);
                            }
                            else
                            {
                                f = getPhone(f, columns, columns.Length -1);
                            }
                        break;

                        case "A":
                            if(personKey){
                                p = getAddress(p, columns, columns.Length -1);
                            }
                            else
                            {
                                f = getAddress(f, columns, columns.Length -1);
                            }
                        break;

                        case "F":
                            if(f.isNewMember == false){

                                //lägg till personen
                                p.Members.Add(f);

                                //skapa ny familjemedlem
                                f = new family(true);
                            }

                            f = getFamily(f, columns, columns.Length -1);
                            //person nyckel F aktiv
                            personKey = false;
                        break;
                    }
                }
            }

            //avsluta med att lägga till
            if(p.isNewPerson == false){
                if(f.isNewMember == false){
                    p.Members.Add(f);
                }
                personList.Add(p);
            }
            return personList;
        }

        static person getPerson(person p, string[] columns, int length){
            
            p.isNewPerson = false;
            //ta data om längen stämmer annars blank
            p.firstName = length >= 1 ? columns[1] : "";
            p.lastName = length == 2 ? columns[2] : "";

            return p;
        }

        static family getFamily(family f, string[]columns, int length){

            f.isNewMember = false;
            f.name = length >= 1 ? columns[1] : "";
            f.born = length == 2 ? columns[2] : "";

            return f;
        }

        static person getPhone(person p, string[] columns, int length){

            p.mobile = length >= 1 ? columns[1] : "";
            p.landLine = length == 2 ? columns[2] : "";

            return p;
        }

        static family getPhone(family f, string[] columns, int length){

            f.mobile = length >= 1 ? columns[1] : "";
            f.landLine = length == 2 ? columns[2] : "";

            return f;
        }

        static person getAddress(person p, string[] columns, int length){

            p.street = length >= 1 ? columns[1] : "";
            p.city = length >= 2 ? columns[2] : "";
            p.zip = length == 3 ? columns[3] : "";

            return p;
        }

        static family getAddress(family f, string[] columns, int length){

            f.street = length >= 1 ? columns[1] : "";
            f.city = length >= 2 ? columns[2] : "";
            f.zip = length == 3 ? columns[3] : "";

            return f;
        }

        static void writeXMLFile(List<person> persons){

            XmlWriterSettings settings = new XmlWriterSettings();
            //Ta bort XML version i början på filen
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;

            //skapar ett XML skrivar objekt
            using(XmlWriter writer = XmlWriter.Create(@"Persons.xml", settings)){
                
                //huvudelement
                writer.WriteStartElement("people");

                //går igenom alla objekt
                foreach(person p in persons){

                    writer.WriteStartElement("person");
                    writeNode("firstname", p.firstName, writer);
                    writeNode("lastname", p.lastName, writer);

                    writer.WriteStartElement("address");
                    writeNode("street", p.street, writer);
                    writeNode("city", p.city, writer);
                    writeNode("zip", p.zip, writer);

                    writer.WriteEndElement();

                    writer.WriteStartElement("phone");
                    writeNode("mobile", p.mobile, writer);
                    writeNode("landline", p.landLine, writer);

                    writer.WriteEndElement();

                    if(p.Members.Count() > 0){
                        foreach(family f in p.Members){

                            writer.WriteStartElement("family");
                            writeNode("name", f.name, writer);
                            writeNode("born", f.born, writer);

                            writer.WriteStartElement("address");
                            writeNode("steet", f.street, writer);
                            writeNode("city", f.city, writer);
                            writeNode("zip", f.zip, writer);

                            writer.WriteEndElement();
                            writer.WriteEndElement();
                        }
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Close();
            }
        }
        static void writeNode(string elementName, string elementData, XmlWriter writer){

            //start element
            writer.WriteStartElement(elementName);
            writer.WriteString(elementData);
            writer.WriteEndElement();

        }
    }
}