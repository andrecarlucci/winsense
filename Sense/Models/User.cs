using System;

namespace Sense.Models {
    public class User {
        public int Id { get; set; }
        public string Name { get; set; }

        public User() {
            Name = "";
        }

        public User(int id, string name) {
            Id = id;
            Name = name;
        }

        public string Serialize() {
            return String.Format("{0}|{1}", Id, Name ?? "");
        }

        public static User Deserialize(string userInString) {
            var idName = userInString.Split('|');
            return new User {Id = Convert.ToInt32(idName[0]), Name = idName[1]};
        }
    }
}