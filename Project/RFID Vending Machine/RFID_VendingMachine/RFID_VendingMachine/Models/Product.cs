using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID_VendingMachine.Models
{
    public class Product
    {
        private int _id;
        private decimal _amount;
        private string _name;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
    }
}
