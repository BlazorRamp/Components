using Sandbox.Client.Common.Models;

namespace Sandbox.Client.Common.Utilities;


public class StaticData
{
   public static readonly List<string> Countries = [
                                                    "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua & Deps", "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bhutan",
                                                    "Bolivia", "Bosnia Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria", "Burkina", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Central African Rep", "Chad", "Chile", "China", "Colombia", "Comoros", "Congo", "Congo {Democratic Rep}",
                                                    "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "East Timor", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Fiji", "Finland", "France",
                                                    "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Greece", "Grenada", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Honduras", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland {Republic}",
                                                    "Israel", "Italy", "Ivory Coast", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea North", "Korea South", "Kosovo", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya",
                                                    "Liechtenstein", "Lithuania", "Luxembourg", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco",
                                                    "Mozambique", "Myanmar, {Burma}", "Namibia", "Nauru", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland",
                                                    "Portugal", "Qatar", "Romania", "Russian Federation", "Rwanda", "St Kitts & Nevis", "St Lucia", "Saint Vincent & the Grenadines", "Samoa", "San Marino", "Sao Tome & Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands",
                                                    "Somalia", "South Africa", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Togo", "Tonga", "Trinidad & Tobago", "Tunisia", "Turkey",
                                                    "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Yemen", "Zambia", "Zimbabwe"
                                                    ];

    public static readonly  List<string> BoysNames = [
                                                        "Liam", "Noah", "Oliver", "James", "Elijah", "William", "Henry", "Lucas", "Benjamin", "Theodore", "Mateo", "Levi", "Sebastian", "Daniel", "Jack", "Michael", "Alexander", "Owen", "Asher", "Samuel",
                                                        "Ethan", "Leo", "Jackson", "Mason", "Ezra", "John", "Hudson", "Luca", "Jonathan", "Wyatt", "David", "Miles", "Luke", "Carter", "Julian", "Grayson", "Logan", "Jayden", "Gabriel", "Thomas",
                                                        "Isaac", "Lincoln", "Christopher", "Dylan", "Maverick", "Josiah", "Elias", "Jaxon", "Caleb", "Nathan"
                                                    ];

    public static readonly List<string> GirlsNames = [
                                                        "Olivia", "Emma", "Charlotte", "Amelia", "Sophia", "Mia", "Isabella", "Ava", "Evelyn", "Luna", "Harper", "Sofia", "Camila", "Eleanor", "Elizabeth", "Violet", "Scarlett", "Emily", "Hazel", "Lily",
                                                        "Gianna", "Aurora", "Penelope", "Aria", "Nora", "Chloe", "Ellie", "Mila", "Avery", "Layla", "Elena", "Maya", "Abigail", "Isla", "Eliana", "Nova", "Ivy", "Grace", "Emilia", "Willow",
                                                        "Zoey", "Naomi", "Stella", "Elena", "Victoria", "Liana", "Chloe", "Paisley", "Elena", "Audrey"
                                                    ];


    public static List<string> FamilyNames = [
                                            "Smith", "Jones", "Taylor", "Brown", "Williams", "Wilson", "Johnson", "Davies", "Robinson", "Wright", "Thompson", "Evans", "Walker", "White", "Roberts", "Green", "Hall", "Thomas", "Clarke", "Jackson",
                                            "Harris", "Lewis", "Martin", "Freeman", "Cooper", "Harrison", "Ward", "Turner", "Martin", "Foster", "Moore", "Clark", "King", "Lee", "Baker", "Hill", "Edwards", "Hughes", "Davis", "Wood",
                                            "Simpson", "Shaw", "Watson", "Richards", "Scott", "Spencer", "Kennedy", "Gough", "Brooks", "Price", "Bennett", "Wood", "Gray", "James", "Adams", "Myers", "Jenkins", "Perry", "Morgan", "Morris",
                                            "Page", "Cook", "Bell", "Murphy", "Bailey", "Kelly", "Cox", "Marshall", "Simpson", "Collins", "Carter", "Miller", "Shaw", "Mitchell", "Holmes", "Mason", "Barker", "Hunt", "Palmer", "Yates",
                                            "Forbes", "Murray", "Owen", "Lloyd", "Reynolds", "Ellis", "Richards", "Griffiths", "Stevens", "Webb", "Hunt", "Davies", "Russell", "Ford", "Phillips", "Ellis", "Marlow", "Howell", "Vaughan", "Bevan"
                                        ];

    public static List<Contact> GetContacts(int numberToGenerate)
    {
        var startDate    = new DateOnly(1900, 1, 1);
        var ednDate      = new DateOnly(2026, 1, 1);
        int dayRange     = ednDate.DayNumber - startDate.DayNumber;
        var countryCount = Countries.Count;
        var boysCount    = BoysNames.Count;
        var girlsCount   = GirlsNames.Count;
        var familyCount  = FamilyNames.Count;
        decimal minRate  = 25.00m;
        decimal maxRate  = 75.00m;


        List<Contact> contacts = new List<Contact>(numberToGenerate);

        for (int index = 0; index < numberToGenerate; index++)
        {            
            var isBoy       = index % 2 == 0;
            var country     = Countries[Random.Shared.Next(countryCount)];
            var dob         = startDate.AddDays(Random.Shared.Next(dayRange));
            var familyName  = FamilyNames[Random.Shared.Next(familyCount)];
            var givenName   = isBoy ? BoysNames[Random.Shared.Next(boysCount)] : GirlsNames[Random.Shared.Next(girlsCount)];
            var title       = isBoy ? "Mr." : "Ms.";
            decimal rate    = Math.Round(minRate + (decimal)Random.Shared.NextDouble() * (maxRate - minRate), 2);

            contacts.Add(new(index + 1, title, givenName, familyName, dob, country, rate));
        }

        return contacts;
    }

}
