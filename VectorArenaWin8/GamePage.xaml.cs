using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;
using Microsoft.AspNet.SignalR.Client.Hubs;


namespace VectorArenaWin8
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly VectorArenaGame game;

        public GamePage(string launchArguments)
        {
            this.InitializeComponent();

            // Create the game.
            game = XamlGame<VectorArenaGame>.Create(launchArguments, Window.Current.CoreWindow, this);
        }
    }
}
