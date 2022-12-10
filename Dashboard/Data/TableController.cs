using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.Dtos;

namespace Dashboard.Data;

public class TableController
{
    private readonly SensorsService _service;

    private List<ISensorDto> _list;

    public TableController(SensorsService service)
    {
        _service = service;
    }

    public async Task<List<ISensorDto>> FetchData()
    {
        if (_list == null)
        {
            _list = new List<ISensorDto>();
            _list.AddRange(await _service.GetHumidityData());
            _list.AddRange(await _service.GetTemperatureData());
            _list.AddRange(await _service.GetPressureData());
            _list.AddRange(await _service.GetWindData());
        }
        return _list;
    }

    public async Task<List<ISensorDto>> SortBySensorType(bool ascending = true)
    {
        if (_list == null)
        {
            await FetchData();
        }

        List<ISensorDto> tmp = new(_list);
        if (ascending)
        {
            tmp.Sort((a, b) => String.Compare(a.type, b.type, StringComparison.CurrentCulture));
        }
        else
        {
            tmp.Sort((a, b) => - String.Compare(a.type, b.type, StringComparison.CurrentCulture));
        }

        return tmp;
    }
    public async Task<List<ISensorDto>> SortBySensorId(bool ascending = true)
    {
        if (_list == null)
        {
            await FetchData();
        }

        List<ISensorDto> tmp = new List<ISensorDto>(_list);

        if (ascending)
        {
            tmp.Sort((a, b) => a.SensorId.CompareTo(b.SensorId));
        }
        else
        {
            tmp.Sort((a, b) => - a.SensorId.CompareTo(b.SensorId));
        }

        return tmp;
    }

    public async Task<List<ISensorDto>> SortByDate(bool ascending = true)
    {
        if (_list == null)
        {
            await FetchData();
        }

        List<ISensorDto> tmp = new(_list);

        if (ascending)
        {
            tmp.Sort((a, b) => a.Time.CompareTo(b.Time));
        }
        else
        {
            tmp.Sort((a, b) => - a.Time.CompareTo(b.Time));
        }


        return tmp;
    }

    public async Task<List<ISensorDto>> ApplyFilters(string selectedType = "", int selectedId = -1, DateTime? after = null, DateTime? before = null)
    {
        if (_list == null)
        {
            await FetchData();
        }
        List<ISensorDto> tmp = new(_list);

        if (!String.IsNullOrEmpty(selectedType))
        {
            tmp = tmp.Where(f => f.type == selectedType).ToList();
        }
        if (selectedId > -1)
        {
            tmp = tmp.Where(f => f.SensorId == selectedId).ToList();
        }

        tmp = tmp.FindAll(
            i => RoundToSeconds(i.Time) >= RoundToSeconds(after ?? DateTime.MinValue)
                 && RoundToSeconds(i.Time) <= RoundToSeconds(before ?? DateTime.MaxValue)
                 );

        return tmp;
    }

    private static DateTime RoundToSeconds(DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
    }

}
