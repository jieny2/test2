using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Common
{
    public class Captcha
    {
        private readonly Random _rand;

        public CaptchaOptions CaptchaOptions { get; set; }
        public string Text { get; private set; }

        private static readonly string[] RandomFontFamily = { "arial", "arial black", "comic sans ms", "courier new", "estrangelo edessa", "franklin gothic medium", "georgia", "lucida console", "lucida sans unicode", "mangal", "microsoft sans serif", "palatino linotype", "sylfaen", "tahoma", "times new roman", "trebuchet ms", "verdana" };
        private static readonly Color[] RandomColor = { Color.Red, Color.Green, Color.Blue, Color.Black, Color.Purple, Color.Orange, Color.Aqua };

        /// <summary>
        /// 验证码：默认4个字符，宽度为160，高度为40，字体扭曲级别为中，背景噪点级别为低，线条杂色级别为低
        /// 字符随机内容为【ACDEFGHJKLMNPQRSTUVWXYZ2346789】
        /// </summary>
        public Captcha() : this(new CaptchaOptions())
        {

        }

        public Captcha(CaptchaOptions options)
        {
            CaptchaOptions = options;
            _rand = new Random();
            Text = GenerateRandomText();
        }


        private string GetRandomFontFamily()
        {
            return RandomFontFamily[_rand.Next(0, RandomFontFamily.Length)];
        }

        /// <summary> 
        /// 验证码文本
        /// </summary> 
        private string GenerateRandomText()
        {
            string txtChars = CaptchaOptions.TextChars;
            if (string.IsNullOrEmpty(txtChars))
            {
                txtChars = "ACDEFGHJKLMNPQRSTUVWXYZ2346789";
            }
            var sb = new StringBuilder(CaptchaOptions.TextLength);
            int maxLength = txtChars.Length;
            for (int n = 0; n <= CaptchaOptions.TextLength - 1; n++)
            {
                sb.Append(txtChars.Substring(_rand.Next(maxLength), 1));
            }

            return sb.ToString();
        }

        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF(_rand.Next(xmin, xmax), _rand.Next(ymin, ymax));
        }

        private Color GetRandomColor()
        {
            return RandomColor[_rand.Next(0, RandomColor.Length)];
        }

        private Font GetFont()
        {
            float fsize;
            string fname = GetRandomFontFamily();

            switch (CaptchaOptions.FontWarp)
            {
                case Level.Low:
                    fsize = Convert.ToInt32(CaptchaOptions.Height * 0.8);
                    break;
                case Level.Medium:
                    fsize = Convert.ToInt32(CaptchaOptions.Height * 0.85);
                    break;
                case Level.High:
                    fsize = Convert.ToInt32(CaptchaOptions.Height * 0.9);
                    break;
                case Level.Extreme:
                    fsize = Convert.ToInt32(CaptchaOptions.Height * 0.95);
                    break;
                default:
                    fsize = Convert.ToInt32(CaptchaOptions.Height * 0.7);
                    break;
            }

            return new Font(fname, fsize, FontStyle.Bold);
        }

        private static GraphicsPath TextPath(string s, Font f, Rectangle r)
        {
            var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            var gp = new GraphicsPath();
            gp.AddString(s, f.FontFamily, (int)f.Style, f.Size, r, sf);

            return gp;
        }

        private void WarpText(GraphicsPath textPath, Rectangle rect)
        {
            float warpDivisor;
            float rangeModifier;

            switch (CaptchaOptions.FontWarp)
            {
                case Level.Low:
                    warpDivisor = 6F;
                    rangeModifier = 1F;
                    break;
                case Level.Medium:
                    warpDivisor = 5F;
                    rangeModifier = 1.3F;
                    break;
                case Level.High:
                    warpDivisor = 4.5F;
                    rangeModifier = 1.4F;
                    break;
                case Level.Extreme:
                    warpDivisor = 4F;
                    rangeModifier = 1.5F;
                    break;
                default:
                    return;
            }

            var rectF = new RectangleF(Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height);

            int hrange = Convert.ToInt32(rect.Height / warpDivisor);
            int wrange = Convert.ToInt32(rect.Width / warpDivisor);
            int left = rect.Left - Convert.ToInt32(wrange * rangeModifier);
            int top = rect.Top - Convert.ToInt32(hrange * rangeModifier);
            int width = rect.Left + rect.Width + Convert.ToInt32(wrange * rangeModifier);
            int height = rect.Top + rect.Height + Convert.ToInt32(hrange * rangeModifier);

            if (left < 0)
                left = 0;
            if (top < 0)
                top = 0;
            if (width > CaptchaOptions.Width)
                width = CaptchaOptions.Width;
            if (height > CaptchaOptions.Height)
                height = CaptchaOptions.Height;

            PointF leftTop = RandomPoint(left, left + wrange, top, top + hrange);
            PointF rightTop = RandomPoint(width - wrange, width, top, top + hrange);
            PointF leftBottom = RandomPoint(left, left + wrange, height - hrange, height);
            PointF rightBottom = RandomPoint(width - wrange, width, height - hrange, height);

            var points = new[] { leftTop, rightTop, leftBottom, rightBottom };
            var m = new Matrix();
            m.Translate(0, 0);
            textPath.Warp(points, rectF, m, WarpMode.Perspective, 0);
        }

        private void AddNoise(Graphics g, Rectangle rect)
        {
            int density;
            int size;

            switch (CaptchaOptions.BackgroundNoise)
            {
                case Level.None:
                    goto default;
                case Level.Low:
                    density = 30;
                    size = 40;
                    break;
                case Level.Medium:
                    density = 18;
                    size = 40;
                    break;
                case Level.High:
                    density = 16;
                    size = 39;
                    break;
                case Level.Extreme:
                    density = 12;
                    size = 38;
                    break;
                default:
                    return;
            }
            var br = new SolidBrush(GetRandomColor());
            int max = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size);
            for (int i = 0; i <= Convert.ToInt32((rect.Width * rect.Height) / density); i++)
            {
                g.FillEllipse(br, _rand.Next(rect.Width), _rand.Next(rect.Height), _rand.Next(max), _rand.Next(max));
            }
            br.Dispose();
        }

        private PointF RandomPoint(Rectangle rect)
        {
            return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        private void AddLine(Graphics g, Rectangle rect)
        {
            int length;
            float width;
            int linecount;

            switch (CaptchaOptions.LineNoise)
            {
                case Level.None:
                    goto default;
                case Level.Low:
                    length = 4;
                    width = Convert.ToSingle(CaptchaOptions.Height / 31.25);
                    linecount = 1;
                    break;
                case Level.Medium:
                    length = 5;
                    width = Convert.ToSingle(CaptchaOptions.Height / 27.7777);
                    linecount = 1;
                    break;
                case Level.High:
                    length = 3;
                    width = Convert.ToSingle(CaptchaOptions.Height / 25);
                    linecount = 2;
                    break;
                case Level.Extreme:
                    length = 3;
                    width = Convert.ToSingle(CaptchaOptions.Height / 22.7272);
                    linecount = 3;
                    break;
                default:
                    return;
            }

            var pf = new PointF[length + 1];
            using (var p = new Pen(GetRandomColor(), width))
            {
                for (int l = 1; l <= linecount; l++)
                {
                    for (int i = 0; i <= length; i++)
                    {
                        pf[i] = RandomPoint(rect);
                    }
                    g.DrawCurve(p, pf, 1.75F);
                }
            }
        }

        /// <summary> 
        /// 渲染验证码图片
        /// </summary> 
        public Bitmap RenderImage()
        {
            Bitmap bmp = new Bitmap(CaptchaOptions.Width, CaptchaOptions.Height, PixelFormat.Format24bppRgb);

            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.Clear(Color.White);

                int charOffset = 0;
                double charWidth = CaptchaOptions.Width / CaptchaOptions.TextLength;
                Rectangle rectChar;
                List<Color> colorUsed = new List<Color>();

                foreach (char c in Text)
                {
                    using (Font fnt = GetFont())
                    {
                        Color color = GetRandomColor();
                        if (RandomColor.Length > Text.Length)
                        {
                            while (colorUsed.Contains(color))
                            {
                                color = GetRandomColor();
                            }
                        }
                        colorUsed.Add(color);
                        using (Brush fontBrush = new SolidBrush(color))
                        {
                            rectChar = new Rectangle(Convert.ToInt32(charOffset * charWidth), 0, Convert.ToInt32(charWidth), CaptchaOptions.Height);
                            GraphicsPath gp = TextPath(c.ToString(), fnt, rectChar);
                            WarpText(gp, rectChar);
                            gr.FillPath(fontBrush, gp);
                            charOffset += 1;
                        }
                    }
                }
                colorUsed.Clear();

                var rect = new Rectangle(new Point(0, 0), bmp.Size);
                AddNoise(gr, rect);
                AddLine(gr, rect);
            }

            return bmp;
        }

        public void RenderImage(out byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                RenderImage().Save(ms, ImageFormat.Jpeg);
                bytes = ms.ToArray();
            }
        }
    }
}
