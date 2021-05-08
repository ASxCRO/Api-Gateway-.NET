using AutoStoper.Mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AutoStoper.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}