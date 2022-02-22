using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using AppleStore.ViewModels;


namespace AppleStore.InputModels
{
    public class CartList_All_PurchasedInputModel : IEnumerable
    {
        public CartList_All_PurchasedInputModel(
            ICollection<CartListPurchasedAppleFormModel> allPurchased, 
            decimal total, 
            ICollection<DiscountsViewModel> discountsApplied,
            string cartId)
        {
            AllPurchased = allPurchased;
            Total = total;
            DiscountsApplied = discountsApplied ?? new List<DiscountsViewModel>();
            CartId = cartId;
        }

        public decimal Total { get; set; }

        public string CartId { get; set; }

        public ICollection<CartListPurchasedAppleFormModel> AllPurchased { get; set; }

        public ICollection<DiscountsViewModel> DiscountsApplied { get; set; }

        private class MyEnumerator : IEnumerator
        {
            private List<CartListPurchasedAppleFormModel> list;
            private int position = -1;

            public MyEnumerator(List<CartListPurchasedAppleFormModel> list)
            {
                this.list = list;
            }
            private IEnumerator getEnumerator()
            {
                return (IEnumerator)this;
            }
            public bool MoveNext()
            {
                position++;
                return (position < list.Count);
            }
            public void Reset()
            {
                position = -1;
            }
            public object Current
            {
                get
                {
                    try
                    {
                        return list[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        } 
       
        public IEnumerator GetEnumerator()
        {
            return new MyEnumerator(AllPurchased.ToList());
        }
    }
}
