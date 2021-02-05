using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        }

        void InvokeAction(Action action)
        {
            this.Invoke(action);
        }

        private void Update()
        {
            while (true)
            {
                this.InvokeAction(() =>
                {
                });
            }
        }
    }
}
