using UnityEngine;
using System.Collections;

public struct Amunition
{
    private int _magSize;

    public int currentMag { get; set; }
    public int reserve { get; set; }

    public int magSize
    {
        get { return _magSize; }
    }

    public Amunition(int _allAvailableBullets, int _clipsize)
    {
        _magSize = _clipsize;

        if (_allAvailableBullets - _clipsize <= 0)
        {
            this.currentMag = _allAvailableBullets;
            reserve = 0;
        }
        else
        {
            currentMag = _clipsize;
            reserve = _allAvailableBullets - _clipsize;
        }
    }

    public Amunition(int _current, int _reserve, int _magsize)
    {
        _magSize = _magsize;
        this.currentMag = _current;
        reserve = _reserve;
    }
}