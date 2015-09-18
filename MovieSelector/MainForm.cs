using System;
using System.Windows.Forms;

using MovieSelector.Controller;

namespace MovieSelector.View
{
    public partial class MainForm : Form, IMovieView
    {
        public MainForm()
        {
            InitializeComponent();

            playButton.Click += new EventHandler(playButton_Click);
            nextButton.Click += new EventHandler(nextButton_Click);

            configToolStripMenuItem.Click += new EventHandler(configToolStripMenuItem_Click);
            exitToolStripMenuItem.Click += new EventHandler(exitToolStripMenuItem_Click);
            aboutToolStripMenuItem.Click += new EventHandler(aboutToolStripMenuItem_Click);
        }

        public void setController(MovieController controller)
        {
            this.controller = controller;
        }

        public void setViewFilename(string name)
        {
            this.Filename = name;
            this.textBox.Text = this.Filename;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            controller.tryPlayVideo(true);
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            Filename = controller.getNextFileName();
            textBox.Text = Filename;
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Config Menu Clicked");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("About Menu Clicked");
        }

        public string Filename { get; set; }
    }
}
