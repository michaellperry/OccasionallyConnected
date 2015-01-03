using Assisticant;
using CardBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace CardBoard.BoardView
{
    public static class DialogManager
    {
        public static void ShowCardDetail(CardDetailModel cardDetail, Action<CardDetailModel> completed)
        {
            Popup popup = new Popup()
            {
                ChildTransitions = new TransitionCollection { new PopupThemeTransition() }
            };
            var detail = new CardDetailControl()
            {
                Width = Window.Current.Bounds.Width,
                Height = Window.Current.Bounds.Height,
                DataContext = ForView.Wrap(cardDetail)
            };
            detail.Ok += delegate
            {
                popup.IsOpen = false;
                completed(cardDetail);
            };
            detail.Cancel += delegate
            {
                popup.IsOpen = false;
            };
            popup.Child = detail;
            popup.IsOpen = true;
        }
    }
}
