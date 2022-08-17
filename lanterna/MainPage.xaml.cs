using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Battery;
using Xamarin.Essentials;

namespace lanterna
{
    public partial class MainPage : ContentPage
    {
        bool lanterna_ligada = false;
        public MainPage()
        {
            InitializeComponent();
            btnLanternaOnOff.Source = ImageSource.FromResource("lanterna.Botao.desligado.jpg");

            Informacoes_Bateria();
        }

        private async void Informacoes_Bateria()
        {
            try
            {
                if (CrossBattery.IsSupported)
                {
                    CrossBattery.Current.BatteryChanged -= Status_Bateria;
                    CrossBattery.Current.BatteryChanged += Status_Bateria;
                }
                else
                {
                    lbl_bateria_fraca.Text = "Informações não disponíveis";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro: \n ", ex.Message, "OK");
            }
        }

        private async void Status_Bateria(object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs e)
        {
            try
            {
                lbl_carga.Text = e.RemainingChargePercent.ToString() + "%";

                if (e.IsLow)
                {
                    lbl_carga.Text = "A Bateria está fraca!";
                }
                else
                {
                    lbl_carga.Text = "";
                }

                switch (e.Status)
                {
                    case Plugin.Battery.Abstractions.BatteryStatus.Charging:
                        lbl_status.Text = "Carregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Discharging:
                        lbl_status.Text = "Descarregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Full:
                        lbl_status.Text = "Carregada";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.NotCharging:
                        lbl_status.Text = "Sem carregar";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Unknown:
                        lbl_status.Text = "Desconhecido";
                        break;
                }

                switch (e.PowerSource)
                {
                    case Plugin.Battery.Abstractions.PowerSource.Battery:
                        lbl_fonte.Text = "Bateria";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Ac:
                        lbl_fonte.Text = "Carregador";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Usb:
                        lbl_fonte.Text = "USB";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Wireless:
                        lbl_fonte.Text = "Sem fio";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Other:
                        lbl_fonte.Text = "Desconhecida";
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocorreu um erro: \n ", ex.Message, "OK");
            }
        }
        private async void btnLanternaOnOff_Clicked(object sender, EventArgs e)
        {
            try
            {

                if(!lanterna_ligada)
                {
                    lanterna_ligada = true;
                    btnLanternaOnOff.Source = ImageSource.FromResource("lanterna.Botao.ligado.png");

                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOnAsync();
                }
                else
                {
                    lanterna_ligada = false;
                    btnLanternaOnOff.Source = ImageSource.FromResource("lanterna.Botao.desligado.jpg");

                    Vibration.Vibrate(TimeSpan.FromMilliseconds(250));

                    await Flashlight.TurnOffAsync();
                }
            }
            catch (Exception ex)
            {
               await DisplayAlert("Vish", ex.Message, "Ok");
            }

        }
    }
}
