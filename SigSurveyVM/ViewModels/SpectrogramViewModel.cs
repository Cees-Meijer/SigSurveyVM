using OxyPlot;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SurveyVM
{
    class SpectrogramViewModel :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private BitmapSource currentImage;
        public int Width { get { return img_width; } }
        public int Height { get { return img_height; } }
        private WriteableBitmap writeableBmp;
        

        public byte[] ImageData { get; set; }
        public int Bytesperpixel { get { return 4; } }
        public int Stride { get; set; }
        private int img_width, img_height;
        private int scale_min, scale_max;
        private OxyPalette palette;
        private int x_axis_height=10;
        private int x_axis_tickspacing = 20;
        private int tick_count = 0;
        private Color axis_color = Color.FromRgb(0, 0, 0);
        private Color background_color = Color.FromRgb(255,255,255);

        public bool Init(ref Image image)
        {

            img_width = (int)image.Width; img_height = (int)image.Height;
            Init(ref image, img_width, img_height);
            return true;
        }
        /// <summary>
        /// Initialise the Spectrogram chart, to an Image context, with preset width and height
        /// </summary>
        /// <param name="image">Reference to an WPF Image component</param>
        /// <param name="setWidth">Set the width</param>
        /// <param name="setHeight">Set the Height</param>
        /// <returns></returns>
        public bool Init(ref Image image,int setWidth,int setHeight)
        {
            scale_min = -1000; scale_max = 1000;
            Stride = setWidth * Bytesperpixel;
            writeableBmp = BitmapFactory.New(setWidth, setHeight);
            
            ImageData = new byte[setWidth * setHeight * Bytesperpixel];
            Palette = OxyPalette.Interpolate(256, OxyColors.Blue, OxyColors.Green, OxyColors.Yellow, OxyColors.DarkRed);

            tick_count = 0;
            return true;
        }

            /// <summary>
            /// Scroll a bitmap image, stored as a byte-array, pixel(s) to the left by copying all bytes row by row
            /// </summary>
            /// <param name="cols">Nr of columns (pixels) to scroll</param>

            public void ScrollLeft(int cols)
        {
            int row;
           
            for (row = 0; row < img_height; row++)
            {
                int start_row = row * Stride; // Byte index where the row starts
                                              //Copy all bytes in the row 4 bytes left
                Buffer.BlockCopy(ImageData, start_row + Bytesperpixel * cols, ImageData, start_row, Stride - Bytesperpixel * cols);
            }
            Draw();
        }
        /// <summary>
        /// Add a trace of data on the right side of the plot, and scroll all data to the left
        /// </summary>
        /// <param name="TraceData">An array with data. Can be any length, it will be scaled over the total height.</param>
        public void AddTrace(short[]TraceData)
        {
            if (TraceData.Length == 0) return;
            byte[] trace = new byte[img_height];
        

            float index_step = (float) TraceData.Length/trace.Length;
            float index = 0;
            int col = img_width-1;
            // Draw the trace from the top down, but leave room for the X-Axis ticks
            for (int row = 0; row < img_height-x_axis_height; row++)
            {
                
                Int32 datapoint = TraceData[(int)index];
                int col_pos = row * Stride + col*Bytesperpixel; // Byte index for this column

                byte pt = (byte)Map(datapoint, scale_min, scale_max, 0, 255);
                var clr = palette.Colors[pt];
                ImageData[col_pos] =  clr.B;
                ImageData[col_pos+1] = clr.G;
                ImageData[col_pos+2] = clr.R;
                ImageData[col_pos+3] = clr.A;
                index = index + index_step;
                //Console.WriteLine("{0},{1}", col_pos, pt);
            }
            // Draw the X-Axis tick if we are at the tick-spacing.
            for (int row = img_height - x_axis_height; row < img_height; row++)
            {
                Color clr = background_color;
                if (tick_count % x_axis_tickspacing == 0)
                {
                    clr = axis_color;
                }
                int col_pos = row * Stride + col * Bytesperpixel; // Byte index for this column
                ImageData[col_pos] = clr.B;
                ImageData[col_pos + 1] = clr.G;
                ImageData[col_pos + 2] = clr.R;
                ImageData[col_pos + 3] = clr.A;

            }
            tick_count += 1;
            ScrollLeft(1);
            
        }

        private float Map(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        public void Draw()
        {

            using (writeableBmp.GetBitmapContext())
            {
                writeableBmp.FromByteArray(ImageData);
            }
            CurrentImage = writeableBmp;
        }

        public BitmapSource CurrentImage
        {
            get { return currentImage; }
            set
            {
                currentImage = value;
                PropertyChanged?.Invoke(
                    this, new PropertyChangedEventArgs(nameof(CurrentImage)));
            }
        }
        public void SetMinMax(int Min,int Max)
        {
            Scale_min = Min;Scale_max = Max;
        }

        public int Img_width { get => img_width; set => img_width = value; }
        public int Scale_min { get => scale_min; set => scale_min = value; }
        public int Scale_max { get => scale_max; set => scale_max = value; }
        public OxyPalette Palette { get => palette; set => palette = value; }
        public int X_axis_height { get => x_axis_height; set => x_axis_height = value; }
        public int X_axis_tickspacing { get => x_axis_tickspacing; set => x_axis_tickspacing = value; }
        public Color Axis_color { get => axis_color; set => axis_color = value; }
        public Color Background_color { get => background_color; set => background_color = value; }
    }
}
