﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using System.ComponentModel.Design;
using System.Drawing;
using System.Text;

namespace System.Windows.Forms.PropertyGridInternal
{
    internal partial class HotCommands : PropertyGrid.SnappableControl
    {
        private object _component;
        private DesignerVerb[] _verbs;
        private LinkLabel _label;
        private bool _allowVisible = true;
        private int _optimalHeight = -1;

        internal HotCommands(PropertyGrid owner) : base(owner)
        {
            Text = "Command Pane";
        }

        public virtual bool AllowVisible
        {
            get => _allowVisible;
            set
            {
                if (_allowVisible != value)
                {
                    _allowVisible = value;
                    Visible = value && WouldBeVisible;
                }
            }
        }

        /// <summary>
        ///  Constructs the new instance of the accessibility object for this control.
        /// </summary>
        /// <returns>The accessibility object for this control.</returns>
        protected override AccessibleObject CreateAccessibilityInstance()
            => new HotCommandsAccessibleObject(this, OwnerPropertyGrid);

        public override Rectangle DisplayRectangle
        {
            get
            {
                Size size = ClientSize;
                return new Rectangle(4, 4, size.Width - 8, size.Height - 8);
            }
        }

        public LinkLabel Label
        {
            get
            {
                if (_label is null)
                {
                    _label = new LinkLabel
                    {
                        Dock = DockStyle.Fill,
                        LinkBehavior = LinkBehavior.AlwaysUnderline,

                        // Use default LinkLabel colors for regular, active, and visited.
                        DisabledLinkColor = SystemColors.ControlDark
                    };

                    _label.LinkClicked += LinkClicked;
                    Controls.Add(_label);
                }

                return _label;
            }
        }

        public virtual bool WouldBeVisible => _component is not null;

        public override int GetOptimalHeight(int width)
        {
            if (_optimalHeight == -1)
            {
                int lineHeight = (int)(1.5 * Font.Height);
                int verbCount = 0;
                if (_verbs is not null)
                {
                    verbCount = _verbs.Length;
                }

                _optimalHeight = verbCount * lineHeight + 8;
            }

            return _optimalHeight;
        }

        public override int SnapHeightRequest(int request) => request;

        /// <summary>
        ///  Indicates whether or not the control supports UIA Providers via
        ///  IRawElementProviderFragment/IRawElementProviderFragmentRoot interfaces.
        /// </summary>
        internal override bool SupportsUiaProviders => true;

        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (!e.Link.Enabled)
                {
                    return;
                }

                ((DesignerVerb)e.Link.LinkData).Invoke();
            }
            catch (Exception ex)
            {
                RTLAwareMessageBox.Show(
                    this,
                    ex.Message,
                    SR.PBRSErrorTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1,
                    0);
            }
        }

        private void OnCommandChanged(object sender, EventArgs e) => SetupLabel();

        protected override void OnGotFocus(EventArgs e)
        {
            Label.Focus();
            Label.Invalidate();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            _optimalHeight = -1;
        }

        internal void SetColors(Color background, Color normalText, Color link, Color activeLink, Color visitedLink, Color disabledLink)
        {
            Label.BackColor = background;
            Label.ForeColor = normalText;
            Label.LinkColor = link;
            Label.ActiveLinkColor = activeLink;
            Label.VisitedLinkColor = visitedLink;
            Label.DisabledLinkColor = disabledLink;
        }

        public void FocusLabel() => Label.Focus();

        public virtual void SetVerbs(object component, DesignerVerb[] verbs)
        {
            if (_verbs is not null)
            {
                for (int i = 0; i < _verbs.Length; i++)
                {
                    _verbs[i].CommandChanged -= OnCommandChanged;
                }

                _component = null;
                _verbs = null;
            }

            if (component is null || verbs is null || verbs.Length == 0)
            {
                Visible = false;
                Label.Links.Clear();
                Label.Text = null;
            }
            else
            {
                _component = component;
                _verbs = verbs;

                for (int i = 0; i < verbs.Length; i++)
                {
                    verbs[i].CommandChanged += OnCommandChanged;
                }

                if (_allowVisible)
                {
                    Visible = true;
                }

                SetupLabel();
            }

            _optimalHeight = -1;
        }

        private void SetupLabel()
        {
            Label.Links.Clear();
            StringBuilder sb = new();
            var links = new Point[_verbs.Length];
            int charLoc = 0;
            bool firstVerb = true;

            for (int i = 0; i < _verbs.Length; i++)
            {
                if (_verbs[i].Visible && _verbs[i].Supported)
                {
                    if (!firstVerb)
                    {
                        sb.Append(Application.CurrentCulture.TextInfo.ListSeparator);
                        sb.Append(' ');
                        charLoc += 2;
                    }

                    string name = _verbs[i].Text;

                    links[i] = new Point(charLoc, name.Length);
                    sb.Append(name);
                    charLoc += name.Length;
                    firstVerb = false;
                }
            }

            Label.Text = sb.ToString();

            for (int i = 0; i < _verbs.Length; i++)
            {
                if (_verbs[i].Visible && _verbs[i].Supported)
                {
                    LinkLabel.Link link = Label.Links.Add(links[i].X, links[i].Y, _verbs[i]);
                    if (!_verbs[i].Enabled)
                    {
                        link.Enabled = false;
                    }
                }
            }
        }
    }
}
