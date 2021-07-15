using System;
using System.Threading;
using System.Windows.Forms;

namespace StarshipComputer
{
    public partial class Telemetry : Form
    {
        public Telemetry()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread.Sleep(1000);

            Thread update = new Thread(Update);
            update.Start();
        }

        void InvokeAction(Action action)
        {
            this.Invoke(action);
        }

        private void Update()
        {
            while (Starship.starship == null)
            {
            }

            while (true)
            {
                this.InvokeAction(() =>
                {
                    Speed_lb.Text = "Speed : " + Math.Round(Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).TrueAirSpeed * 3.6f) + "km/h";
                    altitude_lb.Text = "Altitude : " + Math.Round(Starship.starship.Flight(Starship.starship.SurfaceReferenceFrame).SurfaceAltitude - 25) + "m";
                });
            }
        }
    }
}
