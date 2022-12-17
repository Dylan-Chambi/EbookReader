using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EbookReader.Views
{
    public class CustomRadioButton : RadioButton
    {
        [System.ComponentModel.DefaultValue(typeof(Color), "0, 122, 204")]
        private Color checkedColor = Color.FromArgb(0, 122, 204);

        [System.ComponentModel.DefaultValue(typeof(Color), "0, 122, 204")]
        private Color unCheckedColor = Color.FromArgb(0, 122, 204);

        private Color hoverCheckedColor = Color.FromArgb(0, 122, 204);
        private Color hoverUnCheckedColor = Color.FromArgb(0, 122, 204);

        private Color tempCheckedColor = Color.FromArgb(0, 122, 204);
        private Color tempUnCheckedColor = Color.FromArgb(0, 122, 204);

        public Color CheckedColor
        {
            get { return checkedColor; }
            set
            {
                checkedColor = value;
                hoverCheckedColor = Color.FromArgb(checkedColor.R - 40 < 0 ? 0 : checkedColor.R - 40, checkedColor.G - 40 < 0 ? 0 : checkedColor.G - 40, checkedColor.B - 40 < 0 ? 0 : checkedColor.B - 40);
                tempCheckedColor = checkedColor;
                this.Invalidate();
                // if (this.Checked)
                // {
                //     this.FlatAppearance.CheckedBackColor = checkedColor;
                //     this.FlatAppearance.MouseOverBackColor = checkedColor;
                //     this.FlatAppearance.MouseDownBackColor = checkedColor;
                // }
            }
        }

        public Color UncheckedColor
        {
            get { return unCheckedColor; }
            set
            {
                unCheckedColor = value;
                hoverUnCheckedColor = Color.FromArgb(unCheckedColor.R - 40 < 0 ? 0 : unCheckedColor.R - 40, unCheckedColor.G - 40 < 0 ? 0 : unCheckedColor.G - 40, unCheckedColor.B - 40 < 0 ? 0 : unCheckedColor.B - 40);
                tempUnCheckedColor = unCheckedColor;
                this.Invalidate();
                // if (!this.Checked)
                // {
                //     this.FlatAppearance.CheckedBackColor = uncheckedColor;
                //     this.FlatAppearance.MouseOverBackColor = uncheckedColor;
                //     this.FlatAppearance.MouseDownBackColor = uncheckedColor;
                // }
            }
        }

        public CustomRadioButton()
        {
            this.MinimumSize = new Size(0, 21);
            this.Padding = new Padding(10, 6, 1, 7);
            this.hoverCheckedColor = Color.FromArgb(checkedColor.R - 40 < 0 ? 0 : checkedColor.R - 40, checkedColor.G - 40 < 0 ? 0 : checkedColor.G - 40, checkedColor.B - 40 < 0 ? 0 : checkedColor.B - 40);
            this.hoverUnCheckedColor = Color.FromArgb(unCheckedColor.R - 40 < 0 ? 0 : unCheckedColor.R - 40, unCheckedColor.G - 40 < 0 ? 0 : unCheckedColor.G - 40, unCheckedColor.B - 40 < 0 ? 0 : unCheckedColor.B - 40);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            //Fields
            Graphics graphics = pevent.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            float rbBorderSize = 24F;
            float rbCheckSize = 16F;
            RectangleF rectRbBorder = new RectangleF()
            {
                X = 0,
                Y = (this.Height - rbBorderSize) / 2, //Center
                Width = rbBorderSize,
                Height = rbBorderSize
            };
            RectangleF rectRbCheck = new RectangleF()
            {
                X = (rbBorderSize - rbCheckSize) / 2, //Center
                Y = (this.Height - rbCheckSize) / 2, //Center
                Width = rbCheckSize,
                Height = rbCheckSize
            };
            //Drawing
            using (Pen penBorder = new Pen(checkedColor, 1.6F))
            using (SolidBrush brushRbCheck = new SolidBrush(checkedColor))
            using (SolidBrush brushText = new SolidBrush(this.ForeColor))
            {
                //Draw surface
                graphics.Clear(this.BackColor);
                //Draw Radio Button
                if (this.Checked)
                {
                    graphics.DrawEllipse(penBorder, rectRbBorder);//Circle border
                    graphics.FillEllipse(brushRbCheck, rectRbCheck); //Circle Radio Check
                }
                else
                {
                    penBorder.Color = unCheckedColor;
                    graphics.DrawEllipse(penBorder, rectRbBorder); //Circle border
                }
                //Draw text
                graphics.DrawString(this.Text, this.Font, brushText,
                    rbBorderSize + 8, (this.Height - TextRenderer.MeasureText(this.Text, this.Font).Height) / 2);//Y=Center
            }
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            // change the pointer to a hand
            this.Cursor = Cursors.Hand;

            // make the color more darker, like a hover effect
            this.tempCheckedColor = checkedColor;
            this.tempUnCheckedColor = unCheckedColor;
            this.checkedColor = hoverCheckedColor;
            this.unCheckedColor = hoverUnCheckedColor;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            // change the pointer to a hand
            this.Cursor = Cursors.Default;

            // make the color the original color
            this.checkedColor = tempCheckedColor;
            this.unCheckedColor = tempUnCheckedColor;
            this.Invalidate();
        }
    }
}
