using System.Collections.Generic;
namespace Doleance
{
    public  class Constants
    {
        public static string admin = "admin";
        public static string user = "user";
        public static string structadmin= "structadmin";

        public static string EmailPattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        public static IEnumerable<string> qualities = new List<string> {"Etudaint" ,"Enseignant","Fonctionnaire","Autre"};
        
    }
}