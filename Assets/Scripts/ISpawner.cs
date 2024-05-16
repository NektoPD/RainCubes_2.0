using System;

public interface ISpawner 
{
    public event Action<int> TotalCountChange;
    public event Action<int> ActiveCountChange;
}

