using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;


namespace Mp3Tag_Manager.Common {
    public class Mp3 : INotifyPropertyChanged {

        // Notificacoes para mudancas nas propriedades
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }



        // Dados adicionais
        public StorageFile file { get; set; }
        public bool modificado { get; set; }
        public bool artwork_modificado { get; set; }



        // Imagem
        private BitmapImage _artwork = null;
        public BitmapImage artwork {
            get { return this._artwork; }
            set { if (this._artwork != value) { this._artwork = value; OnPropertyChanged("artwork"); this.modificado = true; this.artwork_modificado = true; } }
        }



        // Informacoes
        public string nome_arquivo { get; set; }  // file.Name        nome do arquivo [Filename]
        public string caminho { get; set; }       // file.Path        url [Location/ Directory/Path]
        public string tempo { get; set; }       // mp.Duration      3:48 (228 seg) [Length]
        public string formato { get; set; }       // file.FileType    MPEG-1 layer 3 ou mp3 [Format]
        public string bitrate { get; set; }         // mp.Bitrate       320 kbps [Bitrate]
        //public int tamanho { get; set; }        //                  3.59 mb (3763614 bytes) [Duration]
        //public int frequencia { get; set; }     //                  44.100 kHz [Frequency]
        //public string canal { get; set; }       //                  Stereo ou 2 ou joint stereo [Channels]
        //public bool copyright { get; set; }     //                  No [Copyright]
        //public bool original { get; set; }      //                  Yes [Original]
        //public int frames { get; set; }         //                  8738 [Frames]
        //public int tamanho_header { get; set; } //                  111360 bytes [Header size]
        //public string encoder { get; set; }     //                  LAME3.99r [Encoder]
        //public int versao_id3 { get; set; }     //                  v1 and v2.3 [Tag]



        // Tags

        // Title
        private string _nome = string.Empty;
        public string nome {
            get { return this._nome; }
            set { if (this._nome != value) { this._nome = value; OnPropertyChanged("nome"); this.modificado = true; } }
        }
        // Artist
        private string _artista = string.Empty;
        public string artista {
            get { return this._artista; }
            set { if (this._artista != value) { this._artista = value; OnPropertyChanged("artista"); this.modificado = true; } }
        }
        // Album
        private string _album = string.Empty;
        public string album {
            get { return this._album; }
            set { if (this._album != value) { this._album = value; OnPropertyChanged("album"); this.modificado = true; } }
        }
        // AlbumArtist
        private string _album_artista = string.Empty;
        public string album_artista {
            get { return this._album_artista; }
            set { if (this._album_artista != value) { this._album_artista = value; OnPropertyChanged("album_artista"); this.modificado = true; } }
        }
        // Genre
        private string _generos = null;
        public string generos {
            get { return this._generos; }
            set { if (this._generos != value) { this._generos = value; OnPropertyChanged("generos"); } }
        }
        // Year
        private uint _ano = 0;
        public uint ano {
            get { return this._ano; }
            set { if (this._ano != value) { this._ano = value; OnPropertyChanged("ano"); this.modificado = true; } }
        }
        // TrackNumber
        private uint _numero = 0;
        public uint numero {
            get { return this._numero; }
            set { if (this._numero != value) { this._numero = value; OnPropertyChanged("numero"); this.modificado = true; } }
        }
        // Rating
        private uint _rating = 0;
        public uint rating {
            get { return this._rating; }
            set { if (this._rating != value) { this._rating = value; OnPropertyChanged("rating"); this.modificado = true; } }
        }




        public Mp3(StorageFile file, string nome_arquivo, string caminho, TimeSpan tempo, string formato, uint bitrate,
                   string nome, string artista, string album, string album_artista, IList<string> generos,
                   uint ano, uint numero, uint rating, StorageItemThumbnail thumb) {
            // Dados adicionais
            this.file = file;
            this.modificado = false;
            this.artwork_modificado = false;

            // Imagens
            this.artwork = new BitmapImage();
            this.artwork.SetSource(thumb);

            // Informacoes
            this.nome_arquivo = nome_arquivo;
            this.caminho = caminho;
            this.tempo = tempo.Minutes.ToString() + ":" + tempo.Seconds.ToString();
            this.formato = formato;
            this.bitrate = (bitrate / 1000).ToString() + " kbps";
            foreach (string g in generos) {
                this._generos = g;
            }

            // Tags
            this._nome = (nome == null ? "" : nome);
            this._artista = (artista == null ? "" : artista);
            this._album = (album == null ? "" : album);
            this._album_artista = (album_artista == null ? "" : album_artista);
            this._ano = ano;
            this._numero = numero;
            this._rating = rating;

        }


        public async void saveFile() {
            if (this.modificado == true) {
                MusicProperties mp = await file.Properties.GetMusicPropertiesAsync();

                // Seta as propriedades
                mp.Title = this.nome;
                mp.Artist = this.artista;
                mp.Album = this.album;
                mp.AlbumArtist = this.album_artista;
                mp.Year = this.ano;
                mp.TrackNumber = this.numero;
                mp.Rating = this.rating;

                // Seta artwork
                if (this.artwork_modificado == true) {
                    StorageItemThumbnail thumb = await file.GetThumbnailAsync(ThumbnailMode.MusicView);
                }

                // Salva dados no arquivo
                await mp.SavePropertiesAsync();
                await file.Properties.SavePropertiesAsync();

                // Seta as boleanas
                this.modificado = false;
                this.artwork_modificado = false;
            }
        }

        //private uint converter_rating(uint value) {
        //    uint _value;

        //    switch (value) {
        //        case 0: { _value = 0; }
        //            break;
        //        case 1: { _value = 1; }
        //            break;
        //        case 2: { _value = 64; }
        //            break;
        //        case 3: { _value = 128; }
        //            break;
        //        case 4: { _value = 196; }
        //            break;
        //        case 5: { _value = 255; }
        //            break;
        //        default: { _value = 0; }
        //            break;
        //    }

        //    return _value;
        //}

        //private uint reverter_converter_rating(uint _value) {
        //    uint _grade = 0;

        //    if (_value == 0) {
        //        _grade = 0;
        //    } else if (1 <= _value && _value <= 31) {
        //        _grade = 1;
        //    } else if (32 <= _value && _value <= 95) {
        //        _grade = 2;
        //    } else if (96 <= _value && _value <= 159) {
        //        _grade = 3;
        //    } else if (160 <= _value && _value <= 223) {
        //        _grade = 4;
        //    } else if (224 <= _value && _value <= 255) {
        //        _grade = 5;
        //    } else {
        //        _grade = 0;
        //    }

        //    return _grade;
        //}
    }
}


/* http://msdn.microsoft.com/en-us/library/windows/apps/windows.storage.fileproperties.musicproperties.aspx

Duration        Read-only	Gets the duration of the song in milliseconds.
Bitrate         Read-only	Gets the bit rate of the song file.

Album           Read/write	Gets or sets the name of the album that contains the song.
AlbumArtist     Read/write	Gets or sets the name of the album artist of the song.
Artist          Read/write	Gets the artists that contributed to the song or sets the album artist.
Genre           Read-only	Gets the names of music genres that the song belongs to.
Year            Read/write	Gets or sets the year that the song was released.
Title           Read/write	Gets or sets the title of the song
TrackNumber     Read/write	Gets or sets the track number of the song on the song's album.
Rating          Read/write	Gets the rating associated with the song.
 * 
Publisher       Read/write	Gets or sets the publisher of the song.
Subtitle        Read/write	Gets or sets the subtitle of the song.
Writers         Read-only	Gets the songwriters.
Composers       Read-only	Gets the composers of the song.
Conductors      Read-only	Gets the conductors of the song.
Producers       Read-only	Gets the producers of the song.
 */
/*

*/



// ID3v1
// [Title]
// [Artist]
// [Album]
// [Genre]
// [Year]
// [Comment]
// [Track]



// ID3v2
// [Title]
// [Artist]
// [Album]
// [Genre]
// [Year]
// [Track]
// [Track of]
// [Disc]
// [Disc of]
// [Composer]
// [Original Artist]
// [Copyright]
// [URL]
// [Encoded by]
// [Comment]
// [Rating]

// [Lyrics]
// [Artwork]