using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop
{
    class Supplier
    {
        public static void Supply(Shop s, string itemName, int quantity)
        {
            s.Supply(itemName, quantity);
        }
    }
}
