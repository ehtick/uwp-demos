#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using MindMapDemo;
using Syncfusion.UI.Xaml.Controls.Input;
using Syncfusion.UI.Xaml.Controls.Navigation;
using Syncfusion.UI.Xaml.Diagram;
using Syncfusion.UI.Xaml.Diagram.Controls;
using Syncfusion.UI.Xaml.Diagram.Stencil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Syncfusion.SampleBrowser.UWP.Diagram;
using WorkFlowEditor;
using SPath = Windows.UI.Xaml.Shapes.Path;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FloorPlannerDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FloorPlannerDiagram : UserControl
    {
        #region Constructor

        private ObservableCollection<FloorPlanNode> _nodes = new ObservableCollection<FloorPlanNode>();
        private ObservableCollection<FloorPlanConnector> _connectors = new ObservableCollection<FloorPlanConnector>();
        private IGraphInfo graphInfo;
        public FloorPlannerDiagram()
        {
            this.InitializeComponent();
            floorplan.Nodes = _nodes;
            floorplan.Connectors = _connectors;
            floorplan.DefaultConnectorType = ConnectorType.Line;
            graphInfo = floorplan.Info as IGraphInfo;
            floorplan.PageSettings = null;
            floorplan.Menu = null;
            floorplan.Loaded += Floorplan_Loaded;
            //createwallinfo();
            floorplan.Unloaded += FloorPlannerDiagram_Unloaded;
            (floorplan.SelectedItems as SelectorViewModel).SelectorConstraints = (floorplan.SelectedItems as SelectorViewModel).SelectorConstraints & ~SelectorConstraints.QuickCommands;
        }

        private void Floorplan_Loaded(object sender, RoutedEventArgs e)
        {
            Events();
            floorplan.CommandManager.View = (Control)Window.Current.Content;
            floorplan.Loaded -= Floorplan_Loaded;
        }

        private void Events()
        {
            _nodes.CollectionChanged += FloorPlannerDiagram_CollectionChanged;
            _connectors.CollectionChanged += ConnectorsChanged;
            floorplan.SnapSettings.SnapConstraints = SnapConstraints.All;
            floorplan.SnapSettings.SnapToObject = SnapToObject.All;

            floorplan.AddHandler(PointerPressedEvent, new PointerEventHandler(floorplan_PointerPressed), true);
            //floorplan.PointerReleased += floorplan_PointerReleased;
            graphInfo.ItemAdded += GraphInfo_ItemAdded;
            graphInfo.ItemSelectedEvent += floorplan_ItemSelectedEvent;
            graphInfo.ItemUnSelectedEvent += floorplan_ItemUnSelectedEvent;
           // floorplan.Tapped += floorplan_Tapped;
            graphInfo.ConnectorSourceChangedEvent += floorplan_ConnectorSourceChangedEvent;
            graphInfo.ConnectorTargetChangedEvent += floorplan_ConnectorTargetChangedEvent;
            //floorplan.PointerMoved += floorplan_PointerMoved;
        }

        private void GraphInfo_ItemAdded(object sender, ItemAddedEventArgs args)
        {
            if (args.Item == (args.Item as FloorPlanNode))
            {
                if ((args.Item as FloorPlanNode).ContentName != null)
                    (args.Item as FloorPlanNode).ContentTemplate = this.Resources[(args.Item as FloorPlanNode).ContentName.ToString()] as DataTemplate;
            }
        }

        void FloorPlannerDiagram_Unloaded(object sender, RoutedEventArgs e)
        {
            goBackGrid.Tapped -= Back_Clicked;
            floorplan.Unloaded -= FloorPlannerDiagram_Unloaded;

            foreach (FloorPlanNode node in _nodes)
            {
                node.Loaded -= node_Loaded;
                node.PropertyChanged -= node_PropertyChanged;
            }

            foreach (FloorPlanConnector con in _connectors)
            {
                con.Loaded -= FloorPlannerDiagram_Loaded;
            }
            _nodes.CollectionChanged -= FloorPlannerDiagram_CollectionChanged;
            _connectors.CollectionChanged -= ConnectorsChanged;
            _nodes = null;
            _connectors = null;
            floorplan.PointerPressed -= floorplan_PointerPressed;
            //floorplan.PointerReleased -= floorplan_PointerReleased;
            graphInfo.ItemSelectedEvent -= floorplan_ItemSelectedEvent;
            graphInfo.ItemUnSelectedEvent -= floorplan_ItemUnSelectedEvent;
            floorplan.Tapped -= floorplan_Tapped;
            graphInfo.ConnectorSourceChangedEvent -= floorplan_ConnectorSourceChangedEvent;
            graphInfo.ConnectorTargetChangedEvent -= floorplan_ConnectorTargetChangedEvent;
            //floorplan.PointerMoved -= floorplan_PointerMoved;
            floorplan = null;
            graphInfo = null;
        }
        #endregion

        #region private variables
        FloorPlanConnector wallInfo = null;
        FloorPlanNode optionnode = null;
        StackPanel st;
        int enable = 0;
        Storyboard sb = new Storyboard();
        Button b;
        bool IsPickerClicked = false;
        MindMapDemo.ColorPicker c = new MindMapDemo.ColorPicker();
        //private bool infoSizeRotate = false;
        Popup p = new Popup();
        FloorPlanNode f;
        SfRadialSlider sf = null;
        Size arrowSize = new Size(13, 23);
        private double dist = 25;
        Button StartButton;
        ToggleButton EndButton;
        Button CenterButton;
        #endregion

        #region Events

        void floorplan_ConnectorTargetChangedEvent(object sender, ChangeEventArgs<object, ConnectorChangedEventArgs> args)
        {
            if (args.NewValue.DragState == DragState.Completed)
            {
                updateWallInfo(null, tail: true);
                if (!args.NewValue.Point.Equals(new Point(0, 0)))
                {
                    FloorPlanConnector con = args.Item as FloorPlanConnector;
                    if (optionnode != null)
                    {
                        UpdateOptionnode(con);
                    }
                }
            }
            else if (args.NewValue.DragState == DragState.Dragging)
            {
                updateWallInfo((args.Item as FloorPlanConnector), tail: true);
                FloorPlanConnector con = args.Item as FloorPlanConnector;
                if (optionnode != null)
                {
                    UpdateOptionnode(null);
                }
            }
        }

        void floorplan_ConnectorSourceChangedEvent(object sender, ChangeEventArgs<object, ConnectorChangedEventArgs> args)
        {
            if (args.NewValue.DragState == DragState.Completed)
            {
                if (!args.NewValue.Point.Equals(new Point(0, 0)))
                {
                    FloorPlanConnector con = args.Item as FloorPlanConnector;
                    updateWallInfo(null, head: true);
                    if (optionnode != null)
                    {
                        UpdateOptionnode(con);
                    }
                }
            }
            else if (args.NewValue.DragState == DragState.Dragging)
            {
                updateWallInfo((args.Item as FloorPlanConnector), head: true);
                FloorPlanConnector con = args.Item as FloorPlanConnector;
                if (optionnode != null)
                {
                    UpdateOptionnode(null);
                }
            }
        }

        void floorplan_Tapped(object sender, TappedRoutedEventArgs e)
        {
            UIElement elemen = e.OriginalSource as UIElement;
            Control n = elemen.GetParent<FloorPlanNode>();
            if (n != null)
            {
                if ((n as FloorPlanNode).IsTextNode)
                {
                    ShowEditors();
                }
                else
                {
                    HideEditors();
                }
            }
            else
            {
                n = elemen.GetParent<FloorPlanConnector>();
                if (n != null)
                {
                    // st.Visibility = Visibility.Visible;
                }
                else
                {

                    n = elemen.GetParent<FloorPlanDiagram>();
                    if (n != null)
                    {
                        if (FloorPlannerViewModel.IsTextAdd)
                        {
                            foreach (FloorPlanNode node in _nodes)
                            {
                                node.IsSelected = false;
                            }

                            foreach (FloorPlanConnector con in _connectors)
                            {
                                con.IsSelected = false;
                            }
                            if (TextNode != null)
                            {
                                TextNode.IsSelected = false;
                                TextNode = null;
                            }
                            Point p = new Point(e.GetPosition(this).X, e.GetPosition(this).Y);
                            AddTextNode(p);
                        }
                        else
                        {
                            if (TextNode != null)
                            {
                                if (!(elemen is DiagramThumb) && ((TextNode.NodeAnnotations.ToArray()[0] as AnnotationEditorViewModel).Content as TextBox).Text == "Text")
                                {
                                    _nodes.Remove(TextNode);
                                }
                            }
                            n = elemen.GetParent<FloorPlanNode>();
                            if (n != null)
                            {
                                HideEditors();
                            }
                            if (f != null)
                            {
                                f.Visibility = Visibility.Collapsed;

                            }
                            UpdateOptionnode(null);              
                            if (sf != null)
                            {
                                sf.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                }
            }
        }

        //void floorplan_PointerReleased(object sender, PointerRoutedEventArgs e)
        //{
        //    //if (FloorPlannerViewModel.Preview != null && FloorPlannerViewModel.PreviewNode != null)
        //    //{
        //    //    _nodes.Remove((FloorPlannerViewModel.PreviewNode as FloorPlanNode));
        //    //}

        //    UpdateInfo();
        //}

        private void ConnectorsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                if (e.NewItems[0] is FloorPlanConnector)
                {
                    FloorPlanConnector newCon = (e.NewItems[0] as FloorPlanConnector);
                    if (newCon.IsWallInfo)
                    {
                        newCon.IsHitTestVisible = false;
                        newCon.ZIndex = 100000;
                        newCon.Constraints = ConnectorConstraints.None;
                    }
                    else
                    {                        
                        newCon.ZIndex = 1;
                        newCon.TargetDecorator = null;
                        newCon.ConnectorGeometryStyle = SetStyle();                       
                        newCon.Loaded += FloorPlannerDiagram_Loaded;
                    }
                }
            }
        }

        private void CreateOptionnode()
        {
            if (optionnode == null)
            {
                optionnode = new FloorPlanNode()
                {
                    UnitWidth = 275,
                    UnitHeight = 150
                };
                optionnode.OffsetY = 0;
                optionnode.OffsetX = 0;
                optionnode.ContentTemplate = this.Resources["optionnode"] as DataTemplate;
                optionnode.Constraints = NodeConstraints.None;
                optionnode.IsOption = true;                               
                optionnode.Loaded += optionnode_Loaded;
            }
        }

        private Style SetStyle()
        {
            Style s = new Style(typeof(Windows.UI.Xaml.Shapes.Path));
            s.Setters.Add(new Setter(Windows.UI.Xaml.Shapes.Path.StrokeProperty, new SolidColorBrush(Colors.Black)));
            s.Setters.Add(new Setter(Windows.UI.Xaml.Shapes.Path.StrokeThicknessProperty, 6));
            return s;
        }

        void FloorPlannerDiagram_Loaded(object sender, RoutedEventArgs e)
        {
            floorplan.CommandManager.View = (Control)Window.Current.Content;

        }

        void floorplan_ItemUnSelectedEvent(object sender, DiagramEventArgs args)
        {
            if (args.Item is FloorPlanNode)
            {
                if ((args.Item as FloorPlanNode).IsTextNode)
                {
                    this.FloorPlannerViewModel.SelectedObject = null;
                }
            }
            else if (args.Item is FloorPlanConnector)
            {
                (args.Item as FloorPlanConnector).ZIndex = 1;
            }
            if (((IEnumerable<object>)(floorplan.SelectedItems as CustomSelector).Nodes).Count() >= 0)
            {
                if (b != null)
                    b.Visibility = Visibility.Visible;
            }
            else if (((IEnumerable<object>)(floorplan.SelectedItems as CustomSelector).Connectors).Count() >= 0)
            {
                b.Visibility = Visibility.Visible;
            }
            else
            {
                b.Visibility = Visibility.Collapsed;
            }

            if (sf != null)
            {
                sf.Visibility = Visibility.Collapsed;
            }
        }

        void floorplan_ItemSelectedEvent(object sender, DiagramEventArgs args)
        {
            if (args.Item is FloorPlanNode)
            {
                if (b != null)
                {
                    b.Visibility = Visibility.Visible;
                }
                UpdateOptionnode(null);              
               // UpdateInfo(size: true);
                if ((args.Item as FloorPlanNode).Content != null && Enum.GetNames(typeof(BasicShapes)).Any(w => w == (args.Item as FloorPlanNode).Content.ToString()))
                {
                    (args.Item as FloorPlanNode).IsShapeNode = true;
                }
                (floorplan.SelectedItems as CustomSelector).SelectedObject = (args.Item as FloorPlanNode);

                if ((args.Item as FloorPlanNode).IsTextNode)
                {
                    this.FloorPlannerViewModel.SelectedObject = (args.Item as FloorPlanNode);
                    this.TextNode = (args.Item as FloorPlanNode);
                    FloorPlannerViewModel.TitleText = ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).Text;
                    FloorPlannerViewModel.TextFont = ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).FontFamily.Source.ToString();
                    FloorPlannerViewModel.TextSize = ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).FontSize;
                    FloorPlannerViewModel.TextColor = (((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).Foreground as SolidColorBrush).Color.ToString();
                    if (PropertyEditor.Visibility == Visibility.Collapsed)
                    {
                        ShowEditors();
                    }
               
                }
            }
            else if (args.Item is FloorPlanConnector)
            {
                if (b != null)
                    b.Visibility = Visibility.Visible;
                (floorplan.SelectedItems as CustomSelector).SelectedObject = (args.Item as FloorPlanConnector);
                FloorPlanConnector con = (floorplan.SelectedItems as CustomSelector).SelectedObject as FloorPlanConnector;
                con.ZIndex = 2000;
                if (optionnode == null)
                {
                    CreateOptionnode();
                }
                if (optionnode != null)
                {
                    UpdateOptionnode(con);
                    if (!_nodes.Contains(optionnode))
                    {                       
                        _nodes.Add(optionnode);
                    }

                }              
                if (st != null)
                {
                    if (EndButton == null)
                        EndButton = st.Children[1] as ToggleButton;
                    if (CenterButton == null)
                        CenterButton = st.Children[2] as Button;
                }
                if (EndButton != null)
                {
                    if (con.IsBesizer)
                    {
                        EndButton.IsChecked = true;
                        CenterButton.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        EndButton.IsChecked = false;
                        CenterButton.Visibility = Visibility.Visible;
                    }
                }

                if (CenterButton != null)
                {
                    if (con.IsSplit)
                    {
                        EndButton.Visibility = Visibility.Collapsed;
                        CenterButton.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        EndButton.Visibility = Visibility.Visible;
                        if (!con.IsBesizer)
                        {
                            CenterButton.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

        private void UpdateOptionnode(FloorPlanConnector con)
        {
            if (con != null)
            {
                double angle = FindAngle(con.SourcePoint, con.TargetPoint);
                Point p = Transform1(new Point((con.SourcePoint.X + con.TargetPoint.X) / 2, (con.SourcePoint.Y + con.TargetPoint.Y) / 2), 28, (angle - 90));
                optionnode.OffsetX = p.X;
                optionnode.OffsetY = p.Y;
                optionnode.Visibility = Visibility.Visible;
                optionnode.RotateAngle = angle;
            }
            else
            {
                if (optionnode != null)
                {
                    optionnode.Visibility = Visibility.Collapsed;
                    optionnode.OffsetX = 0;
                    optionnode.OffsetY = 0;
                }
            }
        }

        private Point Transform1(Point s, int length, double angle)
        {
            return new Point()
            {
                X = s.X + length * Math.Cos(angle * Math.PI / 180),
                Y = s.Y + length * Math.Sin(angle * Math.PI / 180)
            };
        }

        void LoadSt(object sender, RoutedEventArgs e)
        {
            st = (sender as StackPanel);
        }

        void optionnode_Loaded(object sender, RoutedEventArgs e)
        {
            st = (sender as FloorPlanNode).FindDescendantByName("buttonpanel") as StackPanel;
            //  st = (sender as FloorPlanNode).FindDescendantByName("buttonpanel") as StackPanel;
            StartButton = (sender as FloorPlanNode).FindDescendantByName("start") as Button;
            EndButton = (sender as FloorPlanNode).FindDescendantByName("end") as ToggleButton;
            CenterButton = (sender as FloorPlanNode).FindDescendantByName("center") as Button;
            if ((floorplan.SelectedItems as CustomSelector).SelectedObject is FloorPlanConnector)
            {
                if (st != null && (st.Children[0] as Button).Name == "start")
                {
                    StartButton = (st.Children[0] as Button);
                    // StartButton.Tag = ((floorplan.SelectedItems as CustomSelector).SelectedObject as FloorPlanConnector).SourcePoint;
                }
            }
        }

        private void ShowEditors()
        {
            PropertyEditor.DataContext = null;
            PropertyEditor.Visibility = Visibility.Visible;
            FloorPlannerViewModel.ValueType = "Text";
            PropertyEditor.DataContext = this.FloorPlannerViewModel;
            (PropertyEditor.Resources["PropertyEditor_Storyboard_Visible"] as Storyboard).Begin();

            if (FloorPlannerViewModel.textbox != null)
            {
                if (FloorPlannerViewModel.textbox.Text == "Text")
                {
                    FloorPlannerViewModel.textbox.SelectionLength = 4;
                }
                FloorPlannerViewModel.textbox.Focus(Windows.UI.Xaml.FocusState.Keyboard);

            }
        }

        void floorplan_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (IsPickerClicked)
                p.IsOpen = false;
            else
                IsPickerClicked = false;
            foreach (var element in VisualTreeHelper.FindElementsInHostCoordinates(e.GetCurrentPoint(null).Position, null))
            {
                if (element is FloorPlanNode)
                {
                    if ((element as FloorPlanNode).IsTextNode)
                    {
                        ShowEditors();
                    }
                    else
                    {
                        HideEditors();
                    }
                }
                else if (element is FloorPlanDiagram)
                {
                    if (FloorPlannerViewModel.IsTextAdd)
                    {
                        foreach (FloorPlanNode node in _nodes)
                        {
                            node.IsSelected = false;
                        }

                        foreach (FloorPlanConnector con in _connectors)
                        {
                            con.IsSelected = false;
                        }
                        if (TextNode != null)
                        {
                            TextNode.IsSelected = false;
                            TextNode = null;
                        }
                        Point point = e.GetCurrentPoint(null).Position;
                        AddTextNode(point);
                    }
                    else
                    {
                        if (TextNode != null)
                        {
                            if (((TextNode.NodeAnnotations.ToArray()[0] as AnnotationEditorViewModel).Content as TextBox).Text == "Text")
                            {
                                _nodes.Remove(TextNode);
                            }
                        }

                        HideEditors();
                        
                        if (f != null)
                        {
                            f.Visibility = Visibility.Collapsed;

                        }
                        UpdateOptionnode(null);
                        if (sf != null)
                        {
                            sf.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }
        #endregion

        #region Helpers
        public FloorPlannerViewModel FloorPlannerViewModel
        {
            get { return (FloorPlannerViewModel)GetValue(FloorPlannerViewModelProperty); }
            set { SetValue(FloorPlannerViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FloorPlannerViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FloorPlannerViewModelProperty =
            DependencyProperty.Register("FloorPlannerViewModel", typeof(FloorPlannerViewModel), typeof(FloorPlannerDiagram), new PropertyMetadata(null, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FloorPlannerDiagram f = d as FloorPlannerDiagram;
            FloorPlannerViewModel vm = e.NewValue as FloorPlannerViewModel;
            if (vm != null)
            {
                vm.Save = new UMLDiagramDesigner.DelegateCommand<object>(f.OnSave, args => { return true; });
                vm.Load = new UMLDiagramDesigner.DelegateCommand<object>(f.OnLoad, args => { return true; });
                vm.Create = new UMLDiagramDesigner.DelegateCommand<object>(f.OnCreate, args => { return true; });
                vm.Clear = new UMLDiagramDesigner.DelegateCommand<object>(f.OnClear, args => { return true; });
                vm.Delete = new UMLDiagramDesigner.DelegateCommand<object>(f.OnDelete, args => { return true; });
                vm.Back = new UMLDiagramDesigner.DelegateCommand<object>(f.OnBack, args => { return true; });
                vm.AddProp = new UMLDiagramDesigner.DelegateCommand<object>(f.OnAddProp, args => { return true; });
                vm.AddShape = new UMLDiagramDesigner.DelegateCommand<object>(f.OnAddShape, args => { return true; });
                vm.AddText = new UMLDiagramDesigner.DelegateCommand<object>(f.OnAddText, args => { return true; });
                vm.AddWall = new UMLDiagramDesigner.DelegateCommand<object>(f.OnAddWall, args => { return true; });
                vm.BoldCommand = new UMLDiagramDesigner.DelegateCommand<object>(f.OnBold, args => { return true; });
                vm.ItalicCommand = new UMLDiagramDesigner.DelegateCommand<object>(f.OnItalic, args => { return true; });
                vm.ForgroundColor = new UMLDiagramDesigner.DelegateCommand<object>(f.OnColorChanged, args => { return true; });
                vm.TextChanged = new UMLDiagramDesigner.DelegateCommand<object>(f.OnTextChanged, args => { return true; });
                vm.Strokes = new UMLDiagramDesigner.DelegateCommand<object>(f.OnStrokeChanged, args => { return true; });
                vm.TitleFont = new UMLDiagramDesigner.DelegateCommand<object>(f.OnFontChanged, args => { return true; });
                vm.TitleFontSize = new UMLDiagramDesigner.DelegateCommand<object>(f.OnSizeChanged, args => { return true; });
                vm.CancelCommand = new UMLDiagramDesigner.DelegateCommand<object>(f.OnCancel, args => { return true; });
                vm.OKCommand = new UMLDiagramDesigner.DelegateCommand<object>(f.OnOkCommand, args => { return true; });
                // vm.DeleteSelectedObject = new UMLDiagramDesigner.DelegateCommand<object>(f.OnDeletecommand, f.CanDelete);

                vm.ForeColorChanged = new UMLDiagramDesigner.DelegateCommand<object>(f.OnColorCommand, args => { return true; });
            }
        }

        private void HideEditors()
        {
            if (PropertyEditor.Visibility != Visibility.Collapsed &&
                   ((PropertyEditor.Resources["PropertyEditor_Storyboard_Collapse"] as Storyboard).GetCurrentState() == ClockState.Stopped ||
                   (PropertyEditor.Resources["PropertyEditor_Storyboard_Collapse"] as Storyboard).GetCurrentState() == ClockState.Filling))
            {

                (PropertyEditor.Resources["PropertyEditor_Storyboard_Collapse"] as Storyboard).Begin();
            }
        }

        private void createwallinfo()
        {
            wallInfo = new FloorPlanConnector();
            wallInfo.SourceDecorator = GetArrow();
            wallInfo.TargetDecorator = GetArrow();
            wallInfo.SourceDecoratorStyle = GetArrowStyle();
            wallInfo.TargetDecoratorStyle = GetArrowStyle();
            wallInfo.ConnectorGeometryStyle = GetLineStyle();
            wallInfo.Visibility = Visibility.Collapsed;
            wallInfo.IsWallInfo = true;
            wallInfo.ConnectorAnnotations = new ObservableCollection<object>() { new AnnotationEditorViewModel() { ViewTemplate = this.Resources["ViewTemplate1"] as DataTemplate } };
           
        }
        #endregion

        #region Commands

        private void OnDeletecommand(object obj)
        {
            if (floorplan.SelectedItems != null)
            {
                if ((floorplan.SelectedItems as CustomSelector).Nodes != null)
                {
                    ObservableCollection<FloorPlanNode> f = new ObservableCollection<FloorPlanNode>();
                    foreach (FloorPlanNode node in (IEnumerable<object>)(floorplan.SelectedItems as CustomSelector).Nodes)
                    {
                        f.Add(node);
                    }

                    foreach (FloorPlanNode node in f)
                    {
                        _nodes.Remove(node);
                    }
                }

                if ((floorplan.SelectedItems as CustomSelector).Connectors != null)
                {
                    ObservableCollection<FloorPlanConnector> c = new ObservableCollection<FloorPlanConnector>();
                    foreach (FloorPlanConnector con in (IEnumerable<object>)(floorplan.SelectedItems as CustomSelector).Connectors)
                    {
                        c.Add(con);
                    }

                    foreach (FloorPlanConnector con in c)
                    {
                        _connectors.Remove(con);
                    }
                }


            }
        }

        private void OnColorCommand(object obj)
        {
            if (this.FloorPlannerViewModel.TitleText != string.Empty)
            {
                if (TextNode != null)
                {
                    ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).Foreground = ColorConverter(obj.ToString());
                    this.TextNode.ForeColor = obj.ToString();
                    this.FloorPlannerViewModel.TextColor = obj.ToString();
                }
            }
        }

        private void OnCancel(object obj)
        {
            if (TextNode != null)
            {
                _nodes.Remove(TextNode);
            }
            HideEditors();
        }

        private void OnOkCommand(object obj)
        {
            if (sb != null)
                sb.Stop();
            HideEditors();
        }

        private void OnSizeChanged(object obj)
        {
            if (this.FloorPlannerViewModel.TitleText != string.Empty)
            {
                if (TextNode != null)
                {
                    ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).FontSize = double.Parse(obj.ToString());
                    this.TextNode.TextFontSize = ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).FontSize;
                    this.FloorPlannerViewModel.TextSize = ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).FontSize;
                }
            }
        }

        private void OnFontChanged(object obj)
        {
            if (this.FloorPlannerViewModel.TitleText != string.Empty)
            {
                if (TextNode != null)
                {
                    ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).FontFamily = new FontFamily(obj.ToString());
                    this.TextNode.Font = obj.ToString();
                    this.FloorPlannerViewModel.TextFont = obj.ToString();
                    if (sb != null)
                        sb.Stop();
                }
            }
        }

        private void OnStrokeChanged(object obj)
        {
            if (this.FloorPlannerViewModel.TitleText != string.Empty)
            {
                // if((this.FloorPlannerViewModel.NodeText as TextBlock).fontv
            }
        }

        private void OnTextChanged(object obj)
        {
            if (TextNode != null)
                ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).Text = obj.ToString();
            TextNode.NodeText = obj.ToString();
            if (sb != null)
                sb.Stop();
        }

        private void OnColorChanged(object obj)
        {
            if (enable % 2 == 0)
            {
                this.FloorPlannerViewModel.Enabled = true;
            }
            else
            {
                this.FloorPlannerViewModel.Enabled = false;
            }
            if (this.FloorPlannerViewModel.TitleText != string.Empty)
            {

            }
            enable += 1;
            if (sb != null)
                sb.Stop();
        }

        private void OnItalic(object obj)
        {
            if (this.FloorPlannerViewModel.TitleText != string.Empty)
            {
                if (TextNode != null)
                {
                    if (((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).FontStyle == FontStyle.Italic)
                    {
                        ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).FontStyle = FontStyle.Normal;
                    }
                    else
                    {
                        ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).FontStyle = FontStyle.Italic;
                    }
                    if (sb != null)
                        sb.Stop();
                    ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).FontStyle = ((this.TextNode.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).FontStyle;
                }
            }
        }

        private void OnBold(object obj)
        {
            if (this.FloorPlannerViewModel.TitleText != string.Empty)
            {
                if (TextNode != null)
                {
                    CheckFontWeight(TextNode);
                    if (sb != null)
                        sb.Stop();
                }
            }
        }

        private void CheckFontWeight(FloorPlanNode t)
        {
            if (t.NodeAnnotations != null)
            {
                TextBox tb = (t.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox;
                if (tb.FontWeight.Equals(FontWeights.Bold))
                {
                    tb.FontWeight = FontWeights.Normal;
                    t.TextWeight = "Normal";
                }
                else
                {
                    tb.FontWeight = FontWeights.Bold;
                    t.TextWeight = "Bold";
                }
            }
        }

        #region AppBar

        private void OnAddText(object obj)
        {
            FloorPlannerViewModel.IsTextAdd = true;
            HideEditors();
        }

        FloorPlanNode TextNode = null;

        private void AddTextNode(Point point)
        {
            TextNode = new FloorPlanNode();
            TextNode.UnitWidth = 50;
            TextNode.UnitHeight = 30;
            TextNode.IsTextNode = true;
            TextNode.IsSelected = true;

            TextBox t = new TextBox() { BorderBrush = new SolidColorBrush(Colors.LightGray), BorderThickness = new Thickness(0), Foreground = ColorConverter("#FFC40C0C"), FontSize = 12, FontFamily = new FontFamily("Segoe UI") };

            t.TextChanged += t_TextChanged;
            // t.Text = this.FloorPlannerViewModel.TitleText;          
            TextNode.NodeAnnotations = new ObservableCollection<object>()
            {
                new AnnotationEditorViewModel()
                {
                    Content=t,ViewTemplate=this.Resources["ViewTemplate"] as DataTemplate,
                    EditTemplate=this.Resources["EditTemplate"] as DataTemplate,
                    TextHorizontalAlignment= TextAlignment.Center,
                    TextVerticalAlignment= VerticalAlignment.Center
                }
            };
            t.Text = "Text";
            TextNode.OffsetX = point.X;
            TextNode.OffsetY = point.Y;
            TextNode.Loaded += TextNode_Loaded;
            _nodes.Add(TextNode);

            //// TextNode.InvalidateMeasure();
            // TextNode.InvalidateArrange();
            //TextNode.Loaded += (s, e) =>
            //    {
            //        // ExpandAnimation(TextNode);
            //    };
            if (FloorPlannerViewModel.textbox != null)
            {
                if (FloorPlannerViewModel.textbox.Text == "Text")
                {
                    FloorPlannerViewModel.textbox.SelectionLength = 4;
                }
                FloorPlannerViewModel.textbox.Focus(Windows.UI.Xaml.FocusState.Keyboard);

            }
            FloorPlannerViewModel.IsTextAdd = false;
        }

        void TextNode_Loaded(object sender, RoutedEventArgs e)
        {
            (((sender as FloorPlanNode).NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).IsTabStop = true;
            (((sender as FloorPlanNode).NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).UpdateLayout();
            (((sender as FloorPlanNode).NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).Focus(Windows.UI.Xaml.FocusState.Keyboard);

        }

        void t_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextNode != null)
                TextNode.NodeText = (this.TextNode.Content as TextBox).Text.ToString();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (TextNode != null)
                TextNode.NodeText = (sender as TextBox).Text.ToString();
            FloorPlannerViewModel.TitleText = (sender as TextBox).Text.ToString();
        }

        private void OnAddWall(object obj)
        {
            FloorPlannerViewModel.IsTextAdd = false;
            floorplan.DefaultConnectorType = ConnectorType.Line;
            floorplan.Tool = floorplan.Tool | Tool.DrawOnce;
            if (wallInfo == null)
            {
                createwallinfo();
            }

           
            if (wallInfo != null)
            {
                if (_connectors != null)
                {
                    if (!_connectors.Contains(wallInfo))
                    {
                        _connectors.Add(wallInfo);
                    }
                }
            }
            //if (st != null)
            //{
            //    st.Visibility = Visibility.Collapsed;
            //}
        }

        private void OnAddShape(object obj)
        {
            FloorPlannerViewModel.IsTextAdd = false;
            PropertyEditor.DataContext = null;
            PropertyEditor.Visibility = Visibility.Visible;
            SymbolCollectionType s = new SymbolCollectionType();
            FloorPlannerViewModel.ValueType = "Shape";
            PropertyEditor.DataContext = FloorPlannerViewModel;
            (PropertyEditor.Resources["PropertyEditor_Storyboard_Visible"] as Storyboard).Begin();
        }

        private void OnAddProp(object obj)
        {
            FloorPlannerViewModel.IsTextAdd = false;
            PropertyEditor.DataContext = null;
            PropertyEditor.Visibility = Visibility.Visible;
            FloorPlannerViewModel.ValueType = "Prop";
            PropertyEditor.DataContext = FloorPlannerViewModel;
            (PropertyEditor.Resources["PropertyEditor_Storyboard_Visible"] as Storyboard).Begin();
        }

        private void Back_Clicked(object sender, TappedRoutedEventArgs e)
        {
            this.FloorPlannerViewModel.Back.Execute(null);
        }

        private void OnBack(object obj)
        {
            this.FloorPlannerViewModel.DiagramVisibility = Visibility.Collapsed;
            this.FloorPlannerViewModel.Save.Execute(null);
        }

        private void OnDelete(object obj)
        {
            if (obj != null)
            {
                if (obj is Button)
                {
                    b = obj as Button;
                }
                else
                {

                    if ((floorplan.SelectedItems as CustomSelector).Nodes != null)
                    {
                        ObservableCollection<FloorPlanNode> fn = new ObservableCollection<FloorPlanNode>();

                        foreach (FloorPlanNode f in (IEnumerable<object>)(floorplan.SelectedItems as CustomSelector).Nodes)
                        {
                            fn.Add(f);
                        }

                        foreach (FloorPlanNode f1 in fn)
                        {
                            _nodes.Remove(f1);
                        }
                    }

                    if ((floorplan.SelectedItems as CustomSelector).Connectors != null)
                    {
                        ObservableCollection<FloorPlanConnector> con = new ObservableCollection<FloorPlanConnector>();
                        foreach (FloorPlanConnector c in (IEnumerable<object>)(floorplan.SelectedItems as CustomSelector).Connectors)
                        {
                            con.Add(c);
                        }

                        foreach (FloorPlanConnector c1 in con)
                        {
                            _connectors.Remove(c1);
                        }
                    }
                    b.Visibility = Visibility.Collapsed;
                    UpdateOptionnode(null);
                }
            }
        }

        private void OnClear(object obj)
        {
            _nodes.Clear();
            _connectors.Clear();
        }
        #endregion

        #region Options
        //Wall Thickness
        private void start_Click_1(object sender, RoutedEventArgs e)
        {
            f = new FloorPlanNode();
            f.Constraints = NodeConstraints.None;
            f.UnitWidth = 115;
            f.UnitHeight = 115;
            Grid g = new Grid();
            sf = new SfRadialSlider() { Width = 110, Height = 110 };
            //sf.SmallChange = 0.2;
            //sf.TickFrequency = 1;
            sf.SmallChange = 2;
            sf.TickFrequency = 2;
            sf.LabelVisibility = Visibility.Visible;
            sf.Minimum = 2;
            sf.Maximum = 10;
            sf.LabelTemplate = this.Resources["SliderTemplate"] as DataTemplate;
            sf.Content = new TextBlock() { Text = "6''", Foreground = new SolidColorBrush(Colors.Black) };
            sf.InnerRimRadiusFactor = 0.40;
            f.Content = sf;
            f.IsOption = true;
            _nodes.Add(f);
            sf.Visibility = Visibility.Visible;
            if ((floorplan.SelectedItems as CustomSelector).SelectedObject is FloorPlanConnector)
            {
                FloorPlanConnector con = ((floorplan.SelectedItems as CustomSelector).SelectedObject as FloorPlanConnector);
                sf.Value = con.ConThickness;
                sf.ValueChanged += sf_ValueChanged;
                double angle = FindAngle(con.SourcePoint, con.TargetPoint);
                Point p = Transform(new Point(((con.SourcePoint.X - (f.UnitWidth / 2)) + (con.TargetPoint.X - (f.UnitWidth) / 2)) / 2, ((con.SourcePoint.Y - (f.UnitHeight / 2)) + (con.TargetPoint.Y - (f.UnitHeight / 2))) / 2), 15, (angle - 90));
                f.OffsetX = p.X;
                f.OffsetY = p.Y;
            }
            UpdateOptionnode(null);              
        }

        //Besizer
        private void end_Click_1(object sender, RoutedEventArgs e)
        {
            if ((floorplan.SelectedItems as CustomSelector).SelectedObject is FloorPlanConnector)
            {
                FloorPlanConnector con = ((floorplan.SelectedItems as CustomSelector).SelectedObject as FloorPlanConnector);
                con.Constraints = con.Constraints | ConnectorConstraints.SegmentThumbs;
                if ((con.Segments as ObservableCollection<object>)[0] is LineSegmentLength)
                {
                    (con.Segments as ObservableCollection<object>).Remove((con.Segments as ObservableCollection<object>)[0] as IConnectorSegment);
                    (con.Segments as ObservableCollection<object>).Add(new CubicCurveSegment());
                    con.IsBesizer = true;
                }
                else if ((con.Segments as ObservableCollection<object>)[0] is CubicCurveSegment)
                {
                    (con.Segments as ObservableCollection<object>).Remove((con.Segments as ObservableCollection<object>)[0] as CubicCurveSegment);
                    (con.Segments as ObservableCollection<object>).Add(new LineSegmentLength());
                    con.IsBesizer = false;
                }
            }
            UpdateOptionnode(null);              
        }

        //Split
        private void center_Click_1(object sender, RoutedEventArgs e)
        {
            if ((floorplan.SelectedItems as CustomSelector).SelectedObject is FloorPlanConnector)
            {
                FloorPlanConnector con = ((floorplan.SelectedItems as CustomSelector).SelectedObject as FloorPlanConnector);
                con.Constraints = con.Constraints | ConnectorConstraints.SegmentThumbs;
                Point centerpoint = new Point((con.SourcePoint.X + con.TargetPoint.X) / 2, (con.SourcePoint.Y + con.TargetPoint.Y) / 2);
                Point center = new Point((con.SourcePoint.X + centerpoint.X) / 2, (con.SourcePoint.Y + centerpoint.Y) / 2);
                //con.Segments.Insert(new CubicCurveSegment() { Point1 = centerpoint, Point2 = null });
                con.Segments = new ObservableCollection<object>()
                {
                    new StraightSegment(){Point=centerpoint},
                    new StraightSegment()
                };
                con.IsSplit = true;
            }
            UpdateOptionnode(null);              
        }
        #endregion

        #region Save & Load
        private async void OnLoad(object param)
        {
            _nodes.Clear();
            _connectors.Clear();
         
            //if (EnsureUnsnapped())
            {
                StorageFile s;
                if (param == null)
                {
                    FileOpenPicker file = new FileOpenPicker();
                    file.CommitButtonText = "Load";
                    file.FileTypeFilter.Add(".xml");
                    this.FloorPlannerViewModel.CurrentlyLoaded = string.Empty;
                    s = await file.PickSingleFileAsync();
                }
                else
                {
                    StorageFolder installedLocation = ApplicationData.Current.RoamingFolder;
                    StorageFolder st = await installedLocation.GetFolderAsync("FloorPlan");
                    s = await st.GetFileAsync(param.ToString() + ".xml");
                    this.FloorPlannerViewModel.CurrentlyLoaded = param.ToString();
                }
                if (s != null)
                {
                    using (IRandomAccessStream readStream = await s.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        if (readStream != null)
                        {
                            Stream str = readStream.AsStream();
                            floorplan.Load(str);
                            UpdateLoad();
                        }

                    }
                }
            }
            this.GetParent<FloorPlannerDemo.MainPage>().BottomAppBar.IsSticky = true;
            this.GetParent<FloorPlannerDemo.MainPage>().BottomAppBar.IsOpen = true;
        }

        //private bool EnsureUnsnapped()
        //{
        //    bool unsnapped = ((ApplicationView.Value != ApplicationViewState.Snapped) || ApplicationView.TryUnsnap());
        //    if (!unsnapped)
        //    {

        //    }
        //    return unsnapped;
        //}

        private async void OnSave(object param)
        {
            StorageFile s = null;
            //if (EnsureUnsnapped())
            {
                if (param == null)
                {
                    if (!string.IsNullOrEmpty(this.FloorPlannerViewModel.CurrentlyLoaded))
                    {
                        StorageFolder installedLocation = ApplicationData.Current.RoamingFolder;
                        StorageFolder st = await installedLocation.GetFolderAsync("FloorPlan");
                        s = await st.CreateFileAsync(this.FloorPlannerViewModel.CurrentlyLoaded + ".xml", CreationCollisionOption.ReplaceExisting);
                    }
                }
                else
                {
                    StorageFolder installedLocation = ApplicationData.Current.RoamingFolder;
                    StorageFolder st = await installedLocation.GetFolderAsync("FloorPlan");
                    s = await st.CreateFileAsync(param.ToString() + ".xml", CreationCollisionOption.ReplaceExisting);
                    this.FloorPlannerViewModel.CurrentlyLoaded = param.ToString();
                }
                if (s != null)
                {
                    Updatesave();
                    using (IRandomAccessStream writeStream = await s.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        if (writeStream != null)
                        {
                            Stream str = writeStream.AsStream();
                            floorplan.Save(str);
                        }
                    }
                }
            }
        }

        private void UpdateLoad()
        {
            wallInfo = null;
            optionnode = null;
            HideEditors();
            foreach (FloorPlanConnector c in _connectors)
            {
                if (!c.IsWallInfo)
                {
                    c.ConnectorGeometryStyle = SetStyle(c.ConThickness);
                    if (c.BezierString != string.Empty)
                    {
                        string[] splits = c.BezierString.Split(',');
                        c.Segments = new ObservableCollection<object>();
                        CubicCurveSegment cc = new CubicCurveSegment();
                        if (splits.Length == 3)
                        {
                            cc.Point1 = new Point(double.Parse(splits[1]), double.Parse(splits[2]));
                        }
                        else if (splits.Length == 5)
                        {
                            cc.Point1 = new Point(double.Parse(splits[1]), double.Parse(splits[2]));
                            cc.Point2 = new Point(double.Parse(splits[3]), double.Parse(splits[4]));
                        }
                        else if (splits.Length == 7)
                        {
                            cc.Point1 = new Point(double.Parse(splits[1]), double.Parse(splits[2]));
                            cc.Point2 = new Point(double.Parse(splits[3]), double.Parse(splits[4]));
                            cc.Point3 = new Point(double.Parse(splits[5]), double.Parse(splits[6]));
                        }
                        (c.Segments as ObservableCollection<object>).Add(cc);
                    }
                    if (c.IntermediateString != string.Empty)
                    {
                        string[] splits = c.IntermediateString.Split(',');
                        if (splits.Length == 3)
                        {
                            if (c.Segments == null)
                            {
                                c.Segments = new ObservableCollection<object>()
                                            {
                                                new StraightSegment()
                                                {
                                                    Point=new Point(double.Parse(splits[1]),double.Parse(splits[2]))
                                                }
                                            };
                            }
                            else
                            {
                                (c.Segments as ObservableCollection<object>).Add(new StraightSegment()
                                {
                                    Point = new Point(double.Parse(splits[1]), double.Parse(splits[2]))
                                });
                            }
                        }
                        else if (splits.Length == 4)
                        {
                            c.Segments = new ObservableCollection<object>()
                                            {
                                                new StraightSegment()
                                                {
                                                    Point=new Point(double.Parse(splits[1]),double.Parse(splits[2]))
                                                }
                                            };

                            if (splits[3] == "S")
                            {
                                (c.Segments as ObservableCollection<object>).Add(new StraightSegment());
                            }
                        }
                    }
                }
            }         
            foreach (FloorPlanNode f1 in _nodes)
            {
                if (f1.IsTextNode)
                {
                    TextBox t = new TextBox() { Foreground = ColorConverter(f1.ForeColor), FontSize = f1.TextFontSize, FontFamily = new FontFamily(f1.Font.ToString()), Text = f1.NodeText, FontStyle = f1.TextStyle };
                    // f1.Content = t;
                    f1.NodeAnnotations = new ObservableCollection<object>()
                     {
                        new AnnotationEditorViewModel()
                          {
                             Content=t,ViewTemplate=this.Resources["ViewTemplate"] as DataTemplate,EditTemplate=this.Resources["EditTemplate"] as DataTemplate
                          }
                     };
                    CheckFontWeight(f1);
                    if ((f1 as FloorPlanNode).TextWeight == "Bold")
                    {
                        t.FontWeight = FontWeights.Bold;
                    }
                    else if (f1.TextWeight == "Normal")
                    {
                        t.FontWeight = FontWeights.Normal;
                    }

                }
                else
                {
                    if (f1.ContentName != null)
                    {
                        if (!f1.IsOption)
                        {
                            f1.ContentTemplate = this.Resources[f1.ContentName.ToString()] as DataTemplate;
                            if (f1.ContentName == "Window")
                            {
                                // node.Width = 5;
                                f1.UnitHeight = 6;
                            }
                        }
                    }
                    else if (f1.IsShapeNode)
                    {
                        f1.ShapeStyle = GetStyle(f1.SelectedColor.ToString());
                    }
                }

            }
            this.FloorPlannerViewModel.DiagramVisibility = Visibility.Visible;
        }

        private void Updatesave()
        {
            List<FloorPlanConnector> flc = new List<FloorPlanConnector>();
            List<FloorPlanNode> fpn = new List<FloorPlanNode>();
            if (p != null)
            {
                p.IsOpen = false;
            }

            foreach (FloorPlanConnector pc in _connectors)
            {
                pc.IsSelected = false;
                if (pc.IsWallInfo)
                {
                    flc.Add(pc);
                }
                if (pc.Segments != null)
                {
                    if ((pc.Segments as ObservableCollection<object>)[0] is CubicCurveSegment)
                    {
                        pc.BezierString = "C";
                        if (((pc.Segments as ObservableCollection<object>)[0] as CubicCurveSegment).Point1 != null)
                        {
                            pc.BezierString += "," + ((pc.Segments as ObservableCollection<object>)[0] as CubicCurveSegment).Point1.ToString();
                        }
                        if (((pc.Segments as ObservableCollection<object>)[0] as CubicCurveSegment).Point2 != null)
                        {
                            pc.BezierString += "," + ((pc.Segments as ObservableCollection<object>)[0] as CubicCurveSegment).Point2.ToString();
                        }
                        if (((pc.Segments as ObservableCollection<object>)[0] as CubicCurveSegment).Point3 != null)
                        {
                            pc.BezierString += "," + ((pc.Segments as ObservableCollection<object>)[0] as CubicCurveSegment).Point3.ToString();
                        }
                    }
                    if ((pc.Segments as ObservableCollection<object>).Count > 1)
                    {
                        if ((pc.Segments as ObservableCollection<object>)[0] is StraightSegment)
                        {
                            pc.IntermediateString = "S";
                            if (((pc.Segments as ObservableCollection<object>)[0] as StraightSegment).Point != null)
                            {
                                pc.IntermediateString += "," + ((pc.Segments as ObservableCollection<object>)[0] as StraightSegment).Point.ToString();
                            }
                        }
                        if ((pc.Segments as ObservableCollection<object>)[1] is StraightSegment)
                        {
                            pc.IntermediateString += "," + "S";
                            if (((pc.Segments as ObservableCollection<object>)[1] as StraightSegment).Point != null)
                            {
                                pc.IntermediateString += "," + ((pc.Segments as ObservableCollection<object>)[1] as StraightSegment).Point.ToString();
                            }
                        }
                    }
                }
            }


            foreach (FloorPlanConnector f in flc)
            {
                _connectors.Remove(f);
            }

            foreach (FloorPlanNode f in _nodes)
            {
                if (f.IsOption)
                {
                    fpn.Add(f);
                }
                else if(f.IsTextNode)
                {
                    if (f.NodeAnnotations != null && ((f.NodeAnnotations.ToList()[0] as AnnotationEditorViewModel).Content as TextBox).Text == "Text")
                    {
                        fpn.Add(f);
                    }
                }
            }
            foreach (FloorPlanNode f1 in fpn)
            {
                int i = _nodes.ToList().IndexOf(f1);
                _nodes.RemoveAt(i) ;
            }
        }

        private async void OnCreate(object parameter)
        {
            if (_nodes != null)
                _nodes.Clear();
            if (_connectors != null)
                _connectors.Clear();

            wallInfo = null;
            optionnode = null;
            StorageFile s = null;
            //if (EnsureUnsnapped())
            {
                StorageFolder installedLocation = ApplicationData.Current.RoamingFolder;
                StorageFolder st = await installedLocation.GetFolderAsync("FloorPlan");
                s = await st.CreateFileAsync(parameter.ToString() + ".xml", CreationCollisionOption.FailIfExists);
                this.FloorPlannerViewModel.CurrentlyLoaded = parameter.ToString();
            }
            if (s != null)
            {
                using (StorageStreamTransaction writeStream = await s.OpenTransactedWriteAsync())
                {
                    FloorPlanDiagram empty = new FloorPlanDiagram();
                    empty.Nodes = new ObservableCollection<FloorPlanNode>();
                    empty.Connectors = new ObservableCollection<FloorPlanConnector>();
                    empty.Save(writeStream.Stream.AsStreamForWrite());
                    await writeStream.CommitAsync();
                }
                FloorPlannerViewModel.DiagramVisibility = Visibility.Visible;
                this.GetParent<FloorPlannerDemo.MainPage>().BottomAppBar.IsSticky = true;
                this.GetParent<FloorPlannerDemo.MainPage>().BottomAppBar.IsOpen = true;                
            }
        }
        #endregion


        #endregion

        #region Shapes
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IsPickerClicked = true;
            if ((floorplan.SelectedItems as CustomSelector).SelectedObject is FloorPlanNode && !(p.IsOpen))
            {
                FloorPlanNode SelectedNode = ((floorplan.SelectedItems as CustomSelector).SelectedObject as FloorPlanNode);
                double x = SelectedNode.OffsetX;
                double y = SelectedNode.OffsetY;
                double w1 = (SelectedNode.Info as INodeInfo).ActualWidth;
                double h1 = (SelectedNode.Info as INodeInfo).ActualHeight;
                double w = graphInfo.Viewport.Width;
                double h = graphInfo.Viewport.Height;
                double x1 = graphInfo.Viewport.Left;
                double y1 = graphInfo.Viewport.Top;
                double width = (((c as MindMapDemo.ColorPicker).Content as Grid).Children.First() as Syncfusion.UI.Xaml.Controls.Media.SfColorPicker).Width;
                double height = (((c as MindMapDemo.ColorPicker).Content as Grid).Children.First() as Syncfusion.UI.Xaml.Controls.Media.SfColorPicker).Height;
                if (width > (w - x + w1 + x1))
                    p.HorizontalOffset = SelectedNode.OffsetX - width - (SelectedNode.Info as INodeInfo).DesiredSize.Width / 2 - x1;
                else
                    p.HorizontalOffset = (SelectedNode.OffsetX + (SelectedNode.Info as INodeInfo).DesiredSize.Width / 2) - x1;
                if (height > h - y + y1)
                    p.VerticalOffset = SelectedNode.OffsetY - height - y1;
                else
                    p.VerticalOffset = SelectedNode.OffsetY - y1;
                p.Child = c;
                (p.Child as MindMapDemo.ColorPicker).DataContext = SelectedNode;
                p.IsOpen = true;
            }
        }

        void sf_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if ((floorplan.SelectedItems as CustomSelector).SelectedObject is FloorPlanConnector)
            {
                FloorPlanConnector con = ((floorplan.SelectedItems as CustomSelector).SelectedObject as FloorPlanConnector);
                con.ConThickness = (double)e.NewValue;                    //* 50) / 12;
                ((sender as SfRadialSlider).Content as TextBlock).Text = con.ConThickness.ToString() + "''";
                con.ConnectorGeometryStyle = SetStyle(con.ConThickness);
            }
        }

        private Style SetStyle(double p)
        {
            Style s = new Style(typeof(Windows.UI.Xaml.Shapes.Path));
            s.Setters.Add(new Setter(Windows.UI.Xaml.Shapes.Path.StrokeProperty, new SolidColorBrush(Colors.Black)));
            s.Setters.Add(new Setter(Windows.UI.Xaml.Shapes.Path.StrokeThicknessProperty, p));
            return s;
        }
        #endregion

        #region Info

        private Geometry GetArrow()
        {
            PathGeometry arrow = new PathGeometry();
            PathFigure fig = new PathFigure();
            fig.StartPoint = new Point(0, dist - arrowSize.Height / 2);
            fig.Segments = new PathSegmentCollection();
            fig.Segments.Add(new LineSegment() { Point = new Point(arrowSize.Width - 2, dist) });
            fig.Segments.Add(new LineSegment() { Point = new Point(0, dist + arrowSize.Height / 2) });
            fig.IsFilled = true;
            fig.IsClosed = true;
            arrow.Figures.Add(fig);
            fig = new PathFigure();
            fig.StartPoint = new Point(arrowSize.Width - 2, 0);
            fig.Segments.Add(new LineSegment() { Point = new Point(arrowSize.Width - 2, (dist * 2)) });
            arrow.Figures.Add(fig);
            return arrow;
        }

        private Style GetArrowStyle()
        {
            Style style = new Style(typeof(SPath));
            style.Setters.Add(new Setter(SPath.WidthProperty, arrowSize.Width));
            style.Setters.Add(new Setter(SPath.HeightProperty, dist));
            style.Setters.Add(new Setter(SPath.StretchProperty, Stretch.Fill));
            style.Setters.Add(new Setter(SPath.StrokeThicknessProperty, 1));
            style.Setters.Add(new Setter(SPath.FillProperty, ColorConverter("#FFBF2121")));
            style.Setters.Add(new Setter(SPath.StrokeProperty, ColorConverter("#FFBF2121")));
            return style;
        }

        private Style GetLineStyle()
        {
            Style style = new Style(typeof(SPath));
            style.Setters.Add(new Setter(SPath.StrokeThicknessProperty, 1));
            style.Setters.Add(new Setter(SPath.StrokeProperty, new SolidColorBrush(Colors.Red)));
            return style;
        }

        private void updateWallInfo(FloorPlanConnector con, bool head = false, bool tail = false)
        {
            if (con != null)
            {
                if (wallInfo == null)
                {
                    createwallinfo();
                    if (wallInfo != null)
                    {
                        if (_connectors != null)
                        {
                            if (!_connectors.Contains(wallInfo))
                            {
                                _connectors.Add(wallInfo);
                            }
                        }
                    }
                }
                if (!con.IsSplit)
                {
                    if (wallInfo != null)
                    {
                        wallInfo.Visibility = Visibility.Visible;
                        double angle = FindAngle(con.SourcePoint, con.TargetPoint);
                        wallInfo.SourcePoint = Transform(con.SourcePoint, dist, (angle - 90));
                        wallInfo.TargetPoint = Transform(con.TargetPoint, dist, (angle - 90));
                        double val = FindLength(wallInfo.SourcePoint, wallInfo.TargetPoint);
                        (wallInfo.ConnectorAnnotations.First() as IAnnotation).Content = val.Feetwithinches();
                      //  wallInfo.ZIndex = 2000;
                    }
                }
                else
                {
                    wallInfo.Visibility = Visibility.Visible;
                    double angle = FindAngle(con.SourcePoint, con.TargetPoint);
                    Point centerpoint;
                    //new Point((con.SourcePoint.X + con.TargetPoint.X) / 2, (con.SourcePoint.Y + con.TargetPoint.Y) / 2);
                    if (head)
                    {
                        centerpoint = (Point)((con.Segments as ObservableCollection<object>)[0] as StraightSegment).Point;
                        wallInfo.SourcePoint = Transform(con.SourcePoint, dist, (angle - 90));
                        wallInfo.TargetPoint = Transform(centerpoint, dist, (angle - 90));
                        double val = FindLength(wallInfo.SourcePoint, centerpoint);
                        (wallInfo.ConnectorAnnotations.First() as IAnnotation).Content = val.Feetwithinches();
                    }
                    if (tail)
                    {
                        centerpoint = (Point)((con.Segments as ObservableCollection<object>)[0] as StraightSegment).Point;
                        wallInfo.SourcePoint = Transform(centerpoint, dist, (angle - 90));
                        wallInfo.TargetPoint = Transform(con.TargetPoint, dist, (angle - 90));
                        double val = FindLength(centerpoint, wallInfo.TargetPoint);
                        (wallInfo.ConnectorAnnotations.First() as IAnnotation).Content = val.Feetwithinches();
                    }
                }
            }
            else
            {
                if (wallInfo != null)
                {
                    wallInfo.Visibility = Visibility.Collapsed;
                    wallInfo.ZIndex = 0;
                }
            }
        }

        public static double FindLength(Point s, Point e)
        {
            double length;
            length = Math.Sqrt(Math.Pow((s.X - e.X), 2) + Math.Pow((s.Y - e.Y), 2));
            return length;
        }

        public static double FindAngle(Point s, Point e)
        {
            if (s.Equals(e))
            {
                return 0d;
            }
            Point r = new Point(e.X, s.Y);
            double sr = FindLength(s, r);
            double re = FindLength(r, e);
            double es = FindLength(e, s);
            double ang = Math.Asin(re / es);
            ang = ang * 180 / Math.PI;
            if (s.X < e.X)
            {
                if (s.Y < e.Y)
                {

                }
                else
                {
                    ang = 360 - ang;
                }
            }
            else
            {
                if (s.Y < e.Y)
                {
                    ang = 180 - ang;
                }
                else
                {
                    ang = 180 + ang;
                }
            }
            return ang;
        }

        public static Point Transform(Point s, double length, double angle)
        {
            return new Point()
            {
                X = s.X + length * Math.Cos(angle * Math.PI / 180),
                Y = s.Y + length * Math.Sin(angle * Math.PI / 180)
            };
        }

        //private void floorplan_PointerMoved(object sender, PointerRoutedEventArgs e)
        //{
        //    if (e.Pointer.IsInContact && !infoSizeRotate)
        //    {
        //        UpdateInfo(size: true);
        //    }
        //}

        //private void thumb_ResizeStarting(object sender, DiagramThumbDragEventArgs args)
        //{
        //    UpdateInfo(size: true);
        //}

        //private void thumb_ResizeComplete(object sender, DiagramThumbDragEventArgs args)
        //{
        //    UpdateInfo();
        //}

        //private void thumb_RotateStarting(object sender, DiagramThumbDragEventArgs args)
        //{
        //    UpdateInfo(angle: true);
        //}

        //private void UpdateInfo(bool size = false, bool angle = false, bool pos = false)
        //{
        //    CustomSelector sel = this.floorplan.SelectedItems as CustomSelector;
        //    ISelectorInfo info = sel.Info as ISelectorInfo;
        //    if (size)
        //    {
        ////        infoSizeRotate = true;
        ////        string val = info.ActualWidth.Feetwithinches();
        ////        string val1 = info.ActualHeight.Feetwithinches();
        ////        sel.Information = "W: " + val + ", H: " + val1;
        ////    }
        ////    else if (angle)
        ////    {
        ////        infoSizeRotate = true;
        ////        sel.Information = "Angle: " + (int)sel.RotateAngle % 360;
        ////    }
        ////    else if (pos)
        ////    {
        ////        infoSizeRotate = false;
        ////        //string x = c.Feetwithinches();
        ////        //string y = info.OffsetY.Feetwithinches();
        ////        // sel.Information = "X: " + (int)info.OffsetX + ", Y: " + (int)info.OffsetY;
        ////    }
        ////    else
        ////    {
        ////        infoSizeRotate = false;
        ////        sel.Information = "";
        ////    }
        ////}

        //private void thumb_RotateComplete(object sender, DiagramThumbDragEventArgs args)
        //{
        //    UpdateInfo();
        //}
        #endregion

        #region TestPurpose
        void FloorPlannerDiagram_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                FloorPlanNode node = e.NewItems[0] as FloorPlanNode;
                node.Background = new SolidColorBrush(Colors.Transparent);
                node.Loaded += node_Loaded;
                node.PropertyChanged += node_PropertyChanged;
                if (node.ContentName != null)
                {
                    node.Content = node.ContentName;
                }
                UpdateOptionnode(null);              
                if (p != null)
                {
                    p.IsOpen = false;
                }               
            }
            else
            {
                
            }
        }

        void node_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as FloorPlanNode).Content != null)
            {
                if (Enum.GetNames(typeof(BasicShapes)).Any(w => w == (sender as FloorPlanNode).Content.ToString()))
                {
                    (sender as FloorPlanNode).IsShapeNode = true;

                    foreach (BasicShapes b in Enum.GetValues(typeof(BasicShapes)))
                    {
                        if (b.ToString() == (sender as FloorPlanNode).Content.ToString())
                        {
                            (sender as FloorPlanNode).Shape = b.ToGeometry();
                            (sender as FloorPlanNode).ShapeStyle = GetStyle("#FFFFFFFF");
                        }
                    }
                    (sender as FloorPlanNode).Content = null;
                    (sender as FloorPlanNode).ContentTemplate = null;
                }
                else
                {
                    if (!(sender as FloorPlanNode).IsTextNode)
                    {

                        (sender as FloorPlanNode).ContentName = (sender as FloorPlanNode).Content.ToString();
                    }
                }
                if ((sender as FloorPlanNode).ContentName == "Window")
                {

                    // node.Width = 5;
                    (sender as FloorPlanNode).UnitHeight = 6;
                }
            }
        }

        private Style GetStyle(string p)
        {
            Style s = new Style(typeof(Windows.UI.Xaml.Shapes.Path));
            s.Setters.Add(new Setter(Windows.UI.Xaml.Shapes.Path.StrokeProperty, ColorConverter("#FFC4C4C4")));
            s.Setters.Add(new Setter(Windows.UI.Xaml.Shapes.Path.FillProperty, ColorConverter(p)));
            s.Setters.Add(new Setter(Windows.UI.Xaml.Shapes.Path.StretchProperty, Stretch.Fill));
            s.Setters.Add(new Setter(Windows.UI.Xaml.Shapes.Path.StrokeThicknessProperty, 2));
            return s;
        }

        void node_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Content")
            {
                (sender as FloorPlanNode).ContentName = (sender as FloorPlanNode).Content.ToString();
                if ((sender as FloorPlanNode).ContentName == "Window")
                {
                    // node.Width = 5;
                    (sender as FloorPlanNode).UnitHeight = 6;
                }
            }
            else if (e.PropertyName == "SelectedColor")
            {
                (sender as FloorPlanNode).ShapeStyle = GetStyle((sender as FloorPlanNode).SelectedColor.ToString());
            }
        }

        void node_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if ((sender as FloorPlanNode).Content.ToString() == "Door" || (sender as FloorPlanNode).Content.ToString() == "Door1")
            {

                FloorPlanConnector Hconn = null;
                foreach (FloorPlanConnector con in _connectors)
                {
                    if (Math.Round(con.SourcePoint.Y) > Math.Round(con.TargetPoint.Y) || Math.Round(con.SourcePoint.Y) < Math.Round(con.TargetPoint.Y))
                    {
                        if (Math.Round((con.SourcePoint.X) - ((sender as FloorPlanNode).OffsetX - ((sender as FloorPlanNode).UnitWidth / 2))) <= 25)
                        {
                            (sender as FloorPlanNode).OffsetX = con.SourcePoint.X - ((sender as FloorPlanNode).UnitWidth / 2);
                            (sender as FloorPlanNode).OffsetY = (con.SourcePoint.Y + con.TargetPoint.Y) / 2;
                            Hconn = con;
                        }
                    }
                    else if (Math.Round(con.SourcePoint.X) < Math.Round(con.TargetPoint.X))
                    {
                        if (Math.Round((con.SourcePoint.Y) - ((sender as FloorPlanNode).OffsetY - ((sender as FloorPlanNode).UnitHeight / 2))) <= 25)
                        {
                            (sender as FloorPlanNode).OffsetY = con.SourcePoint.Y - ((sender as FloorPlanNode).UnitHeight / 2);
                            (sender as FloorPlanNode).OffsetX = (con.SourcePoint.X + con.TargetPoint.X) / 2;
                            Hconn = con;
                        }
                    }

                    if (Hconn != null)
                    {
                        return;
                    }

                }
            }
        }

        void node_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {

        }

        void node_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {

        }

        private bool Filter(Syncfusion.UI.Xaml.Diagram.Stencil.SymbolFilterProvider sender, Syncfusion.UI.Xaml.Diagram.Stencil.ISymbol symbol)
        {
            return true;
        }

        private void CreateNodes()
        {
            Node singlebed = AddNode(Shapes.SingleBed, 400, 250, 100, 50, "#FFEA8C1C");
            Node singlebed1 = AddNode(Shapes.SingleBed, 400, 450, 100, 50, "#FF2D8BBA");
            Node doublebed = AddNode(Shapes.DoubleBed, 600, 250, 100, 80, "#FF749E19");
            Node doublebed1 = AddNode(Shapes.DoubleBed, 900, 250, 100, 80, "#FFC93667");
            Node dinningNode = AddNode(Shapes.DinningTable, 610, 450, 50, 100, "#FF5B1B02");
            Node chairNode = AddNode(Shapes.Chair, 560, 450, 15, 12, "#FFBA8E2A");
            Node chairNode1 = AddNode(Shapes.Chair, 690, 450, 15, 12, "#FFBA8E2A");
            Node chairNode2 = AddNode(Shapes.Chair, 585, 420, 15, 12, "#FFBA8E2A");
            Node chairNode3 = AddNode(Shapes.Chair, 615, 420, 15, 12, "#FFBA8E2A");
            Node chairNode4 = AddNode(Shapes.Chair, 645, 420, 15, 12, "#FFBA8E2A");
            Node chairNode5 = AddNode(Shapes.Chair, 585, 480, 15, 12, "#FFBA8E2A");
            Node chairNode6 = AddNode(Shapes.Chair, 615, 480, 15, 12, "#FFBA8E2A");
            Node chairNode7 = AddNode(Shapes.Chair, 645, 480, 15, 12, "#FFBA8E2A");
            Node SofaNode = AddNode(Shapes.CenterSofa, 625, 600, 50, 100, "sofa");
            Node sofaNode1 = AddNode(Shapes.SideSofa, 550, 700, 50, 50, "sofa");
            Node sofanode2 = AddNode(Shapes.SideSofa, 700, 700, 50, 50, "sofa");
            Node tablenode = AddNode(Shapes.Table, 625, 700, 50, 80, "sofa");
        }

        private Node AddNode(Shapes shapes, double offx, double offy, double hei, double wid, string color)
        {
            FloorPlanNode n = new FloorPlanNode();
            n.UnitWidth = wid;
            n.UnitHeight = hei;
            n.OffsetX = offx;
            n.OffsetY = offy;
            //Shape = shapes.ToGeometry(),
            n.ShapeStyle = GetBedStyle(color);
            _nodes.Add(n);
            return n;
        }

        private Brush ColorConverter(string hexaColor)
        {
            if (hexaColor != null)
            {
                if (hexaColor.StartsWith("#"))
                {
                    byte r = Convert.ToByte(hexaColor.Substring(1, 2), 16);
                    byte g = Convert.ToByte(hexaColor.Substring(3, 2), 16);
                    byte b = Convert.ToByte(hexaColor.Substring(5, 2), 16);
                    byte a = Convert.ToByte(hexaColor.Substring(7, 2), 16);
                    SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(r, g, b, a));
                    return myBrush;
                }
                else
                {
                    return this.Resources["sofa"] as LinearGradientBrush;
                }
            }
            return null;
        }

        private Style GetBedStyle(string colorvalue)
        {
            Style s = new Style();
            s.TargetType = typeof(Windows.UI.Xaml.Shapes.Path);
            s.Setters.Add(new Setter(Windows.UI.Xaml.Shapes.Path.FillProperty, ColorConverter(colorvalue)));
            s.Setters.Add(new Setter(Windows.UI.Xaml.Shapes.Path.StretchProperty, Stretch.Fill));
            return s;
        }

        private void con_Click_1(object sender, RoutedEventArgs e)
        {
            FloorPlanConnector c = new FloorPlanConnector()

            {
                SourcePoint = new Point(200, 200),
                TargetPoint = new Point(400, 200)

            };
            c.TargetDecorator = null;
            _connectors.Add(c);
        }

        private async void save_Click_1(object sender, RoutedEventArgs e)
        {
            StorageFile s = null;
            FileSavePicker f = new FileSavePicker();
            f.CommitButtonText = "Save";
            f.FileTypeChoices.Add("xml", new List<string>() { ".xml" });
            s = await f.PickSaveFileAsync();
            if (s != null)
            {
                using (IRandomAccessStream writeStream = await s.OpenAsync(FileAccessMode.ReadWrite))
                {
                    if (writeStream != null)
                    {
                        Stream str = writeStream.AsStream();
                        floorplan.Save(str);
                    }
                }
            }
        }

        private async void load_Click_1(object sender, RoutedEventArgs e)
        {
            _nodes.Clear();
            _connectors.Clear();
            StorageFile s = null;
            FileOpenPicker f = new FileOpenPicker();
            f.CommitButtonText = "Load";
            f.FileTypeFilter.Add(".xml");
            s = await f.PickSingleFileAsync();
            if (s != null)
            {
                using (IRandomAccessStream writeStream = await s.OpenAsync(FileAccessMode.ReadWrite))
                {
                    if (writeStream != null)
                    {
                        Stream str = writeStream.AsStream();
                        floorplan.Load(str);

                        foreach (FloorPlanConnector c in _connectors)
                        {
                            c.TargetDecorator = null;
                        }
                        foreach (FloorPlanNode f1 in _nodes)
                        {
                            f1.ContentTemplate = this.Resources[f1.ContentName.ToString()] as DataTemplate;
                        }
                    }
                }
            }
        }

        private void del_Click_1(object sender, RoutedEventArgs e)
        {
            if (floorplan.SelectedItems != null)
            {
                if ((floorplan.SelectedItems as CustomSelector).Nodes != null)
                {
                    ObservableCollection<FloorPlanNode> v = new ObservableCollection<FloorPlanNode>();
                    foreach (FloorPlanNode fnode in (IEnumerable<object>)(floorplan.SelectedItems as CustomSelector).Nodes)
                    {
                        v.Add(fnode);
                    }

                    foreach (FloorPlanNode fnode in v)
                    {
                        _nodes.Remove(fnode);
                    }
                }
                if ((floorplan.SelectedItems as CustomSelector).Connectors != null)
                {
                    ObservableCollection<FloorPlanConnector> c = new ObservableCollection<FloorPlanConnector>();
                    foreach (FloorPlanConnector fnode in (IEnumerable<object>)(floorplan.SelectedItems as CustomSelector).Connectors)
                    {
                        c.Add(fnode);
                    }

                    foreach (FloorPlanConnector fnode in c)
                    {
                        _connectors.Remove(fnode);
                    }
                }
            }
        }
        #endregion
    }


    public class CustomSelector : SelectorViewModel
    {
        public IGraph Graph { get; set; }

        public CustomSelector()
        {
            this.Nodes = new ObservableCollection<object>();
            this.Connectors = new ObservableCollection<object>();
            this.Groups = new ObservableCollection<object>();
            //Information = "Test";
            InfoVis = Visibility.Collapsed;
        }

        private bool allowdelete;

        public bool AllowDelete
        {
            get
            {
                return allowdelete;
            }
            set
            {
                if (allowdelete != value)
                {
                    allowdelete = value;
                    OnPropertyChanged("AllowDelete");
                }
            }
        }
        private IGroupable _SelectedObject;
        public IGroupable SelectedObject
        {
            get { return _SelectedObject; }
            set
            {
                _SelectedObject = value;

                OnPropertyChanged("SelectedObject");
            }
        }

        //private string _information;
        private Visibility _infoVis;

        //public string Information
        //{
        //    get { return _information; }
        //    set
        //    {
        //        _information = value;
        //        OnPropertyChanged("Information");
        //        if (string.IsNullOrEmpty(value))
        //        {
        //            InfoVis = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            InfoVis = Visibility.Visible;
        //        }
        //    }
        //}

        public Visibility InfoVis
        {
            get { return _infoVis; }
            set { _infoVis = value; OnPropertyChanged("InfoVis"); }
        }
    }

    public static class StringExtensions
    {
        public static string Feetwithinches(this double display)
        {
            int defaultvalue = 50;
            var value = display / defaultvalue;
            var feet = Math.Floor(value);
            var inches = Math.Round((value - feet) * 12);
            return feet + "'" + inches + "''";
        }
    }

}
