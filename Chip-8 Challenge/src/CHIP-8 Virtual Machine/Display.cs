namespace CHIP_8_Virtual_Machine;

public class Display
{
    private bool[,] _pixels;

    public bool this[int x, int y] 
    { 
        get => _pixels[x, y]; 
        set => _pixels[x, y] = value;
    }

    public Display()
    {
        _pixels = new bool[64, 32];
    }

    public bool DisplaySprite(int x, int y, byte[] bytes)
    {
        bool pixelErased = false;
        for (int i = 0; i < bytes.Length; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                byte @byte = bytes[i];

                int pixelIndex = (x + j) % 64;

                bool currentPixelState = _pixels[pixelIndex, y];
                bool shouldDisplay = (@byte & (1 << j)) != 0;
                if (currentPixelState && shouldDisplay)
                {
                    pixelErased = true;
                    shouldDisplay = false;
                }
            }
            y++;
        }

        return pixelErased;
    }

}
