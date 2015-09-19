﻿using System.Collections.Generic;
using System.Windows.Forms;

namespace DarkUI
{
    public class DarkDockPanel : UserControl
    {
        #region Field Region

        private List<DarkDockContent> _contents;
        private Dictionary<DarkDockArea, DarkDockRegion> _regions;

        #endregion

        #region Property Region

        public IMessageFilter MessageFilter { get; private set; }

        #endregion

        #region Constructor Region

        public DarkDockPanel()
        {
            MessageFilter = new DarkDockResizeFilter(this);

            _regions = new Dictionary<DarkDockArea, DarkDockRegion>();
            _contents = new List<DarkDockContent>();

            BackColor = Colors.GreyBackground;

            CreateRegions();
        }

        #endregion

        #region Method Region

        public void AddContent(DarkDockContent dockContent)
        {
            if (_contents.Contains(dockContent))
                return;

            if (dockContent.DockArea == DarkDockArea.None)
                return;

            dockContent.DockPanel = this;
            _contents.Add(dockContent);

            var region = _regions[dockContent.DockArea];
            region.AddContent(dockContent);
        }

        public void RemoveContent(DarkDockContent dockContent)
        {
            if (!_contents.Contains(dockContent))
                return;

            dockContent.DockPanel = null;
            _contents.Remove(dockContent);

            var region = _regions[dockContent.DockArea];
            region.RemoveContent(dockContent);
        }

        private void CreateRegions()
        {
            var documentRegion = new DarkDockRegion(this, DarkDockArea.Document);
            _regions.Add(DarkDockArea.Document, documentRegion);

            var leftRegion = new DarkDockRegion(this, DarkDockArea.Left);
            _regions.Add(DarkDockArea.Left, leftRegion);

            var rightRegion = new DarkDockRegion(this, DarkDockArea.Right);
            _regions.Add(DarkDockArea.Right, rightRegion);

            var bottomRegion = new DarkDockRegion(this, DarkDockArea.Bottom);
            _regions.Add(DarkDockArea.Bottom, bottomRegion);

            // Add the regions in this order to force the bottom region to be positioned
            // between the left and right regions properly.
            Controls.Add(documentRegion);
            Controls.Add(bottomRegion);
            Controls.Add(leftRegion);
            Controls.Add(rightRegion);

            // Create tab index for intuitive tabbing order
            documentRegion.TabIndex = 0;
            rightRegion.TabIndex = 1;
            bottomRegion.TabIndex = 2;
            leftRegion.TabIndex = 3;
        }

        #endregion
    }
}
    