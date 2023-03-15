using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using AISOptimization.Utils;

using ChartDirector;

using Wpf.Ui.Appearance;


namespace AISOptimization.Views.Pages;

public static class ColorExtension
{
    public static int ToInt(this Color c)
    {
        return (int) (((c.A << 24) | (c.R << 16) | (c.G << 8) | c.B) & 0xffffffffL);
    }
}

/// <summary>
/// Элемент для отображения 3D поверхности
/// </summary>
public partial class ChartDirectorSurface : IViewWithVM<ChartDirectorChartVM>
{
    private ChartDirectorChartVM _viewModel;

    // 3D view angles
    private double m_elevationAngle = 30;
    private bool m_isDragging;

    // Keep track of mouse drag
    private int m_lastMouseX = -1;
    private int m_lastMouseY = -1;
    private double m_rotationAngle = 45;

    public ChartDirectorSurface(ChartDirectorChartVM vm)
    {
        ViewModel = vm;
        InitializeComponent();
        WPFChartViewer1.ChartSizeMode = WinChartSizeMode.AutoSize;
        WPFChartViewer1.updateViewPort(true, false);
    }

    public ChartDirectorChartVM ViewModel
    {
        get => _viewModel;
        set
        {
            _viewModel = value;
            _viewModel.OptimizationResultVmChanged += ViewModelOnOptimizationResultVmChanged;
        }
    }

    private void ViewModelOnOptimizationResultVmChanged(object? sender, EventArgs e)
    {
        createChart(WPFChartViewer1, 1000, 1500);
    }
    
    public void createChart(WPFChartViewer viewer, int width, int height)
    {
        if (ViewModel.OptimizationResultVM is null)
        {
            return;
        }

        var k = 0;

        //
        // for (int i = 0; i < dataX.Count; i++)
        // {
        //     for (int j = 0; j < dataY.Count; j++)
        //     {
        //         mathModel.p1.parameter.Value = dataX[i];
        //         mathModel.p2.parameter.Value = dataY[j];
        //         dataZ[j * dataX.Count + i] = mathModel.Function();
        //     }
        // }

        // Create a SurfaceChart object of size 680 x 580 pixels
        var c = new SurfaceChart(width, height);

        // Set the center of the plot region at (310, 280), and set width x depth x height to
        // 320 x 320 x 240 pixels
        var regionWidth = (int) (Math.Sqrt(width * width / 2) - width * 0.2);
        var zHeight = (int) Math.Sqrt(height * height - regionWidth * regionWidth * 2);
        c.setPlotRegion(width / 2, height / 2, regionWidth, regionWidth, regionWidth);

        // Set the elevation and rotation angles to 30 and 45 degrees
        c.setViewAngle(m_elevationAngle, m_rotationAngle);

        if (m_isDragging)
        {
            c.setShadingMode(Chart.RectangularFrame);
        }

        // Set the data to use to plot the chart
        c.setData(ViewModel.DataX, ViewModel.DataY, ViewModel.DataZ);

        // Spline interpolate data to a 80 x 80 grid for a smooth surface
        c.setInterpolation(80, 80);

        // Use semi-transparent black (c0000000) for x and y major surface grid lines. Use
        // dotted style for x and y minor surface grid lines.
        var majorGridColor = unchecked((int) 0xc0000000);
        var minorGridColor = c.dashLineColor(majorGridColor, Chart.DotLine);
        c.setSurfaceAxisGrid(majorGridColor, majorGridColor, minorGridColor, minorGridColor);

        // Add XY projection
        c.addXYProjection();


        // Set contour lines to semi-transparent white (0x7fffffff)
        c.setContourColor(0x7f000000);
        c.setBackground(Accent.PrimaryAccent.ToInt());

        // Add a color axis (the legend) in which the left center is anchored at (620, 250). Set
        // the length to 200 pixels and the labels on the right side.
        c.setColorAxis(700, 700, Chart.Left, 200, Chart.Right);

        // Set the x, y and z axis titles using 12 pt Arial Bold font
        c.xAxis().setTitle(ViewModel.X.Description, "Arial Bold", 12, Accent.SecondaryAccent.ToInt());
        c.yAxis().setTitle(ViewModel.Y.Description, "Arial Bold", 12, Accent.SecondaryAccent.ToInt());
        c.zAxis().setTitle(ViewModel.OptimizationResultVM.ObjectiveFunctionDescription, "Arial Bold", 12, 0x000088);

        // Output the chart
        viewer.Chart = c;

        //include tool tip for the chart
        viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                                            "title='<*cdml*>X: {x|2}<*br*>Y: {y|2}<*br*>Z: {z|2}'");
    }

    private void WPFChartViewer1_ViewPortChanged(object sender, WPFViewPortEventArgs e)
    {
        if (e.NeedUpdateChart)
        {
            createChart((WPFChartViewer) sender, 800, 800);
        }
    }

    private void WPFChartViewer1_MouseMoveChart(object sender, MouseEventArgs e)
    {
        var mouseX = WPFChartViewer1.ChartMouseX;
        var mouseY = WPFChartViewer1.ChartMouseY;

        // Drag occurs if mouse button is down and the mouse is captured by the m_ChartViewer
        if (Mouse.LeftButton == MouseButtonState.Pressed)
        {
            if (m_isDragging)
            {
                // The chart is configured to rotate by 90 degrees when the mouse moves from 
                // left to right, which is the plot region width (360 pixels). Similarly, the
                // elevation changes by 90 degrees when the mouse moves from top to buttom,
                // which is the plot region height (270 pixels).
                m_rotationAngle += (m_lastMouseX - mouseX) * 90.0 / 360;
                m_elevationAngle += (mouseY - m_lastMouseY) * 90.0 / 270;
                WPFChartViewer1.updateViewPort(true, true);
            }

            // Keep track of the last mouse position
            m_lastMouseX = mouseX;
            m_lastMouseY = mouseY;
            m_isDragging = true;
        }
    }

    private void WPFChartViewer1_MouseUpChart(object sender, MouseButtonEventArgs e)
    {
        m_isDragging = false;
        WPFChartViewer1.updateViewPort(true, false);
    }

    private void ChartDirectorSurfacePage_OnLoaded(object sender, RoutedEventArgs e)
    {
    }


    private void ChartDirectorSurfacePage_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        // var width = (int) e.NewSize.Width;
        // var height = (int) e.NewSize.Height;
        // WPFChartViewer1.Width = width;
        // WPFChartViewer1.Height = e.NewSize.Height;
        // WPFChartViewer1.Chart.setSize((int) e.NewSize.Height,(int) e.NewSize.Width);
        // (WPFChartViewer1.Chart as SurfaceChart).setPlotRegion(width,height,width,height,width);
        // WPFChartViewer1.updateViewPort(true,fa);
    }
}



