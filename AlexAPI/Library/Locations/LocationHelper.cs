using System.Collections.Generic;
using AlexAPI.Models;


namespace AlexAPI.Library.Locations
{
    public static class LocationHelper
    {
        public static List<Location> MediterraneanLocations
        {
            get
            {
                return new List<Location>
                {

                    new Location { Name = "Menorca", Latitude = 40.0156m, Longitude = 4.1550m },
                    new Location { Name = "Ibiza", Latitude = 38.9067m, Longitude = 1.4200m },
                    new Location { Name = "Mallorca", Latitude = 39.6953m, Longitude = 3.0176m },
                    new Location { Name = "Formentera", Latitude = 38.7111m, Longitude = 1.4583m },
                    new Location { Name = "Stromboli", Latitude = 38.7891m, Longitude = 15.2136m },
                    new Location { Name = "Procida", Latitude = 40.7500m, Longitude = 14.0000m },
                    new Location { Name = "Elba", Latitude = 42.7650m, Longitude = 10.2389m },
                    new Location { Name = "Sicily", Latitude = 37.6000m, Longitude = 13.4000m },
                    new Location { Name = "Ponza", Latitude = 40.9136m, Longitude = 13.1778m },
                    new Location { Name = "Capri", Latitude = 40.5500m, Longitude = 14.2500m },
                    new Location { Name = "Sardinia", Latitude = 40.1200m, Longitude = 9.0100m },
                    new Location { Name = "La Maddalena", Latitude = 41.2146m, Longitude = 9.3949m },
                    new Location { Name = "Aeolian Islands", Latitude = 38.5000m, Longitude = 14.3000m },
                    new Location { Name = "Poros", Latitude = 37.5036m, Longitude = 23.4825m },
                    new Location { Name = "Kefalonia", Latitude = 38.1900m, Longitude = 20.6400m },
                    new Location { Name = "Antipaxos", Latitude = 39.2423m, Longitude = 20.2458m },
                    new Location { Name = "Paxos", Latitude = 39.2100m, Longitude = 20.1000m },
                    new Location { Name = "Ios", Latitude = 36.7200m, Longitude = 25.3670m },
                    new Location { Name = "Zakynthos", Latitude = 37.7833m, Longitude = 20.4333m },
                    new Location { Name = "Paros", Latitude = 37.0667m, Longitude = 25.1500m },
                    new Location { Name = "Hydra", Latitude = 37.3500m, Longitude = 23.4500m },
                    new Location { Name = "Syros", Latitude = 37.4367m, Longitude = 24.9444m },
                    new Location { Name = "Naxos", Latitude = 37.1000m, Longitude = 25.4000m },
                    new Location { Name = "Halki", Latitude = 36.3000m, Longitude = 28.2000m },
                    new Location { Name = "Symi", Latitude = 36.6000m, Longitude = 27.9000m },
                    new Location { Name = "Mykonos", Latitude = 37.4450m, Longitude = 25.3275m },
                    new Location { Name = "Folegandros", Latitude = 36.4450m, Longitude = 24.7650m },
                    new Location { Name = "Spetses", Latitude = 37.2500m, Longitude = 23.0333m },
                    new Location { Name = "Monemvasia", Latitude = 36.6964m, Longitude = 23.0478m },
                    new Location { Name = "Delos", Latitude = 37.4000m, Longitude = 25.2670m },
                    new Location { Name = "Skopelos", Latitude = 39.1000m, Longitude = 23.6000m },
                    new Location { Name = "Milos", Latitude = 36.7200m, Longitude = 24.4600m },
                    new Location { Name = "Crete", Latitude = 35.2400m, Longitude = 24.8090m },
                    new Location { Name = "Skiathos", Latitude = 39.1350m, Longitude = 23.4930m },
                    new Location { Name = "Meganissi", Latitude = 38.6800m, Longitude = 20.6370m },
                    new Location { Name = "Corfu", Latitude = 39.6245m, Longitude = 19.9215m },
                    new Location { Name = "Santorini", Latitude = 36.3933m, Longitude = 25.4611m },
                    new Location { Name = "Lefkada", Latitude = 38.7000m, Longitude = 20.6000m },
                    new Location { Name = "Antiparos", Latitude = 37.1000m, Longitude = 25.0000m },
                    new Location { Name = "Rhodes Island", Latitude = 36.4349m, Longitude = 28.2176m },
                    new Location { Name = "Kos", Latitude = 36.8936m, Longitude = 27.2897m },
                    new Location { Name = "Alonissos", Latitude = 39.1900m, Longitude = 23.8590m },
                    new Location { Name = "Lérins Islands", Latitude = 43.5500m, Longitude = 7.0167m },
                    new Location { Name = "Porquerolles", Latitude = 43.0100m, Longitude = 6.2250m },
                    new Location { Name = "Corsica", Latitude = 42.0000m, Longitude = 9.0000m },
                    new Location { Name = "St Tropez", Latitude = 43.2670m, Longitude = 6.6330m },
                    new Location { Name = "Menton", Latitude = 43.7667m, Longitude = 7.5000m },
                    new Location { Name = "Cannes", Latitude = 43.5528m, Longitude = 7.0176m },
                    new Location { Name = "St Jean Cap Ferrat", Latitude = 43.6886m, Longitude = 7.3181m },
                    new Location { Name = "Villefranche-sur-Mer", Latitude = 43.7000m, Longitude = 7.4167m },
                    new Location { Name = "Porto-Vecchio", Latitude = 41.5500m, Longitude = 9.2833m },
                    new Location { Name = "Calvi", Latitude = 42.5833m, Longitude = 8.7500m },
                    new Location { Name = "Bonifacio", Latitude = 41.3833m, Longitude = 9.1500m },
                    new Location { Name = "Antibes", Latitude = 43.5800m, Longitude = 7.1250m },
                    new Location { Name = "Ajaccio", Latitude = 41.9190m, Longitude = 8.7390m },
                    new Location { Name = "Vis", Latitude = 43.0833m, Longitude = 16.1667m },
                    new Location { Name = "Lastovo Island", Latitude = 42.7600m, Longitude = 16.9000m },
                    new Location { Name = "Mljet", Latitude = 42.7500m, Longitude = 17.4667m },
                    new Location { Name = "Hvar", Latitude = 43.1667m, Longitude = 16.6000m },
                    new Location { Name = "West Mediterranean", Latitude = 38.5000m, Longitude = 7.0000m },
                    new Location { Name = "The Balearics", Latitude = 39.5423m, Longitude = 3.0100m },
                    new Location { Name = "West Coast Italy", Latitude = 42.0000m, Longitude = 10.0000m },
                    new Location { Name = "Šolta", Latitude = 43.4333m, Longitude = 16.5167m },
                    new Location { Name = "Sporades", Latitude = 39.0750m, Longitude = 23.5467m },
                    new Location { Name = "Saronic Islands", Latitude = 37.5000m, Longitude = 23.2500m },
                    new Location { Name = "Peloponnesus", Latitude = 37.5000m, Longitude = 22.5000m },
                    new Location { Name = "Ligurian Riviera", Latitude = 44.0000m, Longitude = 9.0000m },
                    new Location { Name = "Korčula", Latitude = 42.9667m, Longitude = 17.1333m },
                    new Location { Name = "Ionian Islands", Latitude = 38.0000m, Longitude = 20.0000m },
                    new Location { Name = "French Riviera", Latitude = 43.5000m, Longitude = 7.0000m },
                    new Location { Name = "East Mediterranean", Latitude = 34.9000m, Longitude = 32.0000m },
                    new Location { Name = "East Coast Italy", Latitude = 41.0000m, Longitude = 13.0000m },
                    new Location { Name = "Cyclades Islands", Latitude = 37.0000m, Longitude = 25.0000m },
                    new Location { Name = "Cinque Terre", Latitude = 44.1200m, Longitude = 9.6333m },
                    new Location { Name = "Brač", Latitude = 43.3833m, Longitude = 16.6000m },
                    new Location { Name = "Amalfi Coast", Latitude = 40.6300m, Longitude = 14.6022m },
                    new Location { Name = "Éze", Latitude = 43.7467m, Longitude = 7.3486m },
                    new Location { Name = "Aegean Island", Latitude = 37.5000m, Longitude = 25.5000m },
                    new Location { Name = "Istanbul", Latitude = 41.0082m, Longitude = 28.9784m },
                    new Location { Name = "Ekincik", Latitude = 36.7419m, Longitude = 28.0900m },
                    new Location { Name = "Göcek Bay", Latitude = 36.7472m, Longitude = 28.9250m },
                    new Location { Name = "Datça", Latitude = 36.7367m, Longitude = 28.0047m },
                    new Location { Name = "Marmaris", Latitude = 36.9000m, Longitude = 28.3000m },
                    new Location { Name = "Fethiye", Latitude = 36.6433m, Longitude = 29.1250m },
                    new Location { Name = "Bodrum", Latitude = 37.0395m, Longitude = 27.4385m },
                    new Location { Name = "Valencia", Latitude = 39.4702m, Longitude = -0.3763m },
                    new Location { Name = "Malaga", Latitude = 36.7213m, Longitude = -4.4213m },
                    new Location { Name = "Barcelona", Latitude = 41.3784m, Longitude = 2.1915m },
                    new Location { Name = "Positano", Latitude = 40.6281m, Longitude = 14.4802m },
                    new Location { Name = "Naples", Latitude = 40.8522m, Longitude = 14.2681m },
                    new Location { Name = "Villasimius", Latitude = 39.1233m, Longitude = 9.5411m },
                    new Location { Name = "Portovenere", Latitude = 44.1072m, Longitude = 9.8328m },
                    new Location { Name = "La Spezia", Latitude = 44.1025m, Longitude = 9.8238m },
                    new Location { Name = "Venice", Latitude = 45.4408m, Longitude = 12.3155m },
                    new Location { Name = "Portofino", Latitude = 44.2950m, Longitude = 9.2125m },
                    new Location { Name = "Ischia", Latitude = 40.7300m, Longitude = 13.9280m },
                    new Location { Name = "Taormina", Latitude = 37.8536m, Longitude = 15.2850m },
                    new Location { Name = "Porto Rotondo", Latitude = 41.0850m, Longitude = 9.5511m },
                    new Location { Name = "Genoa", Latitude = 44.4056m, Longitude = 8.9463m },
                    new Location { Name = "Sorrento", Latitude = 40.6281m, Longitude = 14.3754m },
                    new Location { Name = "Porto Pollo", Latitude = 41.2000m, Longitude = 9.2000m },
                    new Location { Name = "Forte dei Marmi", Latitude = 43.9500m, Longitude = 10.1500m },
                    new Location { Name = "Santa Margherita Ligure", Latitude = 44.3411m, Longitude = 9.2139m },
                    new Location { Name = "Porto Cervo", Latitude = 41.1242m, Longitude = 9.5200m },
                    new Location { Name = "Forte Village", Latitude = 39.0919m, Longitude = 9.0325m },
                    new Location { Name = "San Remo", Latitude = 43.7822m, Longitude = 7.7833m },
                    new Location { Name = "Rome", Latitude = 41.9028m, Longitude = 12.4964m },
                    new Location { Name = "Panarea", Latitude = 38.7814m, Longitude = 15.0744m },
                    new Location { Name = "Cagliari", Latitude = 39.2238m, Longitude = 9.1217m },
                    new Location { Name = "Ravello", Latitude = 40.6472m, Longitude = 14.6028m },
                    new Location { Name = "Palermo", Latitude = 38.1157m, Longitude = 13.3615m },
                    new Location { Name = "Anacapri", Latitude = 40.5530m, Longitude = 14.2258m },
                    new Location { Name = "Rapallo", Latitude = 44.3542m, Longitude = 9.2206m },
                    new Location { Name = "Olbia", Latitude = 40.9231m, Longitude = 9.4958m },
                    new Location { Name = "Amalfi", Latitude = 40.6333m, Longitude = 14.6028m },
                    new Location { Name = "Propriano", Latitude = 41.7600m, Longitude = 8.9333m },
                    new Location { Name = "Nerano", Latitude = 40.6167m, Longitude = 14.3667m },
                    new Location { Name = "Alghero", Latitude = 40.5573m, Longitude = 8.3193m },
                    new Location { Name = "Kyparissi", Latitude = 37.1910m, Longitude = 23.0190m },
                    new Location { Name = "Patras", Latitude = 38.2400m, Longitude = 21.7400m },
                    new Location { Name = "Parga", Latitude = 39.2800m, Longitude = 20.4000m },
                    new Location { Name = "Pylos", Latitude = 36.9378m, Longitude = 21.6511m },
                    new Location { Name = "Nafplion", Latitude = 37.5667m, Longitude = 22.8000m },
                    new Location { Name = "Epidavros", Latitude = 37.6000m, Longitude = 23.0100m },
                    new Location { Name = "Athens", Latitude = 37.9838m, Longitude = 23.7275m },
                    new Location { Name = "Trogir", Latitude = 43.5167m, Longitude = 16.2500m },
                    new Location { Name = "Split", Latitude = 43.5081m, Longitude = 16.4402m },
                    new Location { Name = "Dubrovnik", Latitude = 42.6500m, Longitude = 18.1000m },
                    new Location { Name = "Turkey", Latitude = 38.9637m, Longitude = 35.2433m },
                    new Location { Name = "Spain", Latitude = 40.4637m, Longitude = -3.7492m },
                    new Location { Name = "Montenegro", Latitude = 42.7087m, Longitude = 19.3744m },
                    new Location { Name = "Monaco", Latitude = 43.7333m, Longitude = 7.4167m },
                    new Location { Name = "Malta", Latitude = 35.8997m, Longitude = 14.5147m },
                    new Location { Name = "Italy", Latitude = 41.8719m, Longitude = 12.5674m },
                    new Location { Name = "Greece", Latitude = 39.0742m, Longitude = 21.8243m },
                    new Location { Name = "South of France", Latitude = 43.6115m, Longitude = 3.8767m },
                    new Location { Name = "Cyprus", Latitude = 35.1264m, Longitude = 33.4299m },
                    new Location { Name = "Croatia", Latitude = 45.1m, Longitude = 15.2m },
                    new Location { Name = "Albania", Latitude = 41.1533m, Longitude = 20.1683m },

                };
            }
        }

        public static List<Location> MiddleEastLocations {
            get
            {
                return new List<Location>
                {
                    new Location { Name = "Masirah Island", Latitude = 20.4833m, Longitude = 58.8667m },
                    new Location { Name = "Bani Khalid", Latitude = 22.5667m, Longitude = 58.6833m },
                    new Location { Name = "Manama", Latitude = 26.2167m, Longitude = 50.5833m },
                    new Location { Name = "Red Sea", Latitude = 20.0000m, Longitude = 38.0000m },
                    new Location { Name = "Burj al Arab", Latitude = 25.1412m, Longitude = 55.1853m },
                    new Location { Name = "Musandam Peninsula", Latitude = 26.1500m, Longitude = 56.2500m },
                    new Location { Name = "Byblos", Latitude = 34.1233m, Longitude = 35.6519m },
                    new Location { Name = "Bahrain", Latitude = 26.0667m, Longitude = 50.5577m },
                    new Location { Name = "Arabian Gulf", Latitude = 25.0000m, Longitude = 52.0000m },
                    new Location { Name = "Oman Islands", Latitude = 20.5600m, Longitude = 58.6000m },
                    new Location { Name = "Middle East", Latitude = 29.0000m, Longitude = 45.0000m },
                    new Location { Name = "Amman", Latitude = 31.9497m, Longitude = 35.9329m },
                    new Location { Name = "Kuwait City", Latitude = 29.3759m, Longitude = 47.9774m },
                    new Location { Name = "Riyadh", Latitude = 24.7136m, Longitude = 46.6753m },
                    new Location { Name = "Jeddah", Latitude = 21.4858m, Longitude = 39.1925m },
                    new Location { Name = "Doha", Latitude = 25.276987m, Longitude = 51.520008m },
                    new Location { Name = "Muscat", Latitude = 23.5880m, Longitude = 58.3829m },
                    new Location { Name = "Abu Dhabi", Latitude = 24.4539m, Longitude = 54.3773m },
                    new Location { Name = "Dubai", Latitude = 25.276987m, Longitude = 55.296249m }


                };
            }
        }
 
        public static List<Location> IndianOceanLocations { 
            get
            {
                return new List<Location>
                {
                    new Location { Name = "South Africa", Latitude = -30.5595m, Longitude = 22.9375m },
                    new Location { Name = "Mozambique", Latitude = -18.6657m, Longitude = 35.5296m },
                    new Location { Name = "India", Latitude = 20.5937m, Longitude = 78.9629m },
                    new Location { Name = "North Africa", Latitude = 25.0m, Longitude = 13.0m },
                    new Location { Name = "Southern Africa", Latitude = -22.0m, Longitude = 24.0m },
                    new Location { Name = "East Africa", Latitude = -1.0m, Longitude = 38.0m },
                    new Location { Name = "Zanzibar", Latitude = -6.1659m, Longitude = 39.2026m },
                    new Location { Name = "Thanda Island", Latitude = -9.4200m, Longitude = 39.0800m },
                    new Location { Name = "Pemba Island", Latitude = -4.3111m, Longitude = 39.4750m },
                    new Location { Name = "Tanzania", Latitude = -6.369028m, Longitude = 34.888822m },
                    new Location { Name = "Reunion", Latitude = -21.1151m, Longitude = 55.5364m },
                    new Location { Name = "Sri Lanka", Latitude = 7.8731m, Longitude = 80.7718m },
                    new Location { Name = "Mauritius", Latitude = -20.348404m, Longitude = 57.552152m },
                    new Location { Name = "Seychelles", Latitude = -4.6796m, Longitude = 55.4915m },
                    new Location { Name = "Maldives", Latitude = 3.2028m, Longitude = 73.2207m },



                };
            }
        }

        public static List<Location> AsiaLocations
        {
            get
            {
                return new List<Location>
                {
                    new Location { Name = "Andaman Sea", Latitude = 10.0m, Longitude = 95.0m },
                    new Location { Name = "Cambodia", Latitude = 12.5657m, Longitude = 104.9910m },
                    new Location { Name = "China", Latitude = 35.8617m, Longitude = 104.1954m },
                    new Location { Name = "South East Asia", Latitude = 10.0m, Longitude = 105.0m },
                    new Location { Name = "Phuket", Latitude = 7.8804m, Longitude = 98.3923m },
                    new Location { Name = "Phi Phi Islands", Latitude = 7.7160m, Longitude = 98.7742m },
                    new Location { Name = "Ko Poda", Latitude = 8.0170m, Longitude = 98.5955m },
                    new Location { Name = "Misool Island", Latitude = -1.9110m, Longitude = 130.2065m },
                    new Location { Name = "Komodo", Latitude = -8.5741m, Longitude = 119.5726m },
                    new Location { Name = "Gam Island", Latitude = -0.4069m, Longitude = 131.2494m },
                    new Location { Name = "Flores", Latitude = -8.7360m, Longitude = 121.3146m },
                    new Location { Name = "Myanmar (Burma)", Latitude = 21.9139m, Longitude = 95.9560m },
                    new Location { Name = "Philippines", Latitude = 12.8797m, Longitude = 121.7740m },
                    new Location { Name = "Japan", Latitude = 36.2048m, Longitude = 138.2529m },
                    new Location { Name = "Indonesia", Latitude = -0.7893m, Longitude = 113.9213m },
                    new Location { Name = "Singapore", Latitude = 1.3521m, Longitude = 103.8198m },
                    new Location { Name = "Malaysia", Latitude = 4.2105m, Longitude = 101.9758m },
                    new Location { Name = "Vietnam", Latitude = 14.0583m, Longitude = 108.2772m },
                    new Location { Name = "Thailand", Latitude = 15.8700m, Longitude = 100.9925m },


                };
            }
        }

        public static List<Location> NorthAmericaLocations
        {
            get
            {
                return new List<Location>
                {
                    new Location { Name = "Bocas del Toro Islands", Latitude = 9.3333m, Longitude = -82.2500m },
                    new Location { Name = "Baja California", Latitude = 30.0m, Longitude = -115.0m },
                    new Location { Name = "Napa Valley", Latitude = 38.5025m, Longitude = -122.2654m },
                    new Location { Name = "St. Petersburg", Latitude = 27.7676m, Longitude = -82.6403m },
                    new Location { Name = "Tampa", Latitude = 27.9506m, Longitude = -82.4572m },
                    new Location { Name = "California", Latitude = 36.7783m, Longitude = -119.4179m },
                    new Location { Name = "Fort Lauderdale", Latitude = 26.1223m, Longitude = -80.1434m },
                    new Location { Name = "Gulf Islands", Latitude = 48.6800m, Longitude = -123.4667m },
                    new Location { Name = "Chesapeake Bay", Latitude = 37.0m, Longitude = -76.0m },
                    new Location { Name = "Northwest America", Latitude = 48.0m, Longitude = -123.0m },
                    new Location { Name = "Northeast America", Latitude = 42.0m, Longitude = -71.0m },
                    new Location { Name = "Florida", Latitude = 27.9944m, Longitude = -81.7603m },
                    new Location { Name = "USA", Latitude = 37.0902m, Longitude = -95.7129m },
                    new Location { Name = "New England", Latitude = 41.2033m, Longitude = -73.1839m },
                    new Location { Name = "Canada", Latitude = 56.1304m, Longitude = -106.3468m },
                    new Location { Name = "British Columbia", Latitude = 53.7267m, Longitude = -127.6476m },
                    new Location { Name = "Jervis Inlet", Latitude = 49.7111m, Longitude = -123.9833m },
                    new Location { Name = "San Juan Islands", Latitude = 48.5m, Longitude = -122.9m },
                    new Location { Name = "Desolation Sound", Latitude = 50.0167m, Longitude = -124.5m },
                    new Location { Name = "Maine", Latitude = 44.6937m, Longitude = -69.3819m },
                    new Location { Name = "Alaska", Latitude = 61.0494m, Longitude = -149.4937m },
                    new Location { Name = "Chicago", Latitude = 41.8781m, Longitude = -87.6298m },
                    new Location { Name = "Boston", Latitude = 42.3601m, Longitude = -71.0589m },
                    new Location { Name = "San Francisco", Latitude = 37.7749m, Longitude = -122.4194m },
                    new Location { Name = "Vancouver", Latitude = 49.2827m, Longitude = -123.1207m },
                    new Location { Name = "Cancun", Latitude = 21.1743m, Longitude = -86.8466m },
                    new Location { Name = "Los Angeles", Latitude = 34.0522m, Longitude = -118.2437m },
                    new Location { Name = "Miami", Latitude = 25.7617m, Longitude = -80.1918m },
                    new Location { Name = "New York", Latitude = 40.7128m, Longitude = -74.0060m },

                };
            }
        }

        public static List<Location> SouthAmericaLocations
        {
            get
            {
                return new List<Location>
                {
                    new Location { Name = "Panama", Latitude = 8.5375m, Longitude = -80.7821m },
                    new Location { Name = "Belize", Latitude = 17.1899m, Longitude = -88.4976m },
                    new Location { Name = "Costa Rica", Latitude = 9.7489m, Longitude = -83.7534m },
                    new Location { Name = "Honduras", Latitude = 13.9445m, Longitude = -83.1573m },
                    new Location { Name = "Guadeloupe", Latitude = 16.9950m, Longitude = -62.0675m },
                    new Location { Name = "Galapagos Islands", Latitude = -0.9538m, Longitude = -90.9656m },
                    new Location { Name = "Cuba", Latitude = 21.5218m, Longitude = -77.7812m },
                    new Location { Name = "Cancun", Latitude = 21.1743m, Longitude = -86.8466m },
                    new Location { Name = "Mexican Riviera", Latitude = 19.0m, Longitude = -105.0m },
                    new Location { Name = "Acapulco", Latitude = 16.8531m, Longitude = -99.8237m },
                    new Location { Name = "Brazil", Latitude = -14.2350m, Longitude = -51.9253m },
                    new Location { Name = "Angra dos Reis", Latitude = -23.0097m, Longitude = -44.3187m },
                    new Location { Name = "Easter Island", Latitude = -27.1127m, Longitude = -109.3497m },
                    new Location { Name = "Chile", Latitude = -35.6751m, Longitude = -71.5430m },

                };
            }
        }

        public static List<Location> OceaniaLocations
        {
            get
            {
                return new List<Location>
                {
                    new Location { Name = "Micronesia", Latitude = 7.4255m, Longitude = 150.5508m },
                    new Location { Name = "Bora Bora", Latitude = -16.5000m, Longitude = -151.7415m },
                    new Location { Name = "The Kimberley", Latitude = -15.0m, Longitude = 128.0m },
                    new Location { Name = "Vanuatu", Latitude = -15.3767m, Longitude = 167.1986m },
                    new Location { Name = "Tonga", Latitude = -21.1789m, Longitude = -175.1982m },
                    new Location { Name = "Tahiti", Latitude = -17.6509m, Longitude = -149.4260m },
                    new Location { Name = "Solomon Islands", Latitude = -29.0836m, Longitude = 151.6944m },
                    new Location { Name = "Papua New Guinea", Latitude = -6.314993m, Longitude = 143.9555m },
                    new Location { Name = "Palau Islands", Latitude = 7.5140m, Longitude = 134.5825m },
                    new Location { Name = "New Caledonia", Latitude = -20.9043m, Longitude = 165.6180m },
                    new Location { Name = "French Polynesia", Latitude = -17.6797m, Longitude = -149.4068m },
                    new Location { Name = "Fiji", Latitude = -17.7134m, Longitude = 178.0650m },
                    new Location { Name = "Cook Islands", Latitude = -21.2367m, Longitude = -159.7776m },
                    new Location { Name = "Melbourne", Latitude = -37.8136m, Longitude = 144.9631m },
                    new Location { Name = "Victoria", Latitude = -37.4713m, Longitude = 144.7854m },
                    new Location { Name = "Queensland", Latitude = -20.0m, Longitude = 145.0m },
                    new Location { Name = "Hamilton Island", Latitude = -20.3589m, Longitude = 148.9516m },
                    new Location { Name = "Perth", Latitude = -31.9505m, Longitude = 115.8605m },
                    new Location { Name = "Hobart Tasmania", Latitude = -42.8821m, Longitude = 147.3272m },
                    new Location { Name = "Pittwater", Latitude = -33.6456m, Longitude = 151.3152m },
                    new Location { Name = "Fraser Island", Latitude = -25.2520m, Longitude = 152.8020m },
                    new Location { Name = "Sydney", Latitude = -33.8688m, Longitude = 151.2093m },
                    new Location { Name = "Whitsunday Islands", Latitude = -20.2871m, Longitude = 148.7837m },
                    new Location { Name = "Fiordland", Latitude = -45.4103m, Longitude = 167.7106m },
                    new Location { Name = "Marlborough Sounds", Latitude = -41.2997m, Longitude = 173.9412m },
                    new Location { Name = "Bay of Islands", Latitude = -35.2833m, Longitude = 174.0833m },
                    new Location { Name = "Auckland", Latitude = -36.8485m, Longitude = 174.7633m },
                    new Location { Name = "New Zealand", Latitude = -40.9006m, Longitude = 174.8860m },
                    new Location { Name = "Northern Territory", Latitude = -19.4937m, Longitude = 133.9753m },
                    new Location { Name = "Great Barrier Reef", Latitude = -18.2871m, Longitude = 147.6992m },
                    new Location { Name = "Western Australia", Latitude = -27.5000m, Longitude = 121.0000m },
                    new Location { Name = "South Australia", Latitude = -30.0000m, Longitude = 137.0000m },
                    new Location { Name = "Australia", Latitude = -25.2744m, Longitude = 133.7751m },


                    
                };
            }
        }

        public static List<Location> CaribbeanLocations { 
            get
            {
                return new List<Location>
                {
                    new Location { Name = "St Thomas", Latitude = 18.3370m, Longitude = -64.9307m },
                    new Location { Name = "St John", Latitude = 18.3200m, Longitude = -64.7006m },
                    new Location { Name = "St Croix", Latitude = 17.7450m, Longitude = -64.7391m },
                    new Location { Name = "Bequia", Latitude = 13.0030m, Longitude = -61.2280m },
                    new Location { Name = "Sint Eustatius", Latitude = 17.4872m, Longitude = -62.9762m },
                    new Location { Name = "Saba", Latitude = 17.6349m, Longitude = -63.2262m },
                    new Location { Name = "Virgin Gorda", Latitude = 18.4423m, Longitude = -64.6390m },
                    new Location { Name = "Jost Van Dyke", Latitude = 18.4450m, Longitude = -64.7699m },
                    new Location { Name = "Tortola", Latitude = 18.4310m, Longitude = -64.6235m },
                    new Location { Name = "Cooper Island", Latitude = 18.3940m, Longitude = -64.5936m },
                    new Location { Name = "Anegada Island", Latitude = 18.7142m, Longitude = -64.3899m },
                    new Location { Name = "Warderick Wells Cay", Latitude = 24.6495m, Longitude = -76.6215m },
                    new Location { Name = "Staniel Cay", Latitude = 24.1864m, Longitude = -76.5939m },
                    new Location { Name = "Shroud Cay", Latitude = 24.6117m, Longitude = -76.9860m },
                    new Location { Name = "Rum Cay", Latitude = 23.6580m, Longitude = -74.9741m },
                    new Location { Name = "Ragged Island", Latitude = 22.0300m, Longitude = -74.9930m },
                    new Location { Name = "Mayaguana", Latitude = 22.0500m, Longitude = -73.0900m },
                    new Location { Name = "Long Island", Latitude = 23.1650m, Longitude = -75.0985m },
                    new Location { Name = "Inagua", Latitude = 20.9800m, Longitude = -73.6167m },
                    new Location { Name = "Harbour Island", Latitude = 25.5303m, Longitude = -76.6360m },
                    new Location { Name = "Grand Bahama Islands", Latitude = 26.6476m, Longitude = -78.3967m },
                    new Location { Name = "Eleuthera", Latitude = 25.1119m, Longitude = -76.1959m },
                    new Location { Name = "Compass Cay", Latitude = 24.7260m, Longitude = -76.2850m },
                    new Location { Name = "Cat Islands", Latitude = 24.45m, Longitude = -75.0167m },
                    new Location { Name = "Berry Islands", Latitude = 25.6200m, Longitude = -77.8000m },
                    new Location { Name = "Andros Islands", Latitude = 24.6820m, Longitude = -77.7760m },
                    new Location { Name = "Abacos Islands", Latitude = 26.5350m, Longitude = -77.2430m },
                    new Location { Name = "Windward Islands", Latitude = 12.7m, Longitude = -61.5m },
                    new Location { Name = "Virgin Islands", Latitude = 18.3350m, Longitude = -64.8241m },
                    new Location { Name = "Tobago Cays", Latitude = 12.6250m, Longitude = -61.3125m },
                    new Location { Name = "Leeward Islands", Latitude = 18.0m, Longitude = -63.0m },
                    new Location { Name = "Mustique", Latitude = 12.8750m, Longitude = -61.1825m },
                    new Location { Name = "Greater Antilles", Latitude = 19.0m, Longitude = -74.0m },
                    new Location { Name = "Bimini", Latitude = 25.7296m, Longitude = -79.2680m },
                    new Location { Name = "Acklins & Crooked Island", Latitude = 22.0800m, Longitude = -74.4200m },
                    new Location { Name = "The Exumas", Latitude = 24.4289m, Longitude = -76.6339m },
                    new Location { Name = "Trellis Bay", Latitude = 18.4631m, Longitude = -64.5290m },
                    new Location { Name = "Freeport", Latitude = 26.5387m, Longitude = -78.6350m },
                    new Location { Name = "Nassau", Latitude = 25.0343m, Longitude = -77.3963m },
                    new Location { Name = "US Virgin Islands", Latitude = 18.3350m, Longitude = -64.8241m },
                    new Location { Name = "Turks & Caicos Islands", Latitude = 21.6940m, Longitude = -71.7979m },
                    new Location { Name = "Trinidad & Tobago", Latitude = 10.6918m, Longitude = -61.2225m },
                    new Location { Name = "St Vincent and the Grenadines", Latitude = 13.2520m, Longitude = -61.1971m },
                    new Location { Name = "St Lucia", Latitude = 13.9094m, Longitude = -61.0985m },
                    new Location { Name = "St Kitts and Nevis", Latitude = 17.3576m, Longitude = -62.7833m },
                    new Location { Name = "St Barts", Latitude = 17.9000m, Longitude = -62.8500m },
                    new Location { Name = "Saint Martin", Latitude = 18.0700m, Longitude = -63.0600m },
                    new Location { Name = "Puerto Rico", Latitude = 18.2208m, Longitude = -66.5901m },
                    new Location { Name = "Montserrat", Latitude = 16.7425m, Longitude = -62.2185m },
                    new Location { Name = "Martinique", Latitude = 14.6415m, Longitude = -61.0242m },
                    new Location { Name = "Jamaica", Latitude = 18.1096m, Longitude = -77.2975m },
                    new Location { Name = "Haiti", Latitude = 18.9712m, Longitude = -72.2852m },
                    new Location { Name = "Guadeloupe", Latitude = 16.2650m, Longitude = -61.5510m },
                    new Location { Name = "Grenada", Latitude = 12.1167m, Longitude = -61.6742m },
                    new Location { Name = "Dominica", Latitude = 15.4148m, Longitude = -61.3700m },
                    new Location { Name = "Cuba", Latitude = 21.5218m, Longitude = -77.7812m },
                    new Location { Name = "Cayman Islands", Latitude = 19.3131m, Longitude = -81.2546m },
                    new Location { Name = "British Virgin Islands", Latitude = 18.4310m, Longitude = -64.6235m },
                    new Location { Name = "Barbados", Latitude = 13.1939m, Longitude = -59.5432m },
                    new Location { Name = "Bahamas", Latitude = 25.0343m, Longitude = -77.3963m },
                    new Location { Name = "Antigua", Latitude = 17.0608m, Longitude = -61.7964m },
                    new Location { Name = "Anguilla", Latitude = 18.2206m, Longitude = -63.0686m },



                };
            }
        }


        public static List<Location> EuropeanLocations
        {
            get
            {
                return new List<Location>
                {
                    new Location { Name = "Northern Europe", Latitude = 55.0m, Longitude = 13.0m },
                    new Location { Name = "Brittany", Latitude = 48.3794m, Longitude = -4.4861m },
                    new Location { Name = "Gosport", Latitude = 50.7947m, Longitude = -1.1085m },
                    new Location { Name = "Greenland", Latitude = 71.7069m, Longitude = -42.6043m },
                    new Location { Name = "Guernsey", Latitude = 49.4657m, Longitude = -2.5850m },
                    new Location { Name = "Ireland", Latitude = 53.1424m, Longitude = -7.6921m },
                    new Location { Name = "Latvia", Latitude = 56.8796m, Longitude = 24.6032m },
                    new Location { Name = "Channel Islands", Latitude = 49.4144m, Longitude = -2.5870m },
                    new Location { Name = "Solent", Latitude = 50.8000m, Longitude = -1.3000m },
                    new Location { Name = "Baltic", Latitude = 54.0m, Longitude = 18.0m }, 
                    new Location { Name = "Scandinavia", Latitude = 60.0m, Longitude = 10.0m },
                    new Location { Name = "Scotland", Latitude = 56.4907m, Longitude = -4.2026m },
                    new Location { Name = "Flåm", Latitude = 60.8596m, Longitude = 7.1144m },
                    new Location { Name = "Stavanger", Latitude = 58.9690m, Longitude = 5.7331m },
                    new Location { Name = "Geiranger", Latitude = 62.1042m, Longitude = 7.2076m },
                    new Location { Name = "Bergen", Latitude = 60.39299m, Longitude = 5.32415m },
                    new Location { Name = "Alesund", Latitude = 62.4726m, Longitude = 6.1432m },
                    new Location { Name = "Sweden", Latitude = 60.1282m, Longitude = 18.6435m },
                    new Location { Name = "Denmark", Latitude = 56.2639m, Longitude = 9.5018m },
                    new Location { Name = "Finland", Latitude = 61.9241m, Longitude = 25.7482m },
                    new Location { Name = "England", Latitude = 52.3555m, Longitude = -1.1743m },
                    new Location { Name = "Iceland", Latitude = 64.9631m, Longitude = -19.0208m },
                    new Location { Name = "Norway", Latitude = 60.4720m, Longitude = 8.4689m },
                    new Location { Name = "United Kingdom", Latitude = 55.3781m, Longitude = -3.4360m },
                    new Location { Name = "Oslofjord", Latitude = 59.0000m, Longitude = 10.5000m },
                    new Location { Name = "Arctic Circle", Latitude = 66.5622m, Longitude = 0.0000m },
                    new Location { Name = "Orkney & Shetland Islands", Latitude = 59.0000m, Longitude = -3.0000m },
                    new Location { Name = "Nuuk", Latitude = 64.1833m, Longitude = -51.7216m },
                    new Location { Name = "Gotland", Latitude = 57.5000m, Longitude = 18.5000m }
                };
            }
        }

        public static List<Location> AntarcticaLocations
         {
            get
            {
                return new List<Location>
                {
                    new Location { Name = "South Shetland Islands", Latitude = -62.0000m, Longitude = -58.0000m },
                    new Location { Name = "Weddell Sea", Latitude = -73.0000m, Longitude = -45.0000m },
                    new Location { Name = "Ross Sea", Latitude = -75.0000m, Longitude = 175.0000m },
                    new Location { Name = "James Ross Island", Latitude = -64.1667m, Longitude = -57.7500m },
                    new Location { Name = "Antarctic Circle", Latitude = -66.5622m, Longitude = 0.0000m },
                    new Location { Name = "Paulet Island", Latitude = -63.5833m, Longitude = -55.7667m },
                    new Location { Name = "Peterman Island", Latitude = -65.1742m, Longitude = -64.1303m },
                    new Location { Name = "Charcot Island", Latitude = -69.7500m, Longitude = -75.2500m },
                    new Location { Name = "Danco Island", Latitude = -64.7333m, Longitude = -62.6000m },
                    new Location { Name = "Wilhelmina Bay", Latitude = -64.6333m, Longitude = -62.1000m },
                    new Location { Name = "Horseshoe Island", Latitude = -67.8167m, Longitude = -67.1500m },
                    new Location { Name = "Adelaide Island", Latitude = -67.4333m, Longitude = -68.3500m },
                    new Location { Name = "Deception Island", Latitude = -62.9833m, Longitude = -60.5667m },
                    new Location { Name = "South Georgia Island", Latitude = -54.5000m, Longitude = -36.0000m },
                    new Location { Name = "The Amundsen Sea", Latitude = -72.0000m, Longitude = -120.0000m },
                    new Location { Name = "The Balleny Islands", Latitude = -66.9000m, Longitude = 163.4167m },
                    new Location { Name = "The Larsen Ice Shelf", Latitude = -67.0000m, Longitude = -60.0000m },
                    new Location { Name = "The South Orkney Islands", Latitude = -60.6000m, Longitude = -45.5000m },
                    new Location { Name = "Vernadsky Research Station", Latitude = -65.2500m, Longitude = -64.2667m },
                    new Location { Name = "Snow Hill Island", Latitude = -64.4667m, Longitude = -57.2167m },
                    new Location { Name = "Crystal Sound", Latitude = -66.5000m, Longitude = -66.5000m }

                };

            }
        }
        public static List<Location> GetAllLocations()
        {
            List<Location> result = new List<Location>();
            result.AddRange(MediterraneanLocations);
            result.AddRange(CaribbeanLocations);
            result.AddRange(MiddleEastLocations);
            result.AddRange(IndianOceanLocations);
            result.AddRange(SouthAmericaLocations);
            result.AddRange(NorthAmericaLocations);
            result.AddRange(AsiaLocations);
            result.AddRange(EuropeanLocations);
            result.AddRange(AntarcticaLocations);

            return result;
        }
    }
}
