using Assisticant;
using Assisticant.Fields;
using CardBoard.BoardView;
using CardBoard.Models;
using CardBoard.ViewModels;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CardBoard
{
    public sealed partial class MainPage : Page
    {
        private ComputedSubscription _selectedCardSubscription;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ForView.Unwrap<MainViewModel>(DataContext, vm =>
            {
                vm.ClearSelection();
            });
        }

        private void NewCard_Click(object sender, RoutedEventArgs e)
        {
            ForView.Unwrap<MainViewModel>(DataContext, vm =>
            {
                vm.PrepareNewCard();
                Frame.Navigate(typeof(CardDetailPage));
            });
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ForView.Unwrap<MainViewModel>(DataContext, vm =>
            {
                var computed = new Computed<Card>(() => vm.SelectedCard);
                _selectedCardSubscription = computed.Subscribe(c => CardSelected(c));
            });
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_selectedCardSubscription != null)
                _selectedCardSubscription.Unsubscribe();
        }

        private void CardSelected(Card card)
        {
            if (card != null)
            {
                ForView.Unwrap<MainViewModel>(DataContext, vm =>
                    vm.PrepareEditCard(card));
                Frame.Navigate(typeof(CardDetailPage));
            }
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private void Ellipse_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ForView.Unwrap<MainViewModel>(DataContext, vm =>
            {
                var dialog = new MessageDialog(vm.ErrorMessage);
                dialog.ShowAsync();
            });
        }
    }
}
