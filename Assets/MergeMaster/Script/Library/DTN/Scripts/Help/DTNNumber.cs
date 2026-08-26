using System;
public class DTNNumber
{
    public static string FomatCoin(long coinCount)
    {
        long TotalCoinCount;
        TotalCoinCount = coinCount;
        decimal convertedCoinCount;
        string label = "";
        if (coinCount < 1000)
        {
            label = "";
            convertedCoinCount = TotalCoinCount;
        }
        else if (coinCount < 1000000)
        {
            label = "K";
            convertedCoinCount = Math.Round(TotalCoinCount / 1000M, 2);
        }
        else if (coinCount < 1000000000)
        {
            label = "M";
            convertedCoinCount = Math.Round(TotalCoinCount / 1000000M, 2);
        }
        else
        {
            label = "B";
            convertedCoinCount = Math.Round(TotalCoinCount / 1000000000M, 2);
        }

        return convertedCoinCount + label;
    }
}
