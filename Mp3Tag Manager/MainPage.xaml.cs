using Mp3Tag_Manager.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;



namespace Mp3Tag_Manager {
    public sealed partial class MainPage : Page {

        // Lista de mp3s
        public ObservableCollection<Mp3> lista_mp3 = new ObservableCollection<Mp3>();



        public MainPage() {
            this.InitializeComponent();

            // Integra lista_mp3 com a list_view
            list_view.ItemsSource = lista_mp3;

            //Eventos para os botoes
            //button_adicionar.Click += new RoutedEventHandler(button_adicionar_Click);
            //button_remover.Click += new RoutedEventHandler(button_remover_Click);
            //button_cima.Click += new RoutedEventHandler(button_cima_Click);
            //button_baixo.Click += new RoutedEventHandler(button_baixo_Click);
            //button_salvar.Click += new RoutedEventHandler(button_salvar_Click);
            //button_salvar_todos.Click += new RoutedEventHandler(button_salvar_todos_Click);

            //list_view.SelectionChanged += new SelectionChangedEventHandler(listView_SelectionChanged);
        }



        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e) {
        }



        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // Pega o item selecionado
            Mp3 item = list_view.SelectedItem as Mp3;

            // Seta os valores para os textboxs e textblocks especificos
            if (item == null) {
                // Seta para null pra limpar os valores das textbox
                this.DataContext = null;
            } else {
                // Joga os dados no DataContext pra atualizar os dados da textbox
                this.DataContext = item;
            }
        }



        private async void button_adicionar_Click(object sender, RoutedEventArgs e) {
            // Abrir arquivos do tipo mp3
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".mp3");

            // Grava os dados do arquivo
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null) {
                bool tem_arquivo = false;

                foreach (Mp3 mp3 in lista_mp3) {
                    if (mp3.nome_arquivo.Equals(file.Name)) {
                        tem_arquivo = true;
                        break;
                    }
                }

                if (!tem_arquivo) {
                    // Pega as tags do mp3
                    MusicProperties mp = await file.Properties.GetMusicPropertiesAsync();
                    StorageItemThumbnail image = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 250, ThumbnailOptions.ResizeThumbnail);


                    // Adiciona o Mp3 na lista
                    lista_mp3.Add(new Mp3(file, file.Name, file.Path, mp.Duration, file.FileType, mp.Bitrate,
                                          mp.Title, mp.Artist, mp.Album, mp.AlbumArtist, mp.Genre, mp.Year,
                                          mp.TrackNumber, mp.Rating, image));

                    // Atualiza listview
                    list_view.ItemsSource = lista_mp3;
                } else {
                    // Nao existe arquivo
                    if (!adicionar_popup.IsOpen) { adicionar_popup.IsOpen = true; }
                }
            } else {
                // Quando clica no botao cancelar, nao faz nada
            }
        }



        private void button_remover_Click(object sender, RoutedEventArgs e) {
            // Pega posicao do item selecionado
            int idx = list_view.SelectedIndex;

            // remove item selecionado da lista
            if (idx != -1) {
                lista_mp3.RemoveAt(idx);
                if (!remover_popup.IsOpen) { remover_popup.IsOpen = true; }
            }
        }



        private void button_cima_Click(object sender, RoutedEventArgs e) {
            // Pega a posicao do item selecionado
            int idx = list_view.SelectedIndex;

            // Modifica para o seu anterior
            if ((idx != 0) && (idx != -1)) {
                lista_mp3.Move(idx, idx - 1);
            }
        }



        private void button_baixo_Click(object sender, RoutedEventArgs e) {
            // Pega a posicao do item selecionado
            int idx = list_view.SelectedIndex;

            // Modifica para o seu posterior
            if ((idx != lista_mp3.Count - 1) && (idx != -1)) {
                lista_mp3.Move(idx, idx + 1);
            }
        }



        private void button_salvar_Click(object sender, RoutedEventArgs e) {
            // Pega o item selecionado
            Mp3 mp3 = list_view.SelectedItem as Mp3;

            if (mp3 != null) {
                // Salva mp3 selecionado
                mp3.saveFile();

                if (!salvar_popup.IsOpen) { salvar_popup.IsOpen = true; }
            }
        }



        private void button_salvar_todos_Click(object sender, RoutedEventArgs e) {
            if (lista_mp3.Count != 0) {
                // Salva todos da lista
                foreach (Mp3 mp3 in lista_mp3) {
                    if (mp3 != null) {
                        mp3.saveFile();
                    }
                }

                if (!salvar_todos_popup.IsOpen) { salvar_todos_popup.IsOpen = true; }
            }
        }



        private void button_adicionar_artwork_Click(object sender, RoutedEventArgs e) {

        }



        private void button_remover_artwork_Click(object sender, RoutedEventArgs e) {

        }

        private void fecha_popup_Clicked(object sender, RoutedEventArgs e) {
            if (adicionar_popup.IsOpen) { adicionar_popup.IsOpen = false; }
            if (remover_popup.IsOpen) { remover_popup.IsOpen = false; }
            if (salvar_popup.IsOpen) { salvar_popup.IsOpen = false; }
            if (salvar_todos_popup.IsOpen) { salvar_todos_popup.IsOpen = false; }
        }
    }
}