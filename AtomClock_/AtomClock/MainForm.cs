using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AtomClock
{
    public partial class MainForm : Form
    {
        Point moveStart;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            GraphicsPath gp = new GraphicsPath();
            int baseRadius = ClientSize.Width > ClientSize.Height ? ClientSize.Height / 2 : ClientSize.Width / 2;

            gp.AddBezier(
                new Point(ClientSize.Width / 2, ClientSize.Height / 4),
                new Point(ClientSize.Width / 3, 0),
                new Point(0, ClientSize.Height * 2 / 4),
                new Point(ClientSize.Width / 2, ClientSize.Height)
                );
            gp.AddBezier(
                new Point(ClientSize.Width / 2, ClientSize.Height),
                new Point(ClientSize.Width, ClientSize.Height * 2 / 4),
                new Point(ClientSize.Width - ClientSize.Width / 3, 0),
                new Point(ClientSize.Width / 2, ClientSize.Height / 4)
                );

            PathGradientBrush br = new PathGradientBrush(gp)
            {
                CenterPoint = new PointF(ClientSize.Width / 2, ClientSize.Height * 5 / 9),
                CenterColor = Color.White,
                SurroundColors = new Color[] { Color.MediumVioletRed }
            };

            gr.FillPath(br, gp);

            DrawArrow(Color.Yellow, 2, gr, baseRadius * 2 / 5, TimeReceiver.GetTime().Second * 6); //Get time from the server
            DrawArrow(Color.DarkBlue, 5, gr, baseRadius / 3, TimeReceiver.GetTime().Minute * 6);  //Get time from the server
            DrawArrow(Color.DeepPink, 8, gr, baseRadius / 4, TimeReceiver.GetTime().Hour * 30);   //Get time from the server
            DrawScale(Color.Black, 1, gr, baseRadius, -30);
        }

        private void DrawScale(Color color, int penWidth, Graphics gr, int radius, int length)
        {
            for (int i = 1; i < 13; ++i)
            {
                GraphicsContainer container =
                    gr.BeginContainer(
                        new Rectangle(ClientSize.Width / 2, ClientSize.Height * 5 / 9, ClientSize.Width / 2, ClientSize.Height / 2),
                        new Rectangle(0, 0, ClientSize.Width, ClientSize.Height),
                        GraphicsUnit.Pixel);
                gr.RotateTransform(i * 30);
                gr.DrawString(i.ToString(), new Font(Font.FontFamily, 30, FontStyle.Bold), new SolidBrush(Color.Blue), length, -radius);
                gr.EndContainer(container);
            }
        }

        private void DrawArrow(Color color, int penWidth, Graphics gr, int length, int angle)
        {
            GraphicsContainer container =
                gr.BeginContainer(
                    new Rectangle(ClientSize.Width / 2, ClientSize.Height * 5 / 9, ClientSize.Width, ClientSize.Height),
                    new Rectangle(0, 0, ClientSize.Width, ClientSize.Height),
                    GraphicsUnit.Pixel);
            gr.RotateTransform(angle);

            gr.DrawLine(
                new Pen(color, penWidth),
                new Point(0, 0),
                new Point(0, -length)
            );
            gr.EndContainer(container);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
            label1.Text = TimeReceiver.GetTime().ToString("T");  //Get time from the server
        }


        private void ButExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                moveStart = new Point(e.X, e.Y);
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != 0)
            {
                Point deltaPos = new Point(e.X - moveStart.X, e.Y - moveStart.Y);
                this.Location = new Point(this.Location.X + deltaPos.X,
                this.Location.Y + deltaPos.Y);
            }
        }
    }
}
