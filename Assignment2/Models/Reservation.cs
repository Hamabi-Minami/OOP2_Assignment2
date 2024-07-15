using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class Reservation
    {
        public Flight Flight { get; set; }
        public string Name { get; set; }
        public string Citizenship { get; set; }
        public string ReservationCode { get; set; }

        private string status;
        public string Status
        {
            get => status;
            set
            {
                if (value != "Active" && value != "InActive")
                {
                    throw new ArgumentException("Status must be either 'Active' or 'InActive'.");
                }
                status = value;
            }
        }

        public Reservation(Flight flight, string name, string citizenship, string reservationCode, string status="Active") 
        {
            this.Flight = flight;
            this.Name = name;
            this.Citizenship = citizenship;
            this.ReservationCode = reservationCode;
            this.Status = status;
        }

        public string ToCsvString()
        {
            return $"{this.Flight.ToCsvString()},{this.Name},{this.Citizenship},{this.ReservationCode},{this.Status}";
        }
    }
}
