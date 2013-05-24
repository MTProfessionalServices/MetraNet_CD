﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MetraTech.UI.Controls.Layouts;
using MetraTech.UI.Common;
using MetraTech.DomainModel.Enums.Core.UI;

namespace MetraTech.UI.Controls
{
  public class DashboardColumn
  {
    #region Properties
    private Guid id;
    public Guid ID
    {
      get { return id; }
      set { id = value; }
    }

    private string name;
    public string Name
    {
      get { return name; }
      set { name = value; }
    }

    private string description;
    public string Description
    {
      get { return description; }
      set { description = value; }
    }

    private string title;
    public string Title
    {
      get { return title; }
      set { title = value; }
    }

    private int position;
    public int Position
    {
      get { return position; }
      set { position = value; }
    }

    private DashboardColumnWidth width;
    public DashboardColumnWidth Width
    {
      get { return width; }
      set { width = value; }
    }

    private string cssClass;
    public string CssClass
    {
      get { return cssClass; }
      set { cssClass = value; }
    }

    private string style;
    public string Style
    {
      get { return style; }
      set { style = value; }
    }

    private List<BaseWidget> widgets;
    public List<BaseWidget> Widgets {
      get
      {
        if (widgets == null)
        {
          widgets = new List<BaseWidget>();
        }
        return widgets;
      }
    }
    #endregion

    public void LoadFromBME(Core.UI.Column column)
    {
      ID = column.Id;
      Name = column.Name;
      Description = column.Description;
      Title = column.Title;
      Position = column.Position;
      Width = column.Width;
      CssClass = column.CssClass;
      Style = column.Style;

      foreach (Core.UI.Widget wl in column.Widgets)
      {
        //Create object based on type
        BaseWidget widget = BaseWidget.CreateWidget();
        widget.LoadFromBME(wl);

        Widgets.Add(widget);
      }

    }

    public void LoadFromLayout(DashboardColumnLayout layout)
    {
      ID = layout.ID;
      Name = layout.Name;
      Description = layout.Description;
      Title = layout.Title;
      Position = layout.Position;
      Width = layout.Width;
      CssClass = layout.CssClass;
      Style = layout.Style;

      foreach (WidgetLayout wl in layout.Widgets)
      {
        //Create object based on type
        BaseWidget widget = BaseWidget.CreateWidget();
        widget.LoadFromLayout(wl);

        Widgets.Add(widget);
      }
    }
  }
}
