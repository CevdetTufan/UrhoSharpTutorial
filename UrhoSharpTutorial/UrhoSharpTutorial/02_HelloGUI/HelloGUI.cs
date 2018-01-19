using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.Gui;
using Urho.Resources;

namespace UrhoSharpTutorial
{
    public class HelloGUI : MyGame
    {
        private Window window;
        private UIElement uiRoot;
        private IntVector2 dragBeginPositon;
        private Button draggableFish;
             
        public HelloGUI(ApplicationOptions options):base(options)
        {
        }

        protected override void Start()
        {
            base.Start();

            uiRoot = UI.Root;
            Input.SetMouseVisible(true, false);

            var cache = ResourceCache;
            XmlFile style=cache.GetXmlFile("UI/DefaultStyle.xml");

            uiRoot.SetDefaultStyle(style);

            InitWindow();
            InitControls();
            CreateDraggableFish();
        }

        private void InitWindow()
        {
            window = new Window();
            uiRoot.AddChild(window);

            window.SetMinSize(384, 192);
            window.SetLayout(LayoutMode.Vertical, 6, new IntRect(6, 6, 6, 6));
            window.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            window.Name = "Window";

            UIElement titleBar = new UIElement();
            titleBar.SetMinSize(0, 24);
            titleBar.VerticalAlignment = VerticalAlignment.Top;
            titleBar.LayoutMode = LayoutMode.Horizontal;

            var windowTitle = new Text();
            windowTitle.Name = "WindowTitle";
            windowTitle.Value = "Hello GUI!";

            Button buttonClose = new Button();
            buttonClose.Name = "CloseButton";

            titleBar.AddChild(windowTitle);
            titleBar.AddChild(buttonClose);
            window.AddChild(titleBar);

            window.SetStyleAuto(null);
            windowTitle.SetStyleAuto(null);
            buttonClose.SetStyle("CloseButton", null);

            buttonClose.SubscribeToReleased(q => Exit());
            UI.SubscribeToUIMouseClick(HandleControlClicked);
        }

        private void InitControls()
        {
            // Create a CheckBox
            CheckBox checkBox = new CheckBox();
            checkBox.Name = "CheckBox";

            // Create a Button
            Button button = new Button();
            button.Name = "Button";
            button.MinHeight = 24;

            // Create a LineEdit
            LineEdit lineEdit = new LineEdit();
            lineEdit.Name = "LineEdit";
            lineEdit.MinHeight = 24;

            // Add controls to Window
            window.AddChild(checkBox);
            window.AddChild(button);
            window.AddChild(lineEdit);

            // Apply previously set default style
            checkBox.SetStyleAuto(null);
            button.SetStyleAuto(null);
            lineEdit.SetStyleAuto(null);
        }

        private void CreateDraggableFish()
        {
            var cache = ResourceCache;
            var graphics = Graphics;

            draggableFish = new Button();
            draggableFish.Texture = cache.GetTexture2D("Textures/UrhoDecal.dds");
            draggableFish.BlendMode = BlendMode.Add;
            draggableFish.SetSize(128, 128);
            draggableFish.SetPosition((graphics.Width - draggableFish.Width) / 2, 200);
            draggableFish.Name = "Fish";

            uiRoot.AddChild(draggableFish);


            ToolTip toolTip = new ToolTip();
            draggableFish.AddChild(toolTip);
            toolTip.Position = new IntVector2(draggableFish.Width + 5, draggableFish.Width / 2);

            BorderImage textHolder = new BorderImage();
            toolTip.AddChild(textHolder);
            textHolder.SetStyle("ToolTipBorderImage", null);
            var toolTipText = new Text();
            textHolder.AddChild(toolTipText);
            toolTipText.SetStyle("ToolTipText", null);
            toolTipText.Value = "Please drag me!";

            draggableFish.SubscribeToDragBegin(HandleDragBegin);
            draggableFish.SubscribeToDragMove(HandelDragMove);
        }

        private void HandleControlClicked(UIMouseClickEventArgs args)
        {
            var windowTitle = window.GetChild("WindowTitle", true) as Text;
            UIElement clicked = args.Element;

            string name = "...?";
            if (clicked != null)
            {
                // Get the name of the control that was clicked
                name = clicked.Name;
            }

            // Update the Window's title text
            windowTitle.Value = $"Hello {name}!";
        }

        private void HandleDragBegin(DragBeginEventArgs args)
        {
            dragBeginPositon = new IntVector2(args.ElementX, args.ElementY);
        }
        private void HandelDragMove(DragMoveEventArgs args)
        {
            IntVector2 dragCurrentPossiton = new IntVector2(args.X, args.Y);
            args.Element.Position = dragCurrentPossiton - dragBeginPositon;
        }
    }
}
