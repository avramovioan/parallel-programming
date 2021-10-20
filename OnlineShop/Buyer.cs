using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop
{
    class Buyer
    {
        public static bool Purchase(Shop s, string itemName, int quantity)
        {
            return s.Purchase(itemName, quantity);
        }
    }
}
