using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChartJs.Blazor;
using ChartJs.Blazor.BarChart;
using Dashboard.Controllers;
using Dashboard.Dtos;
using Microsoft.AspNetCore.Components;

namespace Dashboard.Models;

public class SensorsDataModel
{
    public IList<ISensorDto> AllSensorsRecords { get; private set; } = new List<ISensorDto>();

    public IList<ISensorDto> FilteredSensorsRecords { get; private set; } = new List<ISensorDto>();

    private readonly ChartController _chartController;

    private readonly TableController _tableController;

    private readonly DownloadController _downloadController;
    
    private bool _sortByTypeAscending;
    private bool _sortByIdAscending;
    private bool _sortByDateAscending;
    private bool _sortByValueAscending;
    
    public BarConfig BarConfig => _chartController.BarConfig;

    public SensorsDataModel(ChartController chartController, TableController tableController, DownloadController downloadController)
    {
        _chartController = chartController;
        _tableController = tableController;
        _downloadController = downloadController;
    }

    public Chart Chart
    {
        get => _chartController.Chart;
        set => _chartController.Chart = value;
    }

    private string SelectedType { get; set; } = "";
    public string CheckType
    {
        get => SelectedType;
        set
        {
            var selectedEventArgs = new ChangeEventArgs
            {
                Value = value
            };
            SelectedType = selectedEventArgs.Value.ToString();
            SendFilters().Wait();
        }
    }
    
    private DateTime SelectedAfter { get; set; } = DateTime.MinValue;

    public DateTime CheckAfter
    {
        get => SelectedAfter;
        set
        {
            ChangeEventArgs selectedEventArgs = new ChangeEventArgs
            {
                Value = value
            };
            SelectedAfter = (DateTime)selectedEventArgs.Value;
            SendFilters().Wait();
        }
    }

    private DateTime SelectedBefore { get; set; } = DateTime.MaxValue;

    public DateTime CheckBefore
    {
        get => SelectedBefore;
        set
        {
            ChangeEventArgs selectedEventArgs = new ChangeEventArgs
            {
                Value = value
            };
            SelectedBefore = (DateTime)selectedEventArgs.Value;
            SendFilters().Wait();
        }
    }

    private int SelectedId { get; set; } = -1;
    public int CheckId
    {
        get => SelectedId;
        set
        {
            ChangeEventArgs selectedEventArgs = new ChangeEventArgs
            {
                Value = value
            };
            if (selectedEventArgs.Value.ToString() != string.Empty)
            {
                SelectedId = int.Parse(selectedEventArgs.Value.ToString() ?? string.Empty);
            }
            SendFilters().Wait();
        }
    }

    public async Task DownloadCsv()
    {
        await _downloadController.DownloadCsv(FilteredSensorsRecords);
    }

    public async Task DownloadJson()
    {
        await _downloadController.DownloadJson(FilteredSensorsRecords);
    }
    
    private async Task SendFilters()
    {
        FilteredSensorsRecords = await _tableController.ApplyFilters(SelectedType, SelectedId, SelectedAfter, SelectedBefore);
        _chartController.UpdatePieConfig(FilteredSensorsRecords);
    }
    
    public async Task SortByType()
    {
        _sortByTypeAscending = !_sortByTypeAscending;
        FilteredSensorsRecords = await _tableController.SortBySensorType(FilteredSensorsRecords, _sortByTypeAscending);
    }

    public async Task SortById()
    {
        _sortByIdAscending = !_sortByIdAscending;
        FilteredSensorsRecords = await _tableController.SortBySensorId(FilteredSensorsRecords, _sortByIdAscending);
    }

    public async Task SortByDate()
    {
        _sortByDateAscending = !_sortByDateAscending;
        FilteredSensorsRecords = await _tableController.SortByDate(FilteredSensorsRecords, _sortByDateAscending);
    }

    public async Task SortByValue()
    {
        _sortByValueAscending = !_sortByValueAscending;
        FilteredSensorsRecords = await _tableController.SortByValue(FilteredSensorsRecords, _sortByValueAscending);
    }

    public async Task Init()
    {
        AllSensorsRecords = await _tableController.FetchData();
        FilteredSensorsRecords = AllSensorsRecords;
        _chartController.ConfigurePieConfig(AllSensorsRecords);
    }
}