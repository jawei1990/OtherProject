using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID_VendingMachine.Models
{
    public class Card
    {
        private int _id;
        private string _cardNo;
        private decimal _amount;
        private User _user;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string CardNo
        {
            get { return _cardNo; }
            set { _cardNo = value; }
        }

        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }
    }
}
