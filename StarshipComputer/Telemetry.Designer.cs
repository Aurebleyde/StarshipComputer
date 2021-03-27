namespace StarshipComputer
{
    partial class Telemetry
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.Speed_lb = new System.Windows.Forms.Label();
            this.altitude_lb = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Speed_lb
            // 
            this.Speed_lb.AutoSize = true;
            this.Speed_lb.Location = new System.Drawing.Point(13, 13);
            this.Speed_lb.Name = "Speed_lb";
            this.Speed_lb.Size = new System.Drawing.Size(94, 13);
            this.Speed_lb.TabIndex = 0;
            this.Speed_lb.Text = "Speed : NaNkm/h";
            // 
            // altitude_lb
            // 
            this.altitude_lb.AutoSize = true;
            this.altitude_lb.Location = new System.Drawing.Point(12, 26);
            this.altitude_lb.Name = "altitude_lb";
            this.altitude_lb.Size = new System.Drawing.Size(81, 13);
            this.altitude_lb.TabIndex = 1;
            this.altitude_lb.Text = "Altitude : NaNm";
            // 
            // Telemetry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.altitude_lb);
            this.Controls.Add(this.Speed_lb);
            this.Name = "Telemetry";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Speed_lb;
        private System.Windows.Forms.Label altitude_lb;
    }
}

