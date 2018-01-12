namespace Common
{
    public class CaptchaOptions
    {
        private string _textChars; // 生成验证码用的字符
        private int _textLength; // 字符长度
        private int _width;
        private int _height;

        public string TextChars
        {
            get { return _textChars; }
            set { _textChars = (string.IsNullOrEmpty(value) || value.Trim().Length < 3) ? "ACDEFGHJKLMNPQRSTUVWXYZ2346789" : value.Trim(); }
        }

        public int TextLength
        {
            get { return _textLength; }
            set { _textLength = value < 3 ? 3 : value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value < (TextLength * 18) ? TextLength * 18 : value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value < 32 ? 32 : value; }
        }

        /// <summary>
        /// 字体扭曲级别
        /// </summary>
        public Level FontWarp { get; set; }

        /// <summary>
        /// 背景噪点级别
        /// </summary>
        public Level BackgroundNoise { get; set; }

        /// <summary>
        /// 线条杂色级别
        /// </summary>
        public Level LineNoise { get; set; }

        public CaptchaOptions()
        {
            TextLength = 4;
            Width = 160;
            Height = 40;
            FontWarp = Level.Medium;
            BackgroundNoise = Level.Low;
            LineNoise = Level.Low;
        }
    }
}
