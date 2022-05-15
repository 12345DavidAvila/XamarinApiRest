using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using XamarinApiRest.Model;

namespace XamarinApiRest
{
    public partial class MainPage : ContentPage
    {
        Uri uri = new Uri("http://192.168.1.212:8080/ProyectoU/post.php");
        public MainPage()
        {
            InitializeComponent();
            llenarDatos();
        }

        public async void llenarDatos()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = uri;
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<List<UsuarioModel>>(content);
                lstUsuarios.ItemsSource = resultado;
            }
        }

        private async void btnguardar_Clicked(object sender, EventArgs e)
        {
            UsuarioModel usuario = new UsuarioModel
            {
                DocumentoIdentidad = txtcedula.Text,
                Nombres = txtnombres.Text,
                Telefono = txttelefono.Text,
                Correo = txtcorreo.Text,
                Ciudad = txtciudad.Text
            };

            Uri RquestUri = uri;
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(usuario);
            var contenJson = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(RquestUri, contenJson);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                await DisplayAlert("Datos", "Se elimino Correctamente", "ok");
                txtcedula.Text = "";
                txtnombres.Text = "";
                txttelefono.Text = "";
                txtcorreo.Text = "";
                txtciudad.Text = "";
                llenarDatos();
            }
            else
            {
                await DisplayAlert("Datos", "Ocurrio un error", "ok");
            }
        }

        private async void lstUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var obj = (UsuarioModel)e.SelectedItem;
            if (!string.IsNullOrEmpty(obj.IdUsuario.ToString()))
            {
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(uri + obj.IdUsuario.ToString());
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");
                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    UsuarioModel usuarioModel = JsonConvert.DeserializeObject<UsuarioModel>(content);
                    txtidUsuario.Text = usuarioModel.IdUsuario.ToString();
                    txtcedula.Text = usuarioModel.DocumentoIdentidad;
                    txtnombres.Text = usuarioModel.Nombres;
                    txttelefono.Text = usuarioModel.Telefono;
                    txtcorreo.Text = usuarioModel.Correo;
                    txtciudad.Text = usuarioModel.Ciudad;

                    btnguardar.IsVisible = false;
                    btnmodificar.IsVisible = true;
                    btneliminar.IsVisible = true;
                }
            }
        }

        private async void btnmodificar_Clicked(object sender, EventArgs e)
        {
            UsuarioModel usuario = new UsuarioModel
            {
                IdUsuario = Convert.ToInt32(txtidUsuario.Text),
                DocumentoIdentidad = txtcedula.Text,
                Nombres = txtnombres.Text,
                Telefono = txttelefono.Text,
                Correo = txtcorreo.Text,
                Ciudad = txtciudad.Text
            };

            Uri RquestUri = uri;
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(usuario);
            var contenJson = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(RquestUri, contenJson);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                await DisplayAlert("Datos", "Se actualizo Correctamente", "ok");
                txtcedula.Text = "";
                txtnombres.Text = "";
                txttelefono.Text = "";
                txtcorreo.Text = "";
                txtciudad.Text = "";
                txtidUsuario.Text = "";
                llenarDatos();
                btnguardar.IsVisible = true;
                btnmodificar.IsVisible = false;
                btneliminar.IsVisible = false;
            }
            else
            {
                await DisplayAlert("Datos", "Ocurrio un error", "ok");
            }
        }

        private async void btneliminar_Clicked(object sender, EventArgs e)
        {

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(uri + txtidUsuario.Text);
            request.Method = HttpMethod.Delete;
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                await DisplayAlert("Datos", "Se elimino correctamente", "ok");
                txtcedula.Text = "";
                txtnombres.Text = "";
                txttelefono.Text = "";
                txtcorreo.Text = "";
                txtciudad.Text = "";
                txtidUsuario.Text = "";
                llenarDatos();
                btnguardar.IsVisible = true;
                btnmodificar.IsVisible = false;
                btneliminar.IsVisible = false;
            }
            else
            {
                await DisplayAlert("Datos", "Ocurrio un error", "ok");
            }

        }
    }
}
