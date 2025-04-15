namespace AlexAPI.Library.Locations
{
    public static class LocationHelper
    {
        // Location Class to Hold Name & Coordinates
        public class Location
        {
            public string Name { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public Location(string name, double latitude, double longitude)
            {
                Name = name;
                Latitude = latitude;
                Longitude = longitude;
            }
        }

        // Dictionary for Fast Lookup by Name
        public static Dictionary<string, Location> LocationDictionary
        {
            get
            {
                var locations = new List<Location>();
                locations.AddRange(MediterraneanLocations);
                locations.AddRange(MiddleEastLocations);
                locations.AddRange(IndianOceanLocations);
                locations.AddRange(AsiaLocations);
                locations.AddRange(NorthAmericaLocations);
                locations.AddRange(SouthAmericaLocations);
                locations.AddRange(OceaniaLocations);
                locations.AddRange(CaribbeanLocations);
                locations.AddRange(EuropeanLocations);
                locations.AddRange(CroatiaLocations);
                locations.AddRange(GreeceLocations);
                locations.AddRange(FranceLocations);
                locations.AddRange(ItalyLocations);
                locations.AddRange(SpainLocations);
                locations.AddRange(TurkeyLocations);

                return locations.ToDictionary(loc => loc.Name, loc => loc);
            }
        }

        // Compatibility Methods for Existing API Functions
        public static List<string> MediterraneanLocationNames => MediterraneanLocations.Select(loc => loc.Name).ToList();
        public static List<string> MiddleEastLocationNames => MiddleEastLocations.Select(loc => loc.Name).ToList();
        public static List<string> IndianOceanLocationNames => IndianOceanLocations.Select(loc => loc.Name).ToList();
        public static List<string> AsiaLocationNames => AsiaLocations.Select(loc => loc.Name).ToList();
        public static List<string> NorthAmericaLocationNames => NorthAmericaLocations.Select(loc => loc.Name).ToList();
        public static List<string> SouthAmericaLocationNames => SouthAmericaLocations.Select(loc => loc.Name).ToList();
        public static List<string> OceaniaLocationNames => OceaniaLocations.Select(loc => loc.Name).ToList();
        public static List<string> CaribbeanLocationNames => CaribbeanLocations.Select(loc => loc.Name).ToList();
        public static List<string> EuropeanLocationNames => EuropeanLocations.Select(loc => loc.Name).ToList();

        // Mediterranean Locations
        public static List<Location> MediterraneanLocations => new()
        {
            new Location("Albania", 41.1533, 20.1683),
            new Location("Croatia", 45.1000, 15.2000),
            new Location("Cyprus", 35.1264, 33.4299),
            new Location("South of France", 43.5297, 5.4474),
            new Location("Greece", 39.0742, 21.8243),
            new Location("Italy", 41.8719, 12.5674),
            new Location("Malta", 35.9375, 14.3754),
            new Location("Monaco", 43.7384, 7.4246),
            new Location("Montenegro", 42.7087, 19.3744),
            new Location("Spain", 40.4637, -3.7492),
            new Location("Turkey", 38.9637, 35.2433),
            new Location("Dubrovnik", 42.6507, 18.0944),
            new Location("Split", 43.5081, 16.4402),
            new Location("Trogir", 43.5126, 16.2513),
            new Location("Athens", 37.9838, 23.7275),
            new Location("Epidavros", 37.6370, 23.1598),
            new Location("Nafplion", 37.5671, 22.8050),
            new Location("Pylos", 36.9133, 21.6940),
            new Location("Parga", 39.2871, 20.4058),
            new Location("Patras", 38.2466, 21.7346),
            new Location("Kyparissi", 36.9936, 23.0064),
            new Location("Alghero", 40.5571, 8.3190),
            new Location("Nerano", 40.5824, 14.3522),
            new Location("Propriano", 41.6764, 8.9034),
            new Location("Amalfi", 40.6333, 14.6023),
            new Location("Olbia", 40.9233, 9.4933),
            new Location("Rapallo", 44.3467, 9.2233),
            new Location("Anacapri", 40.5547, 14.2219),
            new Location("Palermo", 38.1157, 13.3615),
            new Location("Ravello", 40.6349, 14.6024),
            new Location("Cagliari", 39.2238, 9.1217),
            new Location("Panarea", 38.7860, 15.0664),
            new Location("Rome", 41.9028, 12.4964),
            new Location("San Remo", 43.8176, 7.7773),
            new Location("Forte Village", 39.0226, 9.0589),
            new Location("Porto Cervo",  41.1156, 9.5175),
            new Location("Santa Margherita Ligure", 44.3566,9.2162),
            new Location("Forte dei Marmi", 43.9813, 10.1741),
            new Location("Porto Pollo", 41.1727, 9.2556),
            new Location("Sorrento", 40.6263, 14.3753),
            new Location("Genoa", 44.4056, 8.9463),
            new Location("Porto Rotondo", 41.0034, 9.5327),
            new Location("Taormina", 37.8530, 15.2876),
            new Location("Ischia", 40.7230, 13.9444),
            new Location("Portofino", 44.2974, 9.2002),
            new Location("Venice", 45.4408, 12.3155),
            new Location("La Spezia", 44.1025, 9.8220),
            new Location("Portovenere", 44.1062, 9.8321),
            new Location("Villasimius", 39.1434, 9.5411),
            new Location("Naples", 40.8522, 14.2681),
            new Location("Positano", 40.6281, 14.4820),
            new Location("Barcelona", 41.3784, 2.1925),
            new Location("Malaga", 36.7213, -4.4214),
            new Location("Valencia", 39.4699, -0.3763),
            new Location("Bodrum", 37.0392, 27.4307),
            new Location("Fethiye", 36.6349, 29.1158),
            new Location("Marmaris", 36.9104, 28.2783),
            new Location("Datça", 36.7372, 27.6745),
            new Location("Göcek Bay", 36.7486, 28.9250),
            new Location("Ekincik", 36.7041, 28.0000),
            new Location("Istanbul", 41.0082, 28.9784),
            new Location("Aegean Islands", 37.5, 26.5),
            new Location("Éze", 43.7443, 7.4272),
            new Location("Amalfi Coast", 40.6341, 14.6022),
            new Location("Brač", 43.3799, 16.6532),
            new Location("Cinque Terre", 44.1194, 9.6820),
            new Location("Cyclades Islands", 36.5, 25.0),
            new Location("Dodecanese Islands", 36.5, 28.0),
            new Location("East Coast Italy", 42.0, 13.0),
            new Location("East Mediterranean", 34.0, 34.0),
            new Location("French Riviera", 43.7102, 7.2620),
            new Location("Ionian Islands", 38.5, 20.5),
            new Location("Korčula", 42.9600, 17.1416),
            new Location("Ligurian Riviera", 44.0, 9.0),
            new Location("Peloponnesus", 37.5, 22.0),
            new Location("Saronic Islands", 37.5, 23.3),
            new Location("Sporades", 39.1, 23.6),
            new Location("Šolta", 43.3966, 16.3112),
            new Location("West Coast Italy", 42.0, 11.5),
            new Location("The Balearics", 39.5, 3.0),
            new Location("West Mediterranean", 37.5, 7.5),
            new Location("Hvar", 43.1743, 16.4417),
            new Location("Mljet", 42.7663, 17.5206),
            new Location("Lastovo Island", 42.0874, 16.8836),
            new Location("Vis", 43.0894, 16.2000),
            new Location("Ajaccio", 41.9199, 8.7386),
            new Location("Antibes", 43.5804, 7.1251),
            new Location("Bonifacio", 41.3894, 9.1600),
            new Location("Calvi", 42.5667, 8.7583),
            new Location("Porto-Vecchio", 41.5901, 9.2810),
            new Location("Villefranche-sur-Mer", 43.7055, 7.3165),
            new Location("St Jean Cap Ferrat", 43.6884, 7.3169),
            new Location("Cannes", 43.5510, 7.0108),
            new Location("Menton", 43.7766, 7.4989),
            new Location("St Tropez", 43.2680, 6.6403),
            new Location("Corsica", 41.9298, 9.1600),
            new Location("Porquerolles", 43.0053, 6.2210),
            new Location("Lérins Islands", 43.5247, 7.0085),
            new Location("Alonissos", 39.1776, 23.7505),
            new Location("Kos", 36.8932, 27.2875),
            new Location("Rhodes Island", 36.4349, 28.2176),
            new Location("Antiparos", 37.1167, 25.1600),
            new Location("Lefkada", 38.7369, 20.7180),
            new Location("Santorini", 36.3932, 25.4615),
            new Location("Corfu", 39.6249, 19.9216),
            new Location("Meganissi", 38.6941, 20.5395),
            new Location("Skiathos", 39.1851, 23.4864),
            new Location("Crete", 35.2401, 24.8093),
            new Location("Milos", 36.7191, 24.4037),
            new Location("Skopelos", 39.1167, 23.6023),
            new Location("Delos", 37.4000, 25.2670),
            new Location("Monemvasia", 36.6714, 23.0500),
            new Location("Spetses", 37.2755, 23.1155),
            new Location("Folegandros", 36.4181, 24.9467),
            new Location("Mykonos", 37.4467, 25.3289),
            new Location("Symi", 36.5919, 27.8506),
            new Location("Halki", 36.2133, 27.9036),
            new Location("Naxos", 37.1000, 25.3700),
            new Location("Syros", 37.4360, 24.9372),
            new Location("Hydra", 37.3369, 23.4872),
            new Location("Paros", 37.0417, 25.1494),
            new Location("Zakynthos", 37.7928, 20.8953),
            new Location("Ios", 36.7218, 25.2798),
            new Location("Paxos", 39.2083, 20.2078),
            new Location("Antipaxos", 39.1667, 20.2333),
            new Location("Kefalonia", 38.2500, 20.5000),
            new Location("Poros", 37.5024, 23.4566),
            new Location("Aeolian Islands", 38.5500, 14.9000),
            new Location("La Maddalena", 41.2167, 9.4000),
            new Location("Sardinia", 40.0000, 9.0000),
            new Location("Capri", 40.5500, 14.2333),
            new Location("Ponza", 40.8938, 12.9644),
            new Location("Sicily", 37.5000, 14.0000),
            new Location("Elba", 42.7984, 10.2703),
            new Location("Procida", 40.7631, 14.0200),
            new Location("Stromboli", 38.7889, 15.2131),
            new Location("Formentera", 38.6953, 1.4363),
            new Location("Mallorca", 39.5696, 2.6502),
            new Location("Ibiza", 38.9067, 1.4336),
            new Location("Menorca", 39.9040, 4.1084),
        };
        // Middle East Locations
        public static List<Location> MiddleEastLocations => new()
        {
            new Location("Dubai", 25.276987, 55.296249),
            new Location("Abu Dhabi", 24.453884, 54.377344),
            new Location("Muscat", 23.5859, 58.4059),
            new Location("Doha", 25.276987, 51.520008),
            new Location("Jeddah", 21.4858, 39.1925),
            new Location("Riyadh", 24.7136, 46.6753),
            new Location("Manama", 26.2285, 50.5861),
            new Location("Kuwait City", 29.3759, 47.9774),
            new Location("Amman", 31.9454, 35.9284)
        };
        // Indian Ocean Locations
        public static List<Location> IndianOceanLocations => new()
        {
            new Location("Maldives", 3.2028, 73.2207),
            new Location("Seychelles", -4.6796, 55.4920),
            new Location("Mauritius", -20.3484, 57.5522),
            new Location("Sri Lanka", 7.8731, 80.7718),
            new Location("Reunion", -21.1151, 55.5364),
            new Location("Tanzania", -6.3690, 34.8888),
            new Location("Pemba Island", -5.2500, 39.7500),
            new Location("Thanda Island", -8.2333, 39.6667),
            new Location("Zanzibar", -6.1659, 39.2026),
            new Location("East Africa", 1.8333, 34.5167),
            new Location("Southern Africa", -26.0000, 24.0000),
            new Location("North Africa", 30.0000, 20.0000),
            new Location("India", 20.5937, 78.9629),
            new Location("Mozambique", -18.6657, 35.5296),
            new Location("South Africa", -30.5595, 22.9375),
        };
        // Asia Locations
        public static List<Location> AsiaLocations => new()
        {
            new Location("Thailand", 15.8700, 100.9925),
            new Location("Vietnam", 14.0583, 108.2772),
            new Location("Malaysia", 4.2105, 101.9758),
            new Location("Singapore", 1.3521, 103.8198),
            new Location("Indonesia", -0.7893, 113.9213),
            new Location("Japan", 36.2048, 138.2529),
            new Location("Philippines", 12.8797, 121.7740),
            new Location("Myanmar (Burma)", 21.9162, 95.9560),
            new Location("Flores", -8.5000, 120.5000),
            new Location("Gam Island", -0.8333, 130.7333),
            new Location("Komodo", -8.5083, 119.4628),
            new Location("Misool Island", -2.0000, 130.0000),
            new Location("Pianemo Island", -0.8333, 130.4333),
            new Location("Raja Ampat", -0.6333, 130.6667),
            new Location("Waigeo Island", -0.3667, 130.7667),
            new Location("Ko Poda", 7.9797, 98.7618),
            new Location("Phi Phi Islands", 7.7333, 98.7667),
            new Location("Phuket", 7.9875, 98.3554),
            new Location("South East Asia", 8.5000, 115.0000),
            new Location("China", 35.8617, 104.1954),
            new Location("Cambodia", 12.5657, 104.9910),
            new Location("Andaman Sea", 12.0000, 95.0000),
        };
        // North America Locations
        public static List<Location> NorthAmericaLocations => new()
        {
            new Location("New York", 40.7128, -74.0060),
            new Location("Miami", 25.7617, -80.1918),
            new Location("Los Angeles", 34.0522, -118.2437),
            new Location("Cancun", 21.1619, -86.8515),
            new Location("Vancouver", 49.2827, -123.1207),
            new Location("San Francisco", 37.7749, -122.4194),
            new Location("Boston", 42.3601, -71.0589),
            new Location("Chicago", 41.8781, -87.6298),
            new Location("Alaska", 64.0000, -150.0000),
            new Location("Maine", 45.2538, -68.9853),
            new Location("Desolation Sound", 50.0667, -124.7167),
            new Location("San Juan Islands", 48.6333, -122.9000),
            new Location("Jervis Inlet", 49.9167, -123.7500),
            new Location("British Columbia", 53.7267, -127.6476),
            new Location("Canada", 56.1304, -106.3468),
            new Location("Vancouver", 49.2827, -123.1207),
            new Location("Miami", 25.7617, -80.1918),
            new Location("New England", 44.0000, -71.0000),
            new Location("USA", 39.7837, -100.4459),
            new Location("Florida", 27.6648, -81.5158),
            new Location("Northeast America", 43.0000, -73.0000),
            new Location("Northwest America", 44.0000, -120.0000),
            new Location("Chesapeake Bay", 37.2588, -76.0463),
            new Location("Gulf Islands", 48.8333, -123.0000),
            new Location("Fort Lauderdale", 26.1221, -80.1434),
            new Location("California", 36.7783, -119.4179),
            new Location("Tampa, Florida", 27.964157, -82.452606),
            new Location("St. Petersburg, Florida", 27.773056, -82.639999),
            new Location("Napa Valley", 38.2975, -122.2869),
            new Location("Baja California", 28.0000, -113.0000),
            new Location("Bocas del Toro Islands", 9.3333, -82.2500),
        };
        // South America Locations
        public static List<Location> SouthAmericaLocations => new()
        {
            new Location("Chile", -35.6751, -71.5429),
            new Location("Easter Island", -27.1125, -109.3490),
            new Location("Angra dos Reis", -23.0067, -44.3189),
            new Location("Brazil", -14.2350, -51.9253),
            new Location("Acapulco", 16.8676, -99.8839),
            new Location("Mexican Riviera", 20.6300, -105.2200),
            new Location("Cancun", 21.1619, -86.8497),
            new Location("Cuba", 21.5218, -77.7812),
            new Location("Galapagos Islands", -0.4000, -90.5000),
            new Location("Guadeloupe", 16.2650, -61.5510),
            new Location("Honduras", 15.2000, -86.2419),
            new Location("Costa Rica", 9.7489, -83.7534),
            new Location("Belize", 17.4988, -88.1958),
            new Location("Panama", 8.537981, -80.782127),
        };
        // Oceania Locations
        public static List<Location> OceaniaLocations => new()
        {
            new Location("Australia", -25.2744, 133.7751),
            new Location("South Australia", -30.0000, 136.0000),
            new Location("Western Australia", -27.0000, 121.0000),
            new Location("Great Barrier Reef", -18.2871, 147.6992),
            new Location("Northern Territory", -19.0000, 133.0000),
            new Location("New Zealand", -40.9006, 174.8860),
            new Location("Auckland", -36.8485, 174.7633),
            new Location("Bay of Islands", -35.2333, 174.1333),
            new Location("Marlborough Sounds", -41.1333, 174.1500),
            new Location("Fiordland", -45.4500, 167.0000),
            new Location("Whitsunday Islands", -20.2500, 149.0000),
            new Location("Sydney", -33.8688, 151.2093),
            new Location("Fraser Island", -25.2333, 153.1333),
            new Location("Pittwater", -33.6267, 151.2767),
            new Location("Hobart, Tasmania", -42.8821, 147.3272),
            new Location("Perth", -31.9505, 115.8605),
            new Location("Hamilton Island", -20.3500, 148.9500),
            new Location("Queensland", -20.917574, 142.702789),
            new Location("Victoria", -37.0000, 144.0000),
            new Location("Melbourne", -37.8136, 144.9631),
            new Location("Cook Islands", -21.2367, -159.7777),
            new Location("Fiji", -17.7134, 178.0650),
            new Location("French Polynesia", -17.6509, -149.4260),
            new Location("New Caledonia", -20.9043, 165.6180),
            new Location("Palau Islands", 7.5000, 134.5000),
            new Location("Papua New Guinea", -6.314993, 143.955550),
            new Location("Solomon Islands", -9.6457, 160.1562),
            new Location("Tahiti", -17.6509, -149.4260),
            new Location("Tonga", -21.178986, -175.198242),
            new Location("Vanuatu", -15.376706, 166.959158),
            new Location("The Kimberley", -17.4316, 125.4334),
            new Location("Bora Bora", -16.5004, -151.7415),
            new Location("Micronesia", 7.4258, 150.5508),
            new Location("French Polynesia", -17.6509, -149.4260)
        };
        // Caribbean Locations
        public static List<Location> CaribbeanLocations => new()
        {
            new Location("Anguilla", 18.2206, -63.0686),
            new Location("Antigua", 17.0608, -61.7964),
            new Location("Bahamas", 25.0343, -77.3963),
            new Location("Barbados", 13.1939, -59.5432),
            new Location("British Virgin Islands", 18.436539, 64.618103),
            new Location("Cayman Islands", 19.5135, -80.5670),
            new Location("Cuba", 21.5218, -77.7812),
            new Location("Dominica", 15.4150, -61.3710),
            new Location("Grenada", 12.2628, -61.6042),
            new Location("Guadeloupe", 16.2650, -61.5510),
            new Location("Haiti", 18.9712, -72.2852),
            new Location("Jamaica", 18.1096, -77.2975),
            new Location("Martinique", 14.6415, -61.0242),
            new Location("Montserrat", 16.7425, -62.1873),
            new Location("Puerto Rico", 18.2208, -66.5901),
            new Location("Saint Martin", 18.0708, -63.0501),
            new Location("St Barts", 17.9000, -62.8333),
            new Location("St Kitts and Nevis", 17.3578, -62.7830),
            new Location("St Lucia", 13.9094, -60.9789),
            new Location("St Vincent and the Grenadines", 13.2500, -61.2000),
            new Location("Trinidad & Tobago", 10.6918, -61.2225),
            new Location("Turks & Caicos Islands", 21.6940, -71.7979),
            new Location("US Virgin Islands", 18.3358, -64.8963),
            new Location("Nassau", 25.0343, -77.3963),
            new Location("Freeport", 26.5417, -78.6417),
            new Location("Trellis Bay", 18.4365, -64.5365),
            new Location("The Exumas", 23.5500, -75.8000),
            new Location("Acklins & Crooked Island", 22.65, -74.00),
            new Location("Bimini", 25.7275, -79.2873),
            new Location("Greater Antilles", 19.8968, -74.8710),
            new Location("Mustique", 12.8750, -61.1833),
            new Location("Leeward Islands", 17.7500, -63.0000),
            new Location("Tobago Cays", 12.6333, -61.3500),
            new Location("Virgin Islands", 18.3358, -64.8963),
            new Location("Windward Islands", 13.9094, -60.9789),
            new Location("Abacos Islands", 26.5500, -77.4500),
            new Location("Andros Islands", 24.3333, -78.0000),
            new Location("Berry Islands", 25.6667, -77.8000),
            new Location("Cat Islands", 24.4167, -75.4833),
            new Location("Compass Cay", 24.3667, -76.5333),
            new Location("Eleuthera", 25.2000, -76.1333),
            new Location("Grand Bahama Islands", 26.5000, -78.5000),
            new Location("Harbour Island", 25.4833, -76.6333),
            new Location("Inagua", 21.0500, -73.4000),
            new Location("Long Island", 23.2500, -75.1000),
            new Location("Mayaguana", 22.3833, -73.0000),
            new Location("Ragged Island", 22.3500, -75.7333),
            new Location("Rum Cay", 23.6667, -74.8333),
            new Location("Shroud Cay", 24.3667, -76.8333),
            new Location("Staniel Cay", 24.1667, -76.4333),
            new Location("Warderick Wells Cay", 24.3667, -76.5333),
            new Location("Anegada Island", 18.7333, -64.3333),
            new Location("Cooper Island", 18.3667, -64.4833),
            new Location("Tortola", 18.4167, -64.5167),
            new Location("Jost Van Dyke", 18.4500, -64.7500),
            new Location("Virgin Gorda", 18.4500, -64.4167),
            new Location("Saba", 17.6333, -63.2333),
            new Location("Sint Eustatius", 17.4833, -62.9833),
            new Location("Bequia", 13.0000, -61.2333),
            new Location("St Croix", 17.7000, -64.7000),
            new Location("St John", 18.3333, -64.7333),
            new Location("St Thomas", 18.3400, -64.9300),
               };
        // European Locations
        public static List<Location> EuropeanLocations => new()
        {
            new Location("United Kingdom", 55.3781, -3.4360),
            new Location("Norway", 60.4720, 8.4689),
            new Location("Iceland", 64.9631, -19.0208),
            new Location("England", 52.3555, -1.1743),
            new Location("Finland", 61.9241, 25.7482),
            new Location("Denmark", 56.2639, 9.5018),
            new Location("Sweden", 60.1282, 18.6435),
            new Location("Alesund", 62.4722, 6.1549),
            new Location("Bergen", 60.3913, 5.3221),
            new Location("Geiranger", 62.1025, 7.2094),
            new Location("Stavanger", 58.9700, 5.7300),
            new Location("Flåm", 60.8615, 7.1110),
            new Location("Scotland", 56.4907, -4.2026),
            new Location("Scandinavia", 63.0, 15.0),
            new Location("Baltic", 59.0, 20.0),
            new Location("Solent", 50.75, -1.25),
            new Location("Channel Islands", 49.4000, -2.3500),
            new Location("Latvia", 56.8796, 24.6032),
            new Location("Ireland", 53.4129, -8.2439),
            new Location("Guernsey", 49.4500, -2.5833),
            new Location("Greenland", 71.7069, 42.6043),
            new Location("Gosport", 50.8005, 1.1410),
            new Location("Brittany", 48.2020, 2.9326),
            new Location("Northern Europe", 62.2786, 12.3402)
                };
        // Croatia Locations
        public static List<Location> CroatiaLocations => new()
        {
            new Location("Croatia", 45.1000, 15.2000),
            new Location("Dubrovnik", 42.6507, 18.0944),
            new Location("Split", 43.5081, 16.4402),
            new Location("Trogir", 43.5126, 16.2513),
            new Location("Korčula", 42.9600, 17.1416),
            new Location("Brač", 43.3799, 16.6532),
            new Location("Hvar", 43.1743, 16.4417),
            new Location("Mljet", 42.7663, 17.5206),
            new Location("Lastovo Island", 42.0874, 16.8836),
            new Location("Vis", 43.0894, 16.2000),
            new Location("Šolta", 43.3966, 16.3112),

                };
        // Greece Locations
        public static List<Location> GreeceLocations => new()
        {
            new Location("Greece", 39.0742, 21.8243),
            new Location("Athens", 37.9838, 23.7275),
            new Location("Epidavros", 37.6370, 23.1598),
            new Location("Nafplion", 37.5671, 22.8050),
            new Location("Pylos", 36.9133, 21.6940),
            new Location("Parga", 39.2871, 20.4058),
            new Location("Patras", 38.2466, 21.7346),
            new Location("Kyparissi", 36.9936, 23.0064),
            new Location("Aegean Islands", 37.5, 26.5),
            new Location("Cyclades Islands", 36.5, 25.0),
            new Location("Dodecanese Islands", 36.5, 28.0),
            new Location("Ionian Islands", 38.5, 20.5),
            new Location("Peloponnesus", 37.5, 22.0),
            new Location("Saronic Islands", 37.5, 23.3),
            new Location("Sporades", 39.1, 23.6),
            new Location("Alonissos", 39.1776, 23.7505),
            new Location("Kos", 36.8932, 27.2875),
            new Location("Rhodes Island", 36.4349, 28.2176),
            new Location("Antiparos", 37.1167, 25.1600),
            new Location("Lefkada", 38.7369, 20.7180),
            new Location("Santorini", 36.3932, 25.4615),
            new Location("Meganissi", 38.6941, 20.5395),
            new Location("Skiathos", 39.1851, 23.4864),
            new Location("Crete", 35.2401, 24.8093),
            new Location("Milos", 36.7191, 24.4037),
            new Location("Skopelos", 39.1167, 23.6023),
            new Location("Monemvasia", 36.6714, 23.0500),
            new Location("Delos", 37.4000, 25.2670),
            new Location("Spetses", 37.2755, 23.1155),
            new Location("Folegandros", 36.4181, 24.9467),
            new Location("Mykonos", 37.4467, 25.3289),
            new Location("Symi", 36.5919, 27.8506),
            new Location("Halki", 36.2133, 27.9036),
            new Location("Naxos", 37.1000, 25.3700),
            new Location("Syros", 37.4360, 24.9372),
            new Location("Hydra", 37.3369, 23.4872),
            new Location("Paros", 37.0417, 25.1494),
            new Location("Zakynthos", 37.7928, 20.8953),
            new Location("Ios", 36.7218, 25.2798),
            new Location("Paxos", 39.2083, 20.2078),
            new Location("Antipaxos", 39.1667, 20.2333),
            new Location("Kefalonia", 38.2500, 20.5000),
            new Location("Poros", 37.5024, 23.4566),
        };
        // France Locations
        public static List<Location> FranceLocations => new()
        {
            new Location("South of France", 43.5297, 5.4474),
            new Location("Monaco", 43.7384, 7.4246),
            new Location("Éze", 43.7443, 7.4272),
            new Location("French Riviera", 43.7102, 7.2620),
            new Location("Ajaccio", 41.9199, 8.7386),
            new Location("Antibes", 43.5804, 7.1251),
            new Location("Bonifacio", 41.3894, 9.1600),
            new Location("Calvi", 42.5667, 8.7583),
            new Location("Porto-Vecchio", 41.5901, 9.2810),
            new Location("Villefranche-sur-Mer", 43.7055, 7.3165),
            new Location("St Jean Cap Ferrat", 43.6884, 7.3169),
            new Location("Cannes", 43.5510, 7.0108),
            new Location("Menton", 43.7766, 7.4989),
            new Location("St Tropez", 43.2680, 6.6403),
            new Location("Corsica", 41.9298, 9.1600),
            new Location("Porquerolles", 43.0053, 6.2210),
            new Location("Lérins Islands", 43.5247, 7.0085),
        };
        // Italy Locations
        public static List<Location> ItalyLocations => new()
        {
            new Location("Italy", 41.8719, 12.5674),
            new Location("Alghero", 40.5571, 8.3190),
            new Location("Nerano", 40.5824, 14.3522),
            new Location("Propriano", 41.6764, 8.9034),
            new Location("Amalfi", 40.6333, 14.6023),
            new Location("Olbia", 40.9233, 9.4933),
            new Location("Rapallo", 44.3467, 9.2233),
            new Location("Anacapri", 40.5547, 14.2219),
            new Location("Palermo", 38.1157, 13.3615),
            new Location("Ravello", 40.6349, 14.6024),
            new Location("Cagliari", 39.2238, 9.1217),
            new Location("Panarea", 38.7860, 15.0664),
            new Location("Rome", 41.9028, 12.4964),
            new Location("San Remo", 43.8176, 7.7773),
            new Location("Forte Village", 39.0226, 9.0589),
            new Location("Porto Cervo", 41.1156, 9.5175),
            new Location("Santa Margherita Ligure", 44.3566, 9.2162),
            new Location("Forte dei Marmi", 43.9813, 10.1741),
            new Location("Porto Pollo", 41.1727, 9.2556),
            new Location("Sorrento", 40.6263, 14.3753),
            new Location("Porto Rotondo", 41.0034, 9.5327),
            new Location("Taormina", 37.8530, 15.2876),
            new Location("Ischia", 40.7230, 13.9444),
            new Location("Portofino", 44.2974, 9.2002),
            new Location("Venice", 45.4408, 12.3155),
            new Location("La Spezia", 44.1025, 9.8220),
            new Location("Portovenere", 44.1062, 9.8321),
            new Location("Villasimius", 39.1434, 9.5411),
            new Location("Naples", 40.8522, 14.2681),
            new Location("Positano", 40.6281, 14.4820),
            new Location("Amalfi Coast", 40.6341, 14.6022),
            new Location("Cinque Terre", 44.1194, 9.6820),
            new Location("East Coast Italy", 42.0, 13.0),
            new Location("Ligurian Riviera", 44.0, 9.0),
            new Location("West Coast Italy", 42.0, 11.5),
            new Location("La Maddalena", 41.2167, 9.4000),
            new Location("Sardinia", 40.0000, 9.0000),
            new Location("Ponza", 40.8938, 12.9644),
            new Location("Sicily", 37.5000, 14.0000),
            new Location("Elba", 42.7984, 10.2703),
            new Location("Procida", 40.7631, 14.0200),
            new Location("Stromboli", 38.7889, 15.2131),
        };
        // Spain Locations
        public static List<Location> SpainLocations => new()
        {
            new Location("Barcelona", 41.3784, 2.1925),
            new Location("Malaga", 36.7213, -4.4214),
            new Location("Valencia", 39.4699, -0.3763),
            new Location("Formentera", 38.6953, 1.4363),
            new Location("Mallorca", 39.5696, 2.6502),
            new Location("Ibiza", 38.9067, 1.4336),
            new Location("Menorca", 39.9040, 4.1084),
            new Location("Costa Brava", 41.9999, 3.1667),
            new Location("The Balearics", 39.5, 3.0)
        };
        // Turkey Locations
        public static List<Location> TurkeyLocations => new()
        {
            new Location("Bodrum", 37.0392, 27.4307),
            new Location("Fethiye", 36.6349, 29.1158),
            new Location("Marmaris", 36.9104, 28.2783),
            new Location("Datça", 36.7372, 27.6745),
            new Location("Göcek Bay", 36.7486, 28.9250),
            new Location("Ekincik", 36.7041, 28.0000),
            new Location("Istanbul", 41.0082, 28.9784),
            new Location("Aegean Islands", 37.5, 26.5)
        };
    };
};