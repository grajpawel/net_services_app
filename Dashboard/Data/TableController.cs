using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.Dtos;

namespace Dashboard.Data;

public class TableController
{
    private SensorsService _service;

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
}
