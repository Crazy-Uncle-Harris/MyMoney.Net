#define TRACE
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Walkabout.Utilities;
using Walkabout.Charts;

#if PerformanceBlocks
using Microsoft.VisualStudio.Diagnostics.PerformanceProvider;
#endif

namespace LovettSoftware.Charts
{
    /// <summary>
    /// It is used by the TrendGraph to create stacked translucent area graphs, with interactive mouse pointer
    /// for selecting specific points.  There is one area graph per "ChartSeries".
    /// </summary>
    public class AreaChart : Grid
    {
        NumberFormatInfo nfi;
        ChartData data;
        Size controlSize = new Size(1000, 200);
        double graphMin = 0;
        double graphMax = 0;
        double graphWidth = 0;
        List<Geometry> geometries = new List<Geometry>();
        ScaleTransform scale;
        TransformGroup transform;
        Shape pointer;
        Border tooltip;
        ChartDataSeries selectedSeries;
        StackPanel legend;

        public AreaChart()
        {
        }

        public AreaChart(ChartData data)
        {
            this.Data = data;
        }

        public ToolTipGenerator ToolTipGenerator { get; set; }

        public ChartDataValue Selected { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
#if PerformanceBlocks
            using (PerformanceBlock.Create(ComponentId.Money, CategoryId.View, MeasurementId.AreaChartInitialize))
            {
#endif
                base.OnInitialized(e);

                nfi = new NumberFormatInfo();
                nfi.CurrencyDecimalDigits = 0;
                nfi.CurrencySymbol = string.Empty;
                nfi.CurrencyNegativePattern = 0;
                this.Background = Brushes.Transparent; // so mouseclicks work.

#if PerformanceBlocks
            }
#endif
        }

        public ChartData Data
        {
            get { return data; }
            set
            {
                data = value; 
                Relayout();
            }
        }


        void Relayout()
        {
            HidePointer();
            this.Children.Clear();
            this.ColumnDefinitions.Clear();
            this.RowDefinitions.Clear();
            if (data != null)
            {
                ShowAreaGraph();
                TransformGraph();
            }
            InvalidateArrange();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            controlSize = sizeInfo.NewSize;
            TransformGraph();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size result = base.MeasureOverride(constraint);
            return result;
        }

        void TransformGraph()
        {
            transform = new TransformGroup();
            double height = graphMax - graphMin;

            if (graphWidth > 0 && height > 0)
            {
                double scaleX = controlSize.Width / graphWidth;
                double scaleY = -(controlSize.Height*.9) / height;
                transform.Children.Add(scale = new ScaleTransform(scaleX, scaleY));
                double minScale = graphMin * scaleY;
                transform.Children.Add(new TranslateTransform(0, controlSize.Height - minScale));

                foreach (Geometry g in geometries)
                {
                    g.Transform = transform;
                }
            }
        }

        public ChartDataSeries SelectedSeries { get { return selectedSeries; } set { selectedSeries = value; OnSelectedSeriesChanged(); } }

        void UpdatePointer(Point pos)
        {
            if (data != null && data.Series.Count > 0 && scale != null && scale.ScaleX > 0)
            {
                if (selectedSeries == null) {
                    selectedSeries = data.Series[data.Series.Count-1];
                }

                Point legendPos = this.TransformToDescendant(legend).Transform(pos);
                Rect legendArea = new Rect(0, 0, legend.ActualWidth, legend.ActualHeight);
                bool insideLegend = (legendArea.Contains(legendPos));

                if (pointer == null)
                {
                    PathGeometry diamond = new PathGeometry();
                    PathFigure figure = new PathFigure();
                    diamond.Figures.Add(figure);
                    figure.IsClosed = true;
                    figure.IsFilled = true;
                    figure.StartPoint = new Point(0, -5);
                    figure.Segments.Add(new LineSegment(new Point(5, 0), true));
                    figure.Segments.Add(new LineSegment(new Point(0, 5), true));
                    figure.Segments.Add(new LineSegment(new Point(-5, 0), true));

                    pointer = new Path()
                    {
                        Data = diamond,
                        Fill = Brushes.Red
                    };
                    this.Children.Add(pointer);
                }
                if (tooltip == null)
                {
                    tooltip = new Border();
                    tooltip.Padding = new Thickness(2);
                    tooltip.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    tooltip.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    tooltip.BorderBrush = Brushes.Black;
                    tooltip.BorderThickness = new Thickness(1);
                    tooltip.Background = Brushes.White;
                    this.Children.Add(tooltip);
                }

                ChartDataSeries series = selectedSeries;
                var values = series.Values;

                int i = (int)(pos.X / scale.ScaleX);
                if (i >= 0 && i < values.Count)
                {
                    ChartDataValue v = values[i];
                    Selected = v;

                    UIElement content = null;
                    if (this.ToolTipGenerator != null)
                    {
                        content = this.ToolTipGenerator(v);
                    }
                    else
                    {
                        var label = new TextBlock();
                        label.Foreground = Brushes.Black;
                        label.Text = v.Label;
                        content = label;
                    }
                    tooltip.Child = content;

                    if (insideLegend)
                    {
                        tooltip.Visibility = System.Windows.Visibility.Hidden;
                    }
                    else
                    {
                        tooltip.UpdateLayout();

                        double tipPositionX = pos.X;
                        if (tipPositionX + tooltip.ActualWidth > this.ActualWidth)
                        {
                            tipPositionX = this.ActualWidth - tooltip.ActualWidth;
                        }
                        double tipPositionY = pos.Y - tooltip.ActualHeight - 4;
                        if (tipPositionY < 0)
                        {
                            tipPositionY = 0;
                        }
                        tooltip.Margin = new Thickness(tipPositionX, tipPositionY, 0, 0);
                        tooltip.Visibility = System.Windows.Visibility.Visible;
                    }

                    double value = v.Value;
                    if (series.Flipped) value = -value;
                    Point pointerPosition = transform.Transform(new Point(i, value));
                    pointer.RenderTransform = new TranslateTransform(pointerPosition.X, pointerPosition.Y);
                    pointer.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    tooltip.Visibility = System.Windows.Visibility.Hidden;
                    pointer.Visibility = System.Windows.Visibility.Hidden;
                }
                
            }
            else
            {
                // todo
            }
        }

        internal void OnSeriesColorChanged()
        {
            throw new NotImplementedException();
        }

        void HidePointer()
        {
            Selected = null;
            if (pointer != null)
            {
                this.Children.Remove(pointer);
                pointer = null;
            }
            if (tooltip != null)
            {
                this.Children.Remove(tooltip);
                tooltip = null;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            HidePointer();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point pos = e.GetPosition(this);
            UpdatePointer(pos);
            base.OnMouseMove(e);
        }
        
        void ShowAreaGraph()
        {
            graphMin = 0;
            graphMax = 0;
            graphWidth = 0;
            geometries.Clear();
            this.Children.Clear();

            legend = new StackPanel();
            legend.Orientation = Orientation.Vertical;
            legend.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            legend.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            legend.Margin = new Thickness(4);

            PopulateTheChart(legend);

            LineGeometry line = new LineGeometry(new Point(0, 0), new Point(graphWidth, 0));
            geometries.Add(line);

            Path lineShape = new Path();
            lineShape.Data = line;
            lineShape.StrokeThickness = 1;
            lineShape.Stroke = Brushes.Red;
            this.Children.Add(lineShape);

            // legend on top
            this.Children.Add(legend);
        }

        void PopulateTheChart(StackPanel legend)
        {
            selectedSeries = null;

            foreach (ChartDataSeries s in data.Series)
            {
                if (s.Values.Count > 0)
                {
                    PathGeometry path = new PathGeometry();
                    PathFigure figure = new PathFigure();
                    path.Figures.Add(figure);
                    figure.IsClosed = true;
                    double x = 0;
                    double min = (from i in s.Values select i.Value).Min();
                    double max = (from i in s.Values select i.Value).Max();

                    bool flip = false;
                    if (s.Flipped)
                    {
                        double temp = max;
                        max = -min;
                        min = -temp;
                        flip = true;
                    }
                    graphMin = Math.Min(graphMin, min);
                    graphMax = Math.Max(graphMax, max);

                    figure.StartPoint = new Point(0, 0);

                    // Add the grid column labels along the bottom row.
                    foreach (ChartDataValue cv in s.Values)
                    {
                        double v = cv.Value;
                        if (flip) v = -v;
                        figure.Segments.Add(new LineSegment(new Point(x, v), true));
                        x++;
                    }
                    if (x > 0)
                    {
                        x--;
                    }
                    figure.Segments.Add(new LineSegment(new Point(x, 0), true));

                    Path shape = new Path();
                    shape.Data = path;
                    geometries.Add(path);

                    Color c = (Color)s.Category.WpfColor;

                    HlsColor hls = new HlsColor(c);
                    hls.Darken(.25f);

                    shape.Fill = new SolidColorBrush(Color.FromArgb(0xA0, c.R, c.G, c.B));
                    shape.Stroke = new SolidColorBrush(hls.Color);
                    shape.StrokeThickness = 1;
                    this.Children.Add(shape);

                    graphWidth = Math.Max(graphWidth, x);

                    AddLengenEntry(legend, s, shape);
                }
            }
        }

        void AddLengenEntry(StackPanel legend, ChartDataSeries s, Path shape)
        {
            StackPanel legendEntry = new StackPanel();
            legendEntry.Margin = new Thickness(2);
            legendEntry.Orientation = Orientation.Horizontal;
            legendEntry.Background = Brushes.Transparent;
            legendEntry.Tag = s;
            legendEntry.PreviewMouseDown += OnSelectLegendEntry;
            legendEntry.IsHitTestVisible = true;
            legendEntry.MouseEnter += OnLegendEntryMouseEnter;
            legendEntry.MouseLeave += OnLegendEntryMouseLeave; 
            legend.Children.Add(legendEntry);

            s.UserData = shape;
            
            Rectangle swatch = new Rectangle();
            swatch.Margin = new Thickness(4,1,2,1);
            swatch.Width = 12;
            swatch.Height = 12;
            swatch.Stroke = shape.Stroke;
            swatch.StrokeThickness = 1;
            swatch.Fill = shape.Fill;
            legendEntry.Children.Add(swatch);

            TextBlock label = new TextBlock();
            label.Text = s.Category.Name;
            label.Margin = new Thickness(2, 1, 4, 1);
            legendEntry.Children.Add(label);
        }

        void OnLegendEntryMouseEnter(object sender, MouseEventArgs e)
        {
            StackPanel legendEntry = (StackPanel)sender;
            legendEntry.Background = new SolidColorBrush(Color.FromArgb(0x40, 0x00, 0x00, 0x00));
        }

        private void OnLegendEntryMouseLeave(object sender, MouseEventArgs e)
        {
            StackPanel legendEntry = (StackPanel)sender;
            legendEntry.Background = Brushes.Transparent;
        }

        void OnSelectLegendEntry(object sender, MouseButtonEventArgs e)
        {
            // bring this legend entry to the front so user can see item values.
            StackPanel legendEntry = (StackPanel)sender;
            ChartDataSeries series = (ChartDataSeries)legendEntry.Tag;
            this.SelectedSeries = series;
        }

        private void OnSelectedSeriesChanged()
        {
            ChartDataSeries selected = this.SelectedSeries;
            // bring the "Path" shape for this series to the front.
            Path shape = (Path)selected.UserData;

            DoubleAnimation fadeOut = new DoubleAnimation() { To = 0.0, Duration = new Duration(TimeSpan.FromMilliseconds(200)) };
            Storyboard sb = new Storyboard();
            sb.Children.Add(fadeOut);
            sb.Completed += OnFadeCompleted;
            Storyboard.SetTarget(fadeOut, shape);
            Storyboard.SetTargetProperty(fadeOut, new PropertyPath("Opacity"));
            sb.Begin();
        }

        private void OnFadeCompleted(object sender, EventArgs e)
        {
            ChartDataSeries selected = this.SelectedSeries;
            Path shape = (Path)selected.UserData;

            // bring the "Path" shape for this series to the front.
            this.Children.Remove(shape);
            this.Children.Add(shape);

            DoubleAnimation fadeIn = new DoubleAnimation() { To = 1.0, Duration = new Duration(TimeSpan.FromMilliseconds(200)) };
            Storyboard sb = new Storyboard();
            sb.Children.Add(fadeIn);
            Storyboard.SetTarget(fadeIn, shape);
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath("Opacity"));
            sb.Begin();

        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        void AddSeries(Grid grid, ChartDataSeries s, double availableHeight)
        {
            int index = 0;
            // Add the actual grid columns
            foreach (ChartDataValue cv in s.Values)
            {

                // create new column object.
                Color c1 = ChartColors.GetColor(index);
                double colWidth = grid.ColumnDefinitions[index].Width.Value;
                var min = (from i in s.Values select i.Value).Min();
                var max = (from i in s.Values select i.Value).Min();
                var range = max - min;
                ChartColumn col = new ChartColumn(cv, nfi, c1, colWidth - 1, availableHeight, range);
                Grid.SetColumn(col, index++);
                Grid.SetRow(col, 1);
                col.VerticalAlignment = VerticalAlignment.Bottom;
                grid.Children.Add(col);
            }
        }

    }

    public class ChartColumn : StackPanel
    {
        ChartDataValue value;
        Rectangle r;
        double currentValue;
        double availableHeight;
        double range;
        TextBlock vtext;

        static DependencyProperty ColumnValueProperty = DependencyProperty.Register("ColumnValue", typeof(double), typeof(ChartColumn));

        public ChartColumn(ChartDataValue cv, NumberFormatInfo nfi, Color c1, double colWidth, double availableHeight, double range)
        {
            this.availableHeight = availableHeight;
            if (range == 0)
            {
                range = availableHeight;
            }
            this.range = range;

            vtext = new TextBlock();

            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("ColumnValue");
            binding.Converter = new NumberConverter(nfi);

            vtext.SetBinding(TextBlock.TextProperty, binding);
            vtext.HorizontalAlignment = HorizontalAlignment.Center;

            r = new Rectangle();
            r.Width = colWidth;
            r.Height = 0;
            r.RadiusX = r.RadiusY = 2;

            HlsColor hls = new HlsColor(c1);
            hls.Lighten(.3f);
            Color c2 = hls.Color;
            hls = new HlsColor(c1);
            hls.Darken(.3f);
            Color stroke = hls.Color;

            r.Stroke = new SolidColorBrush(stroke);
            r.StrokeThickness = 1;
            r.Fill = new LinearGradientBrush(c1, c2, new Point(0, 0), new Point(0, 1));

            TextBlock label = new TextBlock();
            label.Text = cv.Label;
            label.HorizontalAlignment = HorizontalAlignment.Center;

            this.Children.Add(vtext);
            this.Children.Add(r);
            this.Children.Add(label);

            this.value = cv;
            this.SetValue(ColumnValueProperty, cv.Value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public ChartDataValue Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                this.BeginAnimation(ColumnValueProperty,
                        new DoubleAnimation(value.Value,
                        new Duration(TimeSpan.FromMilliseconds(500))));
            }
        }

        public double ColumnValue
        {
            get { return this.currentValue; }
            set
            {
                this.currentValue = value;
                double height = (Math.Abs(value) * this.availableHeight) / this.range;
                r.Height = height;
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ColumnValueProperty)
            {
                ColumnValue = (double)e.NewValue;
            }
            else
            {
                base.OnPropertyChanged(e);
            }
        }

    }

    static public class ChartColors
    {

        static IList<Color> colors;
        static Random r;

        public static Color GetColor(int i)
        {
            if (colors == null)
            {
                colors = new List<Color>();
                foreach (Color preference in new Color[] { 
                                                Colors.SkyBlue,
                                                Colors.Pink,
                                                Colors.LightSalmon,
                                                Colors.DarkSeaGreen,
                                                Colors.Silver,
                                                Colors.PaleGreen,
                                                Colors.LemonChiffon,
                                                Colors.Wheat,
                                            })
                {
                    colors.Add(preference);
                }
            }
            if (r == null)
            {
                r = new Random(Environment.TickCount);
            }
            while (i >= colors.Count)
            {
                Color c = Color.FromRgb((byte)r.Next(80, 255), (byte)r.Next(80, 255), (byte)r.Next(80, 255));
                colors.Add(c);
            }
            return (Color)colors[i];
        }

    }
}

