using System.Collections.Generic;
using System.Linq;
using ChartJs.Blazor;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Util;
using Dashboard.Dtos;

namespace Dashboard.Controllers;

public class ChartController
{
    public BarConfig BarConfig { get; private set; }

    public Chart Chart { get; set; }

    public void UpdatePieConfig(IList<ISensorDto> pieData)
    {
        BarConfig.Data.Datasets.Clear();
        BarConfig.Data.Labels.Clear();
        foreach (var id in pieData.Select(x => x.SensorId).Distinct().OrderBy(x => x))
        {
            var data = pieData.Where(x => x.SensorId.Equals(id)).ToList();
            var typeData = new List<decimal>();
            foreach (var type in data.Select(x => x.Type).Distinct())
            {
                typeData.Add(data.Where(x => x.Type.Equals(type)).Select(x => x.Value).Average());
            }
            IDataset<decimal> dataset = new BarDataset<decimal>(typeData)
            {
                Label = id.ToString(),

                BackgroundColor = ColorUtil.RandomColorString()
            };
            BarConfig.Data.Datasets.Add(dataset);

        }
        foreach (var type in pieData.Select(x => x.Type).Distinct())
        {
            BarConfig.Data.Labels.Add(type);

        }
        Chart.Update();
    }
    
    public void ConfigurePieConfig(IList<ISensorDto> pieData)
    {
        BarConfig = new BarConfig
        {
            Options = new BarOptions
            {
                Responsive = true,
                Title = new OptionsTitle
                {
                    Display = true,
                    Text = "Average values of sensors"
                },
                Legend = new Legend
                {
                    Position = Position.Top
                }
            }
        };

        BarConfig.Data.Datasets.Clear();
        BarConfig.Data.Labels.Clear();
        foreach (var id in pieData.Select(x => x.SensorId).Distinct().OrderBy(x => x))
        {
            var data = pieData.Where(x => x.SensorId.Equals(id)).ToList();
            var typeData = new List<decimal>();
            foreach (var type in data.Select(x => x.Type).Distinct())
            {
                typeData.Add(data.Where(x => x.Type.Equals(type)).Select(x => x.Value).Average());
            }
            IDataset<decimal> dataset = new BarDataset<decimal>(typeData)
            {
                Label = id.ToString(),

                BackgroundColor = ColorUtil.RandomColorString()
            };
            BarConfig.Data.Datasets.Add(dataset);

        }
        foreach (var type in pieData.Select(x => x.Type).Distinct())
        {
            BarConfig.Data.Labels.Add(type);
        }
    }
}