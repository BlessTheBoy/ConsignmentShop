using ConsignmentShopLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cosignment_Shop_UI
{
    public partial class ConsignmentShop : Form
    {
        private Store store = new Store();
        private List<Item> shoppingCartData = new List<Item>();
        BindingSource itemsBinding = new BindingSource();
        BindingSource cartBinding = new BindingSource();
        BindingSource vendorsBinding = new BindingSource();
        private decimal storeProfit = 0;


        public ConsignmentShop()
        {
            InitializeComponent();
            SetUpData();

            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();
            itemsListBox.DataSource = itemsBinding;

            itemsListBox.DisplayMember ="Display";
            itemsListBox.ValueMember = "Display";

            cartBinding.DataSource = shoppingCartData;
            shoppingCartListBox.DataSource = cartBinding;

            shoppingCartListBox.DisplayMember = "Display";
            shoppingCartListBox.ValueMember = "Display";

            vendorsBinding.DataSource = store.Vendors;
            vendorListBox.DataSource = vendorsBinding;

            vendorListBox.DisplayMember = "Display";
            vendorListBox.ValueMember = "Display";
        }

        private void SetUpData()
        {
            store.Vendors.Add(new Vendor { FirstName="Bills", LastName="Smith" });
            store.Vendors.Add(new Vendor { FirstName = "Sue", LastName = "Jones" });

            store.Items.Add(new Item { Title = "Moby Dick", Description = "A book about a whale", Price = 4.50m, Owner = store.Vendors[0] });
            store.Items.Add(new Item { Title = "A Tale of two Cities", Description = "A book about a revolution", Price = 3.80m, Owner = store.Vendors[1] });
            store.Items.Add(new Item { Title = "Harry Potter Book 1", Description = "A book about a boy", Price = 5.20m, Owner = store.Vendors[1] });
            store.Items.Add(new Item { Title = "jane Eyre", Description = "A book about a girl", Price = 1.50m, Owner = store.Vendors[0] });

            store.Name = "Seconds are better";
        }

        private void addToCart_Click(object sender, EventArgs e)
        {
            //add selected item cart
            shoppingCartData.Add((Item)itemsListBox.SelectedItem);
            cartBinding.ResetBindings(false);
        }

        private void makePurchase_Click(object sender, EventArgs e)
        {
            foreach (Item item in shoppingCartData)
            {
                item.Sold = true;
                item.Owner.PaymentDue += item.Owner.Commission * item.Price;
                storeProfit += (1 - item.Owner.Commission) * item.Price;
            }

            storeProfitValue.Text = string.Format("${0}", storeProfit);
               
            shoppingCartData.Clear();
            cartBinding.ResetBindings(false);
            itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();
            itemsBinding.ResetBindings(false);
            vendorsBinding.ResetBindings(false);

            
        }
    }
}
